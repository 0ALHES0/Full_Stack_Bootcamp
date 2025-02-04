using Microsoft.EntityFrameworkCore;
using FilmLog.Models;

namespace FilmLog.Data{

    public class DataContext : DbContext{

        public DataContext(DbContextOptions<DataContext>options):base(options){}
         public DbSet<Film> Films => Set<Film>();
         public DbSet<Category> Categories { get; set; }
    }
}