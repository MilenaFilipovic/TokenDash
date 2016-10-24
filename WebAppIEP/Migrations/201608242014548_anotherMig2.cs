namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherMig2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Auctions", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Auctions", "Image", c => c.Binary(nullable: false));
        }
    }
}
