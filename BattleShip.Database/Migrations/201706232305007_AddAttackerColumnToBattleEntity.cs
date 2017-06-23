namespace BattleShip.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttackerColumnToBattleEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Battle", "Attacker", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Battle", "Attacker");
        }
    }
}
