using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ApplicationServices.Products
{
    public class ProductsAppService : IProductsAppService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsAppService(IHttpClientFactory httpClient)
        {
            _httpClientFactory = httpClient;
        }
        public async Task<HttpResponseMessage> createOrderAsync(int id, int qty)
        {
            HttpClient client = _httpClientFactory.CreateClient("inventory");
            HttpResponseMessage response;
            string url = "Products";
            response = await client.GetAsync($"{url}/{id}/{qty}");

            return response.IsSuccessStatusCode ? response : null;
        }
    }
}
