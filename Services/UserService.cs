using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Framework.Entityes;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using Entity_Framework.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace Entity_Framework.Services
{
    public class UserService : BaseService
    {
        UserRepository repository = new UserRepository();
        public UserService() { }

        /// <summary>
        /// Создание обьекта - пользователя с консоли
        /// </summary>
        /// <returns></returns>
        public User CreateUser()
        {
            User user = new User();

            Console.WriteLine("Введите имя");
            string name = Console.ReadLine();

            while (!TextChecker(name))
            {
                Console.WriteLine("Некорректный ввод, введите Имя: ");
                name = Console.ReadLine();
            }

            user.Name = name;

            var mailCheck = new EmailAddressAttribute();
            string email = Console.ReadLine();
            Console.WriteLine("Введите адрес электронной почты");

            while (!mailCheck.IsValid(email))
            {
                Console.WriteLine(" Некорректный ввод, введите email: ");
                email = Console.ReadLine();

            }

            user.Email = email;

            return user;
        }

        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        /// <param name="book"></param>
        public void AddUser(User user)
        {
            var ch = repository.FindByEmail(user.Email);
            int res = 0;

            if (ch != null)
            {
                Console.WriteLine($"Пользователь с таким Email уже существует");
            }
            else
            {
                Console.WriteLine($"Добавляем пользователя {user.Name}");
                try
                {
                    res = repository.Add(user);
                    if (res == 0) throw new Exception();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Ошибка при добавлении {user.Name}");
                }
            }
        }

        /// <summary>
        /// Поиск пользователя по Id
        /// </summary>
        public User FindUserById(int id)
        {
            User user = repository.FindById(id);

            return user;
        }

        public void ChangeUserName(User user)
        {
            int res = 0;    

            Console.WriteLine("Введите новое имя");

            string name = Console.ReadLine();

            while (!TextChecker(name))
            {
                Console.WriteLine("Некорректный ввод, введите Имя: ");
                name = Console.ReadLine();
            }

            user.Name = name;

            UpdateRepository(user);            
        }

        /// <summary>
        /// Работает
        /// </summary>
        /// <param name="user"></param>
        /// <param name="books"></param>
        public void GetBooks (User user, List<Book> books)
        {
            user.Books = repository.ListBooks(user);
            int count = books.Except(user.Books).Count();
            if (count > 0) 
            {
                user.Books = user.Books.Union(books).ToList();
                UpdateRepository(user); 
            }
            else
            {
                Console.WriteLine($"Все заданые книги уже были ранее выданы пользователю {user.Name}");
            }
        }

        /// <summary>
        /// Получение из БД списка книг на руках у пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Book> GetUsersBooks(User user)
        {
            return repository.ListBooks(user);
        }

        private void UpdateRepository (User user)
        {
            int res = 0;

            try
            {
                res = repository.Update(user);
                if (res == 0) throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine($"Ошибка при обновлении данных пользователя {user.Name}");
            }
        }
    }
}
