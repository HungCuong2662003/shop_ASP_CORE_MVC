using System.ComponentModel.DataAnnotations;

namespace shop_ASP_CORE_MVC.Models
{
	public class UserModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập Username")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập Email")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Vui lòng nhập Password")]
		public string Password { get; set; }
	}
}
