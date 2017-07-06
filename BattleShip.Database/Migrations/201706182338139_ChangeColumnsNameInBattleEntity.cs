using System.Data.Entity.Migrations;

namespace BattleShip.Database.Migrations
{
    public partial class ChangeColumnsNameInBattleEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Battle", "PlayerIsReady", c => c.Boolean(false));
            AddColumn("dbo.Battle", "OpponentIsReady", c => c.Boolean(false));
            DropColumn("dbo.Battle", "PlayerBoardIsFilled");
            DropColumn("dbo.Battle", "OpponentBoardIsFilled");
        }

        public override void Down()
        {
            AddColumn("dbo.Battle", "OpponentBoardIsFilled", c => c.Boolean(false));
            AddColumn("dbo.Battle", "PlayerBoardIsFilled", c => c.Boolean(false));
            DropColumn("dbo.Battle", "OpponentIsReady");
            DropColumn("dbo.Battle", "PlayerIsReady");
        }
    }
}