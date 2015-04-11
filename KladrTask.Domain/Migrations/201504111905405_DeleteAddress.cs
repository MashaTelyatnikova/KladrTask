namespace KladrTask.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAddress : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Addresses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Region = c.String(nullable: false),
                        Locality = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        House = c.String(nullable: false),
                        Index = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
