using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cambiamos AutorService por AutorServiceAlternativo
builder.Services.AddScoped<BibliotecaMVC.Services.IAutorService, BibliotecaMVC.Services.AutorServiceAlternativo>();

builder.Services.AddDbContext<BibliotecaMVC.Models.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
