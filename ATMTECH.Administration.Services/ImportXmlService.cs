﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ATMTECH.Administration.Services.Interface;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Interface;
using ATMTECH.Web.Services.Base;
using ATMTECH.Web.Services.Interface;

namespace ATMTECH.Administration.Services
{
    public class ImportXmlService : BaseService, IImportXmlService
    {
        public IProductService ProductService { get; set; }
        public IStockService StockService { get; set; }
        public IParameterService ParameterService { get; set; }

        public IList<string> DisplayColor(Enterprise enterprise, string fileXml)
        {
            IList<ImportProduit> importProduits = new List<ImportProduit>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileXml);

            if (xmlDoc.DocumentElement == null) return null;
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/root/item");

            if (nodeList != null)
                foreach (ImportProduit importProduit in from XmlNode node in nodeList
                                                        select new ImportProduit
                                                        {
                                                            // ReSharper disable PossibleNullReferenceException
                                                            ItemID = node.SelectSingleNode("ItemID").InnerText,
                                                            Brand = node.SelectSingleNode("Brand").InnerText,
                                                            Size = node.SelectSingleNode("Size").InnerText,
                                                            ColorId = node.SelectSingleNode("ColorId").InnerText,
                                                            Color_EN = node.SelectSingleNode("Color_EN").InnerText,
                                                            Color_FR = node.SelectSingleNode("Color_FR").InnerText,
                                                            Title_EN = node.SelectSingleNode("Title_EN").InnerText,
                                                            Title_FR = node.SelectSingleNode("Title_FR").InnerText,
                                                            Desc_EN = node.SelectSingleNode("Desc_EN").InnerText,
                                                            Desc_FR = node.SelectSingleNode("Desc_FR").InnerText,
                                                            Sex = node.SelectSingleNode("Sex").InnerText,
                                                            Category1 = node.SelectSingleNode("Category1").InnerText,
                                                            Category2 = node.SelectSingleNode("Category2").InnerText,
                                                            Category3 = node.SelectSingleNode("Category3").InnerText,
                                                            Category4 = node.SelectSingleNode("Category4").InnerText,
                                                            Category5 = node.SelectSingleNode("Category5").InnerText
                                                            // ReSharper restore PossibleNullReferenceException
                                                        })
                {
                    importProduits.Add(importProduit);
                }

            IList<string> couleur = new List<string>();
            foreach (ImportProduit importProduit in importProduits)
            {
                if (!couleur.Contains(importProduit.Color_EN.ToLower()))
                {
                    couleur.Add(importProduit.Color_EN.ToLower());
                }
            }
            return couleur;
        }

        private void ChangerEncodage(string fileXml)
        {

            string path_new = fileXml + ".new";

            Encoding utf8 = new UTF8Encoding(false);
            Encoding ansi = Encoding.GetEncoding(1252);

            string xml = File.ReadAllText(fileXml, ansi);

            XDocument xmlDoc = XDocument.Parse(xml);

            File.WriteAllText(
                path_new,
                @"<?xml version=""1.0"" encoding=""utf-8""?>" + xmlDoc.ToString(),
               utf8
            );

            File.Delete(fileXml);
            File.Move(path_new, fileXml);

        }
        public void ImportProductAndStockXml(Enterprise enterprise, string fileXml)
        {
            //ChangerEncodage(fileXml);

            IList<Product> products = ProductService.GetAllActive().Where(x => x.Enterprise.Id == enterprise.Id).ToList();


            IList<ImportProduit> importProduits = new List<ImportProduit>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileXml);

            if (xmlDoc.DocumentElement == null) return;
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Root/Item");


