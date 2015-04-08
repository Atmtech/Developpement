﻿using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ATMTECH.Common.Constant;
using ATMTECH.ShoppingCart.Entities;

namespace ATMTECH.ShoppingCart.Commerce
{
    public partial class SlideShowProduct : System.Web.UI.UserControl
    {
        public string Langue { get; set; }
        public IList<Product> Produits { get; set; }

        private string ImageDeFondAleatoire()
        {
            Random rnd = new Random();
            int numeroImage = rnd.Next(1, 4);
            switch (numeroImage)
            {
                case 1: return "purple.jpg";
                case 2: return "red.jpg";
                case 3: return "blue.jpg";
            }

            return "blue.jpg";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Produits == null) return;
            foreach (Product produit in Produits)
            {
                string html = string.Empty;
                html += "<div>";
                html += string.Format("<img u='image' src='JQuery/jquery-ui-1.11.2/img/SlideShow/{0}' />", ImageDeFondAleatoire());
                html += "<div style='position: absolute; width: 1000px; height: 120px; top: 0px;'>";
                html += "<div style='float: left; width: 670px; padding-right: 10px; padding-top: 80px; padding-left: 10px;'>";
                html += "<div style='font-weight: bold; font-size: 25px; padding-bottom: 10px; font-weight: bold; border-bottom: solid 1px white; margin-bottom: 10px;'>{0}</div>";
                html += "{1}";
                html += "<br /><br />";
                html += "<div style='font-weight: bold; font-size: 20px;'><a href='AddProductToBasket.aspx?ProductId={2}'>{7}</a></div>";

                if (produit.SalePrice != 0)
                {
                    html += "<div style='margin-top: 40px;width:120px;height:120px;border-radius:50%;font-size:15px;color:#fff;text-align:center;background:#23295a;border:solid 5px white;'>" +
                            "<div style='padding-top: 25px;'>{5} <div style='font-size: 25px;'>{4} %</div>{6}</div></div>";
                    
                }
                html += "</div>";
                html += "<div style='float: left; width: 300px;'>";
                html += "<img src='{3}' style='width: 311px; height: 500px;' />";
                html += "</div>";
                html += "<div style='clear: both;'></div>";
                html += "</div>";
                html += "</div>";

                Literal literal = new Literal();
                switch (Langue)
                {
                    case LocalizationLanguage.FRENCH:
                        literal.Text = string.Format(html, produit.NameFrench, produit.DescriptionFrench, produit.Id, produit.PrincipalFileUrl, produit.PercentageSave, "Épargner", "Maintenant", "Pour épargner acheter dès maintenant !!");
                        break;
                    case LocalizationLanguage.ENGLISH:
                        literal.Text = string.Format(html, produit.NameEnglish, produit.DescriptionEnglish, produit.Id, produit.PrincipalFileUrl, produit.PercentageSave, "Save", "Now", "To save buy it now !!");
                        break;
                }

                placeHolder.Controls.Add(literal);
            }

        }
    }
}