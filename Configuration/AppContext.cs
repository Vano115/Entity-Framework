using Entity_Framework.Entityes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Configuration
{
    internal class AppContext : DbContext
    {
        // Объекты таблицы Users
        public DbSet<User> Users { get; set; }
        
        // Обьекты таблицы Books
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public AppContext()
        {
            //Database.EnsureDeleted();
            Database.Migrate();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString.MsSqlConnection);
        }
    }
}
