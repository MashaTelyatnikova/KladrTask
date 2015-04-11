using System.Data.Entity.Migrations;

namespace KladrTask.Domain.Migrations
{
    public partial class AddRoleToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Role", c => c.Int(nullable: false, defaultValue: (int)Role.Guest));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Role");
        }
    }
}
