using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Agri_Stock_Dashboard.DTOs;

namespace Agri_Stock_Dashboard.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseApiUrl = "http://localhost:8080"; //server java
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseApiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };//tratam maparea case sensitive sa se trateze la fel chiar daca difera putin ce primim
        }

        // GET
        public async Task<List<Machinery>> GetAllMachinery()  //cerere get
        {
            try
            {
                var response = await _httpClient.GetAsync("/machinery"); //trimite cererea
                response.EnsureSuccessStatusCode(); //se asigura ca este corect raspunsul
                return await response.Content.ReadFromJsonAsync<List<Machinery>>(_jsonOptions) ?? new List<Machinery>();//  transforma fisierul json in obiecte c# in caz ca serverul 
            }                                                                                                           //  returneaza null vom afisa null evitand erorile interfetei grafice
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Machinery>();
            }
        }

        public async Task<List<InventoryItem>> GetAllInventory()
        {
            try
            {
                var response = await _httpClient.GetAsync("/inventory");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<InventoryItem>>(_jsonOptions) ?? new List<InventoryItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<InventoryItem>();
            }
        }

        public async Task<List<Warehouse>> GetAllWarehouses()
        {
            try
            {
                var response = await _httpClient.GetAsync("/warehouses");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Warehouse>>(_jsonOptions) ?? new List<Warehouse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Warehouse>();
            }
        }

      

        // DELETE
        public async Task<bool> DeleteMachinery(int id)
        {
            try
            {
                return (await _httpClient.DeleteAsync($"/machinery/{id}")).IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteInventory(int id)
        {
            try
            {
                return (await _httpClient.DeleteAsync($"/inventory/{id}")).IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // UPDATE /PUT
        public async Task<bool> UpdateMachinery(int id, string status, string description, int warehouseId)
        {
            try
            {
                var payload = new { status = status, description = description, warehouseId = warehouseId }; //obiect anonim temporar care e practic UpdateMachinryDTO
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");// obiectul il serializam JSON 
                var response = await _httpClient.PutAsync($"/machinery/{id}/status", content);//si trimitem ulterior
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare update utilaj: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateInventory(int id, double quantity)
        {
            try
            {
                var payload = new { quantity = quantity };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/inventory/{id}/quantity", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare update stoc: {ex.Message}");
                return false;
            }
        }
        // AI
        public async Task<string> AskAi(string userPrompt) //preia textul din front si il transforma exact cum vrea bakendul
        {
            try
            {
                var request = new ChatbotRequest { Prompt = userPrompt };
                string jsonPayload = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });//fortam CamelCase pentru a primi bakendu-ul exact prompt si sa stie raspunde serializarii
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/chatbot", content);
                response.EnsureSuccessStatusCode(); //verificare corectitudine
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) { return $"Eroare: {ex.Message}"; }
        }
    }
}