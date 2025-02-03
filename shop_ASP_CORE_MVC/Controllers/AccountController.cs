using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shop_ASP_CORE_MVC.Models;
using shop_ASP_CORE_MVC.Models.ViewModel;
using shop_ASP_CORE_MVC.Repository;

namespace shop_ASP_CORE_MVC.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUserModel> _userManager;
		private readonly SignInManager<AppUserModel> _signInManager;

		public AccountController(SignInManager<AppUserModel> signInManager,
								 UserManager<AppUserModel> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}



		public IActionResult Index(string returnUrl)
		{
			return View(new LoginViewModel {ReturnUrl= returnUrl});
		}
		[HttpPost]
		public async Task<IActionResult> Index(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(
					loginVM.UserName, loginVM.Password, false, false);

				if (result.Succeeded)
				{
                    return Redirect(loginVM.ReturnUrl ?? Url.Action("Index", "Home"));
                }
            
				else
				{
                    ModelState.AddModelError("", " Username or Password bị sai");
                }
			}

			return View(loginVM);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				var newUser = new AppUserModel
				{
					UserName = user.UserName,
					Email = user.Email,
					
				};

				IdentityResult result = await _userManager.CreateAsync(newUser,user.Password);

				if (result.Succeeded)
				{
					TempData["success"] = "Tạo tài khoản thành công";
					return Redirect("/account"); // Điều hướng đến trang phù hợp
				}

				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			return View(user);
		}
		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			// Đăng xuất người dùng khỏi ứng dụng
			await _signInManager.SignOutAsync();

			// Chuyển hướng người dùng đến URL đã chỉ định hoặc trang chủ nếu không có URL nào
			return Redirect(returnUrl);
		}

	}
}
