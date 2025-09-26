using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class UsuariosController : Controller
{
    private readonly RepositorioUsuario repo;

    public UsuariosController(RepositorioUsuario repo)
    {
        this.repo = repo;
    }

    // GET: /Usuarios
    [Authorize(Roles = "Administrador")]
    public IActionResult Index()
    {
        return View(repo.ObtenerTodos());
    }

    // GET: /Usuarios/Login
    [AllowAnonymous]
    public IActionResult Login() => View();

    // POST: /Usuarios/Login
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Debe ingresar email y contraseña.";
            return View();
        }

        var usuario = repo.ObtenerPorEmail(email);

        if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.ClaveHash))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("Avatar", usuario.Avatar ?? "/img/default-avatar.png")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Credenciales incorrectas.";
        return View();
    }

    // GET: /Usuarios/Logout
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    // GET: /Usuarios/Create
    [Authorize(Roles = "Administrador")]
    public IActionResult Create() => View();

    // POST: /Usuarios/Create
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Usuario usuario, string password)
    {
        usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(password);
        repo.Alta(usuario);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Usuarios/Edit/1
    [Authorize]
    public IActionResult EditarPerfil()
    {
        var id = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
        var usuario = repo.ObtenerPorId(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult EditarPerfil(Usuario model, string? nuevaClave, IFormFile? nuevoAvatar, bool eliminarAvatar = false)
    {
        var id = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
        var usuario = repo.ObtenerPorId(id);
        if (usuario == null) return NotFound();

        usuario.Nombre = model.Nombre;
        usuario.Apellido = model.Apellido;
        usuario.Email = model.Email;

        if (!string.IsNullOrEmpty(nuevaClave))
            usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(nuevaClave);

        if (nuevoAvatar != null)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(nuevoAvatar.FileName)}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                nuevoAvatar.CopyTo(stream);
            }

            usuario.Avatar = "/img/" + fileName;
        }
        else if (eliminarAvatar)
        {
            usuario.Avatar = "/img/default-avatar.png";
        }

        repo.Modificar(usuario);
        TempData["Msg"] = "Perfil actualizado correctamente.";
        return RedirectToAction("EditarPerfil");
    }

    // GET: /Usuarios/Delete/1
    [Authorize(Roles = "Administrador")]
    public IActionResult Delete(int id)
    {
        var usuario = repo.ObtenerPorId(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    // POST: /Usuarios/Delete/1
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        repo.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }
}
