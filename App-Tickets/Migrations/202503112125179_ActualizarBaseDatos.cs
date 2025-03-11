namespace App_Tickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizarBaseDatos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Usuarios", "Contraseña", c => c.String(nullable: false));
            DropColumn("dbo.Usuarios", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Password", c => c.String(nullable: false));
            DropColumn("dbo.Usuarios", "Contraseña");
            DropColumn("dbo.Tickets", "FechaCreacion");
        }
    }
}
