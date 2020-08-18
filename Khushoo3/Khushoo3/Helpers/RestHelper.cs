using System;
using System.Net.Http;
using System.Threading.Tasks;
using Khushoo3.Exceptions;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Khushoo3.Helpers
{
    public static class RestHelper
    {
        public static async Task<T> GetAsync<T>(string resource)
        {
            string json = string.Empty;
            // URL
            string uri = resource;
            
            
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new InternetConnectionException();
            }

            //Make Http Call
           // var httpClient = new HttpClient();
            using (HttpClient client = new HttpClient())
            {
                
                var response = await client.GetAsync(new Uri(uri));
                response.EnsureSuccessStatusCode();

                //Get json result
                json = await response.Content.ReadAsStringAsync();
            }
               

           
          

            
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
