using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;


        // Constructor: Inject DataContext
        public BrandController(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderBy(x => x.Id).ToListAsync());

        }
        public IActionResult Add()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BrandModel Brand)
        {


            if (ModelState.IsValid)
            {
                // Tạo slug cho sản phẩm
                Brand.Slug = Brand.Name.Replace(" ", "-").ToLower();
                // Loại bỏ tất cả thẻ HTML từ Description
                Brand.Description = Regex.Replace(Brand.Description, "<.*?>", string.Empty);
                // Kiểm tra trùng Slug trong database
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == Brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(Brand);
                }



                _dataContext.Add(Brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục sản phẩm thành công";
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
        }
        public async Task<IActionResult> Edit(int Id)
        {
            // Tìm sản phẩm theo ID
            BrandModel Brand = await _dataContext.Brands.FindAsync(Id);

            if (Brand == null)
            {
                // Nếu không tìm thấy sản phẩm, trả về 404
                return NotFound();
            }

            return View(Brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandModel Brand)
        {
            // Lấy sản phẩm hiện tại từ cơ sở dữ liệu
            var existingBrand = await _dataContext.Brands.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (existingBrand == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }
            // Loại bỏ tất cả thẻ HTML từ Description
            Brand.Description = Regex.Replace(Brand.Description, "<.*?>", string.Empty);


            if (ModelState.IsValid)
            {
                // Tạo slug cho sản phẩm
                Brand.Slug = Brand.Name.Replace(" ", "-").ToLower();

                // Kiểm tra trùng Slug trong database (trừ sản phẩm hiện tại)
                var slugExists = await _dataContext.Products
                    .AnyAsync(p => p.Slug == Brand.Slug && p.Id != id);

                if (slugExists)
                {
                    ModelState.AddModelError("Slug", "Slug này đã tồn tại, vui lòng chọn tên khác.");
                    return View(Brand);
                }



                _dataContext.Update(Brand);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Model có một vài thứ đang bị lỗi.";
            return View(Brand);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            // Tìm sản phẩm theo ID
            var Brand = await _dataContext.Brands.FindAsync(Id);
            if (Brand == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            try
            {
                // Xóa sản phẩm khỏi cơ sở dữ liệu
                _dataContext.Brands.Remove(Brand);
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
