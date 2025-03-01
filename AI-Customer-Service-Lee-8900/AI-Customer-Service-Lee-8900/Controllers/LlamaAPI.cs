using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using AI_Customer_Service_Lee_8900.Models;
using System.Diagnostics;
using DocumentFormat.OpenXml.InkML;

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
            chatClient = _chatHistoryService.chatClient;
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
            var chromaContext = GetContext(value);
            string userPrompt = "";

            if (!string.IsNullOrEmpty(chromaContext) && chromaContext != "Error querying" && chromaContext != "No relevant context found.")
            { 
                userPrompt = $"Respond to this prompt as a BCIT customer service agent: {value} \n using the following data if the data is irrelevant to the prompt, ignore it\n\n. {chromaContext}";
            } 
            else
            {
                userPrompt = "Respond to the following user prompt as a BCIT customer service agent. If you don't know the answer, do not make it up. \n" + value;
            }

            //userPrompt = value;
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

        public string GetContext(string query)
        {
            try
            {
                string pythonScriptPath = "../AI-Customer-Service-Lee-8900/ChromaDBContext/GetContextFromChromaDb.py";
                string result = RunPythonScript(pythonScriptPath, query);
                return result;
            }
            catch (Exception ex)
            {
                return "Error querying";
            }
        }

        private string RunPythonScript(string scriptPath, string argument)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{scriptPath} \"{argument}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit(5000);

                return string.IsNullOrEmpty(error) ? result.Trim() : $"Python Error: {error.Trim()}";
            }
        }
    }
}
