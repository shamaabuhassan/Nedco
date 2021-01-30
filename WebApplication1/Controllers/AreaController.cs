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
        public ActionResult Index(string type)
        {
            if (Session["employee"] == null)
            {
                return RedirectToAction("index", "Employees");
            }

            
            else
            {
                Area[] areas = new Area[] { };
                if (type == "s")
                {
                    ViewBag.areas = areas;
                    ViewBag.type = type;
                }
                if (type == "c")
                { int rc;
                    areas = Area.getarea(new AreaParameters { Type = "s" }, out rc);
                    ViewBag.areas = areas;
                    ViewBag.type = type;
                }
                if (type == "t")
                {
                    int rc;
                    areas = Area.getarea(new AreaParameters { Type = "c" }, out rc);
                    ViewBag.areas = areas;
                    ViewBag.type = type;
                }
                return View();
            }
        }

        //[HttpPost]
        //public JsonResult Getparent(string type)
        //{
        //  int state = 0;
        //    int rc = 0;
        //    if (type == "s" || type == "S") {

        //      /*  ViewBag.state = 0;
        //        ViewBag.first = 0;*/
        //        return Json("this is parent");
        //    }
        //    else  if (type == "c" || type == "C") {
        //        Area[] areas = Area.getarea(new AreaParameters {Type= "s"},out rc);
        //        /*ViewBag.areas = areas;
        //        ViewBag.state = state;
        //        ViewBag.first = 0;*/
        //        return Json(areas);

        //    }
        //    else if (type == "t" || type == "T") {
        //        Area[] areas = Area.getarea(new AreaParameters { Type = "c" }, out rc);
        //       /* ViewBag.areas = areas;
        //        ViewBag.state = state;
        //        ViewBag.first = 0;*/
        //        return Json(areas);
        //    }
        //    else{
        //        return Json(state);
        //    }

        //}

        //[HttpPost]
        //    public JsonResult SaveNew(string type, int? id, string name){

        //Area area=new Area(id,name,0,type);
        //       int result= area.SaveData();
        //        return Json(result);
        //    }


        //[HttpPost]
        //    public JsonResult SaveNewParent(string type, int? id, string name, int? parentid){

        //Area area=new Area(id,name,parentid,type);
        //       int result= area.SaveData();
        //        return Json(result);
        //    }



        public ActionResult Save(int? Id, string Name, int? ParentId)
        {
            
            Area area = new Area(Id, Name, ParentId, type);
            int? result = area.SaveData();
            ViewBag.result = result;
            return View();
        }
    }
}