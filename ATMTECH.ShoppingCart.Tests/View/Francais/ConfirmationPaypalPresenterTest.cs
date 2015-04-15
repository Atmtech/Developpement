﻿using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Base;
using ATMTECH.ShoppingCart.Services.Interface;
using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.ShoppingCart.Views.Francais;
using ATMTECH.ShoppingCart.Views.Interface.Francais;
using ATMTECH.Test;
using ATMTECH.Web.Services;
using ATMTECH.Web.Services.Interface;
using ATMTECH.Web.Services.PaypalSandboxService;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace ATMTECH.ShoppingCart.Tests.View.Francais
{
    [TestClass]
    public class ConfirmationPaypalPresenterTest : BaseTest<ConfirmationPaypalPresenter>
    {
        public Mock<IConfirmationPaypalPresenter> ViewMock
        {
            get { return ObtenirMock<IConfirmationPaypalPresenter>(); }
        }

        [TestMethod]
        public void AfficherCommande_DoitSortirLeBonString()
        {
            PaypalReturn paypalReturn = new PaypalReturn();
            paypalReturn.ResponseDetails = new GetExpressCheckoutDetailsResponseDetailsType { InvoiceID = "1" };
            ViewMock.Setup(x => x.PaypalReturn).Returns(paypalReturn);
            ObtenirMock<IPaypalService>().Setup(x => x.GetReplyFromPaypal()).Returns(paypalReturn);
            ObtenirMock<ICommandeService>().Setup(x => x.AfficherCommande(1)).Returns("tata");
            InstanceTest.AfficherCommande();

            ViewMock.VerifySet(x => x.AffichageCommande = "tata");
        }

        [TestMethod]
        public void ImprimerCommande_ImprimerLaCommandeDeLaCommandeDePaypal()
        {
            PaypalReturn paypalReturn = new PaypalReturn();
            Order commande = AutoFixture.Create<Order>();
            paypalReturn.ResponseDetails = new GetExpressCheckoutDetailsResponseDetailsType { InvoiceID = "1" };
            ViewMock.Setup(x => x.PaypalReturn).Returns(paypalReturn);
            ObtenirMock<ICommandeService>().Setup(x => x.ObtenirCommande(1)).Returns(commande);
            InstanceTest.ImprimerCommande();

            ObtenirMock<ICommandeService>().Verify(x => x.ImprimerCommande(commande), Times.Once());
        }

        [TestMethod]
        public void FinaliserCommande_DoitFinaliserAvecCommandePaypalSiRetourVraiSurFinishDuService()
        {
            PaypalReturn paypalReturn = new PaypalReturn();
            Order commande = AutoFixture.Create<Order>();
            paypalReturn.ResponseDetails = new GetExpressCheckoutDetailsResponseDetailsType { InvoiceID = "1" };
            ViewMock.Setup(x => x.PaypalReturn).Returns(paypalReturn);
            ObtenirMock<ICommandeService>().Setup(x => x.ObtenirCommande(1)).Returns(commande);
            ObtenirMock<IPaypalService>().Setup(x => x.FinishPaypalTransaction(paypalReturn)).Returns(true);
            InstanceTest.FinaliserCommande();

            ObtenirMock<ICommandeService>().Verify(x => x.FinaliserCommande(commande), Times.Once());
            ViewMock.VerifySet(x => x.EstFinalise = true);
        }

        [TestMethod]
        public void FinaliserCommande_DoitAfficherErreurSiRetourFauxSurFinishDuService()
        {
            PaypalReturn paypalReturn = new PaypalReturn();
            Order commande = AutoFixture.Create<Order>();
            paypalReturn.ResponseDetails = new GetExpressCheckoutDetailsResponseDetailsType { InvoiceID = "1" };
            ViewMock.Setup(x => x.PaypalReturn).Returns(paypalReturn);
            ObtenirMock<ICommandeService>().Setup(x => x.ObtenirCommande(1)).Returns(commande);
            ObtenirMock<IPaypalService>().Setup(x => x.FinishPaypalTransaction(paypalReturn)).Returns(false);
            InstanceTest.FinaliserCommande();

            ObtenirMock<ICommandeService>().Verify(x => x.FinaliserCommande(commande), Times.Never());
            ViewMock.VerifySet(x => x.EstFinalise = false);
            ObtenirMock<IMessageService>().Verify(x => x.ThrowMessage(Web.Services.ErrorCode.ADM_PAYPAL_FINISH_FAILED), Times.Once());
        }

    }
}