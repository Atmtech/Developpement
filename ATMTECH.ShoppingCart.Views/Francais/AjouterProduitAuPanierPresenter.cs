﻿using System.Collections.Generic;
using System.Linq;
using ATMTECH.Common.Constant;
using ATMTECH.ShoppingCart.DAO.Interface.Francais;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.ShoppingCart.Views.Base;
using ATMTECH.ShoppingCart.Views.Interface.Francais;

namespace ATMTECH.ShoppingCart.Views.Francais
{
    public class AjouterProduitAuPanierPresenter : BaseShoppingCartPresenter<IAjouterProduitAuPanierPresenter>
    {
        public AjouterProduitAuPanierPresenter(IAjouterProduitAuPanierPresenter view)
            : base(view)
        {
        }

        public IProduitService ProduitService { get; set; }
        public IClientService ClientService { get; set; }
        public ICommandeService CommandeService { get; set; }
        public IInventaireService InventaireService { get; set; }
        public IDAOImageTechnoSport DAOImageTechnoSport { get; set; }

        public override void OnViewInitialized()
        {
            base.OnViewInitialized();
            AfficherProduit(View.IdProduit);
            AfficherListeDeroulanteCouleur();
            AfficherTaille();
            AfficherPrix();
            GererAffichagePourPossibiliteCommander();
            AfficherListeDesCouleurs();
        }
        public void AfficherListeDesCouleurs()
        {
            IList<ImageTechnoSport> imageTechnoSports = DAOImageTechnoSport.ObtenirImageTechnoSport(View.Produit.Ident);
            imageTechnoSports.Add(new ImageTechnoSport { Product = View.Produit, Image = View.Produit.PrincipalFileUrl, ImageCouleur = "", NomCouleurFr = "Original", NomCouleurEn = "Original" });

            View.ImageTechnoSports = imageTechnoSports;

        }
        public void AfficherTaille()
        {
            if (View.Produit != null)
            {
                IList<Taille> tailles = new List<Taille>();
                IList<Stock> stocks = View.Produit.Stocks.Where(x => x.ColorEnglish == View.Couleur && x.IsBackOrder == false).ToList();
                if (stocks.Count == 0)
                {
                    stocks = View.Produit.Stocks.Where(x => x.ColorFrench == View.Couleur).ToList();
                }

                foreach (Stock stock in stocks)
                {
                    int ordre = 0;
                    if (stock.Size == "OS") ordre = 0;
                    if (stock.Size == "S") ordre = 0;
                    if (stock.Size == "S/M") ordre = 0;
                    if (stock.Size == "M") ordre = 1;
                    if (stock.Size == "M/L") ordre = 1;
                    if (stock.Size == "L") ordre = 2;
                    if (stock.Size == "L/XL") ordre = 2;
                    if (stock.Size == "XL") ordre = 3;
                    if (stock.Size == "XL/2XL") ordre = 3;
                    if (stock.Size == "2XL") ordre = 4;
                    if (stock.Size == "3XL") ordre = 5;
                    if (stock.Size == "4XL") ordre = 6;
                    if (stock.Size == "5XL") ordre = 7;
                    if (stock.Size == "YXS") ordre = 0;
                    if (stock.Size == "XS") ordre = 1;
                    if (stock.Size == "YS") ordre = 1;
                    if (stock.Size == "YM") ordre = 2;
                    if (stock.Size == "YL") ordre = 3;
                    if (stock.Size == "YXL") ordre = 4;
                    if (tailles.Count(x => x.Nom == stock.Size) == 0)
                    {
                        switch (CurrentLanguage)
                        {
                            case LocalizationLanguage.FRENCH:
                                tailles.Add(new Taille { Nom = stock.FeatureFrench.Substring(0, stock.FeatureFrench.IndexOf("-")).Trim(), Ordre = ordre });
                                break;
                            case LocalizationLanguage.ENGLISH:
                                tailles.Add(new Taille { Nom = stock.FeatureEnglish.Substring(0, stock.FeatureEnglish.IndexOf("-")).Trim(), Ordre = ordre });
                                break;
                        }
                    }
                }

                View.Tailles = tailles.OrderBy(x => x.Ordre).ToList();
            }
        }
        public void AfficherListeDeroulanteCouleur()
        {
            if (View.Produit != null)
            {
                IList<string> couleur = new List<string>();
                foreach (Stock stock in View.Produit.Stocks.Where(stock => !couleur.Contains(stock.ColorFrench) && stock.IsBackOrder == false))
                {
                    couleur.Add(stock.ColorFrench);
                }

                IList<string> color = new List<string>();
                foreach (Stock stock in View.Produit.Stocks.Where(stock => !color.Contains(stock.ColorEnglish) && stock.IsBackOrder == false))
                {
                    color.Add(stock.ColorEnglish);
                }

                switch (CurrentLanguage)
                {
                    case LocalizationLanguage.FRENCH:
                        View.ListeDeroulanteCouleurs = couleur.OrderBy(x => x.ToLower()).ToList();
                        break;
                    case LocalizationLanguage.ENGLISH:
                        View.ListeDeroulanteCouleurs = color.OrderBy(x => x.ToLower()).ToList();
                        break;
                }
            }
        }
        public void AfficherProduit(int id)
        {
            if (id == 0)
            {
                NavigationService.Redirect(Pages.Pages.DEFAULT);
            }
            View.Produit = ProduitService.ObtenirProduit(id);
        }
        public void AfficherPrix()
        {
            Stock stock = TrouverLeStockAvecTailleEtCouleur();
            decimal prixAjusteStock = 0;
            if (stock != null)
            {
                prixAjusteStock = stock.AdjustPrice;
            }

            View.PrixUnitaireEnSolde = View.Produit.SalePrice + prixAjusteStock;
            View.PrixUnitaireOriginal = View.Produit.UnitPrice + prixAjusteStock;


        }
        public void GererAffichagePourPossibiliteCommander()
        {
            View.EstPossibleDeCommander = ClientService.ClientAuthentifie != null;
        }
        public void AjouterLigneCommande()
        {
            Stock stock = TrouverLeStockAvecTailleEtCouleur();
            if (stock != null)
            {
                CommandeService.AjouterLigneCommande(stock.Id, View.Quantite);
                NavigationService.Redirect(Pages.Pages.BASKET);
            }
        }
        private Stock TrouverLeStockAvecTailleEtCouleur()
        {
            Stock stock = null;
            switch (CurrentLanguage)
            {
                case LocalizationLanguage.FRENCH:
                    stock = View.Produit.Stocks.FirstOrDefault(x => x.FeatureFrench == View.Taille + " - " + View.Couleur);
                    break;
                case LocalizationLanguage.ENGLISH:
                    stock = View.Produit.Stocks.FirstOrDefault(x => x.FeatureEnglish == View.Taille + " - " + View.Couleur);
                    break;
            }
            return stock;
        }
    }
    public class Taille
    {
        public string Nom { get; set; }
        public int Ordre { get; set; }
    }

    public class Couleur
    {
        public string Nom { get; set; }
        public string NomAnglais { get; set; }
        public string Images { get; set; }
    }

}