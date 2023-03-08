using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MyAspNetCoreApp.Web.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //[ValidateNever]
        public List<Product>? Products { get; set; }
    }
}
