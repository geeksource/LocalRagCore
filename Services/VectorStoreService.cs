using LocalRAGCore.Models;

namespace LocalRAGCore.Services;

public class VectorStoreService
{
    private readonly List<DocumentChunk> _chunks = new();

    public void Add(DocumentChunk chunk)
    {
        _chunks.Add(chunk);
    }

    public List<DocumentChunk> Search(float[] queryEmbedding, int topK = 3)
    {
        return _chunks
            .Select(c => new
            {
                Chunk = c,
                Score = CosineSimilarity(c.Embedding, queryEmbedding)
            })
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .Select(x => x.Chunk)
            .ToList();
    }

    private float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0;
        float magA = 0;
        float magB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / ((float)(Math.Sqrt(magA) * Math.Sqrt(magB)) + 1e-5f);
    }
}