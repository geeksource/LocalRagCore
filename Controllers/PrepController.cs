using LocalRAGCore.Models;
using LocalRAGCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalRAGCore.Controllers;

[ApiController]
[Route("api/Prep")]
public class PrepController : ControllerBase
{
    private readonly DocumentService _documentService;
    private readonly TextChunker _chunker;
    private readonly EmbeddingService _embeddingService;
    private readonly VectorStoreService _vectorStore;

    public PrepController(
        DocumentService documentService,
        TextChunker chunker,
        EmbeddingService embeddingService,
        VectorStoreService vectorStore)
    {
        _documentService = documentService;
        _chunker = chunker;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Indexing endpoint");
    }

    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var docs = _documentService.LoadTextFiles("Data/docs");
        var pdfs = _documentService.LoadPdfFiles("Data/docs");
        
        docs.AddRange(pdfs);
        Console.WriteLine("docs count: " + docs.Count);
        foreach (var doc in docs)
        {
            var chunks = _chunker.ChunkText(doc,150);

            foreach (var chunk in chunks)
            {
                var embedding =
                    await _embeddingService.GenerateEmbeddingAsync(chunk);

                _vectorStore.Add(new DocumentChunk
                {
                    Id = Guid.NewGuid(),
                    Content = chunk,
                    Embedding = embedding
                });
            }
        }

        return Ok("Indexed successfully");
    }
}
