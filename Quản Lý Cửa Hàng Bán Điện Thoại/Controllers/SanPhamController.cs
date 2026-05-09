using Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Models;
using System.Linq;
using System.Web.Mvc;

namespace Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại.Controllers
{
    public class SanPhamController : Controller
    {
        CuaHangDiDongDataContext db =
            new CuaHangDiDongDataContext(
                @"Data Source=LAPTOP-BL3GLPRI\SQLEXPRESS;
                  Initial Catalog=DienThoaiDiDong;
                  Integrated Security=True"
            );

        // DANH SÁCH SẢN PHẨM
        public ActionResult Index()
        {
            var sp = db.SanPhams.ToList();

            return View(sp);
        }

        // CHI TIẾT SẢN PHẨM
        public ActionResult ChiTiet(int id)
        {
            var sp = db.SanPhams
                       .FirstOrDefault(x => x.MaSP == id);

            return View(sp);
        }
    }
}