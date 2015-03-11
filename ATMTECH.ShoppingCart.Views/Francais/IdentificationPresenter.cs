﻿using System;
using ATMTECH.Entities;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Base;
using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.ShoppingCart.Views.Base;
using ATMTECH.ShoppingCart.Views.Interface.Francais;
using ATMTECH.Web.Services;
using ATMTECH.Web.Services.Interface;

namespace ATMTECH.ShoppingCart.Views.Francais
{
    public class IdentificationPresenter : BaseShoppingCartPresenter<IIdentificationPresenter>
    {
        public IdentificationPresenter(IIdentificationPresenter view)
            : base(view)
        {
        }

        public IAuthenticationService AuthenticationService { get; set; }
        public IClientService ClientService { get; set; }

        public void Identification()
        {
            User user = AuthenticationService.SignIn(View.NomUtilisateurIdentification, View.MotPasseIdentification);
            if (user != null)
            {
                NavigationService.Redirect(Pages.Pages.DEFAULT);
            }
        }

        public void CreerUtilisateur()
        {
            if (View.MotPasseCreation != View.MotPasseConfirmationCreation)
            {
                MessageService.ThrowMessage(Services.ErrorCode.SC_PASSWORD_DONT_EQUAL_PASSWORD_CONFIRM);
            }

            User user = new User
                {
                    Email = View.CourrielCreation,
                    Login = View.CourrielCreation,
                    Password = View.MotPasseCreation,
                    FirstName = View.PrenomCreation,
                    LastName = View.NomCreation
                };
            Customer customer = new Customer
                {
                    User = user,
                    Enterprise = new Enterprise { Id = Convert.ToInt32(ParameterService.GetValue(Constant.ID_ENTERPRISE_WHEN_NOT_AUTHENTIFIED)) }
                };

            if (ClientService.Creer(customer) != null)
            {
                MessageService.ThrowMessage(ErrorCode.ADM_CREATE_SUCCESS);
            }
        }


    }
}