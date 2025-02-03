using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using shop_ASP_CORE_MVC.Repository;

namespace shop_ASP_CORE_MVC.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;

		// Constructor: Inject DataContext
		public CategoryController(DataContext dataContext)
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
			var category = await _dataContext.Categories
				.FirstOrDefaultAsync(c => c.Slug == Slug);

			// Nếu không tìm thấy danh mục, chuyển hướng về Index
			if (category == null)
				return RedirectToAction("Index");

			// Lấy danh sách sản phẩm theo danh mục
			var productsByCategory = await _dataContext.Products
				.Where(p => p.CategoryId == category.Id)
				.OrderByDescending(p => p.Id)
				.ToListAsync();

			// Truyền dữ liệu sang View
			ViewBag.Category = category; // Thêm thông tin danh mục vào ViewBag nếu cần
			return View(productsByCategory);
		}

	}


}
