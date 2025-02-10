using shop_ASP_CORE_MVC.Models.Vnpay;

namespace shop_ASP_CORE_MVC.Services.Vnpay
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
		PaymentResponseModel PaymentExecute(IQueryCollection collections);

	}
}
