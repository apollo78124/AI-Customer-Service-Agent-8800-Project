using Microsoft.Extensions.AI;
using Microsoft.AspNetCore.Http;


namespace AI_Customer_Service_Lee_8900.Models
{   

    public interface IChatHistoryService
    {
        List<ChatMessage> ChatHistory { get; }
        IChatClient chatClient { get; }
    }

    public class ChatHistoryService : IChatHistoryService
    {
        public List<ChatMessage> ChatHistory { get; } = new List<ChatMessage>();
        public IChatClient chatClient { get; } = new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.1");

        public ChatHistoryService()
        {
            ChatHistory.Add(new ChatMessage(ChatRole.System, "You are a BCIT customer service agent. You will answer the customer's questions best to your knowledge as a representative of the BCIT customer service department. When asked who you are, say you are the AI customer service chatbot working for BCIT. If you don't know the answer, just say that \"I don't know\" but don't make up an answer on your own."));
            
        }
    }
}
