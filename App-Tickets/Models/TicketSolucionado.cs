using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Ticket_Solucionados")]
    public class TicketSolucionado
    {
        [Key]
        public int ID_Solucion { get; set; }

        public int? ID_Ticket { get; set; }
        [ForeignKey("ID_Ticket")]
        public virtual Ticket Ticket { get; set; }

        [Required]
        [StringLength(255)]
        public string Solucion { get; set; }

        [StringLength(50)]
        public string Resuelto_Por { get; set; }

        public DateTime Fecha_Resolucion { get; set; } = DateTime.Now;
    }
}