using Entity_Framework.Entityes;
using Entity_Framework.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Entity_Framework.Services
{
    internal class BookService : BaseService
    {
        BookRepository repository = new BookRepository();

        public BookService() { }

        /// <summary>
        /// Создание обьекта-книги
        /// </summary>
        /// <returns>Обьект описывающий книгу</returns>
        public Book Create()
        {
            Book book = new Book();

            // Ввод названия книги
            Console.WriteLine("Введите название книги");
            string title = Console.ReadLine();

            // Проверка корректности ввода и запрос нового ввода
            while (!TextChecker(title))
            {
                Console.WriteLine("Некорректный ввод, введите название книги: ");
                title = Console.ReadLine();
            }

            book.Title = title;

            // Ввод даты издания книги
            Console.WriteLine("Введите дату издания в формате дд/мм/гггг");
            string inputYear = Console.ReadLine();
            DateOnly year = new DateOnly();

            while (!DateOnly.TryParse(inputYear, out year) & !inputYear.IsNullOrEmpty())
            {
                Console.WriteLine("Некорректный ввод, введите дату издания: ");
                inputYear = Console.ReadLine();
            }

            book.Public_year = year;

            // Ввод жанра книги
            Console.WriteLine("Введите жанр");
            string genre = Console.ReadLine();

            while (!TextChecker(genre))
            {
                Console.WriteLine("Некорректный ввод, введите жанр: ");
                genre = Console.ReadLine();
            }

            return book;
        }

        /// <summary>
        /// Добавление книги в БД
        /// </summary>
        /// <param name="book"></param>
        public void AddBook(Book book)
        {
            Book ch = repository.FindByTitle(book.Title);
            int res = 0;

            if (ch != null)
            {
                Console.WriteLine($"Книга {book.Title} уже существует");
            }
            else
            {
                Console.WriteLine("Добавляем книгу");
                try
                {
                    res = repository.Add(book);
                    if (res == 0) throw new Exception();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Ошибка при добавлении книги {book.Title}");
                }

                Console.WriteLine($"Книга {book.Title} добавлена");
            }
        }

        /// <summary>
        /// Добавление списка книг в БД
        /// </summary>
        /// <param name="book"></param>
        public void AddBook(List<Book> books)
        {
            Console.WriteLine("Добавляем книги");

            if ( books.Count > 0)
            {
                try
                {
                    foreach (var book in books) 
                    {
                        var ch = repository.FindByTitle(book.Title);
                        int res = 0;

                        if (ch == null){ res = repository.Add(book); }
                        else { Console.WriteLine($"Книга {book.Title} уже существует"); continue; }

                        Console.WriteLine($"Книга {book.Title} добавлена");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка при добавлении");
                }
            }
        }

        /// <summary>
        /// Добавление книге списка авторов
        /// </summary>
        /// <param name="book"></param>
        /// <param name="authors"></param>
        public void AddAuthors(Book book, List<Author> authors)
        {
            book.Authors = repository.ListAuthors(book);
            int count = authors.Except(book.Authors).Count();
            if (count > 0)
            {
                book.Authors = book.Authors.Union(authors).ToList();
                UpdateRepository(book);
            }
            else
            {
                Console.WriteLine($"Все заданные авторы уже были указаны у книги {book.Title}");
            }
        }

        /// <summary>
        /// Поиск книги по Id
        /// </summary>
        public Book FindBookById(int id)
        {
            Book book = repository.FindById(id);

            return book;
        }

        /// <summary>
        /// Метод меняет год издания книги с консоли
        /// </summary>
        /// <param name="book">Существующая в БД книга</param>
        public void ChangeBookYear(Book book)
        {
            Console.WriteLine("Введите новое значение издания");

            string inputYear = Console.ReadLine();
            DateOnly year = new DateOnly();

            // Пока дата не будет корректно введена нельзя выйти из цикла
            while (!DateOnly.TryParse(inputYear, out year))
            {
                Console.WriteLine("Некорректный ввод, введите дату издания: ");
                inputYear = Console.ReadLine();
            }

            book.Public_year = year;
            int res = 0;

            UpdateRepository(book);
        }

        /// <summary>
        /// Метод меняет год издания книги на заданый
        /// </summary>
        /// <param name="book">Существующая в БД книга</param>
        public void ChangeBookYear(Book book, DateOnly year)
        {
            book.Public_year = year;
            int res = 0;

            try
            {
                res = repository.Update(book);
                if (res == 0) throw new Exception();

            }
            catch (Exception) { Console.WriteLine($"Ошибка при смене даты публикации книги {book.Title}"); }
        }

        /// <summary>
        /// Поиск книг по жанру с консоли
        /// </summary>
        /// <returns></returns>
        public List<Book> GetBooksByGenre()
        {
            List<Book> result = new List<Book> ();

            Console.WriteLine("Введите жанр");
            string genre = Console.ReadLine();

            while (!TextChecker(genre))
            {
                Console.WriteLine("Некорректный ввод, введите жанр: ");
                genre = Console.ReadLine();
            }

            try
            {
                result = repository.FindBooksInGenre(genre);
            }
            catch (Exception) { Console.WriteLine($"Ошибка при поиске книг заданного жанра"); }

            return result;
        }

        /// <summary>
        /// Поиск книг по заданному жанру
        /// </summary>
        /// <returns></returns>
        public List<Book> GetBooksByGenre(string genre)
        {
            List<Book> result = new List<Book>();

            try
            {
                result = repository.FindBooksInGenre(genre);
            }
            catch (Exception) { Console.WriteLine($"Ошибка при поиске книг заданного жанра"); }

            return result;
        }

        /// <summary>
        /// Поиск книг по названию
        /// </summary>
        /// <returns></returns>
        public Book GetBookByTitle(string title)
        {
            Book result = new Book();

            try
            {
                result = repository.FindByTitle(title);
            }
            catch (Exception) { Console.WriteLine($"Ошибка при поиске книг с данным названием"); }

            return result;
        }

        /// <summary>
        /// Поиск книг по заданному автору
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public List<Book> GetBooksByAuthor(Author author)
        {
            return repository.FindByAuthor(author);
        }

        /// <summary>
        /// Поиск книг по заданным датам
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public List<Book> GetBooksByYears(DateOnly from, DateOnly to)
        {
            return repository.GetBooksByYears(from, to);
        }

        public List<Book> GetAllBooks()
        {
            return repository.GetAllTable();
        }

        private void UpdateRepository(Book book)
        {
            int res = 0;
            try
            {
                res = repository.Update(book);
                if (res == 0) throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine($"Ошибка при обновлении данных книги {book.Title}");
            }
        }
    }
}
