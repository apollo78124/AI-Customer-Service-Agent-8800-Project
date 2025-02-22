using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using AI_Customer_Service_Lee_8900.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AI_Customer_Service_Lee_8900.Controllers
{
    [Route("api")]
    [ApiController]
    public class LlamaAPI : ControllerBase
    {
        IChatClient chatClient;

        private readonly IChatHistoryService _chatHistoryService;

        public LlamaAPI(IChatHistoryService chatHistoryService)
        {
            _chatHistoryService = chatHistoryService; 
            chatClient =
    new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.1");
        }



        // GET: api/<LlamaAPI>
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = "";

            return new string[] { response };
        }

        // GET api/<LlamaAPI>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IEnumerable<string>> PostAsync([FromBody] string value)
        {
            var userPrompt = value;
            var chatHistory = _chatHistoryService.ChatHistory;
            chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            var response = "";

            await foreach (var item in chatClient.CompleteStreamingAsync(chatHistory))
            {
                response += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

            return new string[] { response.Replace("\n", "<br />") };
        }

        // PUT api/<LlamaAPI>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LlamaAPI>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
