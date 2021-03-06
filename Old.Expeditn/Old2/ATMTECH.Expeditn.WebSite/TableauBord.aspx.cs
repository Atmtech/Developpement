﻿using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ATMTECH.Entities;
using ATMTECH.Expeditn.Entities;
using ATMTECH.Expeditn.Views;
using ATMTECH.Expeditn.Views.Interface;

namespace ATMTECH.Expeditn.WebSite
{
    public partial class TableauBord : PageBase<TableauBordPresenter, ITableauBordPresenter>, ITableauBordPresenter
    {
        public IList<Expedition> MesExpeditions
        {
            set
            {
                if (value.Count > 0)
                {
                    listeMesExpeditions.DataSource = value;
                    listeMesExpeditions.DataBind();
                }

            }
        }

        public IList<RechercheForfaitExpedia> MesRechercheForfaitExpedias
        {
            set
            {
                if (value.Count > 0)
                {
                    listeMesSuiviPrix.DataSource = value;
                    listeMesSuiviPrix.DataBind();
                }
            }
        }

        public User Utilisateur { get; set; }

        protected void lnkAjouterUneExpeditionClick(object sender, EventArgs e)
        {
            Presenter.AjouterExpedition();
        }

        protected void listeMesExpeditionsItemCommand(object source, DataListCommandEventArgs e)
        {

            int idExpedition = Convert.ToInt32(((Label)e.Item.FindControl("lblIdExpedition")).Text);

            if (e.CommandName == "modifierExpedition")
            {
                Presenter.ModifierExpedition(idExpedition);
            }

            if (e.CommandName == "modifierParticipant")
            {
                Presenter.ModifierParticipant(idExpedition);
            }
            if (e.CommandName == "modifierEtape")
            {
                Presenter.ModifierEtape(idExpedition);
            }
            if (e.CommandName == "modifierMenu")
            {
                Presenter.ModifierMenu(idExpedition);
            }

            if (e.CommandName == "modifierRepartitionBudget")
            {
                Presenter.ModifierRepartitionBudget(idExpedition);
            }


        }

        protected void listeMesSuiviPrixItemCommand(object source, DataListCommandEventArgs e)
        {
            int idRechercheForfaitExpedia = Convert.ToInt32(((Label)e.Item.FindControl("lblIdRechercheForfaitExpedia")).Text);

            if (e.CommandName == "voirListePrix")
            {
                Presenter.VoirListePrix(idRechercheForfaitExpedia);
            }
            if (e.CommandName == "supprimerSuiviPrix")
            {
                Presenter.SupprimerSuiviPrix(idRechercheForfaitExpedia);
            }

        }

        protected void btnAjouterUnSuiviDePrixClick(object sender, EventArgs e)
        {
            Presenter.AjouterRechercheForfaitExpedia();
        }
    }
}