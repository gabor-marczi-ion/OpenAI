using Azure.AI.OpenAI;
using Azure;

namespace AzureOpenAI
{
    internal class Chat
    {

        internal async static Task SendChat()
        {
            OpenAIClient client = new OpenAIClient(
                new Uri("https://westeurope.api.cognitive.microsoft.com/"),
                new AzureKeyCredential(key: Environment.GetEnvironmentVariable("AI_TOKEN")!));

            // ### If streaming is selected
            Response<StreamingCompletions> response = await client.GetCompletionsStreamingAsync(
                deploymentOrModelName: "gpt-35-turbo",
                new CompletionsOptions()
                {
                    Prompts = { @"Provide a summary of the text below: \n\n Former President Donald Trump acknowledged on tape in a 2021 meeting that he had retained “secret” military information that he had not declassified, according to a transcript of the audio recording obtained by CNN. “As president, I could have declassified, but now I can’t,” Trump says, according to the transcript.CNN obtained the transcript of a portion of the meeting where Trump is discussing a classified Pentagon document about attacking Iran. In the audio recording, which CNN previously reported was obtained by prosecutors, Trump says that he did not declassify the document he’s referencing, according to the transcript.
                        Trump was indicted Thursday on seven counts in special counsel Jack Smith’s investigation into the mishandling of classified documents. Details from the indictment have not been made public, so it unknown whether any of the seven counts refer to the recorded 2021 meeting. Still, the tape is significant because it shows that Trump had an understanding the records he had with him at Mar-a-Lago after he left the White House remained classified.
                        Publicly, Trump has claimed that all the documents he brought with him to his Florida residence are declassified, while he’s railed against the special counsel’s investigation as a political witch hunt attempting to interfere with his 2024 presidential campaign.
                        " },
                    
                    Temperature = (float)0.5,
                    MaxTokens = 350,
                    NucleusSamplingFactor = (float)0.95,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0,
                });

            //Completions completions = response.Value;
            //Console.WriteLine(completions.);

            using StreamingCompletions streamingCompletions = response.Value;

            await foreach (StreamingChoice choice in streamingCompletions.GetChoicesStreaming())
            {
                await foreach (string message in choice.GetTextStreaming())
                {
                    Console.Write(message);
                }
                Console.WriteLine();
            }


        }
    }
}
