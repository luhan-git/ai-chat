namespace ai_chat.Settings;

public class SpeechSettings
{
    public string ApiUrl { get; set; } = "https://api.elevenlabs.io/v1/text-to-speech/{voice_id}/stream";
    public string ApiKey{get;set;}="sk_6818097cd288d5a8b4ccc517a2b8b40b3fb7ab6ffb2505ff";
    public string VoiceId{get;set;}="21m00Tcm4TlvDq8ikWAM";
    
}