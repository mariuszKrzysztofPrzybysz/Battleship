using System.Data.Entity.Migrations;

namespace BattleShip.Database.Migrations
{
    public partial class AddAttackerColumnToBattleEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Battle", "Attacker", c => c.Long(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Battle", "Attacker");
        }
    }
}