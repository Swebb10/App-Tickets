using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [Column("ID_Ticket")]
        public int Id { get; set; }

        [Required]
        public string Asunto { get; set; }

        [Column("ID_Categoria")]
        public int? CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }

        [Column("ID_NivelUrgencia")]
        public int? UrgenciaId { get; set; }
        public virtual NivelUrgencia Urgencia { get; set; }

        [Column("ID_NivelImportancia")]
        public int? ImportanciaId { get; set; }
        public virtual NivelImportancia Importancia { get; set; }

        [Column("ID_EstadoTicket")]
        public int? EstadoId { get; set; }
        public virtual EstadoTicket Estado { get; set; }

        [Column("Creado_Por")]
        public string UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Column("Fecha_Creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("Ultima_Modificacion")]
        public DateTime UltimaModificacion { get; set; }
    }
}