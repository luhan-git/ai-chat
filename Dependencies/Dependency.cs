using Microsoft.Extensions.DependencyInjection;

namespace ai_chat.Dependencies;

public static class Dependency
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IOllama,Ollama>();  
        services.AddSingleton<ISpeech,Speech>();
        services.AddSingleton<IRecordAudio,RecordAudio>();
    }
}