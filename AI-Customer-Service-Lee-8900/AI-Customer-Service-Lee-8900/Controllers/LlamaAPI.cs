using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AI_Customer_Service_Lee_8900.Controllers
{
    [Route("api")]
    [ApiController]
    public class LlamaAPI : ControllerBase
    {
        IChatClient chatClient;
        List<ChatMessage> chatHistory = new();
        public LlamaAPI ()
        {
            chatClient =
    new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.1");

        }

        //[HttpGet]
        //public string GetAsync()
        //{
        //    var userPrompt = "Hi, What's your name?";
        //    //chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

        //    var response = "string1";

        //    //    var task = chatClient.CompleteStreamingAsync(chatHistory);
        //    //    await foreach (var item in
        //    //    chatClient.CompleteStreamingAsync(chatHistory))
        //    //    {
        //    //        response += item.Text;
        //    //    }
        //    //    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

        //    return response;
        //}

        // GET: api/<LlamaAPI>
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var userPrompt = "Hi, What's your name?";
            chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            var response = "";

            var task = chatClient.CompleteStreamingAsync(chatHistory);
            await foreach (var item in
            chatClient.CompleteStreamingAsync(chatHistory))
            {
                response += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

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
            chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            var response = "";

            var task = chatClient.CompleteStreamingAsync(chatHistory);
            await foreach (var item in
            chatClient.CompleteStreamingAsync(chatHistory))
            {
                response += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

            return new string[] { response };
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
