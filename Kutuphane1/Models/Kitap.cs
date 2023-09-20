using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUygulamaProje1.Models
{
    public class Kitap
    {
        [Key]    //Primary key bunu yazmasakta sistem id olarak algılar. ama yazmak iyidir kod okunurluğu için.
        public int Id { get; set; }
        [Required(ErrorMessage ="Kitap Adı Boş Bırakılamaz !")]  //not null
        public string KitapAdi { get; set; }
        public string Tanim { get; set; }
        [Required]
        public string Yazar { get; set; }
        [Required]
        [Range(0,5000)]
        public double Fiyat { get; set; }
        [ValidateNever]
        public int KitapTuruId { get; set; }
        [ForeignKey("KitapTuruId")]
        [ValidateNever]
        public KitapTuru KitapTuru { get; set; }
        [ValidateNever]
        public string ResimUrl { get; set; }
    }
}
