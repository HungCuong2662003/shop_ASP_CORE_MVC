using Newtonsoft.Json;

namespace shop_ASP_CORE_MVC.Repository
{
	public static class SessionExtensions
	{
		public static void SetJson(this ISession session, string key, object value)
		{
			// Chuyển đổi đối tượng thành chuỗi JSON và lưu vào session
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		// Phương thức mở rộng để lấy dữ liệu dạng JSON từ Session
		public static T GetJson<T>(this ISession session, string key)
		{
			var jsonString = session.GetString(key);

			// Kiểm tra nếu chuỗi JSON không tồn tại, trả về giá trị mặc định
			return jsonString == null ? default : JsonConvert.DeserializeObject<T>(jsonString);
		}
	}
}
