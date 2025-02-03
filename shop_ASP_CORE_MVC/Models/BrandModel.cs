using System.ComponentModel.DataAnnotations;

namespace shop_ASP_CORE_MVC.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Hay nhap ten thuong hieu")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Hay nhap ten mo ta thuong hieu")]
		public string Description { get; set; }
		public string Slug { get; set; }
		public int status { get; set; }
	}
}