            if (nodeList != null)
                foreach (ImportProduit importProduit in from XmlNode node in nodeList
                                                        select new ImportProduit
                                                            {
                                                                // ReSharper disable PossibleNullReferenceException
                                                                ItemID = node.SelectSingleNode("ITEMID").InnerText,
                                                                Brand = node.SelectSingleNode("BRAND").InnerText,
                                                                Size = node.SelectSingleNode("INVENTSIZEID").InnerText,
                                                                ColorId = node.SelectSingleNode("INVENTCOLORID").InnerText,
                                                                Price = Convert.ToDecimal(node.SelectSingleNode("PRICE").InnerText.Replace(".", ",")),
                                                                Color_EN = node.SelectSingleNode("COLOR_EN").InnerText,
                                                                Color_FR = node.SelectSingleNode("COLOR_FR").InnerText,
                                                                Title_EN = node.SelectSingleNode("Title_EN").InnerText,
                                                                Title_FR = node.SelectSingleNode("Title_FR").InnerText,
                                                                Desc_EN = node.SelectSingleNode("Desc_EN").InnerText,
                                                                Desc_FR = node.SelectSingleNode("Desc_FR").InnerText,
                                                                Sex = node.SelectSingleNode("SEX") == null ? "" : node.SelectSingleNode("SEX").InnerText,
                                                                Category1 = node.SelectSingleNode("Category1") != null ? node.SelectSingleNode("Category1").InnerText : "",
                                                                Category2 = node.SelectSingleNode("Category2") != null ? node.SelectSingleNode("Category2").InnerText : "",
                                                                Category3 = node.SelectSingleNode("Category3") != null ? node.SelectSingleNode("Category3").InnerText : "",
                                                                Category4 = node.SelectSingleNode("Category4") != null ? node.SelectSingleNode("Category4").InnerText : "",
                                                                Category5 = node.SelectSingleNode("Category5") != null ? node.SelectSingleNode("Category5").InnerText : ""
                                                                // ReSharper restore PossibleNullReferenceException
                                                            })
                {
                    importProduits.Add(importProduit);
                }

            
          
            // Générer les catégories
            IList<ProductCategory> categoriesTraite = new List<ProductCategory>();
            IList<ProductCategory> categorieExistante = ProductService.GetProductCategoryWithoutLanguage(enterprise.Id);

            foreach (ImportProduit importProduit in importProduits)
            {
                string categorieAnglaise = TrouverCategorieAnglaise(importProduit);
                string categorieFrancais = TrouverCategorieFrancaise(importProduit);

                ProductCategory productCategory;
                if (!string.IsNullOrEmpty(categorieAnglaise))
                {
                    productCategory = new ProductCategory
                    {
                        Description = categorieAnglaise,
                        Enterprise = enterprise,
                        Language = "en",
                        IsActive = true
                    };
                    if (categoriesTraite.Count(x => x.Description == categorieAnglaise) == 0 && categorieExistante.Count(x => x.Description == categorieAnglaise) == 0)
                    {
                        ProductService.SaveProductCategory(productCategory);
                    }
                    categoriesTraite.Add(productCategory);
                }

                if (!string.IsNullOrEmpty(categorieFrancais))
                {
                    productCategory = new ProductCategory
                    {
                        Description = categorieFrancais,
                        Enterprise = enterprise,
                        Language = "fr",
                        IsActive = true
                    };
                    if (categoriesTraite.Count(x => x.Description == categorieFrancais) == 0 && categorieExistante.Count(x => x.Description == categorieFrancais) == 0)
                    {
                        ProductService.SaveProductCategory(productCategory);
                    }
                    categoriesTraite.Add(productCategory);
                }
            }

            // Générer les produits
            // désactivé les produits inexistants
            foreach (Product product in products)
            {
                if (importProduits.Count(x => x.ItemID == product.Ident) == 0)
                {
                    product.IsActive = false;
                    ProductService.Save(product);
                }
            }
           
