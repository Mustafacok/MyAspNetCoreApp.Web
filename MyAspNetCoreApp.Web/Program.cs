using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Web.Helpers;
using MyAspNetCoreApp.Web.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

/*builder.Services.AddSingleton<IHelper, Helper>();*/ //DI container eklenen yer, IHelper ismindeki Interface i implemente eden helper sýnýfýndaki nesneyi üret.

//builder.Services.AddScoped<IHelper, Helper>(); 

builder.Services.AddTransient<IHelper, Helper>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); //1.adým automapper

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "article",
    pattern: "{controller=Blog}/{action=Article}/{name}/{id}");

//blog/abc -- blog controller > article action methodu çalýþsýn
//blog/eee -- blog controller > article action methodu çalýþsýn
//app.MapControllerRoute(
//    name: "pages",
//    pattern: "blog/{*article}/",
//    defaults:new {controller="Blog",action="Article" });

//controller/action/id
//routing tanýmlama
app.MapControllerRoute(
    name: "pages",
    pattern: "{controller}/{action}/{page}/{pageSize}");

app.MapControllerRoute(
    name: "getbyid",
    pattern: "{controller}/{action}/{productid}");

//app.MapControllerRoute(
//    name: "productgetbyid",
//    pattern: "{controller=products}/{action=getbyid}/{productid}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
