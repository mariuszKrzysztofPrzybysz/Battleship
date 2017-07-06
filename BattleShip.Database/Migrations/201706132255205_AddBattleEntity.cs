using System.Data.Entity.Migrations;

namespace BattleShip.Database.Migrations
{
    public partial class AddBattleEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.AccountRole",
                    c => new
                    {
                        AccountRoleId = c.Long(false, true),
                        AccountId = c.Long(false),
                        RoleId = c.Byte(false)
                    })
                .PrimaryKey(t => t.AccountRoleId)
                .ForeignKey("dbo.Account", t => t.AccountId, true)
                .ForeignKey("dbo.Role", t => t.RoleId, true)
                .Index(t => t.AccountId)
                .Index(t => t.RoleId);

            CreateTable(
                    "dbo.Battle",
                    c => new
                    {
                        BattleId = c.Long(false, true),
                        PlayerId = c.Long(false),
                        OpponentId = c.Long(false),
                        StartUtcDateTime = c.DateTime(false),
                        EndUtcDateTime = c.DateTime(),
                        WinnerId = c.Long()
                    })
                .PrimaryKey(t => t.BattleId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.AccountRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.AccountRole", "AccountId", "dbo.Account");
            DropIndex("dbo.AccountRole", new[] {"RoleId"});
            DropIndex("dbo.AccountRole", new[] {"AccountId"});
            DropTable("dbo.Battle");
            DropTable("dbo.AccountRole");
        }
    }
}