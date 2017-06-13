namespace BattleShip.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllowNewBattleAndAllowPrivateChatColumns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountRole", "AccountId", "dbo.Account");
            DropForeignKey("dbo.AccountRole", "RoleId", "dbo.Role");
            AddColumn("dbo.Account", "AllowNewBattle", c => c.Boolean(nullable: false));
            AddColumn("dbo.Account", "AllowPrivateChat", c => c.Boolean(nullable: false));
            AddColumn("dbo.Battle", "Account_AccountId", c => c.Long());
            CreateIndex("dbo.Battle", "PlayerId");
            CreateIndex("dbo.Battle", "OpponentId");
            CreateIndex("dbo.Battle", "Account_AccountId");
            AddForeignKey("dbo.Battle", "OpponentId", "dbo.Account", "AccountId");
            AddForeignKey("dbo.Battle", "PlayerId", "dbo.Account", "AccountId");
            AddForeignKey("dbo.Battle", "Account_AccountId", "dbo.Account", "AccountId");
            AddForeignKey("dbo.AccountRole", "AccountId", "dbo.Account", "AccountId");
            AddForeignKey("dbo.AccountRole", "RoleId", "dbo.Role", "RoleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.AccountRole", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Battle", "Account_AccountId", "dbo.Account");
            DropForeignKey("dbo.Battle", "PlayerId", "dbo.Account");
            DropForeignKey("dbo.Battle", "OpponentId", "dbo.Account");
            DropIndex("dbo.Battle", new[] { "Account_AccountId" });
            DropIndex("dbo.Battle", new[] { "OpponentId" });
            DropIndex("dbo.Battle", new[] { "PlayerId" });
            DropColumn("dbo.Battle", "Account_AccountId");
            DropColumn("dbo.Account", "AllowPrivateChat");
            DropColumn("dbo.Account", "AllowNewBattle");
            AddForeignKey("dbo.AccountRole", "RoleId", "dbo.Role", "RoleId", cascadeDelete: true);
            AddForeignKey("dbo.AccountRole", "AccountId", "dbo.Account", "AccountId", cascadeDelete: true);
        }
    }
}
