namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAuctionRowVMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Auctions", "RowVersion");
        }
    }
}
