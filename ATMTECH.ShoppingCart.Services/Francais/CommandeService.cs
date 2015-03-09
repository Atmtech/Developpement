﻿using System.Collections.Generic;
using System.Linq;
using ATMTECH.Common.Utils;
using ATMTECH.ShoppingCart.DAO.Interface.Francais;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Interface;
using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.Web.Services;
using ATMTECH.Web.Services.Base;
using ATMTECH.Web.Services.Interface;

namespace ATMTECH.ShoppingCart.Services.Francais
{
    public class CommandeService : BaseService, ICommandeService
    {
        public IProduitService ProduitService { get; set; }
        public IDAOCommande DAOCommande { get; set; }
        public IClientService ClientService { get; set; }
        public ITaxesService TaxesService { get; set; }
        public IMessageService MessageService { get; set; }
        public IValiderCommandeService ValiderCommandeService { get; set; }
        public IShippingService ShippingService { get; set; }
        public IParameterService ParameterService { get; set; }

        public Order ObtenirCommandeSouhaite(Customer client)
        {
            return DAOCommande.ObtenirCommandeSouhaite(client);
        }
        public Order CreerCommande()
        {
            Customer client = ClientService.ClientAuthentifie;
            if (ValiderCommandeService.EstClientValide(client))
            {
                Order order = new Order
                    {
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
            return commande;
        }
        public Order CalculerTotal(Order commande)
        {
            commande.TotalWeight = 0;
            commande.SubTotal = 0;

            foreach (OrderLine orderLine in commande.OrderLines)
            {
                if (orderLine.IsActive)
                {
                    Product product = ProduitService.ObtenirProduit(orderLine.Stock.Product.Id);
                    orderLine.SubTotal = (product.SalePrice != 0 ? product.SalePrice : product.UnitPrice + orderLine.Stock.AdjustPrice) * orderLine.Quantity;
                    commande.TotalWeight += (product.Weight * orderLine.Quantity);
                    commande.SubTotal += orderLine.SubTotal;
                }
            }

            commande = CalculerEnvoiPostal(commande);
            commande = CalculerTaxe(commande);
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
        private Order SauvegarderLigneCommande(Order commande, int idInventaire, int quantite)
        {
            OrderLine orderLine = new OrderLine
            {
                Stock = new Stock { Id = idInventaire },
                Quantity = quantite
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
    }
}

