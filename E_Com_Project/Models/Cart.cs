using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com_Project.Models
{
    [Table("Cart")]
    public class Cart
    {
            public int Id { get; set; }
            public string? UserName { get; set; }

            // Propriété de navigation vers les lignes de panier
            public ICollection<CartLine>? CartLines { get; set; }
        
    }
}
