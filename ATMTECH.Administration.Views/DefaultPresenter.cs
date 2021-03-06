﻿using ATMTECH.Administration.Views.Base;
using ATMTECH.Administration.Views.Interface;
using ATMTECH.Common;
using ATMTECH.Common.Utils;
using ATMTECH.Entities;

namespace ATMTECH.Administration.Views
{
    public class DefaultPresenter : BaseAdministrationPresenter<IDefaultPresenter>
    {
     
        public DefaultPresenter(IDefaultPresenter view)
            : base(view)
        {
        }


        public string Test()
        {
            string display = "tabarnak";

            MessageService.ThrowMessage(Web.Services.ErrorCode.ADM_BAD_LOGIN);

            //display += "Début";
            //User user = AuthenticationService.SignIn("riov01", "10crevette011");
            //if (user != null)
            //    display += "Pas vide";
            //display += "fin";
            return display;
        }
    }
}
