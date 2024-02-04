using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Com_Project.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? Nom { get; set; }

        // Propriété de navigation vers les produits associés à cette catégorie
        public ICollection<Product>? Products { get; set; }
    }
}
