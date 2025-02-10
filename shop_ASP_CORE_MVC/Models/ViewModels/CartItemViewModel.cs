

using shop_ASP_CORE_MVC.Models;

namespace shop_ASP_CORE_MVC.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; }
        public decimal GrandTotal { get; set; }


        public decimal ShippingPrice { get; set; }

        public string CouponCode { get; set; }


    }
}
