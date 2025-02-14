namespace ai_chat.Settings;

public class OllamaSettings
{
    public string Model { get; set; } = "llama3.1:8b";
    public string ApiUrl { get; set; } = "http://localhost:11434/api/chat";
}