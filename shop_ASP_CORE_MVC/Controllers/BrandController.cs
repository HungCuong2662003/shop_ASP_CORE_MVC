using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using shop_ASP_CORE_MVC.Repository;

namespace shop_ASP_CORE_MVC.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;

		// Constructor: Inject DataContext
		public BrandController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		//public IActionResult Index()
		//{
		//	return View();
		//}

		// Index action method
		public async Task<IActionResult> Index(string Slug = "")
		{
			// Tìm danh mục dựa trên Slug
			var Brand = await _dataContext.Brands
				.FirstOrDefaultAsync(c => c.Slug == Slug);

			// Nếu không tìm thấy danh mục, chuyển hướng về Index
			if (Brand == null)
				return RedirectToAction("Index");

			// Lấy danh sách sản phẩm theo danh mục
			var productsByBrand= await _dataContext.Products
				.Where(p => p.BrandId == Brand.Id)
				.OrderByDescending(p => p.Id)
				.ToListAsync();

			// Truyền dữ liệu sang View
			ViewBag.Brand = Brand; // Thêm thông tin danh mục vào ViewBag nếu cần
			return View(productsByBrand);
		}

	}


}
