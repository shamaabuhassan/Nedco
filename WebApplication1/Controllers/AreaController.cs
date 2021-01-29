using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AreaController : Controller
    {
        // GET: Area
        public ActionResult Index()
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }
            return View();
        }

        [HttpPost]
        public JsonResult Getparent(string type)
        {
          int state = 0;
            int rc = 0;
            if (type == "s" || type == "S") {
              
              /*  ViewBag.state = 0;
                ViewBag.first = 0;*/
                return Json("this is parent");
            }
            else  if (type == "c" || type == "C") {
                Area[] areas = Area.getarea(new AreaParameters {Type= "s"},out rc);
                /*ViewBag.areas = areas;
                ViewBag.state = state;
                ViewBag.first = 0;*/
                return Json(areas);

            }
            else if (type == "t" || type == "T") {
                Area[] areas = Area.getarea(new AreaParameters { Type = "c" }, out rc);
               /* ViewBag.areas = areas;
                ViewBag.state = state;
                ViewBag.first = 0;*/
                return Json(areas);
            }
            else{
                return Json(state);
            }
            
        }

        [HttpPost]
        public JsonResult SaveNew(string type, int? id, string name){

    Area area=new Area(id,name,0,type);
           int result= area.SaveData();
            return Json(result);
        }


            [HttpPost]
        public JsonResult SaveNewParent(string type, int? id, string name, int? parentid){

    Area area=new Area(id,name,parentid,type);
           int result= area.SaveData();
            return Json(result);
        }
    }
}