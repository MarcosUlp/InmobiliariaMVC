using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaMVC.Controllers
{
    [Authorize] // Todos los ABM requieren usuario logueado
    public class InmueblesController : Controller
    {
        private readonly RepositorioInmueble repoInmueble;
        private readonly RepositorioPropietario repoPropietario;

        public InmueblesController(RepositorioInmueble repoInmueble, RepositorioPropietario repoPropietario)
        {
            this.repoInmueble = repoInmueble;
            this.repoPropietario = repoPropietario;
        }

        // GET: Inmuebles
        public ActionResult Index(bool disponible = true)
        {
            var lista = repoInmueble.ObtenerTodos();
            var filtrados = lista.Where(i => i.Disponible == disponible).ToList();
            ViewBag.DisponibleActual = disponible;
            return View(filtrados);
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                repoInmueble.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                return View(inmueble);
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                inmueble.IdInmueble = id;
                repoInmueble.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                return View(inmueble);
            }
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        public ActionResult Delete(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, IFormCollection collection)
        {
            try
            {
                bool exito = repoInmueble.BajaLogica(id);
                if (!exito)
                {
                    ViewBag.Error = "No se puede dar de baja. Existen contratos activos asociados a este inmueble.";
                    var inmueble = repoInmueble.ObtenerPorId(id);
                    return View(inmueble);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var inmueble = repoInmueble.ObtenerPorId(id);
                return View(inmueble);
            }
        }

        [HttpGet]
        public IActionResult GetPrecio(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            if (inmueble == null)
                return NotFound();

            return Json(new { precio = inmueble.Precio });
        }
    }
}
