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
        public bool Estado { get; set; } = true;

        // 🔹 Auditoría
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? AnuladoPor { get; set; }
        public DateTime? FechaAnulacion { get; set; }

        // Relaciones
        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }
        public Usuario? UsuarioCreador { get; set; }
        public Usuario? UsuarioAnulador { get; set; }
    }
}
