// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using UdemyEFCore.DatabaseFirstByScaffold.Models;

Console.WriteLine("Hello, World!");

using(var context = new UdemyEfcoreDatabaseFirstDbContext())
{
    var products = await context.Products.ToListAsync();

    products.ForEach(p =>
    {
        Console.WriteLine($"{p.Id} :{p.Name} - {p.Price} - {p.Stock}");
    });
}