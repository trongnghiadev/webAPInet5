using System.ComponentModel.DataAnnotations;

namespace HocHanh6day.Models
{
    public class LoaiMD
    {
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }
    }
}
