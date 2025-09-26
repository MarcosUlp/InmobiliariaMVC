// Models/Pago.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaMVC.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        [Required]
        [Display(Name = "Contrato")]
        public int IdContrato { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime FechaPago { get; set; }
        public bool Estado { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [StringLength(255)]
        public string? Observaciones { get; set; }

        // Auditoría
        public int? CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? AnuladoPor { get; set; }
        public DateTime? FechaAnulacion { get; set; }

        // Propiedades de navegación
        public Contrato? Contrato { get; set; }
        public Usuario? UsuarioCreacion { get; set; }
        public Usuario? UsuarioAnulacion { get; set; }
    }
}
