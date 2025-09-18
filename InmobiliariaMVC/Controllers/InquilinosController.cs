using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InmobiliariaMVC.Models;
// Asegúrate de que el namespace coincida con tu estructura de carpetas (PascalCase es la convención)
using InmobiliariaMVC.Repositories; 

namespace InmobiliariaMVC.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino _repo;

        // El nombre del tipo debe coincidir con el nombre de la clase (PascalCase)
        public InquilinosController(RepositorioInquilino repo)
        {
            _repo = repo;
        }

        // GET: Inquilinos
        // Quitamos async Task<> porque no hay 'await'
        public IActionResult Index()
        {
            var lista = _repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilinos/Details/5
        // Quitamos async Task<>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquilino = _repo.ObtenerPorId(id.Value);
            if (inquilino == null)
            {
                return NotFound();
            }

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
        // Quitamos async Task<>
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
        // Quitamos async Task<>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquilino = _repo.ObtenerPorId(id.Value);

            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Quitamos async Task<>
        public IActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                if (id != inquilino.IdInquilino)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    // La variable correcta es _repo (con guion bajo)
                    _repo.Editar(inquilino); 
                    return RedirectToAction(nameof(Index));
                }
                return View(inquilino);
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilinos/Delete/5
        // Quitamos async Task<>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquilino = _repo.ObtenerPorId(id.Value);
            if (inquilino == null)
            {
                return NotFound();
            }

            return View(inquilino);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Quitamos async Task<>
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