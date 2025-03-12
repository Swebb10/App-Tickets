using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Asunto { get; set; }

        [Required]
        public string Categoria { get; set; } // Ej: Hardware, Software, Red

        [Required]
        public string Urgencia { get; set; } // Baja, Media, Alta

        [Required]
        public string Importancia { get; set; } // Baja, Media, Alta

        [Required]
        public string Estado { get; set; } // Pendiente, Resuelto, Creado

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public string AsignadoA { get; set; } // Correo del analista que atenderá el ticket

        public string Solucion { get; set; } // Campo para documentar la solución del ticket

        // Relación con el usuario que creó el ticket
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}