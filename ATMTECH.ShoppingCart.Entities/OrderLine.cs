﻿using System;
using ATMTECH.Entities;

namespace ATMTECH.ShoppingCart.Entities
{
    [Serializable]
    public partial class OrderLine : BaseEntity
    {
        public const string ORDERS = "Order";

        public Order Order { get; set; }
        public Stock Stock { get; set; }
        public Decimal SubTotal { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public string ComboboxDescriptionUpdate
        {
            get { return Stock == null ? "" : Stock.Product == null ? "" : Stock.Product.NameFrench + "-" + Stock.FeatureFrench; }
        }

        public decimal SavePrice
        {
            get
            {
                return Stock != null && Stock.Product != null && Stock.Product.UnitPrice > UnitPrice
                           ? Stock.Product.UnitPrice - UnitPrice
                           : 0;
            }
        }

    }
}
