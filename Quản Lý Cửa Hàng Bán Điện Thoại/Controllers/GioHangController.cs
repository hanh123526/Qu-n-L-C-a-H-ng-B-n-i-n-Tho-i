using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Models;

namespace Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Controllers
{
    public class GiohangController : Controller
    {
        // ================= KẾT NỐI SQL =================
        CuaHangDiDongDataContext data =
            new CuaHangDiDongDataContext(
                @"Data Source=LAPTOP-BL3GLPRI\SQLEXPRESS;
                  Initial Catalog=DienThoaiDiDong;
                  Integrated Security=True"
            );

        // ================= LẤY GIỎ HÀNG =================
        public List<Giohang> LayGiohang()
        {
            List<Giohang> lstGiohang =
                Session["Giohang"] as List<Giohang>;

            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }

            return lstGiohang;
        }

        // ================= THÊM GIỎ HÀNG =================
        public ActionResult ThemGiohang(int iMaSP, string strURL)
        {
            List<Giohang> lstGiohang = LayGiohang();

            Giohang sanpham =
                lstGiohang.Find(n => n.iMaSP == iMaSP);

            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSP);
                lstGiohang.Add(sanpham);
            }
            else
            {
                sanpham.iSoLuong++;
            }

            return Redirect(strURL);
        }

        // ================= TỔNG SỐ LƯỢNG =================
        private int TongSoLuong()
        {
            int tong = 0;

            List<Giohang> lstGiohang =
                Session["Giohang"] as List<Giohang>;

            if (lstGiohang != null)
            {
                tong = lstGiohang.Sum(n => n.iSoLuong);
            }

            return tong;
        }

        // ================= TỔNG TIỀN =================
        private double TongTien()
        {
            double tong = 0;

            List<Giohang> lstGiohang =
                Session["Giohang"] as List<Giohang>;

            if (lstGiohang != null)
            {
                tong = lstGiohang.Sum(n => n.dThanhTien);
            }

            return tong;
        }

        // ================= GIỎ HÀNG =================
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = LayGiohang();

            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGiohang);
        }

        // ================= GIỎ HÀNG PARTIAL =================
        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return PartialView();
        }

        // ================= XÓA GIỎ HÀNG =================
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = LayGiohang();

            Giohang sanpham =
                lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);

            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSP == iMaSP);
            }

            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }

            return RedirectToAction("Giohang");
        }

        // ================= CẬP NHẬT GIỎ HÀNG =================
        public ActionResult CapNhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = LayGiohang();

            Giohang sanpham =
                lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);

            if (sanpham != null)
            {
                int soLuong = int.Parse(f["txtSoLuong"]);
                sanpham.iSoLuong = soLuong;
            }

            return RedirectToAction("Giohang");
        }

        // ================= XÓA TẤT CẢ GIỎ HÀNG =================
        public ActionResult XoaTatCaGiohang()
        {
            List<Giohang> lstGiohang = LayGiohang();

            lstGiohang.Clear();

            Session["Giohang"] = null;

            return RedirectToAction("Index", "SanPham");
        }

        // ================= ĐẶT HÀNG GET =================
        [HttpGet]
        public ActionResult DatHang()
        {
            // Kiểm tra đăng nhập
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            // Kiểm tra giỏ hàng
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "SanPham");
            }

            List<Giohang> lstGiohang = LayGiohang();

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGiohang);
        }

        // ================= ĐẶT HÀNG POST =================
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            // Kiểm tra đăng nhập
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            // Kiểm tra giỏ hàng
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "SanPham");
            }

            // Lấy khách hàng
            KhachHang_TaiKhoan tk =
                (KhachHang_TaiKhoan)Session["TaiKhoan"];

            // Tạo đơn hàng
            DonHang dh = new DonHang();

            dh.MaKH = tk.MaKH;

            dh.NgayDat = DateTime.Now;

            dh.TrangThai = "Chờ xác nhận";

            dh.DiaChiGiaoHang =
                collection["DiaChiGiaoHang"];

            // Thêm đơn hàng
            data.DonHangs.InsertOnSubmit(dh);

            data.SubmitChanges();

            // Lấy giỏ hàng
            List<Giohang> gh = LayGiohang();

            // Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh =
                    new ChiTietDonHang();

                ctdh.MaDH = dh.MaDH;

                ctdh.MaSP = item.iMaSP;

                ctdh.SoLuong = item.iSoLuong;

                ctdh.DonGia =
                    Convert.ToDecimal(item.dDonGia);

                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }

            // Lưu DB
            data.SubmitChanges();

            // Xóa giỏ hàng
            Session["Giohang"] = null;

            return RedirectToAction("XacNhanDonHang");
        }

        // ================= XÁC NHẬN ĐƠN HÀNG =================
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}