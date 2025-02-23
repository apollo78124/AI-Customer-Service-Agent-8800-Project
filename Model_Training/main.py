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


def add_document_to_collection(collection_name: str, doc_id: str, document: str):
    # Initialize ChromaDB client with Docker instance
    client = chromadb.HttpClient(host="localhost", port=8000)

    try:
        # Get the collection
        collection = client.get_collection(name=collection_name)

        # Add document to the collection
        collection.add(ids=[doc_id], documents=[document])
        print(f"Document added to collection '{collection_name}' with ID '{doc_id}'.")
    except Exception as e:
        print(f"Error adding document to collection '{collection_name}': {e}")


# Example usage
get_collection_contents("docs")
#add_document_to_collection("docs", "doc_001",
#                           "\r\n## **Advanced Forensic Nurse Examiner, Microcredential (0813CM) - BCIT**\r\n**Program Type:**  \r\n- **Microcredential** for forensic nurses to enhance skills, specialize in forensic healthcare, and maintain credibility in legal settings.\r\n- Builds upon the **Forensic Nurse Examiner microcredential**, offering **continuing education**.\r\n\r\n### **Overview:**\r\n- **Objective:**  \r\n  - Provide ongoing education for **Forensic Nurse Examiners (FNEs)** to **refresh skills, gain specialized competencies**, and **serve as expert witnesses in court**.\r\n  - Help prepare nurses **to treat vulnerable patients** while **aligning with legal and healthcare system requirements**.")
