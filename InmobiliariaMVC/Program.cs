// Reemplaza TODO el contenido de este archivo

using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1 sola instancia global de database
builder.Services.AddSingleton(new Database(builder.Configuration.GetConnectionString("DefaultConnection")));

// esto es para que los repos puedan ser usados por los controladores
builder.Services.AddTransient<RepositorioInquilino>();
builder.Services.AddTransient<RepositorioPropietario>();
builder.Services.AddTransient<RepositorioInmueble>();
builder.Services.AddTransient<RepositorioContrato>();
builder.Services.AddTransient<RepositorioPago>();

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

app.Run();