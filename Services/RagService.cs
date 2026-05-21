using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using LocalRAGCore.Models;

namespace LocalRAGCore.Services;

public class RagService
{
    private readonly EmbeddingService _embeddingService;
    private readonly VectorStoreService _vectorStore;

    private readonly IChatCompletionService _chatService;

    public RagService(
        EmbeddingService embeddingService,
        VectorStoreService vectorStore)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;

        var builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion(
            modelId: "phi3",
            endpoint: new Uri("http://localhost:11434"));

        var kernel = builder.Build();

        _chatService = kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<string> AskAsync(string question)
    {
        var queryEmbedding =
            await _embeddingService.GenerateEmbeddingAsync(question);

        var relevantChunks = _vectorStore.Search(queryEmbedding,1);

        var context = relevantChunks.First().Content;
        var prompt = $@"
You are a helpful assistant.

Answer ONLY from the provided context.

Context:
{context}

Question:
{question}
";

        var response = await _chatService.GetChatMessageContentAsync(prompt);

        return response.Content ?? string.Empty;
    }
}