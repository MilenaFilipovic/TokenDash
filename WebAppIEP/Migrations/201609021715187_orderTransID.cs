namespace xx0000xWebAppIEP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderTransID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "transactionID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "transactionID");
        }
    }
}
