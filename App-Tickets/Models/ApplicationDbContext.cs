using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace App_Tickets.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base ("SoporteDB") { }

        // DbSet que representa cada tabla en la BD
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EstadoTicket> Estado_Tickets { get; set; }
        public DbSet<HistorialTicket> Historial_Tickets { get; set; }
        public DbSet<NivelImportancia> Nivel_Importancia { get; set; }
        public DbSet<NivelUrgencia> Nivel_Urgencia { get; set; }
        public DbSet<TicketSolucionado> Ticket_Solucionados { get; set; }
    }
}