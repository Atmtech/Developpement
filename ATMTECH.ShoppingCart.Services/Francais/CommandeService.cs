﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ATMTECH.Common.Constant;
using ATMTECH.Services;
using ATMTECH.Services.Interface;
using ATMTECH.ShoppingCart.DAO.Interface.Francais;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Interface;
using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.Web.Services;
using ATMTECH.Web.Services.Base;
using ATMTECH.Web.Services.Interface;
using Math = ATMTECH.Common.Utils.Math;

namespace ATMTECH.ShoppingCart.Services.Francais
{
    public class CommandeService : BaseService, ICommandeService
    {
        public IDAOCoupon DAOCoupon { get; set; }
        public IDAOProduit DAOProduit { get; set; }
        public IDAOCommande DAOCommande { get; set; }
        public IDAOLigneCommande DAOLigneCommande { get; set; }
        public IDAOInventaire DAOInventaire { get; set; }

        public IProduitService ProduitService { get; set; }
        public IClientService ClientService { get; set; }
        public ITaxesService TaxesService { get; set; }
        public IMessageService MessageService { get; set; }
        public IValiderCommandeService ValiderCommandeService { get; set; }
        public IEnvoiPostalService EnvoiPostalService { get; set; }
        public IParameterService ParameterService { get; set; }
        public IPaypalService PaypalService { get; set; }
        public ILocalizationService LocalizationService { get; set; }
        public IReportService ReportService { get; set; }
        public IMailService MailService { get; set; }
        public ICourrielService CourrielService { get; set; }

        public Order ObtenirCommandeSouhaite(Customer client)
        {
            Order obtenirCommandeSouhaite = DAOCommande.ObtenirCommandeSouhaite(client);
            if (obtenirCommandeSouhaite == null) return null;
            obtenirCommandeSouhaite.Customer = client;
            obtenirCommandeSouhaite.Enterprise = client.Enterprise;
            obtenirCommandeSouhaite.AddressBilling = client.AddressBilling;
            obtenirCommandeSouhaite.AddressShipping = client.AddressShipping;
            obtenirCommandeSouhaite.PostalCodeShipping = client.PostalCodeShipping;
            return obtenirCommandeSouhaite;
        }

        public IList<Order> ObtenirCommande(Customer customer)
        {
            return DAOCommande.ObtenirCommande(customer);
        }
        public Order ValiderCoupon(Order commande, string coupon)
        {
            Coupon couponValider = DAOCoupon.ObtenirCoupon(coupon);
            if (!string.IsNullOrEmpty(coupon))
            {
                if (couponValider != null)
                {
                    commande.Coupon = couponValider;
                    return Enregistrer(commande);
                }

                MessageService.ThrowMessage(CodeErreur.SC_COUPON_INVALIDE);
                return commande;
            }
            return commande;
        }
        public bool ConfirmerCommande(int id)
        {
            Order commande = ObtenirCommande(id);
            if (commande != null)
            {
                commande.OrderStatus = OrderStatus.IsShipped;
                commande.ShippingDate = DateTime.Now;
                Enregistrer(commande);
                CourrielService.EnvoyerConfirmationCommandeEstEnLivraison(commande, ObtenirFacturePourPdf(commande));
                return true;
            }
            return false;
        }

        public Stream ObtenirFacturePourPdf(Order commande)
        {
            Dictionary<string, string> dictionnaire = new Dictionary<string, string>();
            ReportParameter reportParameter = new ReportParameter
            {
                Assembly = "ATMTECH.ShoppingCart.Services",
                PathToReportAssembly = "ATMTECH.ShoppingCart.Services.Reports.CommandeCheckLePrix_" + LocalizationService.CurrentLanguage + ".rdlc",
                ReportFormat = ReportFormat.PDF,
                Parameters = dictionnaire
            };

            IList<Order> order = new List<Order>();
            order.Add(commande);

            reportParameter.AddDatasource("dsCommande", order);
            reportParameter.AddDatasource("dsLigneCommande", commande.OrderLines);
            return ReportService.SaveReportToStream("Invoice.pdf", ReportService.GetReport(reportParameter));
        }

        public Order EffacerCoupon(Order commande)
        {
            commande.Coupon = null;
            return Enregistrer(commande);
        }

