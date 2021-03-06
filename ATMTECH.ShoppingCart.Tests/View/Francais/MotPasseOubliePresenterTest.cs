﻿using ATMTECH.ShoppingCart.Services.Interface.Francais;
using ATMTECH.ShoppingCart.Views.Francais;
using ATMTECH.ShoppingCart.Views.Interface.Francais;
using ATMTECH.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ATMTECH.ShoppingCart.Tests.View.Francais
{
    [TestClass]
    public class MotPasseOubliePresenterTest : BaseTest<MotPasseOubliePresenter>
    {
        public Mock<IMotPasseOubliePresenter> ViewMock
        {
            get { return ObtenirMock<IMotPasseOubliePresenter>(); }
        }

        [TestMethod]
        public void EnvoyerMotPasseOublie_DoitEnvoyerCourrielAvecMotPasse()
        {
            ViewMock.Setup(x => x.Courriel).Returns("test@test.com");
            InstanceTest.EnvoyerMotPasseOublie();

            ObtenirMock<IClientService>().Verify(x => x.EnvoyerMotPasseOublie("test@test.com"));
        }
    }
}