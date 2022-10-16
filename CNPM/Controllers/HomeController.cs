using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPM.Models;
using System.Linq;

namespace CNPM.Controllers
{
    public class HomeController : Controller
    {   
        DBQuanLyShopDataContext data = new DBQuanLyShopDataContext();
        // GET: Home
        public ActionResult Index()
        {
            
            var item = data.MatHangs.Select(i => i);
            
            
            return View(item);
        }

        
       

        public ActionResult Details(int id)
        {
            
            var details = data.MatHangs.Where(i => i.MaMH == id).First();
            
            return View(details);
        }

        public ActionResult Childs(int id)
        {
            var child = data.MatHangs.Where(i => i.MaLoai == id).ToList();
            
          
            

            return View(child);
        }
        public ActionResult Male(int id)
        {
            var male = data.MatHangs.Where(i => i.MaLoai == id).ToList();
            return View(male);
        }
        public ActionResult Female(int id)
        {
            var male = data.MatHangs.Where(i => i.MaLoai == id).ToList();
            return View(male);
        }

        public ActionResult Item(int id)
        {
            var item = data.MatHangs.Where(i => i.MaLoai == id).ToList();
            return View(item);
        }
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string key)
        {
            var item = data.MatHangs.Select(s => s.TenMatHang.Contains(key));
            return View(item);

        }

        
    }
}