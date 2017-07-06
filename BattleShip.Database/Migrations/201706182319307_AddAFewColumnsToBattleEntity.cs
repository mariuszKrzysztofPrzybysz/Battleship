using System.Data.Entity.Migrations;

namespace BattleShip.Database.Migrations
{
    public partial class AddAFewColumnsToBattleEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Battle", "PlayerBoard", c => c.String(maxLength: 500));
            AddColumn("dbo.Battle", "OpponentBoard", c => c.String(maxLength: 500));
            AddColumn("dbo.Battle", "PlayerBoardIsFilled", c => c.Boolean(false));
            AddColumn("dbo.Battle", "OpponentBoardIsFilled", c => c.Boolean(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Battle", "OpponentBoardIsFilled");
            DropColumn("dbo.Battle", "PlayerBoardIsFilled");
            DropColumn("dbo.Battle", "OpponentBoard");
            DropColumn("dbo.Battle", "PlayerBoard");
        }
    }
}