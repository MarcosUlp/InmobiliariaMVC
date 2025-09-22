using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace InmobiliariaMVC.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repoContrato;
        private readonly RepositorioInquilino repoInquilino;
        private readonly RepositorioInmueble repoInmueble;

        public ContratosController(RepositorioContrato repoContrato, RepositorioInquilino repoInquilino, RepositorioInmueble repoInmueble)
        {
            this.repoContrato = repoContrato;
            this.repoInquilino = repoInquilino;
            this.repoInmueble = repoInmueble;
        }

        // GET: Contratos
        public IActionResult Index()
        {
            var lista = repoContrato.ObtenerTodos();
            return View(lista);
        }

        // GET: Contratos/Details/5
        public IActionResult Details(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // GET: Contratos/Create
        // GET: Contratos/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }

            bool disponible = repoContrato.InmuebleDisponible(contrato.IdInmueble, contrato.FechaInicio, contrato.FechaFin);
            if (!disponible)
            {
                ModelState.AddModelError("", "El inmueble ya está reservado o alquilado en esas fechas.");
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }

            contrato.Estado = true;

            repoContrato.Alta(contrato);

            return RedirectToAction(nameof(Index));
        }

        // GET: Contratos/Edit/5
        public IActionResult Edit(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                contrato.IdContrato = id;
                repoContrato.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }
        }

        // GET: Contratos/Delete/5
        public IActionResult Delete(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repoContrato.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

    }
}
