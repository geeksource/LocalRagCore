using UglyToad.PdfPig;

namespace LocalRAGCore.Services;

public class DocumentService
{
    public List<string> LoadTextFiles(string folder)
    {
        var files = Directory.GetFiles(folder, "*.txt");

        return files
            .Select(File.ReadAllText)
            .ToList();
    }

    public List<string> LoadPdfFiles(string folder)
    {
        var result = new List<string>();

        var files = Directory.GetFiles(folder, "*.pdf");

        foreach (var file in files)
        {
            using var pdf = PdfDocument.Open(file);

            var text = string.Join("\n",
                pdf.GetPages().Select(p => p.Text));

            result.Add(text);
        }

        return result;
    }
}