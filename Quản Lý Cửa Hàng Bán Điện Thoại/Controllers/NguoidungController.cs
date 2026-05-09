using System;
using System.Linq;
using System.Web.Mvc;
using Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Models;

namespace Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Controllers
{
    public class NguoidungController : Controller
    {
        // KẾT NỐI DATABASE
        CuaHangDiDongDataContext db =
            new CuaHangDiDongDataContext(
                @"Data Source=LAPTOP-BL3GLPRI\SQLEXPRESS;
                  Initial Catalog=DienThoaiDiDong;
                  Integrated Security=True"
            );

        // ================= INDEX =================
        public ActionResult Index()
        {
            return View();
        }

        // ================= ĐĂNG KÝ =================
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection)
        {
            var hoten = collection["HoTenKH"];
            var email = collection["EmailKH"];
            var sdt = collection["SoDienThoaiKH"];
            var diachi = collection["DiaChiKH"];

            var taikhoan = collection["TenDangNhapKH"];
            var matkhau = collection["MatKhauKH"];

            // ===== VALIDATION =====
            if (string.IsNullOrEmpty(hoten))
                ViewData["Loi1"] = "Họ tên không được để trống";
            else if (string.IsNullOrEmpty(taikhoan))
                ViewData["Loi2"] = "Phải nhập tài khoản";
            else if (string.IsNullOrEmpty(matkhau))
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            else
            {
                // ===== 1. THÊM KHÁCH HÀNG =====
                KhachHang kh = new KhachHang();
                kh.HoTenKH = hoten;
                kh.EmailKH = email;
                kh.SoDienThoaiKH = sdt;
                kh.DiaChiKH = diachi;

                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();

                // ===== 2. TẠO TÀI KHOẢN =====
                KhachHang_TaiKhoan tk = new KhachHang_TaiKhoan();
                tk.MaKH = kh.MaKH;
                tk.TenDangNhapKH = taikhoan;
                tk.MatKhauKH = matkhau;

                db.KhachHang_TaiKhoans.InsertOnSubmit(tk);
                db.SubmitChanges();

                return RedirectToAction("Dangnhap");
            }

            return View();
        }

        // ================= ĐĂNG NHẬP =================
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var taikhoan = collection["TenDangNhapKH"];
            var matkhau = collection["MatKhauKH"];

            // ===== VALIDATION =====
            if (string.IsNullOrEmpty(taikhoan))
                ViewData["Loi1"] = "Phải nhập tài khoản";
            else if (string.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            else
            {
                // ===== KIỂM TRA LOGIN =====
                var user = db.KhachHang_TaiKhoans
                    .SingleOrDefault(x =>
                        x.TenDangNhapKH == taikhoan &&
                        x.MatKhauKH == matkhau);

                if (user != null)
                {
                    Session["Taikhoan"] = user;
                    return RedirectToAction("Index", "SanPham");
                }
                else
                {
                    ViewBag.Thongbao = "Sai tài khoản hoặc mật khẩu!";
                }
            }

            return View();
        }

        // ================= ĐĂNG XUẤT =================
        public ActionResult Dangxuat()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "SanPham");
        }
    }
}