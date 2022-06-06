using Microsoft.EntityFrameworkCore;
using Ordering.Core.Orders;

namespace Ordering.DataAccess
{
    public class OrderingContext : DbContext
    {
        public virtual DbSet<Order> Orders { get; set; }
        public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
        {
        }
    }
}