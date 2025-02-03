using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using shop_ASP_CORE_MVC.Models;
using System.Security.Principal;

namespace shop_ASP_CORE_MVC.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions<DataContext> options):base(options) { 
			
		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Order{ get; set; }
		public DbSet<OrderDetailModel> OrderDetail { get; set; }
	}
}
