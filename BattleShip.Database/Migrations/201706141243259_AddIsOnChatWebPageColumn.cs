using System.Data.Entity.Migrations;

namespace BattleShip.Database.Migrations
{
    public partial class AddIsOnChatWebPageColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Account", "IsOnChatWebPage", c => c.Boolean(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Account", "IsOnChatWebPage");
        }
    }
}