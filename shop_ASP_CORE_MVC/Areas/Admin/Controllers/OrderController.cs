using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_ASP_CORE_MVC.Repository;

namespace shop_ASP_CORE_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController:Controller
    {

        private readonly DataContext _dataContext;


        // Constructor: Inject DataContext
        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Order.OrderByDescending(x => x.Id).ToListAsync());

		}
        public async Task<IActionResult> ViewOrder(string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
            {
                return BadRequest("Order code is required.");
            }

            var orderDetails = await _dataContext.OrderDetail
                .Include(od => od.Product)
                .Where(od => od.OrderCode == orderCode)
                .ToListAsync();

            if (orderDetails == null || orderDetails.Count == 0)
            {
                return NotFound("Order not found.");
            }

            return View(orderDetails);
        }
    }
}
