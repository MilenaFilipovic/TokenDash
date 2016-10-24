namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAuctionAddBidder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "AspNetUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Auctions", "AspNetUserId");
            AddForeignKey("dbo.Auctions", "AspNetUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "AspNetUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Auctions", new[] { "AspNetUserId" });
            DropColumn("dbo.Auctions", "AspNetUserId");
        }
    }
}
