using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using AI_Customer_Service_Lee_8900.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.SemanticKernel.ChatCompletion;
using AI_Customer_Service_Lee_8900.Data;

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
            //var chromaContext = "";
            string userPrompt = "";
            string systemPrompt = "";

            if (!string.IsNullOrEmpty(chromaContext) && chromaContext != "Error querying" && chromaContext != "No relevant context found.")
            {
                systemPrompt = $"Respond to the next user prompt as a BCIT customer service agent using the following data if the data is irrelevant to the prompt, ignore it\n\n. {chromaContext}";
                _chatHistoryService.ChatHistory.Add(new ChatMessage(ChatRole.System, systemPrompt));
                userPrompt = $"{value}";
            } 
            else
            {
                userPrompt = value;
            }

            //userPrompt = value;
            _chatHistoryService.ChatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            var response = "";

            await foreach (var item in chatClient.CompleteStreamingAsync(_chatHistoryService.ChatHistory))
            {
                response += item.Text;
            }
            _chatHistoryService.ChatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

            return new string[] { response.Replace("\n", "<br />") };
        }

        [Route("save-chat")]
        [HttpPost]
        public async Task<string> SaveCurrentChat([FromBody] ChatSaveRequst value)
        {   
            using (var context = new ApplicationDbContext())
            {
                var currentChat = new Conversations() {
                    Name = value.chatname,
                    UserId = value.userid,
                    Timestamp = DateTime.Now,
                    Message = "./Data/Storage/testFile.json"
                };
                context.Conversations.Add(currentChat);
                context.SaveChanges();

                currentChat.Message = $"./Data/Storage/{currentChat.ConversationId}.json";
                context.SaveChanges();

                if (SaveChatHistory($"./Data/Storage/{currentChat.ConversationId}.json", _chatHistoryService.ChatHistory))
                {
                    return "Saved";
                }
                else
                {
                    return "Failed";
                }
            }
        }

        [HttpGet("load-chat/{id}")]
        public async Task<ActionResult<List<ChatMessage>>> LoadChat(int id)
        {
            try
            {
                
                var history  = LoadChatHistory($"./Data/Storage/{id}.json");
                if (history != null)
                {
                    _chatHistoryService.ChatHistory = history;
                }
                return Ok(_chatHistoryService.ChatHistory);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while loading the chat history.", error = ex.Message });
            }
            
            return null;
        }

        public bool SaveChatHistory(string filePath, List<ChatMessage> chatHistory)
        {

            try
            {
                string json = JsonSerializer.Serialize(chatHistory, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(filePath, json);
                return true;
            } catch (Exception e)
            {
                return false;
            }
        }

        static List<ChatMessage> LoadChatHistory(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath)) return null;
                string json = System.IO.File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<ChatMessage>>(json) ?? new List<ChatMessage>();
            } 
            catch (Exception e)
            {
                return null;
            }
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
                process.WaitForExit(3000);

                return string.IsNullOrEmpty(error) ? result.Trim() : "";//$"Python Error: {error.Trim()}";
            }
        }
    }

    public class ChatSaveRequst
    {
        public string chatname { get; set; }
        public int userid { get; set; }
    }
}
