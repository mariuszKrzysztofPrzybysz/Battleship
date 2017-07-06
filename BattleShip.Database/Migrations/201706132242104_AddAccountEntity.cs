using System.Data.Entity.Migrations;

namespace BattleShip.Database.Migrations
{
    public partial class AddAccountEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Account",
                    c => new
                    {
                        AccountId = c.Long(false, true),
                        Login = c.String(false, 50),
                        Password = c.String(false),
                        EmailAddress = c.String(false),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Photo = c.Binary(),
                        Gender = c.Int(false)
                    })
                .PrimaryKey(t => t.AccountId)
                .Index(t => t.Login, unique: true);
        }

        public override void Down()
        {
            DropIndex("dbo.Account", new[] {"Login"});
            DropTable("dbo.Account");
        }
    }
}