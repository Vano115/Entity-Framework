using Entity_Framework.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Repositories
{
    internal class AuthorRepository
    {
        public AuthorRepository() { }

        /// <summary>
        /// Добавление автора в БД
        /// </summary>
        /// <param name="author"></param>
        public int Add(Author author)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                db.Authors.Add(author);
                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Обновление данных автора
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public int Update(Author author)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                Author dbAuthor = db.Authors.First(u => u.Id == author.Id);
                dbAuthor.Name = author.Name;

                db.Authors.Update(author);
                result = db.SaveChanges();
            }

            return result;
        }

        public int AddAuthorBooks(Author author, List<Book> books)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                Author dbAuthor = db.Authors.First(u => u.Id == author.Id);
                dbAuthor.Books.AddRange(books);

                db.Authors.Update(dbAuthor);
                result = db.SaveChanges();
            }

            return result;
        }

        /// <summary>
        /// Удаление автора из БД
        /// </summary>
        /// <param name="author"></param>
        public void Delete(Author author)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                db.Authors.Remove(author);
                result = db.SaveChanges();
            }
        }

        /// <summary>
        /// Поиск автора по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Author FindByName(string name)
        {
            Author result;

            using (var db = new Configuration.AppContext())
            {
                result = db.Authors.FirstOrDefault(a => a.Name == name);
            }

            return result;

        }

        /// <summary>
        /// Поиск автора по ID
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        /// <exception cref="AuthorNotFoundException"> Исключение при отсутствии автора в БД</exception>
        public Author FindById(int id)
        {
            Author result;

            using (var db = new Configuration.AppContext())
            {
                result = db.Authors.First(a => a.Id == id);
            }

            return result;

        }

        /// <summary>
        /// Получение всего списка авторов из БД
        /// </summary>
        /// <returns>List <Author></returns>
        public List<Book> GetAuthorsBook(Author author)
        {
            List<Book> result;

            using (var db = new Configuration.AppContext())
            {
                result = db.Authors.First(a => a.Id == author.Id).Books;
            }

            return result;

        }

        /// <summary>
        /// Получение всего списка авторов из БД
        /// </summary>
        /// <returns>List <Author></returns>
        public List<Author> GetAllAuthors()
        {
            List<Author> result;

            using (var db = new Configuration.AppContext())
            {
                result = db.Authors.ToList();
            }

            return result;

        }
    }
}
