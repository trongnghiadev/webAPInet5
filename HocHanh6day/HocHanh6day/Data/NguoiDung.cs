using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HocHanh6day.Data
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
    }
}
