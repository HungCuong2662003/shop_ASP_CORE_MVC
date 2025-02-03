using Microsoft.AspNetCore.Identity;

namespace shop_ASP_CORE_MVC.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation { get; set; }
	}
}
