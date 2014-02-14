﻿using System;
using System.Collections.Generic;
using ATMTECH.Entities;

namespace ATMTECH.ShoppingCart.Entities
{
    [Serializable]
    public partial class Stock : BaseEntity
    {
        public const string PRODUCT = "Product";
        public const string FEATURE = "Feature";

        public Product Product { get; set; }
        public int InitialState { get; set; }
        public int MinimumAccept { get; set; }
        public bool IsWarningOnLow { get; set; }
        public string Feature { get; set; }
        public decimal AdjustPrice { get; set; }
        public IList<StockTransaction> Transactions { get; set; }
        public bool IsWithoutStock { get; set; }

        public  string SearchUpdate { get { return Product.Name + " " + Feature + " " + Product.Ident; } }
        public string ComboboxDescriptionUpdate { get { return Product.Name + " " + Feature + " " + Product.Ident; } }
    }
}