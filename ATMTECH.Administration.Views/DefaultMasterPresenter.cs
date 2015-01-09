﻿using System.Collections.Generic;
using ATMTECH.Administration.Views.Base;
using ATMTECH.Administration.Views.Interface;
using ATMTECH.DAO;
using ATMTECH.Entities;
using ATMTECH.Services;
using ATMTECH.Services.Interface;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Interface;
using ATMTECH.ShoppingCart.Services.Reports.DTO;

namespace ATMTECH.Administration.Views
{
    public class DefaultMasterPresenter : BaseAdministrationPresenter<IDefaultMasterPresenter>
    {
        
        public IReportService ReportService { get; set; }
        public ICustomerService CustomerService { get; set; }
        public IOrderService OrderService { get; set; }
        public IEnterpriseService EnterpriseService { get; set; }
        public IProductService ProductService { get; set; }
        public IStockService StockService { get; set; }

        public DefaultMasterPresenter(IDefaultMasterPresenter view)
            : base(view)
        {
        }


        public override void OnViewLoaded()
        {
            User user = AuthenticationService.AuthenticateUser;
            View.IsLogged = user != null && user.IsAdministrator;
            if (user != null)
                View.FullName = user.FirstNameLastName;
        }
        public void FillData(User user)
        {
            if (user != null)
            {
                View.FullName = user.FirstNameLastName;
                View.IsLogged = true;
                View.IsAdministrator = user.IsAdministrator;
            }
            else
            {
                View.IsLogged = false;
            }

        }
        public void SignIn(string homePage)
        {
            User user = AuthenticationService.SignIn(View.UserName, View.Password);
            if (user != null)
            {
                FillData(user);
                NavigationService.Redirect(homePage);
            }
        }
        public string InitialiserColonneRecherche()
        {
            string rtn = string.Empty;
            //rtn += Save<Country>();
            rtn += Save<City>();
            rtn += Save<User>();
            rtn += Save<Customer>();
            //rtn += Save<CustomerType>();
            rtn += Save<Enterprise>();
            rtn += Save<EnterpriseAddress>();
            //rtn += Save<EnterpriseEmail>();
            //rtn += Save<EnumOrderInformation>();
            //rtn += Save<GroupProduct>();
            //rtn += Save<Order>();
            //rtn += Save<OrderLine>();
            //rtn += Save<Product>();
            //rtn += Save<ProductCategory>();
            //rtn += Save<ProductFile>();
            //rtn += Save<File>();
            //rtn += Save<ProductPriceHistory>();
            //rtn += Save<Stock>();
            //rtn += Save<StockLink>();
            //rtn += Save<StockTemplate>();
            //rtn += Save<Supplier>();
            //rtn += Save<Taxes>();
            return rtn;
        }
        public void GenerateStockControlReport()
        {

            Dictionary<string, string> dictionnaire = new Dictionary<string, string>();
            ReportParameter reportParameter = new ReportParameter
            {
                Assembly = "ATMTECH.ShoppingCart.Services",
                PathToReportAssembly = "ATMTECH.ShoppingCart.Services.Reports.StockControl.rdlc",
                ReportFormat = ReportFormat.PDF,
                Parameters = dictionnaire
            };


            IList<StockControlReportLine> stockControlReportLines = OrderService.GetStockControlReport();

            reportParameter.AddDatasource("dsStockControl", stockControlReportLines);
            ReportService.SaveReport("StockControl.pdf", ReportService.GetReport(reportParameter));
        }
        public void SignOut(string homePage)
        {
            AuthenticationService.SignOut();
            NavigationService.Redirect(homePage);
        }
        public void Export()
        {

        }
        private string Save<TModel>()
        {


            switch (typeof(TModel).FullName)
            {
                case "ATMTECH.ShoppingCart.Entities.Enterprise":
                    foreach (Enterprise enterprise in EnterpriseService.GetAll())
                    {
                        EnterpriseService.Save(EnterpriseService.GetEnterprise(enterprise.Id));
                    }
                    break;
                case "ATMTECH.ShoppingCart.Entities.Order":
                    foreach (Order order in OrderService.GetAll())
                    {
                        OrderService.Save(OrderService.GetOrder(order.Id));
                    }
                    break;
                case "ATMTECH.ShoppingCart.Entities.Stock":
                    foreach (Stock stock in StockService.GetAllStock())
                    {
                        if (ProductService.GetProductSimple(stock.Product.Id) != null)
                        {
                            StockService.Save(StockService.GetStock(stock.Id));
                        }
                    }
                    break;
                default:
                    BaseDao<TModel, int> daoModel = new BaseDao<TModel, int>();
                    IList<TModel> model = daoModel.GetAll();
                    foreach (TModel model1 in model)
                    {
                        daoModel.Save(model1);
                    }
                    break;
            }



            return typeof(TModel).FullName + " Exécuté !!!<br>";
        }
    }
}

