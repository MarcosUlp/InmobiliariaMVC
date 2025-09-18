using Microsoft.AspNetCore.Mvc;
using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories; // 👈 IMPORTANTE
using System;

namespace InmobiliariaMVC.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietario _repo;

        public PropietariosController(RepositorioPropietario repo)
        {
            _repo = repo;
        }

        // GET: Propietarios
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Propietarios/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var propietario = _repo.ObtenerPorId(id.Value);
            if (propietario == null) return NotFound();

            return View(propietario);
        }

        // GET: Propietarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,Apellido,Dni,Telefono,Email,Clave")] Propietario propietario)
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

        // GET: Propietarios/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var propietario = _repo.ObtenerPorId(id.Value);
            if (propietario == null) return NotFound();

            return View(propietario);
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("IdPropietario,Nombre,Apellido,Dni,Telefono,Email,Clave")] Propietario propietario)
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
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var propietario = _repo.ObtenerPorId(id.Value);
            if (propietario == null) return NotFound();

            return View(propietario);
        }

        // POST: Propietarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _repo.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar propietario: " + ex.Message);
                var propietario = _repo.ObtenerPorId(id);
                return View(propietario);
            }
        }

        private bool PropietarioExists(int id)
        {
            return _repo.ObtenerPorId(id) != null;
        }
    }
}
