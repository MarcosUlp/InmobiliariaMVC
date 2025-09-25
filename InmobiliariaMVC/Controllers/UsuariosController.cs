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
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(repo.ObtenerTodos());
    }

    // GET: /Usuarios/Login
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Usuarios/Login
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password)
    {
        var usuario = repo.ObtenerPorEmail(email);
        if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.ClaveHash))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Credenciales incorrectas";
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
    [AllowAnonymous]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Usuarios/Create
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Usuario usuario, string password)
    {
        usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(password);
        repo.Alta(usuario);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Usuarios/Edit/1
    [AllowAnonymous]
    public IActionResult Edit(int id)
    {
        var usuario = repo.ObtenerPorId(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    // POST: /Usuarios/Edit/1
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Usuario usuario, string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        repo.Modificar(usuario);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Usuarios/Delete/1
    [AllowAnonymous]
    public IActionResult Delete(int id)
    {
        var usuario = repo.ObtenerPorId(id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    // POST: /Usuarios/Delete/1
    [HttpPost, ActionName("Delete")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        repo.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }
}
