using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPM.Models;

namespace CNPM.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        DBQuanLyShopDataContext data = new DBQuanLyShopDataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangKy(FormCollection form , KhachHang kh  )
        {
            var hoten = form["HoTen"];
            var tendn = form["TenDN"];
            var mk = form["MatKhau"];
            var mk2 = form["MatKhau2"];
            var sdt = form["SDT"];
            var diachi = form["DiaChi"];
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Nhập họ tên";
            }    
            else if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(mk))
            {
                ViewData["Loi3"] = "Nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(mk2))
            {
                ViewData["Loi4"] = "Nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi5"] = "Nhập số điện thoại";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Nhập địa chỉ";
            }
            else
            {
                kh.TenKH = hoten;
                kh.TenDangNhap = tendn;
                kh.Mathau = mk;
                kh.SDT = int.Parse(sdt);
                kh.DiaChi = diachi;
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
                
                
            }
            return this.DangKy();
        }

        
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            var tendn = form["tendn"];
            var matkhau = form["matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = " Phải Nhập tên đăng nhập ";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = " phải nhập mật khẩu ";
            }
            else
            {
                KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.TenDangNhap == tendn && n.Mathau == matkhau);
                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;
                    ViewBag.ThongBao = "Chúc Mừng đăng nhập thành công ";
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.ThongBao = "Tên Đăng nhập hoặc mật khẩu không dúng ";
                }

            }
            return View();
            
        }

       
    }
    

}