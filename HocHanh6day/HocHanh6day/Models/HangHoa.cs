using System;

namespace HocHanh6day.Models
{
    public class HangHoaVM
    {
        public string TenHangHoa { get; set; } 
        public double DonGia { get; set; }
    }

    public class HangHoa : HangHoaVM { 
        public Guid MaHangHoa { get; set;}
    }
}
