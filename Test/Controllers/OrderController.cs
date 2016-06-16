using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test.Controllers
{
    public class OrderController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            Models.Service service = new Models.Service();
            List<Models.Order> emptyData =  service.GetEmploee();
            List<SelectListItem> listEmp = new List<SelectListItem>();
            foreach (var item in emptyData)
            {
                listEmp.Add(new SelectListItem()
                {
                    Value = item.EmpId.ToString(),
                    Text = item.EmpName,
                });
            };
            ViewBag.listEmp = listEmp;

            List<Models.Order> shipData = service.GetShipper();
            List<SelectListItem> listShip = new List<SelectListItem>();
            foreach (var item in shipData)
            {
                listShip.Add(new SelectListItem()
                {
                    Value = item.ShipperId.ToString(),
                    Text = item.ShipperName,
                });
            };
            ViewBag.listShip = listShip;

            return View();
        }

        [HttpGet]
        public JsonResult DataPackage()
        {
            Models.Service service = new Models.Service();
            List<Models.Order> getEmp = service.GetEmploee();
            List<Models.Order> getShip = service.GetShipper();
            List<Models.Order> getCust = service.GetCustomer();
            List<List<Models.Order>> list = new List<List<Models.Order>>();
            list.Add(getEmp);
            list.Add(getShip);
            list.Add(getCust);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Result(Models.Order order)
        {
            Models.Service service = new Models.Service();
            List<Test.Models.Order> result =  service.GetResult(order);
            ViewBag.result = result;
            return View();
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Delete(int OrderId)
        {
            Models.Service service = new Models.Service();
            service.DeleteOrderId(OrderId);
            return null;
        }

    }
}