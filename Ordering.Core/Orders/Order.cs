using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Orders
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int clientId { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        [MaxLength(500)]
        public string Comments { get; set; }
    }
}
