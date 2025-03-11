namespace App_Tickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asunto = c.String(nullable: false),
                        Categoria = c.String(nullable: false),
                        Urgencia = c.String(nullable: false),
                        Importancia = c.String(nullable: false),
                        Estado = c.String(nullable: false),
                        UsuarioId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Correo = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Rol = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Usuarios");
            DropTable("dbo.Tickets");
        }
    }
}
