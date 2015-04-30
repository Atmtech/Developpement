﻿using System.Collections.Generic;
using System.Linq;
using ATMTECH.ShoppingCart.DAO.Interface;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Francais;
using ATMTECH.ShoppingCart.Services.Interface;
using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.ShoppingCart.Views.Base;
using ATMTECH.ShoppingCart.Views.Interface.Francais;
using ATMTECH.Web.Services;

namespace ATMTECH.ShoppingCart.Views.Francais
{
    public class InformationClientPresenter : BaseShoppingCartPresenter<IInformationClientPresenter>
    {
        public InformationClientPresenter(IInformationClientPresenter view)
            : base(view)
        {
        }

        public IClientService ClientService { get; set; }
        public ICommandeService CommandeService { get; set; }
        public IAddressService AddressService { get; set; }
        public IDAOCity DAOCity { get; set; }
        public IDAOCountry DAOCountry { get; set; }
        public IEnvoiPostalService EnvoiPostalService { get; set; }

        public override void OnViewInitialized()
        {
            base.OnViewInitialized();
            AfficherListePays();
            AfficherInformationClient();
            AfficherCommandePasse();
        }
        public void AfficherListePays()
        {
            IList<Country> allCountries = DAOCountry.GetAllCountries().Where(x => x.Code == "CAN").ToList();
            View.ListePaysLivraison = allCountries;
            View.ListePaysFacturation = allCountries;
        }
        public void AfficherCommandePasse()
        {
            Customer customer = ClientService.ClientAuthentifie;
            if (customer != null)
            {
                View.ListeCommandePasse = CommandeService.ObtenirCommande(customer);
            }
            else
            {
                NavigationService.Redirect(Pages.Pages.DEFAULT);
            }
        }
        public void AfficherInformationClient()
        {
            Customer customer = ClientService.ClientAuthentifie;
            if (customer != null)
            {
                View.Prenom = customer.User.FirstName;
                View.Nom = customer.User.LastName;
                View.Courriel = customer.User.Email;
                View.MotPasse = customer.User.Password;
                View.MotPasseConfirmation = customer.User.Password;
                if (customer.ShippingAddress != null && customer.ShippingAddress.Id != 0)
                {
                    View.NoCiviqueLivraison = customer.ShippingAddress.No;
                    View.RueLivraison = customer.ShippingAddress.Way;
                    View.CodePostalLivraison = customer.ShippingAddress.PostalCode;
                    View.PaysLivraison = customer.ShippingAddress.Country.Id;
                    View.VilleLivraison = customer.ShippingAddress.City.Description;
                }
                else
                {
                    View.EstAucuneAdresseLivraison = true;
                }

                if (customer.BillingAddress != null && customer.BillingAddress.Id != 0)
                {
                    View.NoCiviqueFacturation = customer.BillingAddress.No;
                    View.RueFacturation = customer.BillingAddress.Way;
                    View.CodePostalFacturation = customer.BillingAddress.PostalCode;
                    View.PaysFacturation = customer.BillingAddress.Country.Id;
                    View.VilleFacturation = customer.BillingAddress.City.Description;
                }
                else
                {
                    View.EstAucuneAdresseFacturation = true;
                }
            }
            else
            {
                NavigationService.Redirect(Pages.Pages.DEFAULT);
            }
        }
        public void Enregistrer()
        {
            if (string.IsNullOrEmpty(View.Courriel))
            {
                MessageService.ThrowMessage(CodeErreur.ADM_CREATION_NOM_UTILISATEUR_OBLIGATOIRE);
                return;
            }

            if (string.IsNullOrEmpty(View.MotPasse))
            {
                MessageService.ThrowMessage(CodeErreur.ADM_CREATION_NOM_UTILISATEUR_OBLIGATOIRE);
                return;
            }

            if (View.MotPasse != View.MotPasseConfirmation)
            {
                MessageService.ThrowMessage(CodeErreur.SC_MOT_PASSE_INEGALE_AVEC_CONFIRMATION);
                return;
            }

            if (EnvoiPostalService.EstCodePostalValideAvecPurolator(View.CodePostalLivraison) == false)
            {
                MessageService.ThrowMessage(CodeErreur.SC_CODE_POSTAL_INVALIDE);
                return;
            }


            Customer customer = ClientService.ClientAuthentifie;
            customer.ShippingAddress = EnregistrerAdresse(customer.ShippingAddress, View.NoCiviqueLivraison,
                                                          View.RueLivraison, View.CodePostalLivraison,
                                                          View.VilleLivraison, View.PaysLivraison);
            customer.BillingAddress = EnregistrerAdresse(customer.BillingAddress, View.NoCiviqueFacturation,
                                                         View.RueFacturation, View.CodePostalFacturation,
                                                         View.VilleFacturation, View.PaysFacturation);
            customer.User.FirstName = View.Prenom;
            customer.User.LastName = View.Nom;
            customer.User.Email = View.Courriel;
            customer.User.Password = View.MotPasse;
            ClientService.Enregistrer(customer);
            MessageService.ThrowMessage(CodeErreur.ADM_ENREGISTRER_AVES_SUCCES);
        }
        public Address EnregistrerAdresse(Address adresse, string noCivique, string rue, string CodePostal, string ville,
                                          int pays)
        {




            if (adresse == null || adresse.Id == 0)
            {
                adresse = new Address
                    {
                        No = noCivique,
                        Way = rue,
                        PostalCode = CodePostal,
                        Country = new Country { Id = pays },
                        City = TrouverVille(ville)
                    };

                return AddressService.SaveNewAddress(adresse);
            }

            adresse.No = noCivique;
            adresse.Way = rue;
            adresse.PostalCode = CodePostal;
            adresse.Country = new Country { Id = pays };
            adresse.City = TrouverVille(ville);

            return AddressService.SaveAddress(adresse);
        }

        private City TrouverVille(string ville)
        {
            City city = DAOCity.FindCity(ville);
            if (city == null)
            {
                int id = DAOCity.CreateCity(new City { Code = ville, Description = ville });
                return DAOCity.GetCity(id);
            }
            return city;
        }
    }
}