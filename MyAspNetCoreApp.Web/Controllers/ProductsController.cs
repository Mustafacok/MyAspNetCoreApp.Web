using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyAspNetCoreApp.Web.Filters;
using MyAspNetCoreApp.Web.Helpers;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;

namespace MyAspNetCoreApp.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductsController : Controller
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;
        //private IHelper _helper;
        private readonly IFileProvider _fileProvider;
        private readonly ProductRepository _productRepository;
        public ProductsController(AppDbContext context, IMapper mapper, IFileProvider fileProvider) /* IHelper helper*/
        {
            //DI Container
            //Dependency Injection Pattern
            _productRepository = new ProductRepository();
            _context = context;
            _mapper = mapper;
            _fileProvider = fileProvider;
            //_helper = helper;   

            //Linq
            //if (!_context.Products.Any())
            //{
            //    _context.Products.Add(new Product { Name = "Kalem1", Price = 100, Stock = 99, Color = "Red" });
            //    _context.Products.Add(new Product { Name = "Silgi", Price = 80, Stock = 9999, Color = "Blue" });
            //    _context.Products.Add(new Product { Name = "Kağıt", Price = 50, Stock = 99, Color = "Green" });
            //    _context.Products.Add(new Product { Name = "Defter", Price = 90, Stock = 9999, Color = "Yellow" });

            //    _context.SaveChanges();
            //}
        }

        //[CacheResourceFilter]
        public IActionResult Index() /*[FromServices]IHelper helper2*/
        {
            //var text = "Asp.Net";
            //IHelper helper = new Helper();  
            //var upperText = _helper.Upper(text);

            //var status = _helper.Equals(helper2);
            List<ProductViewModel> products = _context.Products.Include(x => x.Category).Select(x=> new ProductViewModel() { 
            Id=x.Id,
            Name=x.Name,
            Price=x.Price,
            CategoryName=x.Category.Name,
            Color=x.Color,
            Description=x.Description,
            Expire=x.Expire,
            ImagePath=x.ImagePath,
            isPublish=x.isPublish,
            PublishDate=x.PublishDate
            }).ToList();

            return View(products);
        }

        //[HttpGet("{page}/{pageSize}")]
        [Route("{page}/{pageSize}", Name = "productpage")]
        public IActionResult Pages(int page, int pageSize)
        {
            //page=1 pagesize=3 -- ilk 3 kayıt
            //page=2 pagesize=3 -- ikinci 3 kayıt

            var products = _context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(_mapper.Map<List<ProductViewModel>>(products));
        }


        //attribute routing
        //[Route("[controller]/[action]/{productid}")]
        //[Route("[action]/{productid}",Name ="product")]

        //[NotFoundFilter()] // içerisinde parametre almazsa bu şekilde tanımlanır.
        [ServiceFilter(typeof(NotFoundFilter))] // içerisinde parametre aldığı için bu şekilde tanımlanır.
        [Route("urun/{productid}", Name = "product")]
        public IActionResult GetById(int productid)
        {
            var product = _context.Products.Find(productid);

            return View(_mapper.Map<ProductViewModel>(product));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpGet("{id}")]
        public IActionResult Remove(int id)
        {
            var product = _context.Products.Find(id);

            _context.Products.Remove(product);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Add()
        {

            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1 Ay",1 },
                {"3 Ay",3 },
                {"6 Ay",6 },
                {"12 Ay",12 }
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
                new() {Data="Mavi" ,Value="Mavi" },
                new() {Data="Kırmızı",Value="Kırmızı" },
                new() {Data="Sarı",Value="Sarı" }
            }, "Value", "Data");

            var categories = _context.Category.ToList();
            ViewBag.categorySelect = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel newProduct)
        {
            //Request Header-Body

            //1.yöntem
            //var name = HttpContext.Request.Form["Name"].ToString();
            //var price = decimal.Parse(HttpContext.Request.Form["Price"].ToString());
            //var stock = int.Parse(HttpContext.Request.Form["Stock"].ToString());
            //var color = HttpContext.Request.Form["Color"].ToString();

            // 2.Yöntem
            //Product newProduct = new Product() { Name = Name, Price = Price, Color = Color, Stock = Stock };

            //if (!string.IsNullOrEmpty(newProduct.Name) && newProduct.Name.StartsWith("A"))
            //{
            //    ModelState.AddModelError(String.Empty, "Ürün ismi A harfi ile başlayamaz.");
            //}

            IActionResult result = null;
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<Product>(newProduct);
                    if (newProduct.Image != null && newProduct.Image.Length > 0)
                    {
                        var root = _fileProvider.GetDirectoryContents("wwwroot");
                        var images = root.First(x => x.Name == "images");


                        var randomImageName = Guid.NewGuid() + Path.GetExtension(newProduct.Image.FileName);

                        var path = Path.Combine(images.PhysicalPath, randomImageName);

                        using var stream = new FileStream(path, FileMode.Create);
                        newProduct.Image.CopyTo(stream);

                        product.ImagePath = randomImageName;
                    }

                    //throw new Exception("db hatası");  
                    _context.Products.Add(product);
                    _context.SaveChanges();

                    TempData["status"] = "Ürün Başarıyla Eklendi.";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    result = View();
                }
            }
            else
            {
                result = View();
            }

            var categories = _context.Category.ToList();
            ViewBag.categorySelect = new SelectList(categories, "Id", "Name");

            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1 Ay",1 },
                {"3 Ay",3 },
                {"6 Ay",6 },
                {"12 Ay",12 }
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
                new() {Data="Mavi" ,Value="Mavi" },
                new() {Data="Kırmızı",Value="Kırmızı" },
                new() {Data="Sarı",Value="Sarı" }
            }, "Value", "Data");

            return result;
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        // id leri productid yaptım ilk satırda
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Find(id);
            var categories = _context.Category.ToList();
            ViewBag.categorySelect = new SelectList(categories, "Id", "Name",product.CategoryId);

            ViewBag.ExpireValue = product.Expire;
            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1 Ay",1 },
                {"3 Ay",3 },
                {"6 Ay",6 },
                {"12 Ay",12 }
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
                new() {Data="Mavi" ,Value="Mavi" },
                new() {Data="Kırmızı",Value="Kırmızı" },
                new() {Data="Sarı",Value="Sarı" }
            }, "Value", "Data", product.Color);

            return View(_mapper.Map<ProductUpdateViewModel>(product));
        }
        [HttpPost]
        public IActionResult Update(ProductUpdateViewModel updateProduct)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.radioExpireValue = updateProduct.Expire;
                ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1 Ay",1 },
                {"3 Ay",3 },
                {"6 Ay",6 },
                {"12 Ay",12 }
            };

                ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
                new() {Data="Mavi" ,Value="Mavi" },
                new() {Data="Kırmızı",Value="Kırmızı" },
                new() {Data="Sarı",Value="Sarı" }
            }, "Value", "Data", updateProduct.Color);

                var categories = _context.Category.ToList();
                ViewBag.categorySelect = new SelectList(categories, "Id", "Name",updateProduct.CategoryId);

                return View();
            }

            if (updateProduct.Image != null && updateProduct.Image.Length > 0)
            {
                var root = _fileProvider.GetDirectoryContents("wwwroot");
                var images = root.First(x => x.Name == "images");


                var randomImageName = Guid.NewGuid() + Path.GetExtension(updateProduct.Image.FileName);

                var path = Path.Combine(images.PhysicalPath, randomImageName);

                using var stream = new FileStream(path, FileMode.Create);
                updateProduct.Image.CopyTo(stream);

                updateProduct.ImagePath = randomImageName;
            }

            var product = _mapper.Map<Product>(updateProduct);
            _context.Products.Update(product);
            _context.SaveChanges();

            TempData["status"] = "Ürün Başarıyla Güncellendi.";
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult HasProductName(string Name)
        {
            var anyProduct = _context.Products.Any(x => x.Name.ToLower() == Name.ToLower());
            if (anyProduct)
            {
                return Json("Ürün ismi veritabanında bulunmaktadır.");
            }
            else
            {
                return Json(true);
            }
        }
    }
}
