using System.ComponentModel.DataAnnotations;

namespace shop_ASP_CORE_MVC.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                // Lấy phần mở rộng của file (ví dụ: ".jpg")
                var extension = Path.GetExtension(file.FileName)?.ToLower(); // Chuyển về chữ thường để so sánh đúng
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg" };

                // Kiểm tra xem phần mở rộng có thuộc danh sách hợp lệ không
                if (!allowedExtensions.Contains(extension)) // Lỗi nếu đuôi không hợp lệ
                {
                    return new ValidationResult($"Allowed extensions are: {string.Join(", ", allowedExtensions)}");
                }
            }

            // Trả về thành công nếu file hợp lệ hoặc không phải file
            return ValidationResult.Success;
        }


    }
}
