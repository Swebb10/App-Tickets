using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("ID_Usuario")]
        public string Id { get; set; }

        [Required]
        [Column("Email")]
        public string Correo { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [Column("Primer_Apellido")]
        public string PrimerApellido { get; set; }

        [Required]
        [Column("Segundo_Apellido")]
        public string SegundoApellido { get; set; }

        [Required]
        [Column("Contraseña")]
        public string Password { get; set; }

        [Required]
        [Column("Rol_Usuario")]
        public string Rol { get; set; }
    }
}