using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebsiteDatSan.Models
{
    public partial class WebCauLongs : DbContext
    {
        public WebCauLongs()
            : base("name=WebCauLongs")
        {
        }

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ChiTiet_DatSan> ChiTiet_DatSan { get; set; }
        public virtual DbSet<DatSan> DatSan { get; set; }
        public virtual DbSet<LoaiSan> LoaiSan { get; set; }
        public virtual DbSet<San> San { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<DatSan>()
                .HasMany(e => e.ChiTiet_DatSan)
                .WithRequired(e => e.DatSan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoaiSan>()
                .HasMany(e => e.San)
                .WithRequired(e => e.LoaiSan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<San>()
                .Property(e => e.GiaTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<San>()
                .HasMany(e => e.ChiTiet_DatSan)
                .WithRequired(e => e.San)
                .WillCascadeOnDelete(false);
        }
    }
}
