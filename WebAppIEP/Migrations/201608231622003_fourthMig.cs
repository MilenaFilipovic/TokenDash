namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourthMig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "OrderTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Offers", "UserId");
            DropColumn("dbo.Orders", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "UserId", c => c.String());
            AddColumn("dbo.Offers", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "OrderTime", c => c.Binary());
        }
    }
}
