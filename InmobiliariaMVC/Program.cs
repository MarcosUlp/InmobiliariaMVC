// Reemplaza TODO el contenido de este archivo

using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

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
builder.Services.AddTransient<RepositorioUsuario>(); // 👈 importante para auth

// 🔑 Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";   // si no está logueado, redirige acá
        options.LogoutPath = "/Usuarios/Logout"; // ruta de logout
        options.AccessDeniedPath = "/Home/AccesoDenegado"; // opcional
    });

// 🗂️ Habilitar sesiones (debe ir ANTES de builder.Build())
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//todos los controladores tienen RequireLogin por defecto 
//builder.Services.AddControllersWithViews(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
//});

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

// 🗂️ Activar sesiones ANTES de autenticación
app.UseSession();

// 🔑 Activar auth y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}");

app.Run();
