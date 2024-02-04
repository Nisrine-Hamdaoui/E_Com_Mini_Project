using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com_Project.Models
{
    [Table("CartLine")]
    public class CartLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public int CartId { get; set; }
        public double UnitPrice { get; set; }


        // Propriétés de navigation
        public Product? Product { get; set; }
        public Cart? Cart { get; set; }
    }
}
