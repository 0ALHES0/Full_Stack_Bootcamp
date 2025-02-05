using FilmLog.Data;
using Microsoft.EntityFrameworkCore;
using FilmLog.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("database");
    options.UseSqlite(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔹 Veritabanına başlangıç verilerini ekle
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    if (!context.Categories.Any()) // Eğer tablo boşsa, ekle
    {
        context.Categories.AddRange(
            new Category { Name = "Aksiyon" },
            new Category { Name = "Korku" },
            new Category { Name = "Komedi" }
        );
        context.SaveChanges();
    }
}

app.Run();
