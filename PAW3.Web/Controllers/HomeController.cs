using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PAW3.Architecture.Helpers;
using PAW3.Web.Filters;
using PAW3.Web.Models;
using PAW3.Web.Models.ViewModels;
using System.Diagnostics;

namespace PAW3.Web.Controllers
{
    [RequireLogin]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IFormatHelper _format;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //_format = format;
        }

        public IActionResult Index()
        {
            /*var model = new ProductViewModel //Es solo un ejemplo de cómo se podría implementar el uso de FormatHelper en un controlador
            {
                ProductName = _format.ToTitleCase("sample product name"),
                Description = _format.Truncate("This is a very long product description that needs to be truncated for display purposes.", 50)
                //WelcomeMessage = _format.ToTitleCase("welcome to the home page!"),
                //date = _format.FormatDateLong(DateTime.Now),
                //price = _format.FormatCurrency(12345.6789m),
                //ago = _format.TimeAgo(DateTime.Now.AddHours(-3))
            };*/

            return View();
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
