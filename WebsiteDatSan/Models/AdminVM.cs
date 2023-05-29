using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteDatSan.Models
{
    public class AdminVM
    {
        public HoaDon HoaDon { get; set; }
        public AspNetUsers AspNetUser { get; set; }
        public List<AspNetUsers> AspNetUsers { get; set; }
        public List<HoaDon> HoaDons { get; set; }
        public List<San> Sans { get; set; }

    }
}