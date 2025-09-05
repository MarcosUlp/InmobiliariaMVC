using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaMVC.Models
{
	public class Propietario
    {
        [Key]
        [Display(Name = "Código Int.")]
        public int IdPropietario { get; set; }
        
        [Required]
        public string Nombre { get; set; } = string.Empty; // Valor inicial

        [Required]
        public string Apellido { get; set; } = string.Empty; // Valor inicial

        [Required]
        public string Dni { get; set; } = string.Empty; // Valor inicial

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty; // Valor inicial

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty; // Valor inicial

        [Required(ErrorMessage = "La clave es obligatoria"), DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty; // Valor inicial

		public override string ToString()
		{
			//return $"{Apellido}, {Nombre}";
			//return $"{Nombre} {Apellido}";
			var res = $"{Nombre} {Apellido}";
			if(!String.IsNullOrEmpty(Dni)) {
				res += $" ({Dni})";
			}
			return res;
		}
	}
}