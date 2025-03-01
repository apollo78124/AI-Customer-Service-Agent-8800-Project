using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using AI_Customer_Service_Lee_8900.Models;
using System.Diagnostics;
using DocumentFormat.OpenXml.InkML;
using AI_Customer_Service_Lee_8900.Data;
using Microsoft.AspNetCore.Identity.Data;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AI_Customer_Service_Lee_8900.Controllers
{
    [Route("loginapi")]
    [ApiController]
    public class LoginAPI : ControllerBase
    {

        public LoginAPI() {
            
        }

        // GET: api/<LlamaAPI>
        [Route("getusers")]
        [HttpGet]
        public string GetUsers()
        {
            var result = "";
            var context = new ApplicationDbContext();
            context.Users.ToList();

            foreach (var user in context.Users) { 
                result += user.Name;
            }
            return result;
        }

        public class LoginRequest1
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [Route("login")]
        [HttpPost]
        public int Login([FromBody] LoginRequest1 request)
        {
            var result = -1;
            var username = request.Username;
            var password = request.Password;

            using (var context = new ApplicationDbContext())
            {
                var foundUser = context.Users.FirstOrDefault(u => u.Email.ToLower() == username);
                if (!string.IsNullOrEmpty(username) && foundUser != null)
                {
                    var credential = context.Credentials.Where(c => c.UserId == foundUser.Id).FirstOrDefault();
                    if (credential != null && password == credential.Password)
                    {
                        
                        result = foundUser.Id;
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddDays(1);
                        HttpContext.Response.Cookies.Append("userId", result.ToString(), option);
                    }
                }
            }
            
            return result;
        }

        [Route("logout")]
        public int Logout()
        {
            int result = -1;

            try
            {
                HttpContext.Response.Cookies.Delete("userId");
                result = 1;
            } 
            catch (Exception e)
            {
                return -1;
            }

            return result;
        }

        // GET api/<LlamaAPI>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


    }
}
