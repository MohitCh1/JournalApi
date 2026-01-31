using System.Text;
using System.Text.Json;
using static JournalApi.DTO.GeminiRequestDTO;
using static JournalApi.DTO.GeminiResponseDTO;

namespace JournalApi.Services
{
    public class GeminiServices
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration config;

        public GeminiServices(HttpClient httpClient,IConfiguration config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }

       public async Task<string> getSuggestion(string journaltext)
        {
            var apiKey = config["Gemini:ApiKey"];
            var baseUrl = config["Gemini:BaseUrl"];
            var model = config["Gemini:Model"];
           

            var url =$"{baseUrl}/{model}:generateContent?key={apiKey}";
         

            var request = new GeminiRequest
            {
                contents = new List<RequestContent>
    {
        new RequestContent
        {
            parts = new List<RequestPart>
            {
                new RequestPart
                {
                    text = $"{journaltext}"
                }
            }

        }
    }
            };



            var json =JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return $"Status: {response.StatusCode}, Body: {body}";
            }

            response.EnsureSuccessStatusCode();
           
            var responseJson=response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(await responseJson);
            return geminiResponse?.candidates?[0]?.content?.parts?[0]?.text
       ?? "No Suggestion";

        }
    }
}
