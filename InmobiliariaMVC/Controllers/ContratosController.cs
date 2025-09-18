using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace InmobiliariaMVC.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repo;
        private readonly RepositorioInquilino repoInquilino;
        private readonly RepositorioInmueble repoInmueble;

        public ContratosController(RepositorioContrato repo, RepositorioInquilino repoInquilino, RepositorioInmueble repoInmueble)
        {
            this.repo = repo;
            this.repoInquilino = repoInquilino;
            this.repoInmueble = repoInmueble;
        }

        // GET: Contratos
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        // GET: Contratos/Details/5
        public IActionResult Details(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            return View(contrato);
        }

        // GET: Contratos/Create
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
            try
            {
                repo.Alta(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }
        }

        // GET: Contratos/Edit/5
        public IActionResult Edit(int id)
        {
            var contrato = repo.ObtenerPorId(id);
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
                repo.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
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
            var contrato = repo.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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
