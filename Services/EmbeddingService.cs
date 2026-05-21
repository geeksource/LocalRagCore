using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Embeddings;

namespace LocalRAGCore.Services;

public class EmbeddingService
{
    private readonly ITextEmbeddingGenerationService _embeddingService;

    public EmbeddingService()
    {
        var builder = Kernel.CreateBuilder();

        builder.AddOllamaTextEmbeddingGeneration(
            modelId: "nomic-embed-text",
            endpoint: new Uri("http://localhost:11434"));

        var kernel = builder.Build();

        _embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var embedding = await _embeddingService.GenerateEmbeddingAsync(text);

        return embedding.ToArray();
    }
}