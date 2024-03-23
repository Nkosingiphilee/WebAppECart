using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebAppECart.Models;
using WebAppECart.ViewModel;
using WebAppECart;
using System.Collections.Specialized;

namespace WebAppECart.Controllers
{
    public class ItemController : Controller
    {
        private onlineshopEntities onlineshopEntities;

        public ItemController()
        {
            onlineshopEntities = new onlineshopEntities();
        }
        // GET: Item
        public ActionResult Index()
        {
            ItemView itemView = new ItemView();
            itemView.CategorySelectListItem = (from objcat in onlineshopEntities.Categories
                                               select new SelectListItem
                                               {
                                                   Text = objcat.CategoryName,
                                                   Value=objcat.CategoryId.ToString(),
                                                   Selected = true
                                               }

                ) ;
            return View(itemView);
        }

        [HttpPost]
        public JsonResult Index(ItemView itemView)
        {

            Item objItem = new Item();

            objItem.ImagePath =itemView.ImagePath.ToString();
            objItem.CategoryId =itemView.CategoryId;
            objItem.Description =itemView.Description;  
            objItem.ItemName = itemView.ItemName;
            objItem.ItemPtice =itemView.ItemPrice;
            onlineshopEntities.Items.Add(objItem);
            onlineshopEntities.SaveChanges();

            return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);
        }
    }
}
