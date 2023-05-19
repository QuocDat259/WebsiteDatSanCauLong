using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebsiteDatSan.Models
{
    public partial class Model11 : DbContext
    {
        public Model11()
            : base("name=Model11")
        {
        }

        public virtual DbSet<San> San { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<San>()
                .Property(e => e.GiaTien)
                .HasPrecision(19, 4);
        }
    }
}
