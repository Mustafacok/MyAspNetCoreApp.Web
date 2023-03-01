using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace MyAspNetCoreApp.Web.Filters
{
    public class LogFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Action Metot Çalışmadan Önce");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Action metot çalıştıktan sonra");
        }
        
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Debug.WriteLine("Action metot sonuç üretilmeden önce");
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Debug.WriteLine("Action metot sonuç üretildikten sonra");
        }




    }
}
