namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fifthMig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offers", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Offers", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id" });
            RenameColumn(table: "dbo.Offers", name: "AuctionId", newName: "Auction_Id");
            RenameIndex(table: "dbo.Offers", name: "IX_AuctionId", newName: "IX_Auction_Id");
            AlterColumn("dbo.Offers", "AspNetUser_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "AspNetUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Offers", "AspNetUser_Id");
            CreateIndex("dbo.Orders", "AspNetUser_Id");
            AddForeignKey("dbo.Offers", "AspNetUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Offers", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Offers", new[] { "AspNetUser_Id" });
            AlterColumn("dbo.Orders", "AspNetUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Offers", "AspNetUser_Id", c => c.String(maxLength: 128));
            RenameIndex(table: "dbo.Offers", name: "IX_Auction_Id", newName: "IX_AuctionId");
            RenameColumn(table: "dbo.Offers", name: "Auction_Id", newName: "AuctionId");
            CreateIndex("dbo.Orders", "AspNetUser_Id");
            CreateIndex("dbo.Offers", "AspNetUser_Id");
            AddForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Offers", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
