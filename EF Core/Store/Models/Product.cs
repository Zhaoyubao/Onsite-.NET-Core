using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Please input the product name!")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int Quantity { get; set; }
    }
}