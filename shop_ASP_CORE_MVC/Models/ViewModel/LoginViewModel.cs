using System.ComponentModel.DataAnnotations;

namespace shop_ASP_CORE_MVC.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập Username")]
		public string UserName { get; set; }


		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Vui lòng nhập Password")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
