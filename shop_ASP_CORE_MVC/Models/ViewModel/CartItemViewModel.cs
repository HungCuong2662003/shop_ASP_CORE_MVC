namespace shop_ASP_CORE_MVC.Models.CartItemViewModel
{
	public class CartItemViewModel
	{
		public  List<CartModel> CartItems { get; set; } 
	

		// Tổng giá trị của giỏ hàng
		public decimal GrandTotal
		{ get; set; }
	}
}
