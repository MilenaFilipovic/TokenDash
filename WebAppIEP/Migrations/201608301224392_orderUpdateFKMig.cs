namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderUpdateFKMig : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "AspNetUser_Id", newName: "AspNetUserId");
            RenameIndex(table: "dbo.Orders", name: "IX_AspNetUser_Id", newName: "IX_AspNetUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Orders", name: "IX_AspNetUserId", newName: "IX_AspNetUser_Id");
            RenameColumn(table: "dbo.Orders", name: "AspNetUserId", newName: "AspNetUser_Id");
        }
    }
}
