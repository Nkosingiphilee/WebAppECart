using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebAppECart.Models;
using WebAppECart.ViewModel;

namespace WebAppECart.Controllers
{
    public class ShoppingController : Controller
    {
        private onlineshopEntities onlineshopEntities = new onlineshopEntities();
        private List<ShoppingCart> listOfShoppingCartModels;
        public ShoppingController()
        {
            listOfShoppingCartModels = new List<ShoppingCart>();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            IEnumerable<ShoppingView> listOfShoppingViewModels = (from objItem in onlineshopEntities.Items
                                                                       join
                                                                           objCate in onlineshopEntities.Categories
                                                                           on objItem.CategoryId equals objCate.CategoryId
                                                                       select new ShoppingView()
                                                                       {
                                                                           ImagePath = objItem.ImagePath,
                                                                           ItemName = objItem.ItemName,
                                                                           Description = objItem.Description,
                                                                           ItemPrice = objItem.ItemPtice,
                                                                           ItemId = objItem.ItemId,
                                                                           Category = objCate.CategoryName,
                                                                           
                                                                       }

                ).ToList();
            return View(listOfShoppingViewModels);
        }

        [HttpPost]
        public JsonResult Index(int ItemId)
        {
            ShoppingCart  objShoppingCartModel = new ShoppingCart();
            Item objItem = onlineshopEntities.Items.Single(model => model.ItemId== ItemId);
            if (Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCart>;
            }
            if (listOfShoppingCartModels.Any(model => model.ItemId == ItemId))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPtice;
                objShoppingCartModel.UnitPrice = objItem.ItemPtice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;
            return Json(new { Success = true, Counter = listOfShoppingCartModels.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShoppingCart()
        {
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCart>;
            return View(listOfShoppingCartModels);
        }

        [HttpDelete] 
        public void ShoppingCartDelete(int id) 
        {
            if (listOfShoppingCartModels.Any(model => model.ItemId == id))
            {
                listOfShoppingCartModels.Remove(listOfShoppingCartModels[id]);
            }
            else
            {
                Item objItem = onlineshopEntities.Items.Single(model => model.ItemId == id);
                ShoppingCart objShoppingCartModel = new ShoppingCart();
                objShoppingCartModel.ItemId = id;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPtice;
                objShoppingCartModel.UnitPrice = objItem.ItemPtice;
                listOfShoppingCartModels.Remove(objShoppingCartModel);

            }
           
        }

        [HttpPost]
        public ActionResult AddOrder()
        {
            int OrderId = 0;
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCart>;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmsss}", DateTime.Now)
            };
            onlineshopEntities.Orders.Add(orderObj);
            onlineshopEntities.SaveChanges();

            OrderId = orderObj.OrderId;

            foreach (var item in listOfShoppingCartModels)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = OrderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                onlineshopEntities.OrderDetails.Add(objOrderDetail);
                onlineshopEntities.SaveChanges();
            }

            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Index");
         
        }

    }
}