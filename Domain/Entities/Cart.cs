﻿namespace Domain.Entities
{
    public class Cart : BaseEntity
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItem> Items { get; set; }
    }
}
