using Microsoft.Extensions.Configuration;
using OpenAI;
using Microsoft.Extensions.AI;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string model = config["Modelname"] ?? "openai";
string key = config["OpenAIkey"] ?? "key";

IChatClient chatClient =
    new OpenAIClient(key).AsChatClient(model);

List<ChatMessage> chatHistory =
    [
        new ChatMessage(ChatRole.System, """
            Eres un asistente malhablado
        """)
    ];

while (true)
{
    // Get user prompt and add to chat history aa
    Console.WriteLine("Your prompt:");
    var userPrompt = Console.ReadLine();
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    // Stream the AI response and add to chat history a
    Console.WriteLine("AI Response:");
    var response = "";
    await foreach (var item in
        chatClient.CompleteStreamingAsync(chatHistory))
    {
        Console.Write(item.Text);
        response += item.Text;
    }
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine();
}