using System.ComponentModel.DataAnnotations;

namespace InmobiliariaMVC.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ClaveHash { get; set; } // Hasheada con BCrypt

        [Required]
        public string Rol { get; set; } // "Administrador" | "Empleado"

        public string? Avatar { get; set; }
    }
}
