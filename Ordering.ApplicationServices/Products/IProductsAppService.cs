using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ApplicationServices.Products
{
    public interface IProductsAppService
    {
        Task<HttpResponseMessage> createOrderAsync(int id, int qty);
    }
}
