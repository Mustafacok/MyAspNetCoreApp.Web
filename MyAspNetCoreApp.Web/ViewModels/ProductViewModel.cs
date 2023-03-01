using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyAspNetCoreApp.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Remote(action:"HasProductName",controller:"Products")]
        [StringLength(50,ErrorMessage ="İsim Alanına en fazla 50 karakter girilebilir.")]
        [Required(ErrorMessage ="İsim Alanı Boş Geçilmez")]
        public string? Name { get; set; }

        //[RegularExpression(@"^[0-9]+(\.[0-9]{1,2})",ErrorMessage ="Fiyat alanı noktadan sonra en fazla iki basamak olmalıdır.")]
        [Required(ErrorMessage = "Fiyat Alanı Boş Geçilmez")]
        [Range(1,1000, ErrorMessage = "Fiyat 1-1000 TL Arasında Olmalıdır")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Stok Alanı Boş Geçilmez")]
        [Range(1,9999, ErrorMessage = "Stok Alanı 1-9999 Arasında Olmalıdır")]
        public int? Stock { get; set; }

        [StringLength(50,MinimumLength =50, ErrorMessage = "Açıklama Alanı 50-300 karakter arasında olabilir.")]
        [Required(ErrorMessage = "Açıklama Alanı Boş Geçilmez")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Renk Seçimi Boş Olamaz")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Yayınlanma Tarihi Boş Olamaz")]
        public DateTime? PublishDate { get; set; }

        public bool isPublish { get; set; }

        [Required(ErrorMessage = "Ürünün Satışta Kalma Süresi Boş Olamaz")]
        public int? Expire { get; set; }

        //[EmailAddress(ErrorMessage ="Email adresi uygun değildir.")]
        //public string? EmailAddress { get; set; }
    }
}
