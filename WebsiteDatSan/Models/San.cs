namespace WebsiteDatSan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("San")]
    public partial class San
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public San()
        {
            GioDat = new HashSet<GioDat>();
            HoaDon = new HashSet<HoaDon>();
        }

        [Key]
        public int MaSan { get; set; }

        public int MaLoaiSan { get; set; }

        [StringLength(50)]
        public string TenSan { get; set; }

        [StringLength(150)]
        public string DiaChi { get; set; }

        [Column(TypeName = "money")]
        public decimal? GiaTien { get; set; }

        public bool? TrangThai { get; set; }

        [StringLength(128)]
        public string IdUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioDat> GioDat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

        public virtual LoaiSan LoaiSan { get; set; }
    }
}
