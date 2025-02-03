using Microsoft.AspNetCore.Mvc;
using shop_ASP_CORE_MVC.Repository;

namespace shop_ASP_CORE_MVC.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;

		// Constructor: Inject DataContext
		public ProductController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Detail(int ID)
		{
			if (ID == null)
			{
				return RedirectToAction("Index");
			}
			var productById = _dataContext.Products.Where(x=>x.Id == ID).FirstOrDefault();
			return View(productById);
		}

	}
}
