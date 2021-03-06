﻿using ATMTECH.Views.Interface;

namespace ATMTECH.ShoppingCart.Views.Interface
{
    public interface ILoginPresenter : IViewBase
    {
        string FullName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool IsLogged { set; }
        bool IsAdministrator { set; }
        bool IsCreateCustomerPossible { set; }

        string FirstNameCreate { get; set; }
        string LastNameCreate { get; set; }
        string EmailCreate { get; set; }
        string PasswordCreate { get; set; }
        string PasswordConfirmation { get; set; }
    }
}
