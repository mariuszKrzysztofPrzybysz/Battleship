namespace BattleShip.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColumnsNameInBattleEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Battle", "PlayerIsReady", c => c.Boolean(nullable: false));
            AddColumn("dbo.Battle", "OpponentIsReady", c => c.Boolean(nullable: false));
            DropColumn("dbo.Battle", "PlayerBoardIsFilled");
            DropColumn("dbo.Battle", "OpponentBoardIsFilled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Battle", "OpponentBoardIsFilled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Battle", "PlayerBoardIsFilled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Battle", "OpponentIsReady");
            DropColumn("dbo.Battle", "PlayerIsReady");
        }
    }
}
