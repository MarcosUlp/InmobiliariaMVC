// Reemplaza TODO el contenido de este archivo

using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace InmobiliariaMVC.Controllers
{
    public class InmueblesController : Controller
    {
        // Declaramos los repositorios que vamos a usar
        private readonly RepositorioInmueble repo;
        private readonly RepositorioPropietario repoPropietario;

        // ¡ESTE ES EL GRAN CAMBIO!
        // El constructor ahora RECIBE los repositorios que necesita.
        // Ya no usa "new".
        public InmueblesController(RepositorioInmueble repo, RepositorioPropietario repoPropietario)
        {
            this.repo = repo;
            this.repoPropietario = repoPropietario;
        }

        // GET: InmueblesController
        public ActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: InmueblesController/Details/5
        public ActionResult Details(int id)
        {
            var inmueble = repo.ObtenerPorId(id);
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
                repo.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                return View(inmueble);
            }
        }

        // GET: InmueblesController/Edit/5
        public ActionResult Edit(int id)
        {
            var inmueble = repo.ObtenerPorId(id);
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
                repo.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                return View(inmueble);
            }
        }

        // GET: InmueblesController/Delete/5
        public ActionResult Delete(int id)
        {
            var inmueble = repo.ObtenerPorId(id);
            return View(inmueble);
        }

        // POST: InmueblesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repo.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                 ViewBag.Error = ex.Message;
                 return View();
            }
        }
    }
}