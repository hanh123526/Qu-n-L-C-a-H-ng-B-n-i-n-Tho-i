using Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Models;
using System;
using System.Linq;

namespace Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Models
{
    public class Giohang
    {
        // Kết nối DB điện thoại
        CuaHangDiDongDataContext data =
            new CuaHangDiDongDataContext(
                @"Data Source=LAPTOP-BL3GLPRI\SQLEXPRESS;
                  Initial Catalog=DienThoaiDiDong;
                  Integrated Security=True"
            );

        // ===== THUỘC TÍNH =====
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sHinhAnh { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }

        // ===== THÀNH TIỀN =====
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        // ===== CONSTRUCTOR =====
        public Giohang(int MaSP)
        {
            iMaSP = MaSP;

            // Lấy sản phẩm từ SQL
            SanPham sp = data.SanPhams
                .SingleOrDefault(n => n.MaSP == iMaSP);

            if (sp != null)
            {
                sTenSP = sp.TenSP;
                sHinhAnh = sp.HinhAnh;
                dDonGia = Convert.ToDouble(sp.Gia);
                iSoLuong = 1;
            }
        }
    }
}