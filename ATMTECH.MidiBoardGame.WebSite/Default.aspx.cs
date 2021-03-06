﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATMTECH.MidiBoardGame.DAO;
using ATMTECH.MidiBoardGame.Entites;

namespace ATMTECH.MidiBoardGame.WebSite
{
    public partial class Default1 : Page
    {

        public Utilisateur Utilisateur
        {
            get
            {
                return (Utilisateur)Session["Utilisateur"];
            }
            set { Session["Utilisateur"] = value; }
        }
        public void RemplirListeDeroulante(DropDownList dropDownList, object Source, string colonneAffichage)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataTextField = colonneAffichage;
            dropDownList.DataValueField = "Id";
            dropDownList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RemplirListeDeroulante(ddlJeu, new DAOJeu().ObtenirListeJeuAvecPresence().OrderBy(x => x.Nom), "Nom");
                datalisteVote.DataSource = new DAOProposition().ObtenirListeProposition();
                datalisteVote.DataBind();

                datalisteVoteSansVote.DataSource = new DAOProposition().ObtenirListeProposition();
                datalisteVoteSansVote.DataBind();

                datalistePresence.DataSource = new DAOPresence().ObtenirListePresenceAujourdhui();
                datalistePresence.DataBind();


                TimeSpan heure = new TimeSpan(11, 45, 0);

                TimeSpan maintenant = DateTime.Now.TimeOfDay;



                if (maintenant >= heure)
                {
                    pnlBloque1145message.Visible = true;
                    btnPresence.Visible = false;
                    btnAjouterJeuMidi.Visible = false;
                    datalisteVote.Visible = false;
                    datalisteVoteSansVote.Visible = true;
                }
            }

            if (Session["Utilisateur"] != null)
            {
                lblNomUtilisateur.Text = Utilisateur.Nom;
            }
            else
            {
                Response.Redirect("Identification.aspx");
            }
        }
        protected void btnDeconnecterClick(object sender, EventArgs e)
        {
            Utilisateur = null;
            Response.Redirect("Default.aspx");
        }
        protected void btnPresenceClick(object sender, EventArgs e)
        {

            new DAOPresence().Ajouter(Utilisateur, Utilitaires.Utilitaires.Aujourdhui());
            Response.Redirect("Default.aspx");

        }
        protected void btnAjouterJeuMidiClick(object sender, EventArgs e)
        {
            new DAOProposition().Ajouter(ddlJeu.SelectedValue, Utilisateur.Id.ToString());
            Response.Redirect("Default.aspx");
        }
        protected void btnMonProfileClick(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }
        protected void datalisteVoteItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string id = ((Label)e.Item.FindControl("lblId")).Text;

            if (e.CommandName == "Vote")
            {
                new DAOPropositionVote().Ajouter(id, Utilisateur.Id.ToString());
            }
            if (e.CommandName == "Retirer")
            {
                new DAOPropositionVote().Retirer(id, Utilisateur.Id.ToString());
            }

            if (e.CommandName == "Supprimer")
            {
                new DAOPropositionVote().Supprimer(id, Utilisateur.Id.ToString());
            }
            Response.Redirect("Default.aspx");
        }
        protected void datalisteVoteItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Proposition proposition = (Proposition)e.Item.DataItem;
            Label lblNombreVote = (Label)e.Item.FindControl("lblNombreVote");
            lblNombreVote.Text = new DAOPropositionVote().ObtenirNombreVote(proposition.Id.ToString()).ToString();
            Label lblImageGravatar = (Label)e.Item.FindControl("lblImageGravatar");
            if (lblImageGravatar != null)
            {
                IList<Utilisateur> obtenirVoteur = new DAOPropositionVote().ObtenirVoteur(proposition.Id.ToString());
                foreach (Utilisateur utilisateur in obtenirVoteur)
                {
                    if (!string.IsNullOrEmpty(utilisateur.Gravatar))
                    {
                        lblImageGravatar.Text += "<img src='" + Utilitaires.Utilitaires.ObtenirImageGravatar(utilisateur.Gravatar) + "' Title='" + utilisateur.Nom + "' style='width:25px;height:25px;'> | ";
                    }
                    else
                    {
                        lblImageGravatar.Text += "<img src='/Images/user.png' Title='" + utilisateur.Nom + "' style='width:25px;height:25px;'> | ";
                    }
                }


            }


        }
        protected void datalistePresenceItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Retirer")
            {
                new DAOPresence().Retirer(Convert.ToInt32(e.CommandArgument), Utilitaires.Utilitaires.Aujourdhui(), Utilisateur);
                Response.Redirect("Default.aspx");
            }
        }
        protected void datalistePresenceDatabound(object sender, RepeaterItemEventArgs e)
        {
            Presence presence = (Presence)e.Item.DataItem;
            Label lblImage = (Label)e.Item.FindControl("lblImage");

            if (!string.IsNullOrEmpty(presence.Utilisateur.Gravatar))
            {
                lblImage.Text += "<img src='" + Utilitaires.Utilitaires.ObtenirImageGravatar(presence.Utilisateur.Gravatar) + "' Title='" + presence.Utilisateur.Nom + "' style='width:25px;height:25px;'> ";
            }
            else
            {
                lblImage.Text += "<img src='/Images/user.png' Title='" + presence.Utilisateur.Nom + "' style='width:25px;height:25px;'> ";
            }

        }
    }
}