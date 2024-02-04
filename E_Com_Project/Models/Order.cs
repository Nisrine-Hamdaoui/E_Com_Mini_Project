using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com_Project.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<OrderDetails>? OrderDetails { get; set; }


    }
}
