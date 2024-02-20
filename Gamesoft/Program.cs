using Microsoft.EntityFrameworkCore;
using Gamesoft.Contexts;
using Gamesoft.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Gamesoft.Models;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Gamesoft.Helpers;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

builder.Services.AddDbContext<Gamesoft.Contexts.GamesoftContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IAccountMgt, AccountMgt>();
builder.Services.AddScoped<IEmailMgt, EmailMgt>();
builder.Services.AddScoped<IProductMgt, ProductMgt>();
builder.Services.AddScoped<IImageMgt, ImageMgt>();
builder.Services.AddScoped<SessionHelper>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

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

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NewsHub>("/newsHub");

app.UseCookiePolicy();
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
