namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderUpdateMig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id" });
            AddColumn("dbo.Orders", "AspNetUser_Id1", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "AspNetUser_Id1");
            AddForeignKey("dbo.Orders", "AspNetUser_Id1", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "AspNetUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id1" });
            DropColumn("dbo.Orders", "AspNetUser_Id1");
            CreateIndex("dbo.Orders", "AspNetUser_Id");
            AddForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
