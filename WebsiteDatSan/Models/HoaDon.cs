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

        public int? MaDat { get; set; }

        public int? MaSan { get; set; }

        public int? MaGioDat { get; set; }

        public int? MahinhThuc { get; set; }

        [Column(TypeName = "money")]
        public decimal? TongTien { get; set; }

        public bool? TrangThai { get; set; }

        public virtual DatSan DatSan { get; set; }

        public virtual GioDat GioDat { get; set; }

        public virtual HinhThucThanhToan HinhThucThanhToan { get; set; }

        public virtual San San { get; set; }
    }
}
