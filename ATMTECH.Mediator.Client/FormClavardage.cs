﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ATMTECH.Mediator.Entities;

namespace ATMTECH.Mediator.Client
{
    public partial class FormClavardage : Form
    {
        private GestionPresentation _gestionPresentation;
        public GestionPresentation GestionPresentation { get { return _gestionPresentation ?? (_gestionPresentation = new GestionPresentation { FastColoredTextBox = fastColoredTextBoxClavardage }); } }

        private Boolean fastColoredTextBoxAutoScroll = true;
        FormWindowState LastWindowState = FormWindowState.Minimized;

        public FormClavardage()
        {
            InitializeComponent();
        }

        private void FormClavardage_Load(object sender, EventArgs e)
        {
            ActiveControl = textBoxClavardage;
            GestionPresentation.EnvoyerClavardage("/JOIN");
            timerClavardage.Enabled = true;
            GestionPresentation.AfficherClavardage();
        }

        private void FormClavardage_Activated(object sender, EventArgs e)
        {
            textBoxClavardage.Focus();
        }

        private void FormClavardage_FormClosed(object sender, FormClosedEventArgs e)
        {
            GestionPresentation.EnvoyerClavardage("/q");
        }

        private void FormClavardage_SizeChanged(object sender, EventArgs e)
        {
            if (fastColoredTextBoxAutoScroll == true) fastColoredTextBoxClavardage.GoEnd();
        }

        private void timerClavardage_Tick(object sender, EventArgs e)
        {
            IList<Clavardage> clavardages = GestionPresentation.AfficherClavardage();
            if (clavardages != null)
            {
                foreach (Clavardage clavardage in clavardages.Where(clavardage => !GestionPresentation.EstCommande(clavardage.Texte)))
                {
                    if (clavardage.NoUtilisateur != GestionPresentation.Utilisateur.NoUtilisateur)
                        FlashWindow.Flash(this, 3);
                }
            }

            if (fastColoredTextBoxAutoScroll == true) fastColoredTextBoxClavardage.GoEnd();
        }

        private void textBoxChat_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (string.IsNullOrWhiteSpace(textBoxClavardage.Text)) return;
            GestionPresentation.EnvoyerClavardage(textBoxClavardage.Text);
            textBoxClavardage.Text = "";
        }

        private void btnDernierClavardage_Click(object sender, EventArgs e)
        {
            GestionPresentation.AfficherClavardage(50);
        }

        private void fastColoredTextBoxClavardage_Click(object sender, EventArgs e)
        {
            fastColoredTextBoxAutoScroll = false;
        }

        private void fastColoredTextBoxClavardage_Scroll(object sender, ScrollEventArgs e)
        {
            fastColoredTextBoxClavardage.Focus();
            fastColoredTextBoxAutoScroll = false;
        }

        private void fastColoredTextBoxClavardage_Leave(object sender, EventArgs e)
        {
            fastColoredTextBoxAutoScroll = true;
        }

        private void FormClavardage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) WindowState = FormWindowState.Minimized;
        }

        private void fastColoredTextBoxClavardage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) WindowState = FormWindowState.Minimized;
        }

        private void textBoxClavardage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) WindowState = FormWindowState.Minimized;
        }

        private void btnDernierClavardage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) WindowState = FormWindowState.Minimized;
        }
<<<<<<< .mine
=======
>>>>>>> .theirs        private void FormClavardage_Resize(object sender, EventArgs e)
        {
            if (WindowState != LastWindowState)
            {
                LastWindowState = WindowState;
                
                if (WindowState == FormWindowState.Maximized)
                    textBoxClavardage.Focus();
                if (WindowState == FormWindowState.Normal)
                    textBoxClavardage.Focus();
            }

        }
<<<<<<< .mine=======            var p = fastColoredTextBoxClavardage.PointToPlace(e.Location);
            if (GestionPresentation.EstUnLien(p))
            {
                Process.Start(fastColoredTextBoxClavardage.GetRange(p, p).GetFragment(@"[\S]").Text);
            }
        }
>>>>>>> .theirs    }
}
