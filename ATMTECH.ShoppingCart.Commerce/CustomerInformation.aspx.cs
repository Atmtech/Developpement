﻿using System;
using System.Collections.Generic;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Views.Francais;
using ATMTECH.ShoppingCart.Views.Interface.Francais;

namespace ATMTECH.ShoppingCart.Commerce
{
    public partial class CustomerInformation : PageBase<InformationClientPresenter, IInformationClientPresenter>, IInformationClientPresenter
    {
        public string Nom { get { return txtNom.Text; } set { txtNom.Text = value; } }
        public string Prenom { get { return txtPrenom.Text; } set { txtPrenom.Text = value; } }
        public string Courriel { get { return txtCourriel.Text; } set { txtCourriel.Text = value; } }
        public string MotPasse { get { return txtMotDePasse.Text; } set { txtMotDePasse.Text = value; } }
        public string MotPasseConfirmation { get { return txtMotDePasseConfirmation.Text; } set { txtMotDePasseConfirmation.Text = value; } }
        public string NoCiviqueLivraison { get { return txtNoCiviqueLivraisonClient.Text; } set { txtNoCiviqueLivraisonClient.Text = value; } }
        public string RueLivraison { get { return txtRueLivraisonClient.Text; } set { txtRueLivraisonClient.Text = value; } }
        public string CodePostalLivraison { get { return txtCodePostalLivraisonInformationClient.Text; } set { txtCodePostalLivraisonInformationClient.Text = value; } }
        public string VilleLivraison { get { return txtVilleLivraisonClient.Text; } set { txtVilleLivraisonClient.Text = value; } }
        public int PaysLivraison { get { return Convert.ToInt32(ddlPaysLivraisonClient.SelectedValue); } set { ddlPaysLivraisonClient.SelectedValue = value.ToString(); } }
        public string NoCiviqueFacturation { get { return txtNoCiviqueFacturationClient.Text; } set { txtNoCiviqueFacturationClient.Text = value; } }
        public string RueFacturation { get { return txtRueFacturationClient.Text; } set { txtRueFacturationClient.Text = value; } }
        public string CodePostalFacturation { get { return txtCodePostalFacturationClient.Text; } set { txtCodePostalFacturationClient.Text = value; } }
        public string VilleFacturation { get { return txtVilleFacturationClient.Text; } set { txtVilleFacturationClient.Text = value; } }
        public int PaysFacturation { get { return Convert.ToInt32(ddlPaysFacturationClient.SelectedValue); } set { ddlPaysFacturationClient.SelectedValue = value.ToString(); } }
        public bool EstAucuneAdresseLivraison { get; set; }
        public bool EstAucuneAdresseFacturation { get; set; }
        public IList<Country> ListePaysLivraison { set { FillDropDown(ddlPaysLivraisonClient, value); } }
        public IList<Country> ListePaysFacturation { set { FillDropDown(ddlPaysFacturationClient, value); } }
        public IList<Order> ListeCommandePasse { set; private get; }

        protected void btnEnregistrerInformationClientClick(object sender, EventArgs e)
        {
            Presenter.Enregistrer();
        }

        protected void btnUtiliserMemeAdresseQueLivraisonClick(object sender, EventArgs e)
        {
            txtNoCiviqueFacturationClient.Text = txtNoCiviqueLivraisonClient.Text;
            txtRueFacturationClient.Text = txtRueLivraisonClient.Text;
            txtVilleFacturationClient.Text = txtVilleLivraisonClient.Text;
            txtCodePostalFacturationClient.Text = txtCodePostalLivraisonInformationClient.Text;
            ddlPaysFacturationClient.SelectedValue = ddlPaysLivraisonClient.SelectedValue;
        }
    }
}