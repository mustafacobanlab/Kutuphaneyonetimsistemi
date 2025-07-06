using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class KitapTuru
    {
        [Key] // Primary key anlamına geliyor.
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap Türü Adı Boş Bırakılmaz!")] // Not Null Anlamına Geliyor.
        [MaxLength(25)]
        [DisplayName("Kitap Türü Adı")]
        public string Ad { get; set; }

        

    }
}
