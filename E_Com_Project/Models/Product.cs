using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com_Project.Models
{
    [Table("Product")]
    public class Product

    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Nom { get; set; }
        [Required]
        public double Prix { get; set; }
        [Required]
        public int QteStock { get; set; } 
        public DateTime DateCreation { get; set; }
        public string? Image { get; set; }

        // Clé étrangère vers la catégorie
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]

        // Propriété de navigation vers la catégorie à laquelle ce produit appartient
        public Category? Category { get; set; }

        public ICollection<OrderDetails>? OrderDetails { get; set; }
        public ICollection<CartLine>? CartLine { get; set; }
       
    }

}
