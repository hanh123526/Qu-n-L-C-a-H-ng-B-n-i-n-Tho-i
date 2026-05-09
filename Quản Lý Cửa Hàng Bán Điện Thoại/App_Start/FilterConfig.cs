using System.Web;
using System.Web.Mvc;

namespace Quản_Lý_Cửa_Hàng_Bán_Điện_Thoại
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
