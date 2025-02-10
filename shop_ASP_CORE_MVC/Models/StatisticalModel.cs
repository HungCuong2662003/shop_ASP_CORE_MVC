using System.ComponentModel.DataAnnotations;

namespace shop_ASP_CORE_MVC.Models
{
    public class StatisticalModel
    {
        [Key]
        public int Id { get; set; }
        public decimal revenue { get; set; }
        public int orders { get; set; }
        public DateTime date { get; set; }

    }
}