            IList<Product> productsTraite = new List<Product>();
            IList<ProductCategory> productCategories = ProductService.GetProductCategoryWithoutLanguage(enterprise.Id);
            foreach (ImportProduit importProduit in importProduits)
            {
                Product product = products.FirstOrDefault(x => x.Ident == importProduit.ItemID);
                List<ImportProduit> produits = importProduits.Where(x => x.ItemID == importProduit.ItemID).OrderBy(x => x.Price).ToList();
                Decimal prixUnitaire = produits.First().Price;


                if (product == null && productsTraite.Count(x => x.Ident == importProduit.ItemID) == 0)
                {
                    product = new Product
                        {
                            Brand = importProduit.Brand,
                            Ident = importProduit.ItemID,
                            DescriptionEnglish = importProduit.Desc_EN,
                            DescriptionFrench = importProduit.Desc_FR,
                            NameEnglish = importProduit.Title_EN,
                            NameFrench = importProduit.Title_FR,
                            Enterprise = enterprise,
                            ProductCategoryEnglish = productCategories.FirstOrDefault(x => x.Description == TrouverCategorieAnglaise(importProduit)),
                            ProductCategoryFrench = productCategories.FirstOrDefault(x => x.Description == TrouverCategorieFrancaise(importProduit)),
                            Weight = 1,
                            UnitPrice = prixUnitaire,
                            CostPrice = prixUnitaire
                        };
                    productsTraite.Add(product);
                    ProductService.Save(product);
                }
                else
                {
                    if (product != null)
                    {
                        product.Brand = importProduit.Brand;
                        product.DescriptionEnglish = importProduit.Desc_EN;
                        product.DescriptionFrench = importProduit.Desc_FR;
                        product.NameEnglish = importProduit.Title_EN;
                        product.NameFrench = importProduit.Title_FR;
                        product.ProductCategoryEnglish = productCategories.FirstOrDefault(x => x.Description == TrouverCategorieAnglaise(importProduit));
                        product.ProductCategoryFrench = productCategories.FirstOrDefault(x => x.Description == TrouverCategorieFrancaise(importProduit));
                        product.UnitPrice = prixUnitaire;
                        product.CostPrice = prixUnitaire;
                        productsTraite.Add(product);
                        ProductService.Save(product);
                    }
                }
            }

