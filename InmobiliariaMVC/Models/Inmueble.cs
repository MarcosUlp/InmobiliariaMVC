// InmobiliariaMVC/Models/Inmueble.cs
// ¡Reemplaza el archivo anterior con este código!

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaMVC.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "Código")]
        public int IdInmueble { get; set; }

        [Required]
        [Display(Name = "Dueño")]
        public int IdPropietario { get; set; }

        [Required]
        public string Direccion { get; set; }

        public string? Uso { get; set; } // Puede ser nulo en tu DB

        public string? Tipo { get; set; } // Puede ser nulo en tu DB

        public int? Ambientes { get; set; } // Lo cambié de nombre y puede ser nulo

        public decimal? Superficie { get; set; } // ¡Campo nuevo!

        [Required]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; }

        // Propiedad de navegación para acceder a los datos del Propietario
        [ForeignKey(nameof(IdPropietario))]
        public Propietario Dueño { get; set; }
    }
}