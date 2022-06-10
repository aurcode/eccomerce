using Newtonsoft.Json;
using Ordering.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ApplicationServices.Users
{
    public class UsersAppService : IUsersAppService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersAppService(IHttpClientFactory httpClient)
        {
            _httpClientFactory = httpClient;
        }
        
        public async Task<GetUserResponseDto> FindUserByTokenAsync(string token)
        {
            HttpClient client = _httpClientFactory.CreateClient("user");
            HttpResponseMessage response;
            client.DefaultRequestHeaders.Add("Authorization", token);
            response = await client.GetAsync($"/token");

            if (response.IsSuccessStatusCode && response != null)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(json);
                //var jsonString = await response.Content.ReadAsStringAsync();
                
                
                return JsonConvert.DeserializeObject<GetUserResponseDto>(json);
            }
            else
            {
                throw new Exception("User not found");
            }            
        }
    }
}
