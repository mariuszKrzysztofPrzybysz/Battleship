using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Cryptography.X509Certificates;
using BattleShip.Database.Entities;

namespace BattleShip.Database
{
    public class BattleShipContext :DbContext
    {
        public BattleShipContext() : base("name=BattleShipConnection")
        {
            
        }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Conventions
                .Remove<PluralizingTableNameConvention>();
        }
    }
}