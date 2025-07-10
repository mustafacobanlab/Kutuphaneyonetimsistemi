using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUygulamaProje1.Models
{
    public class Kiralama
    {
        [Key]
        public int Id { get; set; }

        [Required]

        public int OgrenciId { get; set; }

        [ValidateNever]
        
        public int KitaplarId { get; set; }
        [ForeignKey("KitaplarId")]

        [ValidateNever]
        public Kitaplar Kitaplar { get; set; }

        [ValidateNever]
        public DateTime bitis { get; set; }


        [ValidateNever]
        public DateTime olusturma {  get; set; }

        [Required]
        [Display(Name = "Öğrenci")]
        public string ApplicationUserId { get; set; } // Foreign Key (IdentityUser.Id string olduğu için)
        [ForeignKey("ApplicationUserId")]

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; } // Navigasyon Özelliği


    }
}
