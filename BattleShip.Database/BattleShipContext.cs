using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Cryptography.X509Certificates;
using BattleShip.Database.Entities;

namespace BattleShip.Database
{
    public class BattleShipContext : DbContext
    {
        public BattleShipContext() : base("name=BattleShipConnection")
        {

        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountRole> AccountRoles { get; set; }

        public DbSet<Battle> Battles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Conventions
                .Remove<PluralizingTableNameConvention>();

            modelBuilder
                .Conventions
                .Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder
                .Conventions
                .Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}