using Microsoft.AspNetCore.Mvc;

namespace MyAspNetCoreApp.Web.Controllers
{
    //blog/article/makale-ismi/id
    public class BlogController : Controller
    {                
        public IActionResult Article(string name, int id)
        {
            //var routes = Request.RouteValues["article"];


            return View();
        }
    }
}
