using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class KitapTuru
    {
        [Key]    //Primary key bunu yazmasakta sistem id olarak algılar. ama yazmak iyidir kod okunurluğu için.
        public int Id { get; set; }
        [Required(ErrorMessage ="Kitap Türü Adı Boş Bırakılamaz !")]  //not null
        [MaxLength(25)] //Max Lenght
        [DisplayName("Kitap Türü Adı")] //Label ismi nasıl görünsün
        public string Ad { get; set; }
    }
}
