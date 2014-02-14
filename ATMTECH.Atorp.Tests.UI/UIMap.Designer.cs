﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      Ce code a été généré par un générateur de test codé de l'interface utilisateur.
//      Version : 11.0.0.0 
//
//      Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//      le code est régénéré.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace ATMTECH.Atorp.Tests.UI
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "11.0.50727.1")]
    public partial class UIMap
    {
        
        /// <summary>
        /// FermerPage
        /// </summary>
        public void FermerPage()
        {
            #region Variable Declarations
            WinButton uIFermerButton = this.UIHttplocalhost1679MicWindow.UIHttplocalhost1679MicTitleBar.UIFermerButton;
            #endregion

            // Clic 'Fermer' bouton
            Mouse.Click(uIFermerButton, new Point(31, 4));
        }
        
        /// <summary>
        /// OuvrirPage - Utilisez 'OuvrirPageParams' pour passer les paramètres dans cette méthode.
        /// </summary>
        public void OuvrirPage()
        {

            // Atteindre la page Web 'http://localhost:1679/' en utilisant une nouvelle instance du navigateur
            BrowserWindow localhostBrowser = BrowserWindow.Launch(new System.Uri(this.OuvrirPageParams.Url));
        }
        
        #region Properties
        public virtual OuvrirPageParams OuvrirPageParams
        {
            get
            {
                if ((this.mOuvrirPageParams == null))
                {
                    this.mOuvrirPageParams = new OuvrirPageParams();
                }
                return this.mOuvrirPageParams;
            }
        }
        
        public UIHttplocalhost1679MicWindow UIHttplocalhost1679MicWindow
        {
            get
            {
                if ((this.mUIHttplocalhost1679MicWindow == null))
                {
                    this.mUIHttplocalhost1679MicWindow = new UIHttplocalhost1679MicWindow();
                }
                return this.mUIHttplocalhost1679MicWindow;
            }
        }
        #endregion
        
        #region Fields
        private OuvrirPageParams mOuvrirPageParams;
        
        private UIHttplocalhost1679MicWindow mUIHttplocalhost1679MicWindow;
        #endregion
    }
    
    /// <summary>
    /// Paramètres à passer dans 'OuvrirPage'
    /// </summary>
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "11.0.50727.1")]
    public class OuvrirPageParams
    {
        
        #region Fields
        /// <summary>
        /// Atteindre la page Web 'http://localhost:1679/' en utilisant une nouvelle instance du navigateur
        /// </summary>
        public string Url = "http://localhost:1679/";
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "11.0.50727.1")]
    public class UIHttplocalhost1679MicWindow : BrowserWindow
    {
        
        public UIHttplocalhost1679MicWindow()
        {
            #region Critères de recherche 
            this.SearchProperties[UITestControl.PropertyNames.Name] = "http://localhost:1679/";
            this.SearchProperties[UITestControl.PropertyNames.ClassName] = "IEFrame";
            this.WindowTitles.Add("http://localhost:1679/");
            #endregion
        }
        
        public void LaunchUrl(System.Uri url)
        {
            this.CopyFrom(BrowserWindow.Launch(url));
        }
        
        #region Properties
        public UIHttplocalhost1679Document UIHttplocalhost1679Document
        {
            get
            {
                if ((this.mUIHttplocalhost1679Document == null))
                {
                    this.mUIHttplocalhost1679Document = new UIHttplocalhost1679Document(this);
                }
                return this.mUIHttplocalhost1679Document;
            }
        }
        
        public UIHttplocalhost1679MicTitleBar UIHttplocalhost1679MicTitleBar
        {
            get
            {
                if ((this.mUIHttplocalhost1679MicTitleBar == null))
                {
                    this.mUIHttplocalhost1679MicTitleBar = new UIHttplocalhost1679MicTitleBar(this);
                }
                return this.mUIHttplocalhost1679MicTitleBar;
            }
        }
        #endregion
        
        #region Fields
        private UIHttplocalhost1679Document mUIHttplocalhost1679Document;
        
        private UIHttplocalhost1679MicTitleBar mUIHttplocalhost1679MicTitleBar;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "11.0.50727.1")]
    public class UIHttplocalhost1679Document : HtmlDocument
    {
        
        public UIHttplocalhost1679Document(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche 
            this.SearchProperties[HtmlDocument.PropertyNames.Id] = null;
            this.SearchProperties[HtmlDocument.PropertyNames.RedirectingPage] = "False";
            this.SearchProperties[HtmlDocument.PropertyNames.FrameDocument] = "False";
            this.FilterProperties[HtmlDocument.PropertyNames.Title] = null;
            this.FilterProperties[HtmlDocument.PropertyNames.AbsolutePath] = "/";
            this.FilterProperties[HtmlDocument.PropertyNames.PageUrl] = "http://localhost:1679/";
            this.WindowTitles.Add("http://localhost:1679/");
            #endregion
        }
        
        #region Properties
        public HtmlInputButton UITestButton
        {
            get
            {
                if ((this.mUITestButton == null))
                {
                    this.mUITestButton = new HtmlInputButton(this);
                    #region Critères de recherche 
                    this.mUITestButton.SearchProperties[HtmlButton.PropertyNames.Id] = "btnTest";
                    this.mUITestButton.SearchProperties[HtmlButton.PropertyNames.Name] = "btnTest";
                    this.mUITestButton.FilterProperties[HtmlButton.PropertyNames.DisplayText] = "Test";
                    this.mUITestButton.FilterProperties[HtmlButton.PropertyNames.Type] = "submit";
                    this.mUITestButton.FilterProperties[HtmlButton.PropertyNames.Title] = null;
                    this.mUITestButton.FilterProperties[HtmlButton.PropertyNames.Class] = null;
                    this.mUITestButton.FilterProperties[HtmlButton.PropertyNames.ControlDefinition] = "id=btnTest value=Test type=submit name=b";
                    this.mUITestButton.FilterProperties[HtmlButton.PropertyNames.TagInstance] = "3";
                    this.mUITestButton.WindowTitles.Add("http://localhost:1679/");
                    #endregion
                }
                return this.mUITestButton;
            }
        }
        
        public HtmlSpan UICrevetteTigréPane
        {
            get
            {
                if ((this.mUICrevetteTigréPane == null))
                {
                    this.mUICrevetteTigréPane = new HtmlSpan(this);
                    #region Critères de recherche 
                    this.mUICrevetteTigréPane.SearchProperties[HtmlDiv.PropertyNames.Id] = "lblCrevette";
                    this.mUICrevetteTigréPane.SearchProperties[HtmlDiv.PropertyNames.Name] = null;
                    this.mUICrevetteTigréPane.FilterProperties[HtmlDiv.PropertyNames.InnerText] = "Crevette Tigré";
                    this.mUICrevetteTigréPane.FilterProperties[HtmlDiv.PropertyNames.Title] = null;
                    this.mUICrevetteTigréPane.FilterProperties[HtmlDiv.PropertyNames.Class] = null;
                    this.mUICrevetteTigréPane.FilterProperties[HtmlDiv.PropertyNames.ControlDefinition] = "id=lblCrevette";
                    this.mUICrevetteTigréPane.FilterProperties[HtmlDiv.PropertyNames.TagInstance] = "1";
                    this.mUICrevetteTigréPane.WindowTitles.Add("http://localhost:1679/");
                    #endregion
                }
                return this.mUICrevetteTigréPane;
            }
        }
        
        public HtmlSpan UICrevettePane
        {
            get
            {
                if ((this.mUICrevettePane == null))
                {
                    this.mUICrevettePane = new HtmlSpan(this);
                    #region Critères de recherche 
                    this.mUICrevettePane.SearchProperties[HtmlDiv.PropertyNames.Id] = "lblCrevette";
                    this.mUICrevettePane.SearchProperties[HtmlDiv.PropertyNames.Name] = null;
                    this.mUICrevettePane.FilterProperties[HtmlDiv.PropertyNames.InnerText] = "Crevette";
                    this.mUICrevettePane.FilterProperties[HtmlDiv.PropertyNames.Title] = null;
                    this.mUICrevettePane.FilterProperties[HtmlDiv.PropertyNames.Class] = null;
                    this.mUICrevettePane.FilterProperties[HtmlDiv.PropertyNames.ControlDefinition] = "id=lblCrevette";
                    this.mUICrevettePane.FilterProperties[HtmlDiv.PropertyNames.TagInstance] = "1";
                    this.mUICrevettePane.WindowTitles.Add("http://localhost:1679/");
                    #endregion
                }
                return this.mUICrevettePane;
            }
        }
        #endregion
        
        #region Fields
        private HtmlInputButton mUITestButton;
        
        private HtmlSpan mUICrevetteTigréPane;
        
        private HtmlSpan mUICrevettePane;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "11.0.50727.1")]
    public class UIHttplocalhost1679MicTitleBar : WinTitleBar
    {
        
        public UIHttplocalhost1679MicTitleBar(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche 
            this.WindowTitles.Add("http://localhost:1679/");
            #endregion
        }
        
        #region Properties
        public WinButton UIFermerButton
        {
            get
            {
                if ((this.mUIFermerButton == null))
                {
                    this.mUIFermerButton = new WinButton(this);
                    #region Critères de recherche 
                    this.mUIFermerButton.SearchProperties[WinButton.PropertyNames.Name] = "Fermer";
                    this.mUIFermerButton.WindowTitles.Add("http://localhost:1679/");
                    #endregion
                }
                return this.mUIFermerButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUIFermerButton;
        #endregion
    }
}