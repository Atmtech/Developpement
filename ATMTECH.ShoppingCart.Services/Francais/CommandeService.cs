﻿using System;
using System.Collections.Generic;
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
        public IProduitService ProduitService { get; set; }
        public IDAOCommande DAOCommande { get; set; }
        public IDAOLigneCommande DAOLigneCommande { get; set; }
        public IDAOInventaire DAOInventaire { get; set; }
        public IClientService ClientService { get; set; }
        public ITaxesService TaxesService { get; set; }
        public IMessageService MessageService { get; set; }
        public IValiderCommandeService ValiderCommandeService { get; set; }
        public IShippingService ShippingService { get; set; }
        public IParameterService ParameterService { get; set; }
        public IPaypalService PaypalService { get; set; }
        public ILocalizationService LocalizationService { get; set; }
        public IReportService ReportService { get; set; }
        public IDAOCoupon DAOCoupon { get; set; }

        public Order ObtenirCommandeSouhaite(Customer client)
        {
            Order obtenirCommandeSouhaite = DAOCommande.ObtenirCommandeSouhaite(client);
            if (obtenirCommandeSouhaite == null) return null;
            if (obtenirCommandeSouhaite.BillingAddress == null)
                obtenirCommandeSouhaite.BillingAddress = client.BillingAddress;
            if (obtenirCommandeSouhaite.ShippingAddress == null)
                obtenirCommandeSouhaite.ShippingAddress = client.ShippingAddress;
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

                MessageService.ThrowMessage(ErrorCode.SC_INVALID_COUPON);
                return commande;
            }
            return commande;
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
                        ShippingAddress = client.ShippingAddress,
                        BillingAddress = client.BillingAddress
                    };

                return Enregistrer(order);
            }
            return null;
        }
        public Order Enregistrer(Order commande)
        {
            commande = CalculerTotal(commande);
            if (commande.OrderLines != null)
            {
                foreach (OrderLine orderLine in commande.OrderLines)
                {
                    DAOLigneCommande.Save(orderLine);
                }
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
            commande = CalculerCoupon(commande);
            commande.GrandTotal = commande.SubTotal + commande.CountryTax + commande.RegionalTax + commande.ShippingTotal;

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
            if (commande.Coupon != null && commande.Coupon.IsShippingSave)
            {
                commande.ShippingTotal = 0;
            }
            else
            {
                ShippingParameter shippingParameter = new ShippingParameter
                {
                    BillingAccount = "99999999",
                    CountryReceiverCode = ParameterService.GetValue("CountryReceiverCode"),
                    PackageType = PurolatorPackageType.EXPRESS_BOX,
                    ServiceType = PurolatorServiceType.EXPRESS_BOX,
                    SenderPostalCode = ParameterService.GetValue("SenderPostalCode"),
                    ShippingType = ShippingType.Purolator,
                    WeightType = ShippingParameter.WeightTypes.Lbs,
                    AccountId = ParameterService.GetValue("PurolatorBillingAccount"),
                    Login = ParameterService.GetValue("PurolatorUserName"),
                    Password = ParameterService.GetValue("PurolatorPassword"),
                    Url = ParameterService.GetValue("PurolatorWebServiceUrl")
                };

                commande.ShippingTotal = ShippingService.GetShippingTotal(commande, shippingParameter);
            }

            return commande;
        }
        public Order AjouterLigneCommande(int idInventaire, int quantite)
        {
            Customer client = ClientService.ClientAuthentifie;
            if (ValiderCommandeService.EstClientValide(client))
            {
                Order obtenirCommandeSouhaite = DAOCommande.ObtenirCommandeSouhaite(client);
                if (obtenirCommandeSouhaite == null)
                {
                    Order commande = CreerCommande();
                    return SauvegarderLigneCommande(commande, idInventaire, quantite);
                }

                return SauvegarderLigneCommande(obtenirCommandeSouhaite, idInventaire, quantite);
            }
            return null;
        }
        public void FinaliserCommandeAvecPaypal(Order commande)
        {
            string productName = HttpUtility.HtmlDecode(commande.OrderLines.Aggregate(string.Empty, (current, line) => current + (line.ProductDescription + " (" + line.Quantity + ") , ")));
            PaypalDto paypalDto = new PaypalDto
            {
                OrderDescription = string.Format(ParameterService.GetValue("OrderMessagePaypal"), commande.DateModified.ToString(), commande.Enterprise.Name),
                Price = commande.Coupon != null ? (double)commande.GrandTotalWithCoupon : (double)commande.GrandTotal,
                Quantity = 1,
                OrderId = commande.Id.ToString(),
                ProductName = productName
            };

            PaypalService.SendPaypalRequest(paypalDto);
        }
        public Order FinaliserCommande(Order commande)
        {
            commande.FinalizedDate = DateTime.Now;
            commande.OrderStatus = OrderStatus.IsOrdered;
            commande = Enregistrer(commande);
            return commande;
        }
        public Order ImprimerCommande(Order commande)
        {
            Dictionary<string, string> dictionnaire = new Dictionary<string, string>();
            ReportParameter reportParameter = new ReportParameter
            {
                Assembly = "ATMTECH.ShoppingCart.Services",
                PathToReportAssembly = "ATMTECH.ShoppingCart.Services.Reports.Commande_" + LocalizationService.CurrentLanguage + ".rdlc",
                ReportFormat = ReportFormat.PDF,
                Parameters = dictionnaire
            };

            IList<Order> order = new List<Order>();
            order.Add(commande);

            reportParameter.AddDatasource("dsCommande", order);
            reportParameter.AddDatasource("dsLigneCommande", commande.OrderLines);
            ReportService.SaveReport(
                LocalizationService.CurrentLanguage == LocalizationLanguage.ENGLISH ? "Order.pdf" : "Commande.pdf",
                ReportService.GetReport(reportParameter));
            return commande;
        }

        private Order SauvegarderLigneCommande(Order commande, int idInventaire, int quantite)
        {
            OrderLine orderLine = new OrderLine
            {
                Order = new Order { Id = commande.Id },
                Stock = DAOInventaire.ObtenirInventaire(idInventaire),
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
                            orderLine.UnitPrice = product.UnitPrice > product.SalePrice ? product.SalePrice : product.UnitPrice;
                            orderLine.SubTotal = (product.SalePrice != 0
                                                      ? product.SalePrice
                                                      : product.UnitPrice + orderLine.Stock.AdjustPrice) * orderLine.Quantity;
                            commande.TotalWeight += (product.Weight * orderLine.Quantity);
                            commande.SubTotal += orderLine.SubTotal;
                        }
                    }
                }
            }
            return commande;
        }
        private Order CalculerCoupon(Order commande)
        {
            if (commande.Coupon != null && !commande.Coupon.IsShippingSave)
            {
                commande.GrandTotalWithCoupon = commande.SubTotal - (commande.SubTotal * commande.Coupon.PercentageSave / 100) + commande.CountryTax + commande.RegionalTax + commande.ShippingTotal;
            }
            return commande;
        }

    }
}

