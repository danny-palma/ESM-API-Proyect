using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                VersionsInformation[] VersionsResponse = JsonSerializer.Deserialize<VersionsInformation[]>(ResponseBody);
                Console.WriteLine($"Message : {VersionsResponse}");
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
