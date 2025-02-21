using ai_chat.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ai_chat.Dependencies;

public static class Dependency
{
    public static void AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddSingleton<IOllama,Ollama>();  
        services.AddSingleton<ISpeech,Speech>();
        services.AddSingleton<IRecordAudio,RecordAudio>();
        services.Configure<ElevenLabsSettings>(configuration.GetSection("ElevenLabs"));
        services.Configure<DeepGramSettings>(configuration.GetSection("DeepGram"));
        services.Configure<OllamaSettings>(configuration.GetSection("Ollama"));
    }
}