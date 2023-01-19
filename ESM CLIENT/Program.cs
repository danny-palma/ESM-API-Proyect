using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESM_CLIENT
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage Response = await client.GetAsync("http://192.168.35.130");
                Response.EnsureSuccessStatusCode();
                string ResponseBody = await Response.Content.ReadAsStringAsync();
                Console.WriteLine($"Message : {ResponseBody}");
                Console.ReadKey();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\n Exeption caught! ");
                Console.WriteLine($"Message : {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
