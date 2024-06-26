﻿namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Status {  get; set; }
        public decimal TotalPrice { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public DateTime ShipDate { get; set; }
        public List<OrderItem> Items { get; set; }
        public List<ProductWarranty> ProductWarranties { get; set; }
    }
}
