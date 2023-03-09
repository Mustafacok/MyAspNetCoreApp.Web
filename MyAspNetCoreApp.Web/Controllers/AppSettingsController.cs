using Microsoft.AspNetCore.Mvc;

namespace MyAspNetCoreApp.Web.Controllers
{
    //[Route("[controller]/[action]")]
    public class AppSettingsController : Controller
    {

        private readonly IConfiguration _configuration;

        public AppSettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.baseUrl = _configuration["baseUrl"];
            ViewBag.smsKey = _configuration["Keys:Sms"];
            ViewBag.emailKey= _configuration.GetSection("Keys")["email"];
            return View();
        }
    }
}
