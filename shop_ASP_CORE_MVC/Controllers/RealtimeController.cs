using Microsoft.AspNetCore.Mvc;

namespace shop_ASP_CORE_MVC.Controllers
{
    public class RealtimeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
