using Entity_Framework.Entityes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Repositories
{
    internal class BookRepository
    {

        /// <summary>
        /// Метод добавляет книгу в БД
        /// </summary>
        /// <param name="book">Созданая книга</param>
        public int Add(Book book)
        {
            int result = 0;
            using (var db = new Configuration.AppContext())
            {
                db.Books.Add(book);
                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Метод обновляет книгy
        /// </summary>
        /// <param name="book">Существующая в БД книга</param>
        public int Update(Book book)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                db.Books.Attach(book);
                db.Books.Update(book);
                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Удаление книги из БД
        /// </summary>
        /// <param name="book">Существующая в БД книга для удаления</param>
        public int Delete(Book book)
        {
            int result = 0;
            using (var db = new Configuration.AppContext())
            {
                db.Books.Remove(book);
                result = db.SaveChanges();
            }

            return result;
        }

        /// <summary>
        /// Получение из БД списка книг заданного жанра
        /// </summary>
        /// <param name="genre"></param>
        /// <returns>List<Book></returns>
        public List<Book> FindBooksInGenre(string genre)
        {
            List<Book> books = new List<Book>();

            using (var db = new Configuration.AppContext())
            {
                books = db.Books.Where(g => g.Genre == genre).ToList();
            }

            return books;
        }

        /// <summary>
        /// Поиск книги по Id
        /// </summary>
        /// <param name="id">Id книги</param>
        /// <returns>Книга из БД</returns>
        public Book FindById(int id)
        {
            Book result;

            using (var db = new Configuration.AppContext())
            {
                result = db.Books.Find(id);
            }

            return result;

        }

        /// <summary>
        /// Получение списка книг заданного автора
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public List<Book> FindByAuthor(Author author)
        {
            List<Book> result = new List<Book>();

            using (var db = new Configuration.AppContext())
            {
                result = db.Books.Select(x => x).Where(x => x.Authors.Contains(author)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Поиск книги по названию
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Book FindByTitle(string title)
        {
            Book result = new Book();

            using (var db = new Configuration.AppContext())
            {
                result = db.Books.FirstOrDefault(a => a.Title == title);
            }

            return result;
        }

        /// <summary>
        /// Получение всего списка книг
        /// </summary>
        /// <returns></returns>
        public List<Book> GetAllTable()
        {
            List<Book> result = new List<Book>();

            using (var db = new Configuration.AppContext())
            {
                result = db.Books.ToList();
            }

            return result;
        }

        public List<Book> GetBooksByYears(DateOnly from, DateOnly to)
        {
            List <Book> result = new List<Book>();

            using (var db = new Configuration.AppContext())
            {
                result = db.Books.Where(b => b.Public_year >= from & b.Public_year <= to).ToList();
            }

            return result;
        }

        public List<Author> ListAuthors(Book book)
        {
            List<Author> result = new List<Author>();

            using (var db = new Configuration.AppContext())
            {
                result = db.Authors.Include(u => u.Books.Where(i => i.Id == book.Id)).ToList();
            }

            return result;
        }
    }
}
