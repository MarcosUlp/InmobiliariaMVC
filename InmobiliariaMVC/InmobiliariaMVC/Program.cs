// Reemplaza TODO el contenido de este archivo

using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- AQUÍ ESTÁ LA MAGIA ---
// 1. Creamos UNA SOLA instancia de Database para toda la aplicación.
builder.Services.AddSingleton(new Database(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Le decimos al sistema cómo crear cada repositorio cuando un controlador lo pida.
builder.Services.AddTransient<RepositorioInquilino>();
builder.Services.AddTransient<RepositorioPropietario>();
builder.Services.AddTransient<RepositorioInmueble>();
// --- FIN DE LA MAGIA ---

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