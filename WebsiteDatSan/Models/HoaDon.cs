namespace WebsiteDatSan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [Key]
        public int MaHoaDon { get; set; }

        [StringLength(128)]
        public string id { get; set; }

        public int? MaSan { get; set; }

        public int? MaGioDat { get; set; }

        public int? MahinhThuc { get; set; }

        [Column(TypeName = "money")]
        public decimal? TongTien { get; set; }

        public bool? TrangThai { get; set; }

        public DateTime? NgayDat { get; set; }

        public GioDat GioDat { get; set; }

        public virtual HinhThucThanhToan HinhThucThanhToan { get; set; }

        public virtual San San { get; set; }
    }
}
