using Azure;
using Azure.AI.OpenAI;
using static System.Environment;

namespace AzureOpenAI
{
    internal class Chat2
    {
        internal async static Task SendChat()
        {
            string endpoint = "https://westeurope.api.cognitive.microsoft.com/";

            OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(Environment.GetEnvironmentVariable("AI_TOKEN")!));

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                {                    
                    new ChatMessage(ChatRole.System, "You are an AI assistant that helps people find information. If the user asks you a question you don't know the answer to, say so."),

                    new ChatMessage (ChatRole.User, @"Describe the role of an investment bank"),

                },
                MaxTokens = 200
            };

            Response<StreamingChatCompletions> response = await client.GetChatCompletionsStreamingAsync(
                deploymentOrModelName: "gpt-35-turbo",
                chatCompletionsOptions);
            using StreamingChatCompletions streamingChatCompletions = response.Value;

            await foreach (StreamingChatChoice choice in streamingChatCompletions.GetChoicesStreaming())
            {
                await foreach (ChatMessage message in choice.GetMessageStreaming())
                {
                    Console.Write(message.Content);
                }
                Console.WriteLine();
            }
        }
    }
}
 