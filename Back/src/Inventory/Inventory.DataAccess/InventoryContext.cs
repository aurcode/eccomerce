using Inventory.Core.Brands;
using Inventory.Core.Categories;
using Inventory.Core.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess
{
    public class InventoryContext : DbContext
    {
        // create core and entities
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }
    }
}
