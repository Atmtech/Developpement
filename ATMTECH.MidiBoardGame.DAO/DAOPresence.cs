﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ATMTECH.MidiBoardGame.Entites;

namespace ATMTECH.MidiBoardGame.DAO
{
    public class DAOPresence : BaseDao
    {
        public IList<Presence> ObtenirListePresence()
        {

            DataSet dataSet = ObtenirDonneesMssql("SELECT Id, Utilisateur, Date FROM Presence");
            List<Presence> presences = (from DataRow dataRow in dataSet.Tables[0].Rows
                                        select
                                            new Presence
                                            {
                                                Id = Convert.ToInt32(dataRow["Id"].ToString()),
                                                Utilisateur = new Utilisateur { Id = Convert.ToInt32(dataRow["Utilisateur"].ToString()) },
                                                Date = Convert.ToDateTime(dataRow["Date"].ToString())
                                            }).ToList();

            foreach (Presence presence in presences)
            {
                presence.Utilisateur = new DAOUtilisateur().ObtenirUtilisateur(presence.Utilisateur.Id);
            }
            return presences;
        }

        public IList<Presence> ObtenirListePresenceAujourdhui()
        {
            return ObtenirListePresence().Where(x => x.Date == Convert.ToDateTime(DateTime.Now.ToShortDateString())).ToList();
        }

        public void Ajouter(Utilisateur utilisateur, DateTime date)
        {
            ExecuterSql(string.Format("IF NOT EXISTS(SELECT 1 FROM PRESENCE WHERE Utilisateur = {0} and Date ='{1}') INSERT INTO Presence (Utilisateur,Date) VALUES ({0}, '{1}')", utilisateur.Id, Utilitaires.Utilitaires.Aujourdhui()));
        }

        public void Retirer(int utilisateurId, DateTime aujourdhui, Utilisateur moi)
        {

            if (moi.Id == utilisateurId)
            {
                ExecuterSql(string.Format("IF EXISTS(SELECT 1 FROM PRESENCE WHERE Utilisateur = {0} and Date ='{1}') DELETE FROM Presence WHERE Utilisateur = {0} and Date = '{1}'",utilisateurId, Utilitaires.Utilitaires.Aujourdhui()));
                ExecuterSql(string.Format("DELETE FROM Proposition WHERE Utilisateur = {0} and Date = '{1}'",utilisateurId, Utilitaires.Utilitaires.Aujourdhui()));
                ExecuterSql("DELETE FROM PropositionVote WHERE Proposition not in (SELECT Proposition FROM Proposition)");
            }

        }
    }
}