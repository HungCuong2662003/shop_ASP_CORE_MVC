using Microsoft.EntityFrameworkCore;
using shop_ASP_CORE_MVC.Models;

namespace shop_ASP_CORE_MVC.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			// Tự động áp dụng migration nếu có
			_context.Database.Migrate();

			// Kiểm tra nếu bảng Products chưa có dữ liệu
			if (!_context.Products.Any())
			{
				// Tạo dữ liệu mẫu cho CategoryModel
				CategoryModel macbookCategory = new CategoryModel
				{
					Name = "Macbook",
					Slug = "macbook",
					Description = "Macbook is a large product in the world",
					status = 1
				};

				CategoryModel pcCategory = new CategoryModel
				{
					Name = "PC",
					Slug = "pc",
					Description = "PC is a large product in the world",
					status = 1
				};

				// Tạo dữ liệu mẫu cho BrandModel
				BrandModel appleBrand = new BrandModel
				{
					Name = "Apple",
					Slug = "apple",
					Description = "Apple is a large brand in the world",
					status = 1
				};

				BrandModel samsungBrand = new BrandModel
				{
					Name = "Samsung",
					Slug = "samsung",
					Description = "Samsung is a large brand in the world",
					status = 1
				};

				// Thêm sản phẩm mẫu vào Products
				_context.Products.AddRange(
					new ProductModel
					{
						Name = "Macbook 2024",
						Slug = "macbook",
						Description = "Macbook is the best",
						Image = "1.jpg",
						Category = macbookCategory,
						Brand = appleBrand,
						Price=121212
					},
					new ProductModel
					{
						Name = "PC core i5",
						Slug = "pc",
						Description = "PC is the best",
						Image = "2.jpg",
						Category = pcCategory,
						Brand = samsungBrand,
						Price = 121212
					}
				);

				// Lưu thay đổi vào cơ sở dữ liệu
				_context.SaveChanges();
			}
		}

	}
}
