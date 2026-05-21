namespace LocalRAGCore.Services;

public class TextChunker
{
    public List<string> ChunkText(string text, int chunkSize = 500)
    {
        var chunks = new List<string>();

        for (int i = 0; i < text.Length; i += chunkSize)
        {
            var size = Math.Min(chunkSize, text.Length - i);

            chunks.Add(text.Substring(i, size));
        }

        return chunks;
    }
}