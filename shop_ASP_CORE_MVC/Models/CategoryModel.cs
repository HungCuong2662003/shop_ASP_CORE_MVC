using System.ComponentModel.DataAnnotations;

namespace shop_ASP_CORE_MVC.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Hay nhap ten danh muc")]
		public string Name { get; set; }
		[Required( ErrorMessage = "Hay nhap ten mo ta danh muc")]
		public string Description { get; set; }
		public string Slug { get; set; }
		public int status { get; set; }
	}
}
