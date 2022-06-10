using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Products.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(2000)]
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Qty { get; set; }
    }
}
