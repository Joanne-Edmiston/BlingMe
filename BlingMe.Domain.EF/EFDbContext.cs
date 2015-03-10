using BlingMe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingMe.Domain.EF
{
    public class EFDbContext : DbContext
    {
        public DbSet<Bracelet> Bracelets { get; set; }
        public DbSet<Charm> Charms { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Charm>()
                .HasRequired(c => c.Parent).WithMany()
                .HasForeignKey(c => c.ParentID);

            modelBuilder.Entity<Charm>()
                .HasMany(c => c.Children)
                .WithMany(b => b.Charms)
                .Map(t => t.ToTable("CharmBracelets")
                    .MapLeftKey("CharmID")
                    .MapRightKey("BraceletID"));
        }

    }
}
