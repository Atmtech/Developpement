﻿using System.Collections.Generic;
using System.Linq;
using ATMTECH.Common.Utils.Web;
using ATMTECH.Entities;
using ATMTECH.ShoppingCart.DAO.Interface;
using ATMTECH.ShoppingCart.Entities;
using ATMTECH.ShoppingCart.Services.Base;
using ATMTECH.ShoppingCart.Services.Interface;
using ATMTECH.Web.Services.Base;
using ATMTECH.Web.Services.Interface;

namespace ATMTECH.ShoppingCart.Services
{
    public class ProductService : BaseService, IProductService
    {
        public IMessageService MessageService { get; set; }
        public IDAOProduct DAOProduct { get; set; }
        public IDAOProductCategory DAOProductCategory { get; set; }
        public IDAOProductPriceHistory DAOProductPriceHistory { get; set; }
        public IMailService MailService { get; set; }
        public IParameterService ParameterService { get; set; }
        public IDAOProductFile DAOProductFile { get; set; }

        public void UpdateProductPriceHistory(int idProduct, decimal priceBefore, decimal priceAfter)
        {
            Product product = GetProduct(idProduct);
            DAOProductPriceHistory.UpdateProductPriceHistory(product, priceBefore, priceAfter);
        }
        public bool GetProductAccessOrderable(Product product, int idUser)
        {
            return DAOProduct.GetProductAccessOrderable(product, idUser);
        }
        public Product GetProduct(int id)
        {
            Product product = DAOProduct.GetProduct(id);
            if (product == null)
            {
                MessageService.ThrowMessage(ErrorCode.SC_THIS_PRODUCT_NUMBER_DONT_EXIST);
            }
            else
            {
                if (product.Stocks.Count == 0)
                {
                    MailService.SendEmail(ParameterService.GetValue(Constant.ADMIN_MAIL), ParameterService.GetValue(Constant.ADMIN_MAIL),
                                     string.Format("Produit sans inventaire: {0}-{1}-{2}", Pages.RemoveHtmlTag(product.Ident), Pages.RemoveHtmlTag(product.NameFrench), Pages.RemoveHtmlTag(product.Enterprise.Name)),
                                     string.Format(ParameterService.GetValue(Constant.NO_STOCK_AVAILABLE), product.Ident, product.NameFrench, product.Enterprise.Name));
                }
            }

            return product;
        }
        public IList<Product> GetProductsWithoutLanguage(int idEnterprise)
        {
            return idEnterprise != 0 ? DAOProduct.GetProductsWithoutLanguage(idEnterprise) : null;
        }
        public IList<Product> GetProducts(int idEnterprise)
        {
            return idEnterprise != 0 ? DAOProduct.GetProducts(idEnterprise) : null;
        }
        public IList<Product> GetProductsSimple(int idEnterprise)
        {
            return idEnterprise != 0 ? DAOProduct.GetProductsSimple(idEnterprise) : null;
        }

        public IList<ProductFile> GetProductFile()
        {
            return DAOProductFile.GetAll();
        }

        public IList<Product> GetProductsWithoutStock(int idEnterprise)
        {
            return DAOProduct.GetProductsWithoutStock(idEnterprise);
        }

        public IList<Product> GetProducts(int idEnterprise, int idUser, string search)
        {
            return idEnterprise != 0 ? DAOProduct.GetProducts(idEnterprise, idUser, search) : null;
        }
        public IList<Product> GetProducts(int idEnterprise, int idProductCategory, int idUser)
        {
            return idEnterprise != 0 ? DAOProduct.GetProducts(idEnterprise, idProductCategory, idUser) : null;
        }
        public IList<Product> GetProducts(int idEnterprise, int idProductCategory)
        {
            return idEnterprise != 0 ? DAOProduct.GetProducts(idEnterprise, idProductCategory) : null;
        }
        public IList<ProductCategory> GetProductCategory(int idEnterprise)
        {
            return DAOProductCategory.GetProductCategoryFromEnterprise(idEnterprise);
        }
        public IList<ProductCategory> GetProductCategoryWithoutLanguage(int idEnterprise)
        {
            return DAOProductCategory.GetProductCategoryFromEnterpriseWithoutLanguage(idEnterprise);
        }
        public IList<ProductFile> GetProductFile(int idEnterprise)
        {
            IList<Product> products = DAOProduct.GetProducts(idEnterprise);
            return DAOProductFile.GetProductFile().Where(productFile => products.Count(x => x.Enterprise.Id == idEnterprise && x.Id == productFile.Product.Id) > 0).ToList();
        }
        public Product GetProductSimple(int id)
        {
            return DAOProduct.GetProductSimple(id);
        }
        public int GetProductCount(int idEnterprise)
        {
            return DAOProduct.GetProductCount(idEnterprise);
        }

        public void SaveProductFile(ProductFile productFile)
        {
            DAOProductFile.SaveProductFile(productFile);
        }
        public int Save(Product product)
        {
            return DAOProduct.Save(product);
        }

        public void DeleteProductFile(ProductFile productFile)
        {
            DAOProductFile.DeleteProductFile(productFile);
        }

        public IList<ProductFile> GetProductFile(File file)
        {
            return DAOProductFile.GetProductFile(file);
        }
        public IList<Product> GetAllActive()
        {
            return DAOProduct.GetAllActive();
        }

        public int SaveProductCategory(ProductCategory productCategory)
        {
            return DAOProductCategory.Save(productCategory);
        }

        public IList<Product> ObtenirListeProduitEnVente(int id)
        {
            return DAOProduct.ObtenirListeProduitEnVente(id);
        }
    }
}
