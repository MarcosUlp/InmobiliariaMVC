using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaMVC.Models
{
    public class Inquilino
    {
        [Key]
        [Display(Name = "Código")]
        public int IdInquilino { get; set; }
        
        [Required]
        public string Nombre { get; set; } = string.Empty; // Valor inicial

        [Required]
        public string Apellido { get; set; } = string.Empty; // Valor inicial

        [Required]
        public string Dni { get; set; } = string.Empty; // Valor inicial

        public string Telefono { get; set; } = string.Empty; // Valor inicial

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty; // Valor inicial
    }
}