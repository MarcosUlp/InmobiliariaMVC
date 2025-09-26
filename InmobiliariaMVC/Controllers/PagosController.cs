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

        // GET: Pagos/Auditoria
        [Authorize(Roles = "Administrador")] // Solo admins pueden ver auditoría
        public IActionResult Auditoria()
        {
            var lista = repoPago.ObtenerTodos(); // Trae todos los pagos, activos e inactivos
                                                               // Para mostrar nombre completo de usuarios, cargamos aquí mismo
            foreach (var pago in lista)
            {
                // Si hay CreadoPor
                if (pago.CreadoPor.HasValue)
                    pago.UsuarioCreacion = repoPago.ObtenerUsuario(pago.CreadoPor.Value);
                // Si hay AnuladoPor
                if (pago.AnuladoPor.HasValue)
                    pago.UsuarioAnulacion = repoPago.ObtenerUsuario(pago.AnuladoPor.Value);
            }
            return View(lista);
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                pago.FechaPago = DateTime.Now;
                int usuarioId = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
                repoPago.Alta(pago, usuarioId);
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
            int usuarioId = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
            repoPago.Eliminar(id, usuarioId);
            return RedirectToAction(nameof(Index));
        }


    }
}
