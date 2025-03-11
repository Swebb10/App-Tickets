using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Asunto { get; set; }

        [Required]
        public string Categoria { get; set; }

        [Required]
        public string Urgencia { get; set; }

        [Required]
        public string Importancia { get; set; }

        [Required]
        public string Estado { get; set; } // Pendiente, Resuelto, Creado

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string UsuarioId { get; set; }
    }
}