﻿using System.Collections.Generic;
using ATMTECH.Entities;
using ATMTECH.ShoppingCart.Entities;

namespace ATMTECH.ShoppingCart.DAO.Interface
{
    public interface IDAOProductFile
    {
        IList<ProductFile> GetProductFile(int idProduct);
        IList<ProductFile> GetProductFile();
        int SaveProductFile(ProductFile productFile);
        IList<ProductFile> GetAll();
        void DeleteProductFile(ProductFile productFile);
        IList<ProductFile> GetProductFile(File file);

    }
}
