using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreApp.Web.Filters;
using System.Security.Cryptography.Xml;

namespace MyAspNetCoreApp.Web.Controllers
{
    
    public class Product2
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    [CustomResultFilter("x-version","2.0")]
    [Route("[controller]/[action]")]
    public class OrnekController : Controller
    {
        public IActionResult Index()
        {

            var productList = new List<Product2>()
            {
                new(){Id=1, Name="Kalem"},
                new(){Id=2, Name="Defter"},
                new(){Id=3, Name="Kağıt"},
                new(){Id=4, Name="Silgi"}
            };

            ViewBag.name = "Asp.Net CORE";
            ViewData["age"] = 30;

            ViewData["names"] = new List<string>() { "ahmet", "mehmet", "mustafa" };

            ViewBag.Person = new { Id = 1, Name = "Mustafa", Age = 28 };

            TempData["surname"] = "ÇOK";

            return View(productList);
        }

        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult Index3()
        {
            return RedirectToAction("Index","Ornek");
        }

        public IActionResult ParametreView(int id)
        {

            return RedirectToAction("JsonResultParametre", "Ornek", new { id = id });
        }
        public IActionResult JsonResultParametre(int id)
        {
            return Json(new { Id = id });
        }
        public IActionResult ContentResult()
        {
            return Content("ContentResult");
        }
        public IActionResult JsonResult()
        {
            return Json(new { Id = 1, name = "kalem", price = 100 });    
        }
        public IActionResult EmptyResult()
        {
            return new EmptyResult();
        }
    }
}
