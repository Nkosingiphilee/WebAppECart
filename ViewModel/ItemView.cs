using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Mvc;

namespace WebAppECart.ViewModel
{
    public class ItemView
    {
        public int ItemId { set; get; }
        public string ItemName { set; get; }
        public string Description { set; get; }
        public int CategoryId { set; get; }
        public string ImagePath { set; get; }
        public decimal ItemPrice { set; get; }

        public IEnumerable<SelectListItem> CategorySelectListItem { set; get; }
    }
}