using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyAspNetCoreApp.Web.Filters;
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

/*builder.Services.AddSingleton<IHelper, Helper>();*/ //DI container eklenen yer, IHelper ismindeki Interface i implemente eden helper s?n?f?ndaki nesneyi ?ret.

//builder.Services.AddScoped<IHelper, Helper>(); 
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.AddTransient<IHelper, Helper>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); //1.ad?m automapper

builder.Services.AddScoped<NotFoundFilter>(); //NotFoundFilter i?in gerekli.

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

//app.UseAuthentication(); //kimlik do?rulama
app.UseAuthorization(); //kimlik yetkilendirme

//Conventional Routing <------------
//app.MapControllerRoute(
//    name: "article",
//    pattern: "{controller=Blog}/{action=Article}/{name}/{id}");

//blog/abc -- blog controller > article action methodu ?al??s?n
//blog/eee -- blog controller > article action methodu ?al??s?n
//app.MapControllerRoute(
//    name: "pages",
//    pattern: "blog/{*article}/",
//    defaults:new {controller="Blog",action="Article" });

//controller/action/id
//routing tan?mlama
//app.MapControllerRoute(
//    name: "pages",
//    pattern: "{controller}/{action}/{page}/{pageSize}");

//app.MapControllerRoute(
//    name: "getbyid",
//    pattern: "{controller}/{action}/{productid}");

//app.MapControllerRoute(
//    name: "productgetbyid",
//    pattern: "{controller=products}/{action=getbyid}/{productid}");
// -------------->

//app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();
