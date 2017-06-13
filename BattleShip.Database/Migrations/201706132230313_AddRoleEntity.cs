namespace BattleShip.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Byte(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Role");
        }
    }
}
