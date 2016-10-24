namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderUpdateMig2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "AspNetUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id1" });
            DropColumn("dbo.Orders", "AspNetUser_Id");
            RenameColumn(table: "dbo.Orders", name: "AspNetUser_Id1", newName: "AspNetUser_Id");
            AlterColumn("dbo.Orders", "AspNetUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "AspNetUser_Id");
            AddForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id" });
            AlterColumn("dbo.Orders", "AspNetUser_Id", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.Orders", name: "AspNetUser_Id", newName: "AspNetUser_Id1");
            AddColumn("dbo.Orders", "AspNetUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "AspNetUser_Id1");
            AddForeignKey("dbo.Orders", "AspNetUser_Id1", "dbo.AspNetUsers", "Id");
        }
    }
}
