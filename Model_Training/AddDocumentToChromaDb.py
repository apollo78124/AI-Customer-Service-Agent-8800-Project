import chromadb


def get_collection_contents(collection_name: str):
    # Initialize ChromaDB client with Docker instance
    client = chromadb.HttpClient(host="localhost", port=8000)

    try:
        # Get the collection
        collection = client.get_collection(name=collection_name)

        # Fetch all documents (assuming IDs are known or retrieving all stored embeddings)
        documents = collection.get()  # Fetch all documents

        print(f"Contents of collection '{collection_name}':")
        print(documents)
    except Exception as e:
        print(f"Error retrieving collection '{collection_name}': {e}")


import chromadb


def add_document_to_collection(collection_name: str, doc_id: str, file_path: str):
    # Initialize ChromaDB client with Docker instance
    client = chromadb.HttpClient(host="localhost", port=8000)

    try:
        # Read document content from file
        with open(file_path, "r", encoding="utf-8") as file:
            document = file.read()

        # Get the collection
        collection = client.get_collection(name=collection_name)

        # Add document to the collection
        collection.add(ids=[doc_id], documents=[document])
        print(f"Document added to collection '{collection_name}' with ID '{doc_id}'.")
    except Exception as e:
        print(f"Error adding document to collection '{collection_name}': {e}")


# Example usage

# for i in range(1, 48):
#     doc_id = f"doc{i:03d}"
#     file_path = f"./{doc_id}.txt"
#     add_document_to_collection("docs2", doc_id, file_path)
get_collection_contents("docs2")

# client = chromadb.HttpClient(host="localhost", port=8000)
# client.get_or_create_collection(name="docs2")
