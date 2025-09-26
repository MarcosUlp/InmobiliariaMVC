using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;

namespace InmobiliariaMVC.Controllers
{
    [Authorize] // Todos los ABM requieren usuario logueado
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino _repo;

        public InquilinosController(RepositorioInquilino repo)
        {
            _repo = repo;
        }

        // GET: Inquilinos
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilinos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var inquilino = _repo.ObtenerPorId(id.Value);
            if (inquilino == null) return NotFound();

            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("IdInquilino,Nombre,Apellido,Dni,Telefono,Email")] Inquilino inquilino)
        {
            if (ModelState.IsValid)
            {
                _repo.Alta(inquilino);
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var inquilino = _repo.ObtenerPorId(id.Value);
            if (inquilino == null) return NotFound();

            return View(inquilino);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Inquilino inquilino)
        {
            if (id != inquilino.IdInquilino) return NotFound();

            if (ModelState.IsValid)
            {
                _repo.Editar(inquilino);
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Delete/5
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var inquilino = _repo.ObtenerPorId(id.Value);
            if (inquilino == null) return NotFound();

            return View(inquilino);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

        private bool InquilinoExists(int id)
        {
            return _repo.ObtenerPorId(id) != null;
        }
    }
}
