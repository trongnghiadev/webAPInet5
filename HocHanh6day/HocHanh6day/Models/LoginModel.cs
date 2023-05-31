using System.ComponentModel.DataAnnotations;

namespace HocHanh6day.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
