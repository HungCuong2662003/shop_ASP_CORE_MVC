using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shop_ASP_CORE_MVC.Models;
using shop_ASP_CORE_MVC.Models.CartItemViewModel;
using shop_ASP_CORE_MVC.Repository;

namespace shop_ASP_CORE_MVC.Controllers
{

	public class CartController : Controller
	{
		private readonly DataContext _dataContext;

		// Constructor: Inject DataContext
		public CartController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			// Lấy danh sách các mục trong giỏ hàng từ Session
			List<CartModel> cartItems = HttpContext.Session.GetJson<List<CartModel>>("Cart")
										  ?? new List<CartModel>();

			// Tạo một đối tượng ViewModel cho giỏ hàng
			var cartVM = new CartItemViewModel
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};

			// Trả về View với dữ liệu ViewModel
			return View(cartVM);

		}
		public async Task<IActionResult> Add(long id)
		{
			if (!User.Identity.IsAuthenticated)
			{
				// Nếu người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
				return RedirectToAction("Index", "Account");
			}
			// Lấy sản phẩm từ cơ sở dữ liệu
			ProductModel product = await _dataContext.Products.FindAsync(id);

			// Lấy danh sách giỏ hàng từ Session, nếu không có thì tạo mới danh sách
			List<CartModel> cart = HttpContext.Session.GetJson<List<CartModel>>("Cart")
									   ?? new List<CartModel>();

			// Kiểm tra sản phẩm đã có trong giỏ hàng chưa
			CartModel cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

			if (cartItem == null)
			{
				// Nếu sản phẩm chưa có, thêm sản phẩm mới vào giỏ hàng
				cart.Add(new CartModel(product));
			}
			else
			{
				// Nếu sản phẩm đã có, tăng số lượng
				cartItem.Quantity += 1;
			}

			// Cập nhật lại giỏ hàng trong Session
			HttpContext.Session.SetJson("Cart", cart);
			TempData["success"] = "Thêm vào giỏ hàng thành công";
			// Quay lại trang trước
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Decrease(int id)
		{
			// Lấy danh sách giỏ hàng từ session
			var cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();

			// Tìm sản phẩm trong giỏ hàng dựa trên Id
			var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

			if (cartItem != null)
			{
				// Nếu số lượng lớn hơn 1, giảm số lượng
				if (cartItem.Quantity > 1)
				{
					cartItem.Quantity--;
				}
				else
				{
					// Nếu số lượng là 1, xóa sản phẩm khỏi giỏ hàng
					cart.Remove(cartItem);
				}

				// Cập nhật giỏ hàng trong session
				if (cart.Count > 0)
				{
					HttpContext.Session.SetJson("Cart", cart);
				}
				else
				{
					// Nếu giỏ hàng rỗng, xóa session
					HttpContext.Session.Remove("Cart");
				}
				
			}
			TempData["success"] = "Giảm số lượng thành công";
			// Điều hướng về trang Index
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int id)
		{
			// Lấy danh sách giỏ hàng từ session
			var cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();

			// Tìm sản phẩm trong giỏ hàng dựa trên Id
			var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

			if (cartItem != null)
			{
				// Tăng số lượng sản phẩm
				cartItem.Quantity++;

				// Cập nhật giỏ hàng trong session
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Tăng số lượng thành công";
			// Điều hướng về trang Index
			return RedirectToAction("Index");
		}
        // Xử lý xóa một sản phẩm khỏi giỏ hàng
        public async Task<IActionResult> Remove(int id)
        {
            // Lấy danh sách sản phẩm trong giỏ hàng từ Session
            var cart = HttpContext.Session.GetJson<List<CartModel>>("Cart") ?? new List<CartModel>();

			// Xóa sản phẩm theo ProductId
			cart.RemoveAll(item => item.ProductId == id);

			if (cart.Count == 0)
			{
				// Nếu giỏ hàng trống, xóa giỏ hàng khỏi Session
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				// Cập nhật giỏ hàng trong Session
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Xóa mặt hàng thành công";

			// Chuyển hướng về trang Index

			return RedirectToAction("Index");

		}

		// Xử lý xóa toàn bộ giỏ hàng
		public async Task<IActionResult> Clear()
		{
			// Xóa giỏ hàng khỏi Session
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Đã xóa hết giỏ hàng";
			// Chuyển hướng về trang Index
			return RedirectToAction("Index");
		}





	}
}
