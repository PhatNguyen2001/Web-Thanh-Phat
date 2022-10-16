using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPM.Models;

namespace CNPM.Controllers
{
    public class AdminController : Controller
    {
        DBQuanLyShopDataContext data = new DBQuanLyShopDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MatHang()
        {
            return View(data.MatHangs.ToList());
        }

       [HttpGet]
        public ActionResult ThemMatHang()
        {
            
            ViewBag.MaLoai = new SelectList(data.LoaiMatHangs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaSize = new SelectList(data.KichCos.ToList().OrderBy(n => n.Size), "MaSize", "Size");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMatHang(MatHang mathang , FormCollection collection)
        {
            ViewBag.MaLoai = new SelectList(data.LoaiMatHangs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaSize = new SelectList(data.KichCos.ToList().OrderBy(n => n.Size), "MaSize", "Size");

            var E_tensach = collection["tensach"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(E_tensach))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                mathang.TenMatHang = E_tensach.ToString();
                mathang.HinhAnh = E_hinh.ToString();
                mathang.DonGia = E_giaban;
                
                mathang.SoLuongTon = E_soluongton;
                data.MatHangs.InsertOnSubmit(mathang);
                data.SubmitChanges();
                return RedirectToAction("ListSach");
            }
            return this.ThemMatHang();
            return View();
  
        }


        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/HinhMatHang/" + file.FileName));
            return "/HinhMatHang/" + file.FileName;
        }
        public ActionResult ChiTiet(int id )
        {
            MatHang mathang = data.MatHangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.Ma = mathang.MaMH;
            if(mathang==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mathang);

        }

        [HttpGet]
        public ActionResult XoaMatHang(int id)
        {
            MatHang mathang = data.MatHangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.Ma = mathang.MaMH;
            if(mathang==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mathang);
        }
        [HttpPost,ActionName("XoaMatHang")]
        public ActionResult XacNhanXoa(int id )
        {
            MatHang mathang = data.MatHangs.SingleOrDefault(n => n.MaMH == id);
            if(mathang == null )
            {
                Response.StatusCode = 404;
                return null;
            }
            data.MatHangs.DeleteOnSubmit(mathang);
            data.SubmitChanges();
            return RedirectToAction("MatHang");

        }
        [HttpGet]
        public ActionResult SuaMatHang(int id )
        {
            
            MatHang mathang = data.MatHangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.MaLoai = new SelectList(data.LoaiMatHangs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", mathang.MaLoai);
            if (mathang==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mathang);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaMatHang(MatHang mathang)
        {
            ViewBag.MaLoai = new SelectList(data.LoaiMatHangs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", mathang.MaLoai);
            UpdateModel(mathang);
            data.SubmitChanges();
            return RedirectToAction("MatHang");
        }


        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login( FormCollection form)
        {
            var tendn = form["username"];
            var matkhau = form["pass"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = " Phải nhập tài khoản";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = " Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = data.Admins.SingleOrDefault(a => a.UserAdmin == tendn && a.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Admin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = " Tên đăng nhập hoặc mật khẩu không đúng ";
            }
            return View();
                
        }

        public ActionResult DonDatHang()
        {

            return View(data.DonDatHangs.ToList());
        }
        public ActionResult ChiTietDDH()
        {
            return View(data.ChiTietDDHs.ToList());
        }
        public ActionResult KhachHang()
        {
            return View(data.KhachHangs.ToList());
        }

        [HttpGet]
        public ActionResult ThemKhachHang()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemKhachHang(KhachHang khachhang)
        {
            data.KhachHangs.InsertOnSubmit(khachhang);
            data.SubmitChanges();
            return RedirectToAction("KhachHang");

        }

        
        [HttpGet]
        public ActionResult XoaKhachHang(int id)
        {
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.Ma = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpPost, ActionName("XoaKhachHang")]
        public ActionResult XacNhanXoaKH(int id)
        {
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KhachHangs.DeleteOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("KhachHang");
        }

        
    }

      
}