﻿using System;
using ATMTECH.Entities;

namespace ATMTECH.ShoppingCart.Entities
{
    [Serializable]
    public class Customer : BaseEntity
    {
        public const string USER = "User";
        public const string ENTERPRISE = "Enterprise";

        public User User { get; set; }
        public bool IsInvoicePossible { get; set; }
        public bool IsPaypalAuthorized { get; set; }
        public bool IsPaypalRequired { get; set; }
        public Enterprise Enterprise { get; set; }
        public CustomerType CustomerType { get; set; }
        public string CustomerNumber { get; set; }
        public decimal ActualBudget { get; set; }
        public decimal InitialBudget { get; set; }
        public Taxes Taxes { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }
        public string ComboboxDescriptionUpdate { get { return  User == null ? "" : User.FirstNameLastName; } }
        public string AddressBilling { get; set; }
        public string AddressShipping { get; set; }
        public string PostalCodeShipping { get; set; }
    }
}
