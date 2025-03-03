using System.Diagnostics;
using AI_Customer_Service_Lee_8900.Data;
using AI_Customer_Service_Lee_8900.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;

namespace AI_Customer_Service_Lee_8900.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new ViewModel1 { userId = -1 };
            model.chats = new List<Conversations>();

            if (HttpContext.Request.Cookies["userId"] != null) {
                model.userId = int.Parse(HttpContext.Request.Cookies["userId"]);
                using (var context = new ApplicationDbContext())
                {
                    var foundUser = context.Users.FirstOrDefault(u => u.Id == model.userId);
                    if (foundUser != null)
                    {
                        model.username = foundUser.Name;
                        model.chats.AddRange(context.Conversations.Where(c => c.UserId == foundUser.Id));

                    } else
                    {
                        model.username = "Unknown User";
                    }
                }
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
