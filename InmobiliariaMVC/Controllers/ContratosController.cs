using InmobiliariaMVC.Models;
using InmobiliariaMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace InmobiliariaMVC.Controllers
{
    [Authorize] // Todos deben estar logueados para acceder
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
        public IActionResult Index(bool estado = true)
        {
            var lista = repoContrato.ObtenerPorEstado(estado);
            ViewBag.EstadoActual = estado;
            return View(lista);
        }
        // GET: Contratos/Auditoria
        [Authorize(Roles = "Administrador")]
        public IActionResult Auditoria()
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
            contrato.CreadoPor = int.Parse(User.FindFirstValue("IdUsuario")); // ✅ ahora sí
                                                                              // contrato.FechaCreacion lo pone el NOW() del repo, podés sacarlo si querés

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
            if (!ModelState.IsValid)
            {
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }

            bool disponible = repoContrato.InmuebleDisponible(
                contrato.IdInmueble,
                contrato.FechaInicio,
                contrato.FechaFin,
                id
            );

            if (!disponible)
            {
                ModelState.AddModelError("", "El inmueble ya está alquilado o reservado en esas fechas.");
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }

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
        [Authorize(Roles = "Administrador")] // Solo administradores pueden eliminar
        public IActionResult Delete(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue("IdUsuario"));
                repoContrato.Baja(id, userId);
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
