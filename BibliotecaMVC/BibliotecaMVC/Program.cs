using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<BibliotecaMVC.Services.IAutorService, BibliotecaMVC.Services.AutorServiceAlternativo>();

builder.Services.AddDbContext<BibliotecaMVC.Models.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));
builder.Services.AddIdentity<Microsoft.AspNetCore.Identity.IdentityUser, Microsoft.AspNetCore.Identity.IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<BibliotecaMVC.Models.ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
    var roles = new[] { "Administrador", "Usuario" };

    foreach (var role in roles)
    {
        if (!roleManager.RoleExistsAsync(role).Result)
        {
            roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(role)).Wait();
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
