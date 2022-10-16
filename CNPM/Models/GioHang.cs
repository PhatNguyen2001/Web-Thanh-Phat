using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNPM.Models
{
    public class GioHang
    {
        DBQuanLyShopDataContext data = new DBQuanLyShopDataContext();
        public int iMaMH { set; get; }
        public string sTenMH { set; get; }
        public string sHinhAnh { set; get; }
        public int iSoLuong { set; get; }
        public int iMaSize { set; get; }
        public Double dDonGia { set; get; }

        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang(int MaMH)
        {
            iMaMH = MaMH;
            MatHang hang = data.MatHangs.Single(n => n.MaMH == iMaMH);
            sTenMH = hang.TenMatHang;
            sHinhAnh = hang.HinhAnh;
            dDonGia = double.Parse(hang.DonGia.ToString());
            iMaSize = 1;
            iSoLuong = 1;
        }
    }
}