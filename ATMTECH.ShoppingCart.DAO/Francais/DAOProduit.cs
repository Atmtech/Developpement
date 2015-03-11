﻿using System.Collections.Generic;
using ATMTECH.DAO;
using ATMTECH.DAO.Database;
using ATMTECH.Entities;
using ATMTECH.ShoppingCart.DAO.Interface.Francais;
using ATMTECH.ShoppingCart.Entities;

namespace ATMTECH.ShoppingCart.DAO.Francais
{
    public class DAOProduit : BaseDao<Product, int>, IDAOProduit
    {
        public IDAOProduitFichier DAOProduitFichier { get; set; }
        public IDAOInventaire DAOInventaire { get; set; }

        public IList<Product> ObtenirListeProduitEnVente(int id)
        {
            IList<Criteria> criterias = new List<Criteria>();
            Criteria criteriaEnterprise = new Criteria { Column = Product.ENTERPRISE, Operator = DatabaseOperator.OPERATOR_EQUAL, Value = id.ToString() };
            Criteria criteriaSaleNotNull = new Criteria { Column = Product.SALE_PRICE, Operator = DatabaseOperator.OPERATOR_IS_NOT_NULL };
            criterias.Add(criteriaEnterprise);
            criterias.Add(criteriaSaleNotNull);
            criterias.Add(IsActive());
            OrderOperation orderOperation = new OrderOperation { OrderByColumn = BaseEntity.ORDER_ID, OrderByType = OrderBy.Type.Descending };
            PagingOperation pagingOperation = new PagingOperation { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            return GetByCriteria(criterias, pagingOperation, orderOperation);
        }

        public Product ObtenirProduit(int id)
        {
            Product product = GetById(id);
            product.ProductFiles = DAOProduitFichier.ObtenirListeFichier(product.Id);
            product.Stocks = DAOInventaire.ObtenirInventaire(product);
            return product;
        }

        public IList<Product> ObtenirProduit(string recherche)
        {
            IList<Criteria> criterias = new List<Criteria>();
            Criteria criteriaRecherche = new Criteria { Column = BaseEntity.SEARCH, Operator = DatabaseOperator.OPERATOR_LIKE, Value = recherche };
            criterias.Add(criteriaRecherche);
            criterias.Add(IsActive());
            OrderOperation orderOperation = new OrderOperation { OrderByColumn = BaseEntity.ORDER_ID, OrderByType = OrderBy.Type.Descending };
            PagingOperation pagingOperation = new PagingOperation { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            return GetByCriteria(criterias, pagingOperation, orderOperation);
        }

        public IList<Product> ObtenirProduit()
        {
            IList<Criteria> criterias = new List<Criteria>();
            criterias.Add(IsActive());
            OrderOperation orderOperation = new OrderOperation { OrderByColumn = BaseEntity.ORDER_ID, OrderByType = OrderBy.Type.Descending };
            PagingOperation pagingOperation = new PagingOperation { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            return GetByCriteria(criterias, pagingOperation, orderOperation);
        }
    }
}