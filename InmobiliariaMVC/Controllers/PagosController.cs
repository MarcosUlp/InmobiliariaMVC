using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InmobiliariaMVC.Controllers
{
    public class PagosController : Controller
    {
        private readonly RepositorioPago repoPago;
        private readonly RepositorioContrato repoContrato;

        public PagosController(RepositorioPago repoPago, RepositorioContrato repoContrato)
        {
            this.repoPago = repoPago;
            this.repoContrato = repoContrato;
        }

        // GET: Pagos
        public IActionResult Index()
        {
            var lista = repoPago.ObtenerTodos();
            return View(lista);
        }

        // GET: Pagos/Details/5
        public IActionResult Details(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            return View(pago);
        }

        // GET: Pagos/Create
        public IActionResult Create()
        {
            ViewBag.Contratos = repoContrato.ObtenerTodos();
            return View();
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            try
            {
                repoPago.Alta(pago);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Contratos = repoContrato.ObtenerTodos();
                return View(pago);
            }
        }

        // GET: Pagos/Edit/5
        public IActionResult Edit(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            ViewBag.Contratos = repoContrato.ObtenerTodos();
            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pago pago)
        {
            try
            {
                pago.IdPago = id;
                repoPago.Editar(pago);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Contratos = repoContrato.ObtenerTodos();
                return View(pago);
            }
        }

        // GET: Pagos/Delete/5
        public IActionResult Delete(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repoPago.Eliminar(id);
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