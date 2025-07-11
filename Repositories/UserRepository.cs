using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Framework.Entityes;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Repositories
{
    public class UserRepository
    {

        public UserRepository() { }

        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Add(User user)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                db.Users.Add(user);
                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Обновление пользователя в БД
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(User user)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                User dbUser = db.Users.First(u => u.Id == user.Id);
                dbUser.Name = user.Name;
                dbUser.Email = user.Email;

                db.Users.Update(dbUser);
                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Обновление пользователя в БД
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddUserBooks(User user, List<Book> books)
        {
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                User dbUser = db.Users.First(u => u.Id == user.Id);
                dbUser.Books = dbUser.Books.Union(books).ToList();
                db.Users.Update(dbUser);
                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Удаление пользователя в БД
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(User user)
        {
            string name = new string(user.Name);
            int result = 0;

            using (var db = new Configuration.AppContext())
            {
                db.Users.Remove(user);
                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// Поиск пользователя по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public User FindById(int id)
        {
            User result;

            using (var db = new Configuration.AppContext())
            {
                result = db.Users.Find(id);
            }

            return result;

        }

        /// <summary>
        /// Поиск пользователя по Email
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public User FindByEmail(string mail = "")
        {
            User result;

            using (var db = new Configuration.AppContext())
            {
                result = db.Users.FirstOrDefault(m => m.Email == mail);
            }

            return result;

        }

        /// <summary>
        /// Получение всего списка пользователей
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllTable()
        {
            List<User> result = new List<User>();

            using (var db = new Configuration.AppContext())
            {
                result = db.Users.ToList();
            }

            return result;
        }

        /// <summary>
        /// Получение списка книг пользователя
        /// </summary>
        /// <returns></returns>
        public List<Book> ListBooks(User user)
        {
            List<Book> result = new List<Book>();

            using (var db = new Configuration.AppContext())
            {
                result = db.Books.Where(b => b.Users.Contains(user)).ToList();
            }

            return result;
        }
    }
}
