﻿using System.Text.RegularExpressions;
using ATMTECH.DAO.Interface;
using ATMTECH.Entities;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.Web.Services.Base;
using ATMTECH.Web.Services.Interface;

namespace ATMTECH.ShoppingCart.Services.Francais
{
    public class ValiderClientService : BaseService, IValiderClientService
    {
        public IMessageService MessageService { get; set; }
        public IDAOUser DAOUser { get; set; }



        public bool EstCourrielValide(Customer client)
        {
            const string matchEmailPattern =
               @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";
            if (client.User.Email != null)
            {
                if (!Regex.IsMatch(client.User.Email, matchEmailPattern))
                {
                    MessageService.ThrowMessage(ErrorCode.SC_INVALID_EMAIL);
                    return false;
                }
            }

            return true;
        }

        public bool EstClientExistant(Customer client)
        {
            User user = DAOUser.GetUser(client.User.Login);
            if (user != null)
            {
                MessageService.ThrowMessage(ErrorCode.SC_THIS_USER_ALREADY_EXIST);
                return true;
            }
            return false;
        }

        public bool EstNomUtilisateurValide(Customer client)
        {
            if (string.IsNullOrEmpty(client.User.Login))
            {
                MessageService.ThrowMessage(Web.Services.ErrorCode.ADM_CREATE_USER_MANDATORY);
                return false;
            }
            return true;
        }

        public bool EstMotPasseValide(Customer client)
        {
            if (string.IsNullOrEmpty(client.User.Password))
            {
                MessageService.ThrowMessage(Web.Services.ErrorCode.ADM_CREATE_USER_MANDATORY);
                return false;
            }
            return true;
        }

        public bool EstClientValide(Customer client)
        {
            if (EstCourrielValide(client) == false) return false;
            if (EstClientExistant(client) == false) return false;
            if (EstNomUtilisateurValide(client) == false) return false;
            if (EstMotPasseValide(client) == false) return false;
            return true;
        }


    }
}

