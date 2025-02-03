using shop_ASP_CORE_MVC.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop_ASP_CORE_MVC.Models
{
	public class ProductModel
	{
		[Key]
		public long Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Hay nhap ten san pham")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Hay nhap  mo ta san pham")]
		public string Description { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        [Column(TypeName = "decimal(20, 2)")]
        public decimal Price { get; set; }
        public string Slug { get; set; }
		public string Image { get; set; } = "noimage.jpg";

        [Required(ErrorMessage = "Chọn một thương hiệu")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn một thương hiệu hợp lệ")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Chọn một danh mục")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục hợp lệ")]
        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile ImageUpload { get; set; }
	}
}
