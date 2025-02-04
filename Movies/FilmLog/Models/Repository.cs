using Microsoft.AspNetCore.Http.HttpResults;

namespace FilmLog.Models{

    public class Repository{
        private static readonly List<Film> _films = new();
        private static readonly List<Category> _categories = new();

        static Repository(){
            _categories.Add(new Category{CategoryId=1,Name="Aksiyon"});
            _categories.Add(new Category{CategoryId=2,Name="Komedi"});
            _categories.Add(new Category{CategoryId=3,Name="Korku"});
            _categories.Add(new Category{CategoryId=4,Name="Fantastik"});
            _categories.Add(new Category{CategoryId=5,Name="Animasyon"});
            _categories.Add(new Category{CategoryId=6,Name="Dram"});
            _categories.Add(new Category{CategoryId=7,Name="Belgesel"});
            _categories.Add(new Category{CategoryId=8,Name="Macera"});
            _categories.Add(new Category{CategoryId=9,Name="Spor"});

            _films.Add(new Film{MoviesId =1,Name="John Wick",Image="1.png",CategoryId=1});
            _films.Add(new Film{MoviesId =2,Name="Ip Man",Image="2.png",CategoryId=1});
            _films.Add(new Film{MoviesId =3,Name="Düğün Dernek",Image="3.png",CategoryId=2});

        }
        public static List<Film> Films{
            get{
                return _films;
            }
        }

        public static void EditFilm(Film updateFilm){
            var entity = _films.FirstOrDefault(p=>p.MoviesId == updateFilm.MoviesId);

            if(entity != null){
                entity.Name = updateFilm.Name;              
                entity.Image = updateFilm.Image;
                entity.CategoryId = updateFilm.CategoryId;
               
            }
        }

        public static void DeleteFilms(Film entity){
        var prdEntity = _films.FirstOrDefault(p=>p.MoviesId == entity.MoviesId);

        if(prdEntity != null){
           _films.Remove(prdEntity);
        }
        }

        public static void CreateFilms(Film entity){
            _films.Add(entity);
        }

        
        public static List<Category> Categories{
            get{
                return _categories;
            }
        }
    }
}