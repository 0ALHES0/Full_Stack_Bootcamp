using System.ComponentModel.DataAnnotations;
using FilmLog.Models;


namespace FilmLog.Models{
    public class Film{
         [Key]
        [Display(Name="Film Id")]
       
        public int MoviesId { get; set; }

        [Display(Name="Film Adı")]
        [Required(ErrorMessage ="Film adı zorunlu")]
        public string Name { get; set; } = null!;

        [Display(Name="Resim")]
        
        public string? Image {get;set;} = string.Empty;
        

        [Display(Name="Kategori")]
        [Required(ErrorMessage ="Kategori zorunlu")]
        public int CategoryId {get;set;} 

        [Display(Name="Yönetmen")]
        public string? Director { get; set; }

        [Display(Name="Çıkış Yılı")]
        [Required(ErrorMessage ="Çıkış yılını giriniz")]
        public int ReleaseYear { get; set; }

    }
}