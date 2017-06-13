using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BattleShip.Database
{
    public class BattleShipContext :DbContext
    {
        public BattleShipContext() : base("name=BattleShipConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Conventions
                .Remove<PluralizingTableNameConvention>();
        }
    }
}