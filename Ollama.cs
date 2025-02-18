using System.Text;
using System.Text.Json;
using ai_chat.Settings;
using Microsoft.Extensions.Options;

namespace ai_chat;

public interface IOllama
{
    Task<string> AskAsync(string question);
}
public class Ollama(HttpClient httpClient, IOptions<OllamaSettings> settings) : IOllama
{
    private readonly OllamaSettings _settings = settings.Value;

    public async Task<string> AskAsync(string prompt)
    {
        try
        {
            var requestBody = new
            {
                _settings.Model,
                messages = new[]
                {
                    new { role = "system", content = "Eres un sarcastico" },
                    new { role = "user", content = prompt }
                },
                stream = false
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(_settings.ApiUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error en la solicitud HTTP: {ex.Message}");
            return "Error en la solicitud HTTP.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
            return "Ocurrió un error inesperado.";
        }
    }
}