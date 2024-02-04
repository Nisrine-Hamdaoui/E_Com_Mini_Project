using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com_Project.Models
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        public int Id { get; set; }
        public int  Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int MyProperty { get; set; }
        [Required]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order? Order { get; set; }
        public Product? Product { get; set; }


    }

}
