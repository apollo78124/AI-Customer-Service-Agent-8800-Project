import chromadb

def query_chroma_context(collection_name: str, query_text: str, top_k: int = 1):
    # Initialize ChromaDB client with Docker instance
    client = chromadb.HttpClient(host="localhost", port=8000)

    try:
        # Get the collection
        collection = client.get_collection(name=collection_name)

        # Query the collection
        results = collection.query(query_texts=[query_text], n_results=top_k)

        # Extract context from results
        context = results.get("documents", [[]])[0]

        if context:
            return " ".join(context)
        else:
            return "No relevant context found."
    except Exception as e:
        return f"Error querying collection '{collection_name}': {e}"


# Example usage
user_query = "Who are you"
context = query_chroma_context("docs", user_query)
print(f"Using this data: {context}. Respond to this prompt: {{prompt}} in one or two sentences")