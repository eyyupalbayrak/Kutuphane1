using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Kutuphane1.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int OgrenciNo { get; set; }
        public String? Adres { get; set; }
        public string? Fakulte { get; set; }
        public string? Bolum { get; set; }
    }
}
