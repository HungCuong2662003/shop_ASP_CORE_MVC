namespace shop_ASP_CORE_MVC.Models
{
    public class Paginate
    {
        public int TotalItems { get; private set; } // Tổng số items
        public int PageSize { get; private set; }   // Tổng số item/trang
        public int CurrentPage { get; private set; } // Trang hiện tại
        public int TotalPages { get; private set; }  // Tổng số trang
        public int StartPage { get; private set; }   // Trang bắt đầu
        public int EndPage { get; private set; }     // Trang kết thúc

        public class Paginate
        {
            public int TotalItems { get; private set; } //tổng số items
            public int PageSize { get; private set; } //tổng số item/trang
            public int CurrentPage { get; private set; } //trang hiện tại

            public int TotalPages { get; private set; } //tổng trang
            public int StartPage { get; private set; } //trang bắt đầu
            public int EndPage { get; private set; } //trang kết thúc
            public Paginate()
            {

            }
            public Paginate(int totalItems, int page, int pageSize = 10)
            {
                // Tính tổng số trang
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                // Đảm bảo CurrentPage nằm trong khoảng hợp lệ
                CurrentPage = Math.Max(1, Math.Min(page, TotalPages));

                // Số trang hiển thị tối đa (tối đa 10 trang)
                int maxPagesToShow = 10;
                int halfWindow = maxPagesToShow / 2;

                // Tính toán StartPage và EndPage
                StartPage = Math.Max(1, CurrentPage - halfWindow);
                EndPage = Math.Min(TotalPages, StartPage + maxPagesToShow - 1);

                // Điều chỉnh nếu số trang hiển thị nhỏ hơn maxPagesToShow
                if (EndPage - StartPage + 1 < maxPagesToShow)
                {
                    StartPage = Math.Max(1, EndPage - maxPagesToShow + 1);
                }

                // Gán giá trị
                TotalItems = totalItems;
                PageSize = pageSize;
            }
        }


    }

}
