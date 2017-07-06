using System.Data.Entity.Migrations;

namespace BattleShip.Database.Migrations
{
    public partial class AddRoleEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Role",
                    c => new
                    {
                        RoleId = c.Byte(false, true),
                        Name = c.String(false, 25)
                    })
                .PrimaryKey(t => t.RoleId);
        }

        public override void Down()
        {
            DropTable("dbo.Role");
        }
    }
}