namespace BattleShip.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsOnChatWebPageColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "IsOnChatWebPage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Account", "IsOnChatWebPage");
        }
    }
}
