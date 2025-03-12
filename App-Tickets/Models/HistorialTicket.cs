using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Historial_Tickets")]
    public class HistorialTicket
    {
        [Key]
        [Column("ID_Historial")]
        public int Id { get; set; }

        [ForeignKey("Ticket")]
        [Column("ID_Ticket")]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [Column("Modificado_Por")]
        public string ModificadoPor { get; set; }

        [ForeignKey("EstadoPrevio")]
        [Column("Estado_Previo")]
        public int EstadoPrevioId { get; set; }
        public EstadoTicket EstadoPrevio { get; set; }

        [ForeignKey("NuevoEstado")]
        [Column("Nuevo_Estado")]
        public int NuevoEstadoId { get; set; }
        public EstadoTicket NuevoEstado { get; set; }

        [Column("Fecha_Modificacion")]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        public string Comentarios { get; set; }
    }
}