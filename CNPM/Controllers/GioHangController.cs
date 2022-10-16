using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPM.Models;
namespace CNPM.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        DBQuanLyShopDataContext data = new DBQuanLyShopDataContext();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> list = Session["Giohang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["Giohang"] = list;
            }
            return list;
        }

        public ActionResult ThemGioHang(int iMaMH, string strURL)
        {
            List<GioHang> list = LayGioHang();
            GioHang mathang = list.Find(n => n.iMaMH == iMaMH);
            if (mathang == null)
            {
                mathang = new GioHang(iMaMH);
                list.Add(mathang);
                return Redirect(strURL);
            }
            else
            {
                mathang.iSoLuong++;
                return Redirect(strURL);
            }

        }



        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> list = Session["Giohang"] as List<GioHang>;
            if (list != null)
            {
                iTongSoLuong = list.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        public double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> list = Session["Giohang"] as List<GioHang>;
            if (list != null)
            {
                iTongTien = list.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }

      
        public ActionResult GioHang()
        {
            List<GioHang> list = LayGioHang();
            if (list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            var size = data.KichCos.Select(k => k);
            ViewData["Size"] = new SelectList(data.KichCos , "MaSize" , "Size" );
            return View(list);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGioHang( int iMaMH)
        {
            List<GioHang> list = LayGioHang();
            GioHang mathang = list.SingleOrDefault(n => n.iMaMH == iMaMH);
            if(mathang!=null)
            {
                list.RemoveAll(n => n.iMaMH == iMaMH);
                return RedirectToAction("GioHang"); 
            }    
            if(list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaMH , FormCollection form)
        {
            List<GioHang> list = LayGioHang();
            GioHang mathang = list.SingleOrDefault(n => n.iMaMH == iMaMH);
            if(mathang!=null)
            {
                mathang.iSoLuong = int.Parse(form["SoLuong"].ToString());
                
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> list = LayGioHang();
            list.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if(Session["TaiKhoan"] ==null || Session["TaiKhoan"].ToString() == "" )
            {
                return RedirectToAction("DangNhap", "User");
            }
            if(Session["GioHang"] ==null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<GioHang> list = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(list);
        }

        public ActionResult DatHang(FormCollection form)
        {
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang) Session["TaiKhoan"];
            List<GioHang> list = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDatHang = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", form["ngaygiao"]);
            ddh.NgayGiaoHang = DateTime.Parse(ngaygiao);
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach( var item in list)
            {
                ChiTietDDH ct = new ChiTietDDH();
                ct.MaDDH = ddh.MaDDH;
                ct.MaMH = item.iMaMH;
                ct.SoLuong = item.iSoLuong;
                ct.DonGia = item.dDonGia ;
                ct.MaSize = item.iMaSize;
                data.ChiTietDDHs.InsertOnSubmit(ct);

            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDon", "GioHang");
        }

        public ActionResult XacNhanDon()
        {
            return View();
        }
        
    }
}