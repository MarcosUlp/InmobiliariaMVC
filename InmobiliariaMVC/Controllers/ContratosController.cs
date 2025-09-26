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
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.InmueblesDisponibles = null;
            return View(new Contrato());
        }

        // POST: Crear Contrato / Listar inmuebles disponibles
        [HttpPost]
        // POST: Crear Contrato / Listar inmuebles disponibles
        [HttpPost]
        public IActionResult Create(Contrato contrato, string accion, DateTime? FechaInicio, DateTime? FechaFin)
        {
            // Botón "Listar inmuebles disponibles"
            if (accion == "listar")
            {
                if (!FechaInicio.HasValue || !FechaFin.HasValue || FechaInicio > FechaFin)
                {
                    TempData["Error"] = "Debes ingresar un rango de fechas válido.";
                    return RedirectToAction(nameof(Create));
                }

                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                ViewBag.InmueblesDisponibles = repoContrato.ObtenerInmueblesDisponibles(FechaInicio.Value, FechaFin.Value);
                ViewBag.FechaInicio = FechaInicio.Value;
                ViewBag.FechaFin = FechaFin.Value;

                return View(contrato);
            }

            // Botón "Crear contrato"
            if (!ModelState.IsValid)
            {
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }

            // VERIFICAR DISPONIBILIDAD DEL INMUEBLE
            bool disponible = repoContrato.InmuebleDisponible(
                contrato.IdInmueble,
                contrato.FechaInicio,
                contrato.FechaFin
            );

            if (!disponible)
            {
                ModelState.AddModelError("", "El inmueble seleccionado ya está alquilado o reservado en esas fechas.");
                ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                return View(contrato);
            }

            // Crear contrato
            contrato.CreadoPor = int.Parse(User.FindFirstValue("IdUsuario")); // asigna creador
            repoContrato.Alta(contrato);
            TempData["Success"] = "Contrato creado correctamente";
            return RedirectToAction("Index");
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
        [HttpGet]
        public IActionResult InmueblesDisponibles(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue || fechaInicio > fechaFin)
            {
                TempData["Error"] = "Debes ingresar un rango de fechas válido.";
                return RedirectToAction(nameof(Create));
            }

            var disponibles = repoContrato.ObtenerInmueblesDisponibles(fechaInicio.Value, fechaFin.Value);
            ViewBag.FechaInicio = fechaInicio.Value;
            ViewBag.FechaFin = fechaFin.Value;

            return View(disponibles);
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
