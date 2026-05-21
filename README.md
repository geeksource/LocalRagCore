# LocalRagCore

A modern **Retrieval-Augmented Generation (RAG)** API built with .NET 10 and Semantic Kernel, enabling intelligent document search and question-answering over local documents using Ollama LLMs.

## 🚀 Features

- **Document Ingestion** - Load and process TXT and PDF files from local directories
- **Text Chunking** - Intelligent document splitting for optimal embedding quality
- **Vector Storage** - In-memory vector database for semantic search
- **Local LLM Integration** - Connect to Ollama-hosted language models (phi3, llama2, etc.)
- **Semantic Search** - Find contextually relevant documents using embeddings
- **RESTful API** - Simple HTTP endpoints for indexing and querying
- **Swagger UI** - Interactive API documentation included

## 📋 Prerequisites

- **.NET 10 SDK** or later
- **Ollama** installed and running locally (default: `http://localhost:11434`)
- Document files in `Data/docs` directory (TXT and PDF formats supported)

## 🛠️ Setup & Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/geeksource/LocalRagCore.git
   cd LocalRagCore
   ```

2. **Install Ollama** (if not already installed)
   - Download from https://ollama.ai
   - Start Ollama service: `ollama serve`
   - Pull desired model: `ollama pull phi3` (or your preferred model)

3. **Prepare your documents**
   ```bash
   mkdir -p Data/docs
   # Add your .txt and .pdf files to the Data/docs directory
   ```

4. **Restore dependencies and run**
   ```bash
   dotnet restore
   dotnet run
   ```

   The API will start at `https://localhost:5001`

## 📚 API Endpoints

### Index Documents
Index all documents in the `Data/docs` directory and generate embeddings.

**Request:**
```http
POST /api/Prep
```

**Response:**
```json
"Indexed successfully"
```

### Query Documents
Ask a question against the indexed documents. The system retrieves relevant context and generates an answer.

**Request:**
```http
GET /api/chat?question=What%20is%20your%20question%20here?
```

**Response:**
```json
{
  "answer": "Generated response based on document context..."
}
```

## 🏗️ Architecture

### Core Components

- **DocumentService** - Loads TXT and PDF files from disk
- **TextChunker** - Splits documents into manageable chunks (default: 150 tokens)
- **EmbeddingService** - Generates vector embeddings using Ollama
- **VectorStoreService** - In-memory vector database for semantic search
- **RagService** - Orchestrates the RAG pipeline (embedding → search → LLM)
- **ChatController** - HTTP API for querying
- **PrepController** - HTTP API for document indexing

### Data Flow

```
Documents (TXT/PDF)
    ↓
[DocumentService] - Load files
    ↓
[TextChunker] - Split into chunks
    ↓
[EmbeddingService] - Generate embeddings
    ↓
[VectorStoreService] - Store vectors
    ↓
User Question
    ↓
[EmbeddingService] - Embed question
    ↓
[VectorStoreService] - Search similar chunks
    ↓
[RagService] - Build prompt with context
    ↓
[Ollama LLM] - Generate answer
```

## 🔧 Configuration

Key settings in `Program.cs`:

- **HTTP Timeout** - Set to 10 minutes for Ollama requests (handles long inference times)
- **Model** - Default model is `phi3` (change in `RagService.cs` line 25)
- **Ollama Endpoint** - `http://localhost:11434` (configurable)
- **Chunk Size** - 150 tokens per chunk (configurable in `PrepController.cs`)

Environment-specific settings:
- `appsettings.json` - Production configuration
- `appsettings.Development.json` - Development configuration

## 📦 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.AspNetCore.OpenApi | 10.0.8 | OpenAPI support |
| Microsoft.SemanticKernel | 1.76.0 | LLM orchestration |
| Microsoft.SemanticKernel.Connectors.Ollama | 1.76.0-alpha | Ollama integration |
| Qdrant.Client | 1.18.1 | Vector database client |
| CsvHelper | 33.1.0 | CSV data parsing |
| UglyToad.PdfPig | 1.7.0-custom-5 | PDF processing |
| Swashbuckle.AspNetCore | 10.1.7 | Swagger/OpenAPI UI |
| Microsoft.Extensions.AI | 10.6.0 | AI abstraction layer |

## 🚀 Quick Start Example

1. **Start Ollama and pull a model**
   ```bash
   ollama serve
   # In another terminal:
   ollama pull phi3
   ```

2. **Add sample documents**
   ```bash
   echo "Your document content here..." > Data/docs/sample.txt
   ```

3. **Start the API**
   ```bash
   dotnet run
   ```

4. **Index your documents**
   ```bash
   curl -X POST https://localhost:5001/api/Prep
   ```

5. **Ask a question**
   ```bash
   curl "https://localhost:5001/api/chat?question=What%20is%20in%20your%20documents?"
   ```

6. **Browse Swagger UI**
   - Navigate to `https://localhost:5001/swagger`

## 📝 Supported Document Formats

- **TXT** - Plain text files
- **PDF** - PDF documents (processed via UglyToad.PdfPig)

## ⚙️ Performance Tuning

- **Chunk Size** - Adjust in `PrepController.cs` (line 43) for accuracy vs. speed tradeoff
- **Search Results** - Modify `VectorStoreService.Search()` parameter to retrieve more/fewer chunks
- **Model Selection** - Use smaller models (mistral, neural-chat) for faster inference on limited hardware
- **Timeout** - Increase HttpClient timeout if processing large documents

## 🤝 Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## 📄 License

This project is part of the LocalRagCore repository. Check the repository for license details.

## 🆘 Troubleshooting

### "Connection refused" to Ollama
- Ensure Ollama is running: `ollama serve`
- Verify it's accessible at `http://localhost:11434`
- Check that the model is available: `ollama list`

### "No relevant context found"
- Verify documents are properly indexed via `/api/Prep` endpoint
- Check that document chunks contain relevant information
- Adjust chunk size for better semantic segmentation

### Slow response times
- Use a smaller/faster model (e.g., mistral instead of phi3)
- Increase the HttpClient timeout if needed
- Reduce the number of documents or optimize chunk size

## 📖 Additional Resources

- [Microsoft Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
- [Ollama GitHub](https://github.com/ollama/ollama)
- [RAG Concepts](https://blogs.microsoft.com/ai-blog/rag-retrieval-augmented-generation/)

---

**Built with ❤️ using .NET 10, Semantic Kernel, and Ollama**
