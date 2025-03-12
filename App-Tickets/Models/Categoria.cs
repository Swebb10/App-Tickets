using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int ID_Categoria { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        // Relación con Tickets
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}