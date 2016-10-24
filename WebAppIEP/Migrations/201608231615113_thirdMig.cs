namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirdMig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductName = c.String(),
                        Duration = c.Int(nullable: false),
                        StartingPrice = c.Int(nullable: false),
                        CreationDT = c.DateTime(nullable: false),
                        OpeningDT = c.DateTime(),
                        ClosingDT = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Image = c.Binary(),
                        PriceInc = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AuctionId = c.Long(nullable: false),
                        OfferTime = c.DateTime(nullable: false),
                        Outcome = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .ForeignKey("dbo.Auctions", t => t.AuctionId, cascadeDelete: true)
                .Index(t => t.AuctionId)
                .Index(t => t.AspNetUser_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        OrderTime = c.Binary(),
                        NoTokens = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Status = c.Boolean(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id)
                .Index(t => t.AspNetUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Offers", "AuctionId", "dbo.Auctions");
            DropForeignKey("dbo.Offers", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Offers", new[] { "AspNetUser_Id" });
            DropIndex("dbo.Offers", new[] { "AuctionId" });
            DropTable("dbo.Orders");
            DropTable("dbo.Offers");
            DropTable("dbo.Auctions");
        }
    }
}
