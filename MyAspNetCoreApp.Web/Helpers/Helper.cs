using MyAspNetCoreApp.Web.Models;

namespace MyAspNetCoreApp.Web.Helpers
{
    public class Helper : IHelper
    {

        private readonly AppDbContext _context;

        public Helper(AppDbContext context)
        {
            _context = context;
        }

        //private bool _isConfiguration;
        //public Helper(bool isConfiguration)
        //{
        //    _isConfiguration = isConfiguration;
        //}
        public string Upper(string text)
        {





            return text.ToUpper();
        }
    }
}
