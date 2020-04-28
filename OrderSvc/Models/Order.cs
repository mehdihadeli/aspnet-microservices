﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Infrastructure.Extentions;

namespace OrderSvc.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string Currency { get; set; }
        public float Price => (LineItems.HasAny() ? LineItems.Sum(li => li.Qty * li.Price) : 0);
        public float Tax => 0.05f;
        public float Discount { get; set; }
        public float Shipping { get; set; }
        public float TotalPrice => Price * (1 + Tax) + Shipping - Discount;
        public List<LineItem> LineItems { get; set; }
    }
}
