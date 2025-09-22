using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaMVC.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }

        [Required]
        [Display(Name = "Inmueble")]
        public int IdInmueble { get; set; }

        [Required]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Fin")]
        public DateTime FechaFin { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio Mensual")]
        public decimal PrecioMensual { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true; // Se crea como activo por defecto

        // Propiedades de navegación (solo para vistas)
        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }

    }
}
