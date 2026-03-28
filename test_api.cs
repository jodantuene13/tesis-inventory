using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        // Login
        var loginContent = new StringContent("{\"email\":\"admin@ucc.edu.ar\", \"password\":\"123456\"}", Encoding.UTF8, "application/json");
        var loginResp = await httpClient.PostAsync("http://localhost:5139/api/auth/login", loginContent);
        var loginStr = await loginResp.Content.ReadAsStringAsync();
        
        string token = "";
        try {
            var doc = JsonDocument.Parse(loginStr);
            token = doc.RootElement.GetProperty("token").GetString();
        } catch {
            Console.WriteLine("Login failed: " + loginStr);
            return;
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Sede-Contexto", "1");

        var url = "http://localhost:5139/api/Stock/movimientos/historial?page=1&pageSize=50";
        var resp = await httpClient.GetAsync(url);
        Console.WriteLine(await resp.Content.ReadAsStringAsync());
    }
}
