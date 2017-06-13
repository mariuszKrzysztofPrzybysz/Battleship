namespace BattleShip.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBattleEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountRole",
                c => new
                    {
                        AccountRoleId = c.Long(nullable: false, identity: true),
                        AccountId = c.Long(nullable: false),
                        RoleId = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AccountRoleId)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Battle",
                c => new
                    {
                        BattleId = c.Long(nullable: false, identity: true),
                        PlayerId = c.Long(nullable: false),
                        OpponentId = c.Long(nullable: false),
                        StartUtcDateTime = c.DateTime(nullable: false),
                        EndUtcDateTime = c.DateTime(),
                        WinnerId = c.Long(),
                    })
                .PrimaryKey(t => t.BattleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.AccountRole", "AccountId", "dbo.Account");
            DropIndex("dbo.AccountRole", new[] { "RoleId" });
            DropIndex("dbo.AccountRole", new[] { "AccountId" });
            DropTable("dbo.Battle");
            DropTable("dbo.AccountRole");
        }
    }
}
