using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppECart.ViewModel
{
    public class ShoppingView
    {
        public int ItemId { set; get; }
        public string ItemName { set; get; }
        public string Description { set; get; }
        public int CategoryId { set; get; }
        public string ImagePath { set; get; }
        public decimal ItemPrice { set; get; }
        public string Category { get; internal set; }
    }
}