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
	public class CategoryController:Controller
	{
        private readonly DataContext _dataContext;
       

        // Constructor: Inject DataContext
        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        [Route("Index")]
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<CategoryModel> category = _dataContext.Categories.ToList(); //33 datas


            const int pageSize = 10; //10 items/trang

            if (pg < 1) //page < 1;
            {
                pg = 1; //page ==1
            }
            int recsCount = category.Count(); //33 items;

            var pager = new Paginate(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

            //category.Skip(20).Take(10).ToList()

            var data = category.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }
        public IActionResult Add()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryModel category)
        {
           

            if (ModelState.IsValid)
            {
                // Tạo slug cho sản phẩm
                category.Slug = category.Name.Replace(" ", "-").ToLower();
                // Loại bỏ tất cả thẻ HTML từ Description
                category.Description = Regex.Replace(category.Description, "<.*?>", string.Empty);
                // Kiểm tra trùng Slug trong database
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(category);
                }

            
                
                _dataContext.Add(category);
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
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);

            if (category == null)
            {
                // Nếu không tìm thấy sản phẩm, trả về 404
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModel category)
        {
            // Lấy sản phẩm hiện tại từ cơ sở dữ liệu
            var existingProduct = await _dataContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }
            // Loại bỏ tất cả thẻ HTML từ Description
            category.Description = Regex.Replace(category.Description, "<.*?>", string.Empty);
    

            if (ModelState.IsValid)
            {
                // Tạo slug cho sản phẩm
                category.Slug = category.Name.Replace(" ", "-").ToLower();

                // Kiểm tra trùng Slug trong database (trừ sản phẩm hiện tại)
                var slugExists = await _dataContext.Products
                    .AnyAsync(p => p.Slug == category.Slug && p.Id != id);

                if (slugExists)
                {
                    ModelState.AddModelError("Slug", "Slug này đã tồn tại, vui lòng chọn tên khác.");
                    return View(category);
                }

               

                _dataContext.Update(category);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Model có một vài thứ đang bị lỗi.";
            return View(category);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            // Tìm sản phẩm theo ID
            var category = await _dataContext.Categories.FindAsync(Id);
            if (category == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            try
            {
                // Xóa sản phẩm khỏi cơ sở dữ liệu
                _dataContext.Categories.Remove(category);
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