        public bool ValiderCodePostalLivraison(Order order)
        {
            if (EnvoiPostalService.EstCodePostalValideAvecPurolator(order.PostalCodeShipping) == false)
            {
                MessageService.ThrowMessage(CodeErreur.SC_CODE_POSTAL_INVALIDE);
                return false;
            }
            return true;
        }
        public IList<Order> ObtenirCommande()
        {
            return DAOCommande.GetAllActive();
        }
        public IList<OrderLine> ObtenirLigneCommande()
        {
            return DAOLigneCommande.GetAllActive();
        }
        public Order ObtenirCommande(int id)
        {
            return DAOCommande.ObtenirCommande(id);
        }
        public Order CreerCommande()
        {
            Customer client = ClientService.ClientAuthentifie;
            if (ValiderCommandeService.EstClientValide(client))
            {
                Order order = new Order
                    {
                        IsActive = true,
                        OrderStatus = OrderStatus.IsWishList,
                        Customer = client,
                        Enterprise = client.Enterprise,
                        AddressShipping = client.AddressShipping,
                        AddressBilling = client.AddressBilling
                    };

                return Enregistrer(order);
            }
            return null;
        }
        public Order Enregistrer(Order commande)
        {
            if (commande.OrderLines != null && commande.OrderLines.Count(x => x.IsActive) > 0)
            {
                commande = CalculerTotal(commande);
                foreach (OrderLine orderLine in commande.OrderLines)
                {
                    DAOLigneCommande.Save(orderLine);
                }
            }
            else
            {
                commande.Coupon = null;
                commande.GrandTotalWithCoupon = 0;
            }
            commande.Id = DAOCommande.Save(commande);
            return commande;
        }
        public Order CalculerTotal(Order commande)
        {

            commande.TotalWeight = 0;
            commande = CalculerSousTotaux(commande);
            commande = CalculerEnvoiPostal(commande);
            commande = CalculerTaxe(commande);


            commande.GrandTotal =  commande.SubTotal + commande.CountryTax + commande.RegionalTax + commande.ShippingTotal;

            return commande;
        }
        public Order CalculerTaxe(Order commande)
        {
            decimal countryTax = TaxesService.GetCountryTaxes(commande.SubTotal, "QBC");
            decimal regionalTax = TaxesService.GetRegionTaxes(commande.SubTotal, "QBC");
            commande.CountryTax = Math.RoundingMoney(countryTax);
            commande.RegionalTax = Math.RoundingMoney(regionalTax);
            return commande;
        }
        public Order CalculerEnvoiPostal(Order commande)
        {
            if (commande.Enterprise.IsShippingManaged)
            {
                if (commande.Coupon != null && commande.Coupon.IsShippingSave)
                {
                    commande.ShippingTotal = 0;
                }
                else
                {
                    commande.TotalWeight = 0;
                    foreach (OrderLine orderLine in commande.OrderLines)
                    {
                        commande.TotalWeight += orderLine.Stock.Product.Weight * orderLine.Quantity;
                    }

                    if (commande.TotalWeight < 1)
                    {
                        commande.TotalWeight = 1;
                    }

                    commande.ShippingTotal = EnvoiPostalService.ObtenirCotationPurolator(commande);
                }
            }
            else
            {
                commande.ShippingTotal = 0;
            }

            if (commande.Coupon != null && commande.Coupon.IsShippingSave)
            {
                commande.ShippingTotal = 0;
            }

            return commande;
        }
        public Order AjouterLigneCommande(int idInventaire, int quantite)
        {
            Customer client = ClientService.ClientAuthentifie;
            if (ValiderCommandeService.EstClientValide(client))
            {
                if (ValiderCommandeService.EstQuantiteValide(quantite))
                {
                    Stock stock = DAOInventaire.ObtenirInventaire(idInventaire);
                    stock.Product = DAOProduit.ObtenirProduit(stock.Product.Id);
                    if (ValiderCommandeService.EstItemPresentEnInventaire(stock.Product.Ident, stock.Size, stock.ColorEnglish))
                    {
                        Order obtenirCommandeSouhaite = DAOCommande.ObtenirCommandeSouhaite(client);
                        if (obtenirCommandeSouhaite == null)
                        {
                            Order commande = CreerCommande();
                            return SauvegarderLigneCommande(commande, idInventaire, quantite);
                        }
                        return SauvegarderLigneCommande(obtenirCommandeSouhaite, idInventaire, quantite);
                    }
                }
            }
            return null;
        }
        public void FinaliserCommandeAvecPaypal(Order commande)
        {
            if (ValiderCodePostalLivraison(commande))
            {
                string nomProduit = HttpUtility.HtmlDecode(commande.OrderLines.Aggregate(string.Empty, (current, line) => current + (line.ProductDescription + " (" + line.Quantity + ") , ")));
                string descriptionCommande = string.Format("Date: {0}, {1}", commande.DateModified, commande.Enterprise.Name);
//                double prix = commande.Coupon != null ? (double)commande.GrandTotalWithCoupon : (double)commande.GrandTotal;
                double prix = (double)commande.GrandTotal;

                PaypalDto paypalDto = new PaypalDto
                {
                    OrderDescription = descriptionCommande,
                    Price = prix,
                    Quantity = 1,
                    OrderId = commande.Id.ToString(),
                    ProductName = nomProduit
                };

                PaypalService.SendPaypalRequest(paypalDto);
            }
        }
        public Order FinaliserCommande(Order commande)
        {
            if (ValiderCodePostalLivraison(commande))
            {
                commande.FinalizedDate = DateTime.Now;
                commande.OrderStatus = OrderStatus.IsOrdered;
                commande = Enregistrer(commande);
                CourrielService.EnvoyerCommandeFinaliser(commande, ObtenirFacturePourPdf(commande));
            }
            return commande;
        }
        public Order ImprimerCommande(Order commande)
        {
            Dictionary<string, string> dictionnaire = new Dictionary<string, string>();
            ReportParameter reportParameter = new ReportParameter
            {
                Assembly = "ATMTECH.ShoppingCart.Services",
                PathToReportAssembly = "ATMTECH.ShoppingCart.Services.Reports.CommandeCheckLePrix_" + LocalizationService.CurrentLanguage + ".rdlc",
                ReportFormat = ReportFormat.PDF,
                Parameters = dictionnaire
            };

            IList<Order> order = new List<Order>();
            order.Add(commande);

            reportParameter.AddDatasource("dsCommande", order);
            reportParameter.AddDatasource("dsLigneCommande", commande.OrderLines);
            ReportService.SaveReport(
                LocalizationService.CurrentLanguage == LocalizationLanguage.ENGLISH ? commande.Id + "_Order_CheckLePrix.pdf" : commande.Id + "_Commande_CheckLePrix.pdf",
                ReportService.GetReport(reportParameter));
            return commande;
        }
        public void SupprimerLigneCommande(OrderLine ligneCommande)
        {
            ligneCommande.IsActive = false;
            DAOLigneCommande.Save(ligneCommande);
        }

