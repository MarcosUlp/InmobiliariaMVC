// Reemplaza TODO el contenido de este archivo

using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaMVC.Controllers
{
    [Authorize]
    public class InmueblesController : Controller
    {
        // Declaramos los repositorios que vamos a usar
        private readonly RepositorioInmueble repoInmueble;
        private readonly RepositorioPropietario repoPropietario;

        // ¡ESTE ES EL GRAN CAMBIO!
        // El constructor ahora RECIBE los repositorios que necesita.
        // Ya no usa "new".
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


        // GET: InmueblesController/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // GET: InmueblesController/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            return View();
        }

        // POST: InmueblesController/Create
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

        // GET: InmueblesController/Edit/5
        public ActionResult Edit(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            return View(inmueble);
        }

        // POST: InmueblesController/Edit/5
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

        // GET: InmueblesController/Delete/5
        public ActionResult Delete(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: InmueblesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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