using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Nivel_Importancia")]
    public class NivelImportancia
    {
        [Key]
        public int ID_NivelImportancia { get; set; }

        [StringLength(50)]
        public string Nivel_Importancia { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}