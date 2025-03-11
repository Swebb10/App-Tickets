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

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}