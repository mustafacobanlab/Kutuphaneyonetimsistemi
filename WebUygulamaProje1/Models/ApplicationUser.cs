using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public int Ogrencino { get; set; }

        [ValidateNever]
        public string? AdSoyad { get; set; }
        public string? Adres {  get; set; }

        public string? Fakulte { get; set; }

        public string? Bolum { get; set; }



    }
}