        private Order SauvegarderLigneCommande(Order commande, int idInventaire, int quantite)
        {
            Stock stock = DAOInventaire.ObtenirInventaire(idInventaire);
            stock.Product = DAOProduit.ObtenirProduit(stock.Product.Id);

            OrderLine orderLine = new OrderLine
            {
                Order = new Order { Id = commande.Id },
                Stock = stock,
                UnitPrice = stock.Product.UnitPrice + stock.AdjustPrice,
                Quantity = quantite,
                IsActive = true
            };
            if (commande.OrderLines == null)
            {
                commande.OrderLines = new List<OrderLine>();
            }

            if (commande.OrderLines.Count(x => x.Stock.Id == idInventaire) > 0)
            {
                OrderLine ligneCommande = commande.OrderLines.FirstOrDefault(x => x.Stock.Id == idInventaire);
                if (ligneCommande != null) ligneCommande.Quantity = quantite;
            }
            else
            {
                commande.OrderLines.Add(orderLine);
            }


            return Enregistrer(commande);
        }
        private Order CalculerSousTotaux(Order commande)
        {
            if (commande.FinalizedDate == null)
            {
                commande.SubTotal = 0;
                if (commande.OrderLines != null)
                {
                    foreach (OrderLine orderLine in commande.OrderLines)
                    {
                        if (orderLine.IsActive)
                        {
                            Product product = ProduitService.ObtenirProduit(orderLine.Stock.Product.Id);
                            orderLine.SubTotal = orderLine.UnitPrice * orderLine.Quantity;
                            commande.TotalWeight += (product.Weight * orderLine.Quantity);
                            commande.SubTotal += orderLine.SubTotal;
                        }
                    }

                    if (commande.Coupon != null)
                    {
                        commande.SubTotal = commande.SubTotal - (commande.SubTotal*commande.Coupon.PercentageSave/100);
                    }
                }
            }
            return commande;
        }
    

    }
}

