using shop_ASP_CORE_MVC.Models;
using shop_ASP_CORE_MVC.Models.Momo;

namespace shop_ASP_CORE_MVC.Services.Momo
{
	public interface IMomoService
	{
		Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfo model);
		MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
	}
}
