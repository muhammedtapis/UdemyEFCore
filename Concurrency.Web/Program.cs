using Concurrency.Web.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{   //buraya AppDbContext nesnemi servis olarak ekledi�im i�in bu nesneye ihtiyac�m�z oldu�unda new anahtar s�zc��� ile �retmiycez.
    //herhangi bir s�n�f�n constructorunda veya methodunda AppDbContext ge�ti�imizde framework bize bir nesne �rne�i verir.(dependency injection)
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
