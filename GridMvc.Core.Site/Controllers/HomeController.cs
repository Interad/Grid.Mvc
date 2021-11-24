using GridMvc.Core.Site.Code;
using Microsoft.AspNetCore.Mvc;

namespace GridMvc.Core.Site.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(Report.ListAll());
        }
        public IActionResult Details()
        {
            return View(Report.ListAll());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
