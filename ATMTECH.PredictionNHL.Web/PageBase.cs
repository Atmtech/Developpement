﻿using ATMTECH.PredictionNHL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATMTECH.PredictionNHL.Web
{
    public class PageBase : Page
    {
        public Site PageMaitre => Page.Master as Site;
        
        public UtilisateurService UtilisateurService => new UtilisateurService();
        public NhlService NhlService => new NhlService();
        public PredictionService PredictionService => new PredictionService();
        public ClassementService ClassementService => new ClassementService();



        public void MettreBlancTextBox()
        {
            GetAllControls().Where(x => x is TextBox).ToList().ForEach(z => (z as TextBox).Text = string.Empty);
        }

        public void VerifierAcces()
        {
            if (PageMaitre.UtilisateurAuthentifie == null)
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                if (!url.Contains("Default.aspx") && !url.Contains("Identification.aspx"))
                    Response.Redirect("Default.aspx");
            }
        }

        public IEnumerable<Control> GetAllControls()
        {
            var allControls = new List<Control>();
            var currentControls = new Queue<Control>();
            currentControls.Enqueue(Page);

            while (currentControls.Count > 0)
            {
                var c = currentControls.Dequeue();
                if (!allControls.Contains(c))
                {
                    allControls.Add(c);
                    if (c.Controls != null && c.Controls.Count > 0)
                    {
                        foreach (Control e in c.Controls)
                        {
                            currentControls.Enqueue(e);
                        }
                    }
                }
            }
            return allControls;
        }

    }
}