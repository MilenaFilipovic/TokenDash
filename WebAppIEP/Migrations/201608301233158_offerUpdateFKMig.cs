namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offerUpdateFKMig : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Offers", name: "Auction_Id", newName: "AuctionId");
            RenameColumn(table: "dbo.Offers", name: "AspNetUser_Id", newName: "AspNetUserId");
            RenameIndex(table: "dbo.Offers", name: "IX_Auction_Id", newName: "IX_AuctionId");
            RenameIndex(table: "dbo.Offers", name: "IX_AspNetUser_Id", newName: "IX_AspNetUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Offers", name: "IX_AspNetUserId", newName: "IX_AspNetUser_Id");
            RenameIndex(table: "dbo.Offers", name: "IX_AuctionId", newName: "IX_Auction_Id");
            RenameColumn(table: "dbo.Offers", name: "AspNetUserId", newName: "AspNetUser_Id");
            RenameColumn(table: "dbo.Offers", name: "AuctionId", newName: "Auction_Id");
        }
    }
}
