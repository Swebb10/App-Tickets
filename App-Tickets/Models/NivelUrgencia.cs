using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Nivel_Urgencia")]
    public class NivelUrgencia
    {
        [Key]
        public int ID_NivelUrgencia { get; set; }

        [StringLength(50)]
        public string Nivel_Urgencia { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}