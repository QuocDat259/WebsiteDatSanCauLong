using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebsiteDatSan.Models
{
    public partial class WebCauLongss : DbContext
    {
        public WebCauLongss()
            : base("name=WebCauLongss")
        {
        }

        public virtual DbSet<GioDat> GioDat { get; set; }
        public virtual DbSet<HinhThucThanhToan> HinhThucThanhToan { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<LoaiSan> LoaiSan { get; set; }
        public virtual DbSet<San> San { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HoaDon>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<LoaiSan>()
                .HasMany(e => e.San)
                .WithRequired(e => e.LoaiSan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<San>()
                .Property(e => e.GiaTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<San>()
                .HasMany(e => e.GioDat)
                .WithOptional(e => e.San)
                .HasForeignKey(e => e.idsan);
        }
    }
}