            // Générer les stock // caracteristique
            products = ProductService.GetAllActive().Where(x => x.Enterprise.Id == enterprise.Id).ToList();
            IList<Stock> stocks = StockService.GetAllStockByEnterprise(enterprise.Id);
            foreach (Product product in products)
            {
                if (importProduits.Count(x => x.ItemID == product.Ident) > 0)
                {
                    IList<Stock> stocksTraite = new List<Stock>();
                    IList<ImportProduit> listeProduit = importProduits.Where(x => x.ItemID == product.Ident).ToList();
                    foreach (ImportProduit importProduit in listeProduit)
                    {
                        Stock stock = stocks.FirstOrDefault(x => x.Id == product.Id && x.ColorEnglish == importProduit.Color_EN && x.Size == importProduit.Size);

                        List<ImportProduit> produits = importProduits.Where(x => x.ItemID == importProduit.ItemID).OrderBy(x => x.Price).ToList();
                        Decimal prixUnitaire = produits.First().Price;

                        string featureEnglish = TrouverCaracteristiqueAnglais(importProduit.Size, importProduit.Color_EN);
                        string featureFrench = TrouverCaracteristiqueFrancais(importProduit.Size, importProduit.Color_FR);

                        if (stock == null)
                        {
                            stock = new Stock
                                {
                                    FeatureEnglish = featureEnglish,
                                    FeatureFrench = featureFrench,
                                    ColorEnglish = importProduit.Color_EN,
                                    ColorFrench = TrouverCouleurFrancaisManquante(importProduit),
                                    Size = importProduit.Size,
                                    AdjustPrice = importProduit.Price - prixUnitaire,
                                    Product = product,
                                    IsWithoutStock = true,
                                    InitialState = 0,
                                    IsWarningOnLow = false,
                                    ColorId = importProduit.ColorId,
                                    IsBackOrder = false
                                };
                            stocksTraite.Add(stock);
                            StockService.Save(stock);
                        }
                        else
                        {
                            if (stock != null)
                            {
                                stock.FeatureEnglish = featureEnglish;
                                stock.FeatureFrench = featureFrench;
                                stock.AdjustPrice = importProduit.Price - prixUnitaire;
                                stock.ColorEnglish = importProduit.Color_EN;
                                stock.ColorFrench = TrouverCouleurFrancaisManquante(importProduit);
                                stock.Size = importProduit.Size;
                                stock.Product = product;
                                stock.IsWithoutStock = true;
                                stock.InitialState = 0;
                                stock.IsWarningOnLow = false;
                                stock.ColorId = importProduit.ColorId;
                                stock.IsBackOrder = false;
                                stocksTraite.Add(stock);
                                StockService.Save(stock);
                            }
                        }
                    }
                }
            }
        }


        private string TrouverCaracteristiqueAnglais(string grandeur, string couleur)
        {
            if (grandeur.ToUpper() == "2XL") return "2XLarge - " + couleur;
            if (grandeur.ToUpper() == "3XL") return "3XLarge - " + couleur;
            if (grandeur.ToUpper() == "4XL") return "4XLarge - " + couleur;
            if (grandeur.ToUpper() == "5XL") return "5XLarge - " + couleur;
            if (grandeur.ToUpper() == "L") return "Large - " + couleur;
            if (grandeur.ToUpper() == "L/XL") return "Large/XLarge - " + couleur;
            if (grandeur.ToUpper() == "M") return "Medium - " + couleur;
            if (grandeur.ToUpper() == "M/L") return "Medium/Large - " + couleur;
            if (grandeur.ToUpper() == "OS") return "OS - " + couleur;
            if (grandeur.ToUpper() == "S") return "Small - " + couleur;
            if (grandeur.ToUpper() == "S/M") return "Small/Medium - " + couleur;
            if (grandeur.ToUpper() == "XL") return "XLarge - " + couleur;
            if (grandeur.ToUpper() == "XL/2XL") return "Xlarge/2Xlarge - " + couleur;
            if (grandeur.ToUpper() == "XS") return "XSmall - " + couleur;
            if (grandeur.ToUpper() == "YL") return "Y Large - " + couleur;
            if (grandeur.ToUpper() == "YM") return "Y Medium - " + couleur;
            if (grandeur.ToUpper() == "YS") return "Y Small - " + couleur;
            if (grandeur.ToUpper() == "YXL") return "Y XLarge - " + couleur;
            if (grandeur.ToUpper() == "YXS") return "Y XSmall - " + couleur;
            return "";
        }

        private string TrouverCaracteristiqueFrancais(string grandeur, string couleur)
        {
            if (grandeur.ToUpper() == "2XL") return "2XLarge - " + couleur;
            if (grandeur.ToUpper() == "3XL") return "3XLarge - " + couleur;
            if (grandeur.ToUpper() == "4XL") return "4XLarge - " + couleur;
            if (grandeur.ToUpper() == "5XL") return "5XLarge - " + couleur;
            if (grandeur.ToUpper() == "L") return "Large - " + couleur;
            if (grandeur.ToUpper() == "L/XL") return "Large/XLarge - " + couleur;
            if (grandeur.ToUpper() == "M") return "Medium - " + couleur;
            if (grandeur.ToUpper() == "M/L") return "Medium/Large - " + couleur;
            if (grandeur.ToUpper() == "OS") return "OS - " + couleur;
            if (grandeur.ToUpper() == "S") return "Petit - " + couleur;
            if (grandeur.ToUpper() == "S/M") return "Petit/Medium - " + couleur;
            if (grandeur.ToUpper() == "XL") return "XLarge - " + couleur;
            if (grandeur.ToUpper() == "XL/2XL") return "Xlarge/2Xlarge - " + couleur;
            if (grandeur.ToUpper() == "XS") return "XPetit - " + couleur;
            if (grandeur.ToUpper() == "YL") return "Y Large - " + couleur;
            if (grandeur.ToUpper() == "YM") return "Y Medium - " + couleur;
            if (grandeur.ToUpper() == "YS") return "Y Petit - " + couleur;
            if (grandeur.ToUpper() == "YXL") return "Y XLarge - " + couleur;
            if (grandeur.ToUpper() == "YXS") return "Y XPetit - " + couleur;
            return "";
        }

        private string TrouverCouleurFrancaisManquante(ImportProduit importProduit)
        {
            if (string.IsNullOrEmpty(importProduit.Color_FR))
            {
                if (importProduit.Color_EN == "Azelea") return "Azale";
                if (importProduit.Color_EN == "Azure") return "Azure";
                if (importProduit.Color_EN == "Black") return "Noir";
                if (importProduit.Color_EN == "Black Red") return "Noir rouge";
                if (importProduit.Color_EN == "Black Tartan") return "Noir tartan";
                if (importProduit.Color_EN == "Black/Gray") return "Gris noir";
                if (importProduit.Color_EN == "Black/Green") return "Vert noir";
                if (importProduit.Color_EN == "Black/White") return "Noir blanc";
                if (importProduit.Color_EN == "Brown/Khaki") return "Brun";
                if (importProduit.Color_EN == "Denim") return "Denim";
                if (importProduit.Color_EN == "GAMMA GREY/SAIL-MIDNIGHT TURQ") return "Gris";
                if (importProduit.Color_EN == "Gold") return "Or";
                if (importProduit.Color_EN == "Gravel") return "Gris";
                if (importProduit.Color_EN == "Green/Black") return "Vert noir";
                if (importProduit.Color_EN == "Grey") return "Gris";
                if (importProduit.Color_EN == "Heliconia") return "Heliconia";
                if (importProduit.Color_EN == "Light Blue/Navy") return "Bleu clair";
                if (importProduit.Color_EN == "Marble Black") return "Noir";
                if (importProduit.Color_EN == "Maroon Triblend") return "Marron";
                if (importProduit.Color_EN == "Navy") return "Navy";
                if (importProduit.Color_EN == "Orange") return "Orange";
                if (importProduit.Color_EN == "Red") return "Rouge";
                if (importProduit.Color_EN == "Royal") return "Royal";
                if (importProduit.Color_EN == "Safety Pink") return "Rose";
                if (importProduit.Color_EN == "Silver") return "Argent";
                if (importProduit.Color_EN == "Tobacco") return "Tabac";
                if (importProduit.Color_EN == "Tweed") return "Tweed";
                return importProduit.Color_EN;
            }
            return importProduit.Color_FR;
        }

        private string TrouverCategorieAnglaise(ImportProduit importProduit)
        {
            string categorieAnglaise = string.Empty;
            if (importProduit.Category1.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category1)) categorieAnglaise = importProduit.Category1;
            if (importProduit.Category2.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category2)) categorieAnglaise += "/" + importProduit.Category2;
            if (importProduit.Category3.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category3)) categorieAnglaise += "/" + importProduit.Category3;
            if (importProduit.Category4.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category4)) categorieAnglaise += "/" + importProduit.Category4;
            if (importProduit.Category5.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category5)) categorieAnglaise += "/" + importProduit.Category5;
            return categorieAnglaise;
        }
        private string TrouverCategorieFrancaise(ImportProduit importProduit)
        {
            string categorieFrancais = string.Empty;
            if (importProduit.Category1.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category1)) categorieFrancais = TraductionCategorie(importProduit.Category1);
            if (importProduit.Category2.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category2)) categorieFrancais += "/" + TraductionCategorie(importProduit.Category2);
            if (importProduit.Category3.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category3)) categorieFrancais += "/" + TraductionCategorie(importProduit.Category3);
            if (importProduit.Category4.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category4)) categorieFrancais += "/" + TraductionCategorie(importProduit.Category4);
            if (importProduit.Category5.ToLower() != "zcatalog" && !string.IsNullOrEmpty(importProduit.Category5)) categorieFrancais += "/" + TraductionCategorie(importProduit.Category5);
            return categorieFrancais;
        }
        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = new Random().NextDouble();
            return minValue + (next * (maxValue - minValue));
        }
        private string TraductionCategorie(string categorieAnglais)
        {

            if (categorieAnglais == "Fleece") return "Tissus";
            if (categorieAnglais == "Pants") return "Pantalon";
            if (categorieAnglais == "accessories") return "Accessoires";
            if (categorieAnglais == "BathRobes") return "Robe de chambre";
            if (categorieAnglais == "Shirts") return "Gilet";
            if (categorieAnglais == "Unisex") return "Unisexe";
            if (categorieAnglais == "Tshirts") return "Tshirts.";
            if (categorieAnglais == "LongSlv") return "Gilet long";
            if (categorieAnglais == "Polos") return "Polos.";
            if (categorieAnglais == "Shorts") return "Culotte courte";
            if (categorieAnglais == "Jackets") return "Manteau";
            if (categorieAnglais == "Caps") return "Casquette";
            if (categorieAnglais == "Bellacanvas") return "Bellacanvas.";
            if (categorieAnglais == "TankTop") return "TankTop.";
            if (categorieAnglais == "Headwears") return "Chapeau";
            if (categorieAnglais == "Hyp") return "Hyp.";
            if (categorieAnglais == "Knit") return "Tricot";
            if (categorieAnglais == "BathRobes-Unisex") return "F";
            if (categorieAnglais == "Dyenomite") return "Dyenomite.";
            if (categorieAnglais == "Outdoor") return "Extérieur";
            if (categorieAnglais == "Bags") return "Sac";
            if (categorieAnglais == "Oakley") return "Oakley.";
            if (categorieAnglais == "Horst") return "Horst.";
            if (categorieAnglais == "Valubag") return "Sac valeur";
            return "";
        }

    }
}
