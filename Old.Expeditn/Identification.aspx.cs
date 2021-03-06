﻿using System;
using ATMTECH.Expeditn.Views;
using ATMTECH.Expeditn.Views.Interface;

namespace ATMTECH.Expeditn.WebSite
{
    public partial class Identification : PageBase<IdentificationPresenter, IIdentificationPresenter>, IIdentificationPresenter
    {
        public string NomUtilisateurIdentification { get { return txtCourriel.Text; } set { txtCourriel.Text = value; } }
        public string MotPasseIdentification { get { return txtMotDePasse.Text; } set { txtMotDePasse.Text = value; } }
        public string PrenomCreation { get { return txtPrenom.Text; } set { txtPrenom.Text = value; } }
        public string NomCreation { get { return txtNom.Text; } set { txtNom.Text = value; } }
        public string CourrielCreation { get { return txtCourrielCreer.Text; } set { txtCourrielCreer.Text = value; } }
        public string MotPasseCreation { get { return txtMotDePasseCreer.Text; } set { txtMotDePasseCreer.Text = value; } }
        public string MotPasseConfirmationCreation { get { return txtMotDePasseCreerConfirmation.Text; } set { txtMotDePasseCreerConfirmation.Text = value; } }

        protected void btnConnecterLoginClick(object sender, EventArgs e)
        {
            Presenter.Identification();
        }
        protected void btnCreerLoginClick(object sender, EventArgs e)
        {
            Presenter.CreerUtilisateur();

            PrenomCreation = "";
            NomCreation = "";
            CourrielCreation = "";
            MotPasseCreation = "";
            MotPasseConfirmationCreation = "";

        }
        protected void btnOublieMotDePasseClick(object sender, EventArgs e)
        {
            Presenter.NavigationService.Redirect(Pages.MOT_PASSE_OUBLIE);
        }
    }
}