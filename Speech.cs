using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ai_chat.Settings;
using Microsoft.Extensions.Options;

namespace ai_chat;

public interface ISpeech
{
    Task<string> SpeakAsync(string text);
    Task PlayAudioAsync(string filePath);
}
public class Speech(HttpClient httpClient,IOptions<SpeechSettings>settings):ISpeech
{
    private readonly SpeechSettings _settings=settings.Value;
    public async Task<string> SpeakAsync(string text)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Content");
        Directory.CreateDirectory(folderPath);
        var filePath = Path.Combine(folderPath, "output.mp3");
        var url = _settings.ApiUrl.Replace("{voice_id}", _settings.VoiceId);
        var requestBody = new
        {
            text = text,
            model_id = "eleven_multilingual_v2",
            voice_settings = new { stability = 0.5, similarity_boost = 0.5 }
        };
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("xi-api-key", _settings.ApiKey);
        request.Content = content;
        var response = await httpClient.SendAsync(request);
        if(!response.IsSuccessStatusCode) Console.WriteLine(response.StatusCode);
        var audioData=await response.Content.ReadAsByteArrayAsync();
        await File.WriteAllBytesAsync(filePath, audioData);
        return filePath;
    }

    public async Task PlayAudioAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("El archivo de audio no existe.");
            return;
        }

        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "play",
                Arguments = $"\"{filePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
            Console.WriteLine("Reproduciendo audio...");
            await process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al reproducir el audio: {ex.Message}");
        }
    }
}