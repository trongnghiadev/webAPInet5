using System;
using System.Collections.Generic;

namespace HocHanh6day.Data
{
    public class DonHang
    {
        public enum TinhTrangDonDatHang
        {
            New = 0,
            Payment = 1,
            Complete = 2,
            Cancel = -1

        }

        public Guid MaDh { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public TinhTrangDonDatHang TinhTrangDonHang { get; set; }
        public string NguoiNhan { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }


        public ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
        public DonHang()
        {
            DonHangChiTiets = new HashSet<DonHangChiTiet>();
        }


    }
}
