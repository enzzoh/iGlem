using Newtonsoft.Json;
using System.Text;

public class ChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatGptService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"]; // Defina isso no appsettings.json
    }

    public async Task<string> GetChatGptResponse(string documentText)
    {
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "Você é um assistente de processamento de texto. Por favor, identifique as palavras-chave importantes no texto fornecido abaixo." },
                new { role = "user", content = documentText }
            },
            max_tokens = 150
        };

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<dynamic>(result);
            var responseText = responseJson?.choices?[0]?.message?.content;
            return responseText ?? "Sem resposta";
        }

        return "Falha ao obter resposta";
    }
}
