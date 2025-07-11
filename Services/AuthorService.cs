using Entity_Framework.Entityes;
using Entity_Framework.Exceptions;
using Entity_Framework.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Services
{
    internal class AuthorService : BaseService
    {
        public AuthorService() { }
        AuthorRepository repository = new AuthorRepository();

        /// <summary>
        /// Создание обьекта - автора книг с консоли
        /// </summary>
        /// <returns></returns>
        public Author Create()
        {
            Author author = new Author();

            Console.WriteLine("Введите имя добавляемого автора");
            string name = Console.ReadLine();

            while (!TextChecker(name))
            {
                Console.WriteLine("Некорректный ввод, введите имя автора: ");
                name = Console.ReadLine();
            }

            author.Name = name;

            return author;
        }

        /// <summary>
        /// Добавление автора в БД
        /// </summary>
        /// <param name="author"></param>
        public void AddAuthor(Author author)
        {
            try
            {
                Author check = repository.FindByName(author.Name);

                if (check != null)
                {
                    throw new AuthorAlreadyAddedException();
                }

                int result = repository.Add(author);

                if (result == 0) throw new Exception();
                Console.WriteLine($"Автор {author.Name} добавлен ");
            }
            catch (AuthorAlreadyAddedException)
            {
                Console.WriteLine($"Автор {author.Name} уже есть в БД");
            }
            catch(Exception) 
            {
                Console.WriteLine($"Ошибка записи автора {author.Name} в БД");
            }
            
        }

        /// <summary>
        /// Поиск автора по имени
        /// </summary>
        public Author FindAuthorByName(string name)
        {
            Author author = repository.FindByName(name);

            return author;
        }

        /// <summary>
        /// Поиск автора по Id
        /// </summary>
        public Author FindAuthorById(int id)
        {
            Author author = repository.FindById(id);

            return author;
        }

        /// <summary>
        /// Метод принимает у пользователя строку с именем автора и возвращает количество его книг
        /// </summary>
        /// <returns>int количество книг</returns>
        public int GetCountBooks()
        {
            Console.WriteLine("Введите имя автора для подсчёта его книг");
            string author = Console.ReadLine();

            Author findedAuthor = new Author();

            // Проверка и приём пользовательского ввода
            while (!TextChecker(author))
            {
                Console.WriteLine("Некорректный ввод, введите имя автора");
                author = Console.ReadLine();
            }

            try
            {
                findedAuthor = repository.FindByName(author);

                return findedAuthor.Books.Count;
            }
            catch(AuthorNotFoundException)
            {
                Console.WriteLine($"Автор не найден {author}");
            }
            
            return 0;
        }

        /// <summary>
        /// Метод принимает у пользователя автора и возвращает количество его книг
        /// </summary>
        /// <returns>int количество книг</returns>
        public int GetCountBooks(Author author)
        {
            try
            {
                return repository.FindById(author.Id).Books.Count;
            }
            catch (AuthorNotFoundException)
            {
                Console.WriteLine($"Автор не найден {author}");
            }

            return 0;
        }
    }
}
