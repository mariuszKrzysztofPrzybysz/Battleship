namespace BattleShip.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountId = c.Long(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Photo = c.Binary(),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccountId)
                .Index(t => t.Login, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Account", new[] { "Login" });
            DropTable("dbo.Account");
        }
    }
}
