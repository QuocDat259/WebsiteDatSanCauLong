namespace WebsiteDatSan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GioDat")]
    public partial class GioDat
    {
        [Key]
        public int MaGioDat { get; set; }

        public TimeSpan? GioBatDau { get; set; }

        public TimeSpan? GioKetThuc { get; set; }

        public int? idsan { get; set; }

        public virtual San San { get; set; }
    }
}
