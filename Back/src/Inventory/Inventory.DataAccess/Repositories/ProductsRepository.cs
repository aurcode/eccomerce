using Inventory.Core.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Repositories
{
    public class ProductsRepository : Repository<int, Product>
    {
        public ProductsRepository(InventoryContext context) : base(context)
        {
        }

        public override IQueryable<Product> GetAll()
        {
            var products = _context.Products.Include(x => x.Brand).Include(x => x.Category);
            return products;
        }

        public override async Task<Product> GetAsync(int id)
        {
            var product = await _context.Products.Include(x => x.Brand).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }
    }
}
