﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ATMTECH.Entities;
using ATMTECH.ShoppingCart.Views.Base;
using ATMTECH.Views.Interface;
using ATMTECH.Web;
using ATMTECH.WebControls;

namespace ATMTECH.ShoppingCart.Commerce
{
    public class PageBase<TPresenter, TView> : PageBase
        where TView : class, IViewBase
        where TPresenter : BaseShoppingCartPresenter<TView>
    {
        public TPresenter Presenter { get; set; }
        public void ShowMessage(Message message)
        {
            Session["MessageEnvoye"] = message;
            Response.Redirect("Error.aspx");
        }
        public void FillDropDown(DropDownList dropDownList, object Source)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataTextField = BaseEntity.COMBOBOX_DESCRIPTION;
            dropDownList.DataValueField = BaseEntity.ID;
            dropDownList.DataBind();
        }
        public void FillDropDown(ComboBox dropDownList, object Source)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataTextField = BaseEntity.COMBOBOX_DESCRIPTION;
            dropDownList.DataValueField = BaseEntity.ID;
            dropDownList.DataBind();
        }
        public void FillDropDown(ComboBox dropDownList, object Source, string DisplayText)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataTextField = DisplayText;
            dropDownList.DataValueField = BaseEntity.ID;
            dropDownList.DataBind();
        }
        public void FillDropDownWithoutEntity(ComboBox dropDownList, object Source, string DisplayText, string value)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataTextField = DisplayText;
            dropDownList.DataValueField = value;
            dropDownList.DataBind();
        }
        public void FillDropDownWithoutEntity(DropDownList dropDownList, object Source, string DisplayText, string value)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataTextField = DisplayText;
            dropDownList.DataValueField = value;
            dropDownList.DataBind();
        }
        public void FillDropDownWithoutEntity(ComboBox dropDownList, object Source)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataBind();
        }
        public void FillDropDownWithoutEntity(DropDownList dropDownList, object Source)
        {
            dropDownList.DataSource = Source;
            dropDownList.DataBind();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                List<Control> allControls = new List<Control>();
                GetControlList(Page.Controls, allControls);
                Presenter.Controls = allControls;
                Presenter.Localize();
                Presenter.AjouterPageFilAriane();
                string titrePage = Presenter.ObtenirTitrePage();
                Title = titrePage == "N/A" ? "" : titrePage;
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            ResetErrorMessage();
        }

        private void ResetErrorMessage()
        {
            if (Master != null)
            {
                Panel panel = (Panel)Master.FindControl("pnlError");
                panel.Visible = false;
            }
        }
        private void GetControlList<T>(ControlCollection controlCollection, List<T> resultCollection) where T : Control
        {
            foreach (Control control in controlCollection)
            {
                if (control is T)
                    resultCollection.Add((T)control);

                if (control.HasControls())
                    GetControlList(control.Controls, resultCollection);
            }
        }
    }
}