namespace KladrTask.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Housing", c => c.String());
            AddColumn("dbo.Addresses", "Apartment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "Apartment");
            DropColumn("dbo.Addresses", "Housing");
        }
    }
}
