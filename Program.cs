using System.Text.Json;
using ai_chat;
using ai_chat.Dependencies;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDependencies();
var serviceProvider = services.BuildServiceProvider();
var ollama=serviceProvider.GetRequiredService<IOllama>();
var speaker = serviceProvider.GetRequiredService<ISpeech>();
while (true)
{
    Console.WriteLine("user:");
    var question = Console.ReadLine();
    if(string.IsNullOrEmpty(question)) continue;
    if(question=="exit") break;
    var response=await ollama.AskAsync(question);
    using var doc = JsonDocument.Parse(response);
    var content = doc.RootElement.GetProperty("message").GetProperty("content").GetString() ?? "Sin respuesta";
    Console.WriteLine(content);
    var filePath=await speaker.SpeakAsync(content);
    await speaker.PlayAudioAsync(filePath);
}
