using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shop_ASP_CORE_MVC.Models;
using shop_ASP_CORE_MVC.Repository;
using System.Text.RegularExpressions;

namespace shop_ASP_CORE_MVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

		// Constructor: Inject DataContext
		public ProductController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products.OrderBy(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());

		}
        public IActionResult Add()
        {
            // Lấy danh sách danh mục từ cơ sở dữ liệu và truyền vào ViewBag để sử dụng trong View
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");

            // Trả về View để hiển thị giao diện tạo mới
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductModel product)
        {
            // Lấy danh sách Categories và Brands từ cơ sở dữ liệu để hiển thị trong form
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                // Tạo slug cho sản phẩm
                product.Slug = product.Name.Replace(" ", "-").ToLower();
                // Loại bỏ tất cả thẻ HTML từ Description
                product.Description = Regex.Replace(product.Description, "<.*?>", string.Empty);
                // Kiểm tra trùng Slug trong database
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/product-details");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;

                }
                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi.";
                List<string> errors = new List<string>();

                // Thu thập tất cả lỗi từ ModelState
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                // Kết hợp lỗi thành một chuỗi
                string errorMessage = string.Join("\n", errors);

                // Trả về BadRequest với thông báo lỗi
                return BadRequest(errorMessage);
            }

            // Nếu không có lỗi, trả về lại View với dữ liệu người dùng đã nhập
            return View(product);

        }
        public async Task<IActionResult> Edit(int Id)
        {
            // Tìm sản phẩm theo ID
            ProductModel product = await _dataContext.Products.FindAsync(Id);

            if (product == null)
            {
                // Nếu không tìm thấy sản phẩm, trả về 404
                return NotFound();
            }

            // Tạo danh sách các danh mục và thương hiệu
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            // Trả về view với sản phẩm
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel product)
        {
            // Lấy sản phẩm hiện tại từ cơ sở dữ liệu
            var existingProduct = await _dataContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }
            // Loại bỏ tất cả thẻ HTML từ Description
            product.Description = Regex.Replace(product.Description, "<.*?>", string.Empty);
            // Lấy danh sách Categories và Brands để hiển thị trong form
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                // Tạo slug cho sản phẩm
                product.Slug = product.Name.Replace(" ", "-").ToLower();

                // Kiểm tra trùng Slug trong database (trừ sản phẩm hiện tại)
                var slugExists = await _dataContext.Products
                    .AnyAsync(p => p.Slug == product.Slug && p.Id != id);

                if (slugExists)
                {
                    ModelState.AddModelError("Slug", "Slug này đã tồn tại, vui lòng chọn tên khác.");
                    return View(product);
                }

                // Nếu người dùng không chọn hình mới, giữ nguyên hình ảnh cũ
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/product-details");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    // Kiểm tra xem ảnh đã tồn tại trong thư mục chưa
                    if (!System.IO.File.Exists(filePath))
                    {
                        // Nếu chưa tồn tại, thực hiện sao chép tệp
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageUpload.CopyToAsync(fs);
                        }
                    }

                    // Gán giá trị hình ảnh vào thuộc tính của sản phẩm
                    product.Image = imageName;
                }

                else
                {
                    // Giữ nguyên hình ảnh cũ
                    product.Image = existingProduct.Image;
                }

                _dataContext.Update(product);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Model có một vài thứ đang bị lỗi.";
            return View(product);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            // Tìm sản phẩm theo ID
            var product = await _dataContext.Products.FindAsync(Id);
            if (product == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            try
            {
                // Kiểm tra và xóa ảnh nếu cần
                if (!string.Equals(product.Image, "noname.jpg", StringComparison.OrdinalIgnoreCase))
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/product-details");
                    string oldFileImage = Path.Combine(uploadDir, product.Image);

                    if (System.IO.File.Exists(oldFileImage))
                    {
                        System.IO.File.Delete(oldFileImage);
                    }
                }

                // Xóa sản phẩm khỏi cơ sở dữ liệu
                _dataContext.Products.Remove(product);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Sản phẩm đã được xóa thành công.";
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi xóa
                TempData["error"] = $"Có lỗi xảy ra khi xóa sản phẩm: {ex.Message}";
            }

            return RedirectToAction("Index");
        }


    }
}
