using Microsoft.AspNetCore.Mvc;

namespace SignalrD1.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
