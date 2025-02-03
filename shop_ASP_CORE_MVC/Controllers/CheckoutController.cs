using Microsoft.AspNetCore.Mvc;
using shop_ASP_CORE_MVC.Models;
using shop_ASP_CORE_MVC.Repository;
using System.Security.Claims;

namespace shop_ASP_CORE_MVC.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;

		// Constructor: Inject DataContext
		public CheckoutController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);

			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel
				{
					OrderCode = ordercode,
					UserName = userEmail,
					Status = 1,  // Trạng thái đơn hàng, có thể thay đổi tùy thuộc vào yêu cầu của bạn
					CreatedDate = DateTime.Now
				};

				// Lưu đơn hàng vào cơ sở dữ liệu
				_dataContext.Add(orderItem);
				await _dataContext.SaveChangesAsync();
                List<CartModel> cartItems = HttpContext.Session.GetJson<List<CartModel>>("Cart")
                                          ?? new List<CartModel>();
                foreach (var cart in cartItems)
                {
                    var orderdetails = new OrderDetailModel
                    {
                        UserName = userEmail,
                        OrderCode = ordercode,
                        ProductId = cart.ProductId,
                        Price = cart.Price,
                        Quantity = cart.Quantity
                    };

                    // Thêm chi tiết đơn hàng vào cơ sở dữ liệu
                    _dataContext.Add(orderdetails);
                    _dataContext.SaveChanges();
                }

                HttpContext.Session.Remove("Cart");

                // Thông báo thành công
                TempData["success"] = "Đơn hàng đã được tạo , vui lòng chờ duyệt đơn hàng";

				// Chuyển hướng đến trang khác sau khi tạo đơn hàng thành công (ví dụ: trang chủ)
				return RedirectToAction("Index", "Home");
			}
		}

	}
}
