using Microsoft.AspNetCore.Mvc;
using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;

namespace InmobiliariaMVC.Controllers
{
    [Authorize] // Todo el ABM requiere usuario logueado
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietario _repo;

        public PropietariosController(RepositorioPropietario repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var propietario = _repo.ObtenerPorId(id.Value);
            if (propietario == null) return NotFound();

            return View(propietario);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,Apellido,Dni,Telefono,Email")] Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Alta(propietario);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear propietario: " + ex.Message);
                }
            }
            return View(propietario);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var propietario = _repo.ObtenerPorId(id.Value);
            if (propietario == null) return NotFound();

            return View(propietario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("IdPropietario,Nombre,Apellido,Dni,Telefono,Email")] Propietario propietario)
        {
            if (id != propietario.IdPropietario) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Editar(propietario);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al editar propietario: " + ex.Message);
                }
            }
            return View(propietario);
        }

        // GET: Propietarios/Delete/5
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var propietario = _repo.ObtenerPorId(id.Value);
            if (propietario == null) return NotFound();

            bool tieneContratosActivos = _repo.TieneInmueblesConContratosActivos(id.Value);
            ViewBag.TieneContratosActivos = tieneContratosActivos;

            return View(propietario);
        }

        // POST: Propietarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool tieneContratosActivos = _repo.TieneInmueblesConContratosActivos(id);
            if (tieneContratosActivos)
            {
                var propietario = _repo.ObtenerPorId(id);
                ModelState.AddModelError("", "No se puede dar de baja al propietario porque tiene inmuebles con contratos activos.");
                ViewBag.TieneContratosActivos = true;
                return View(propietario);
            }

            _repo.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
