namespace App_Tickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizarBaseDatos1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "AsignadoA", c => c.String(nullable: false));
            AddColumn("dbo.Tickets", "Solucion", c => c.String());
            AlterColumn("dbo.Tickets", "Asunto", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Tickets", "UsuarioId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "UsuarioId");
            AddForeignKey("dbo.Tickets", "UsuarioId", "dbo.Usuarios", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "UsuarioId", "dbo.Usuarios");
            DropIndex("dbo.Tickets", new[] { "UsuarioId" });
            AlterColumn("dbo.Tickets", "UsuarioId", c => c.String());
            AlterColumn("dbo.Tickets", "Asunto", c => c.String(nullable: false));
            DropColumn("dbo.Tickets", "Solucion");
            DropColumn("dbo.Tickets", "AsignadoA");
        }
    }
}
