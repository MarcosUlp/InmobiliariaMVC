using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace InmobiliariaMVC.Controllers
{
    [Authorize] // Todos los ABM requieren usuario logueado
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
        public IActionResult Index(bool activos = true)
        {
            var lista = repoPago.ObtenerTodos(activos);
            ViewBag.ActivosActual = activos;
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
            return View();
        }

        // POST: Pagos/BuscarContratosPorDni
        [HttpPost]
        public IActionResult BuscarContratosPorDni(string dni)
        {
            var contratos = repoContrato.ObtenerPorDniInquilino(dni);
            return PartialView("_ListaContratosPartial", contratos);
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                pago.FechaPago = DateTime.Now;
                repoPago.Alta(pago);
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public IActionResult Edit(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null) return NotFound();

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
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        public IActionResult Delete(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repoPago.Eliminar(id); // ahora hace baja lógica
            return RedirectToAction(nameof(Index));
        }

    }
}
