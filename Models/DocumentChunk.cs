namespace LocalRAGCore.Models;

public class DocumentChunk
{
    public Guid Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public float[] Embedding { get; set; } = Array.Empty<float>();
}