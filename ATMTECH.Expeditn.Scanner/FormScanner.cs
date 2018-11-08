﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ATMTECH.Expeditn.Entites;
using ATMTECH.Expeditn.Services;

namespace ATMTECH.Expeditn.Scanner
{
    public partial class FormScanner : Form
    {
        
        public FormScanner()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
          Scan();
        }

        public void Scan()
        {
            IList<SuiviPrix> obtenirPlanificationScan = new SuiviPrixService().ObtenirsuiviPrix();
            foreach (SuiviPrix planificationScan in obtenirPlanificationScan)
            {
                if (planificationScan.TypeSuiviPrix.ToLower() == "expedia")
                {
                    DateTime dateAvantDateTime = DateTime.Now;
                    new SuiviPrixService().ScanExpedia(planificationScan);
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = dateAvantDateTime.ToString();
                    listViewItem.SubItems.Add(DateTime.Now.ToString());
                    listViewItem.SubItems.Add(planificationScan.Utilisateur.Affichage);
                    listViewItem.SubItems.Add(planificationScan.TypeSuiviPrix);
                    listViewItem.SubItems.Add(planificationScan.UrlSuiviPrix);
                    lsvScan.Items.Add(listViewItem);
                }
            }
        }

        private void FormScanner_Load(object sender, EventArgs e)
        {
            CeduleurService.Instance.CeduleTache(9,38, 24, Scan);
        }
    }
}