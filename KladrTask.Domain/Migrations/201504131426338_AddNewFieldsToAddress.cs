namespace KladrTask.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldsToAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "RegionCode", c => c.String(nullable: false, defaultValue:"0"));
            AddColumn("dbo.Addresses", "LocalityCode", c => c.String(nullable: false, defaultValue:"0"));
            AddColumn("dbo.Addresses", "RoadCode", c => c.String(nullable: false, defaultValue:"0"));
            AddColumn("dbo.Addresses", "HouseCode", c => c.String(nullable: false, defaultValue:"0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "HouseCode");
            DropColumn("dbo.Addresses", "RoadCode");
            DropColumn("dbo.Addresses", "LocalityCode");
            DropColumn("dbo.Addresses", "RegionCode");
        }
    }
}
