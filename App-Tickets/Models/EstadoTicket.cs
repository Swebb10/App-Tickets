using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Estado_Tickets")]
    public class EstadoTicket
    {
        [Key]
        public int ID_EstadoTicket { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}