﻿using System;

namespace ATMTECH.ShoppingCart.Services.Reports.DTO
{
    public class SalesReportLine
    {
        public int OrderId { get; set; }
        public int StockInitialState { get; set; }
        public int StockActualState { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceOrderLine { get; set; }
        public decimal Total { get; set; }
        public string Product { get; set; }
        public string Enterprise { get; set; }
        public string ClientName { get; set; }
        public DateTime FinalizedDate { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public decimal GrandTotalStock { get; set; }
        public decimal GrandTotalSales { get; set; }
    }
}
