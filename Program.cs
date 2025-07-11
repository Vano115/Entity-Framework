
using Entity_Framework.Entityes;
using Entity_Framework.Repositories;
using Entity_Framework.Services;
using System.Data.SqlTypes;

namespace Entity_Framework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> one = new List<int>() { 1, 2, 3, 4 };
            List<int> two = new List<int>() { 1, 2, 3, 4 };
            foreach (var item in one.Except(two))
            {
                Console.WriteLine(item);
            };


            BookService bookService = new BookService();
            AuthorService authorService = new AuthorService();
            UserService userService = new UserService();

            // Наполнение БД

            Author author = new Author() { Name = "Vasily" };
            Author author2 = new Author() { Name = "Eugen" };
            Author author3 = new Author() { Name = "Slava" };
            Author author4 = new Author() { Name = "Tor" };

            // Добавляем авторов в БД
            authorService.AddAuthor(author);
            authorService.AddAuthor(author2);
            authorService.AddAuthor(author3);
            authorService.AddAuthor(author4);

            // Создаём книг
            List<Book> books = new List<Book> { new Book() { Title = "Piece", Genre = "Horror", Public_year = new DateOnly(10, 1, 6) },
            new Book() { Title = "Param", Genre = "Adventure", Public_year = new DateOnly(200, 1, 6)},
            new Book() { Title = "Thomas", Genre = "Adventure", Public_year = new DateOnly(2000, 1, 6)},
            new Book() { Title = "20000", Genre = "Horror", Public_year = new DateOnly(1573, 1, 6) },
            new Book() { Title = "New", Genre = "Horror", Public_year = new DateOnly(1958, 1, 6) },
            new Book() { Title = "Old", Genre = "Doc", Public_year = new DateOnly(1573, 1, 6) },
            new Book() { Title = "One", Genre = "Doc", Public_year = new DateOnly(1200, 1, 6) },
            new Book() { Title = "Two", Genre = "Doc", Public_year = new DateOnly(780, 1, 6) }
            };

            // Добавляем книги в БД

            bookService.AddBook(books);

            // Пользователи
            User user = new User { Name = "Kak", Email = "gmail67@gmail.com"};
            User user2 = new User { Name = "Lata", Email = "gmail2@gmail.com" };
            User user3 = new User { Name = "Rezepov", Email = "g@gmail.com" };
            User user4 = new User { Name = "Vishnyakov", Email = "45591@gmail.com" };

            // Добавление пользователей в БД
            userService.AddUser(user);
            userService.AddUser(user2);
            userService.AddUser(user3);
            userService.AddUser(user4);

            // Переопределение сущностям существующих в БД данных
            user = userService.FindUserById(1);
            user2 = userService.FindUserById(2);
            user3 = userService.FindUserById(3);

            author = authorService.FindAuthorById(1);
            author2 = authorService.FindAuthorById(2);
            author3 = authorService.FindAuthorById(3);
            author4 = authorService.FindAuthorById(4);

            Book book = bookService.FindBookById(1);
            Book book2 = bookService.FindBookById(2);
            Book book3 = bookService.FindBookById(3);

            userService.GetBooks(user, new List<Book> { book, book2});
            userService.GetBooks(user3, new List<Book> { book3, book2 });

            bookService.AddAuthors(book, new List<Author> { author });
            bookService.AddAuthors(book2, new List<Author> {author2, author});
            bookService.AddAuthors(book3, new List<Author> { author3 });



            // Получение списка книг по заданому жанру
            List<Book> genreSelect = bookService.GetBooksByGenre();
            ShowBook(genreSelect);
            Console.WriteLine("---------------------Жанр");

            // Получение списка книг по заданому диапазону дат
            List<Book> dateSelect = bookService.GetBooksByYears(new DateOnly(1, 1, 1), new DateOnly(1000, 1, 1));
            ShowBook(dateSelect);
            Console.WriteLine("---------------------с 1 по 1000 год");

            // Получение списка по заданому жанру и диапазону дат
            List<Book> genreDateSelect = dateSelect.Intersect(genreSelect).ToList();
            ShowBook(genreDateSelect);
            Console.WriteLine("---------------------Жанр и с 1 по 1000 год");

            int countAuthorBook = authorService.GetCountBooks(author);
            Console.WriteLine(countAuthorBook);
            Console.WriteLine($"--------------------Количество книг автора {author.Name}");

            List<Book> usersBook = userService.GetUsersBooks(user);
            ShowBook(usersBook);
            Console.WriteLine($"--------------------Книги пользователя {user.Name}");

            string genre = "Horror";
            int countGenreBook = bookService.GetBooksByGenre(genre).Count;
            Console.WriteLine(countGenreBook);
            Console.WriteLine($"--------------------Количество книг жанра {genre}");

            string title = "20000";
            string title2 = "19999";
            bool checkBookTitle = bookService.GetBookByTitle(title) != null;
            bool checkBookTitle2 = bookService.GetBookByTitle(title2) != null;
            Console.WriteLine(checkBookTitle + "\tПроверка существующего");
            Console.WriteLine(checkBookTitle2 + "\tПроверка не существующего");
            Console.WriteLine($"--------------------True or False на наличие книг {title} and {title2}");

            string authorForFind = "Tor";
            string authorForNotFind = "ajgnn";
            bool checkAuthor = authorService.FindAuthorByName(authorForFind) != null;
            bool checkAuthor2 = authorService.FindAuthorByName(authorForNotFind) != null;
            Console.WriteLine($"--------------------True or False на наличие авторов {authorForFind} and {authorForNotFind}");

            int countUserBook = userService.GetUsersBooks(user).Count;
            int countUserBook2 = userService.GetUsersBooks(user3).Count;
            Console.WriteLine($"Количество книг у пользователя {user.Name}: {countUserBook}");
            Console.WriteLine($"Количество книг у пользователя {user3.Name}: {countUserBook2}");
            Console.WriteLine($"--------------------Количество книг пользователя");

            Book moreLateBook = bookService.GetAllBooks().MaxBy(p => p.Public_year);
            Console.WriteLine($"--------------------Самая поздняя выпущеная книга: {moreLateBook.Title}");

            List<Book> sortedByTitle = bookService.GetAllBooks().OrderBy(p => p.Title).ToList();
            ShowBook(sortedByTitle);
            Console.WriteLine($"--------------------Список книг в алфивитном порядке");

            List<Book> sortedByYear = bookService.GetAllBooks().OrderByDescending(p => p.Public_year).ToList();
            ShowBook(sortedByYear);
            Console.WriteLine($"--------------------Список книг в порядке убывания даты выхода");
        }



        public static void ShowBook(List<Book> books)
        {
            Console.WriteLine("////////////////////////////////////////");
            foreach (Book book in books)
            {
                Console.WriteLine($"Id\t{book.Id}");
                Console.WriteLine($"Title\t{book.Title}");
                Console.WriteLine($"Genre\t{book.Genre}");
                Console.WriteLine($"Public year\t{book.Public_year.Value.Year.ToString()}");
                Console.WriteLine("*****************************************");
            }
            Console.WriteLine("////////////////////////////////////////");
        }

        public static void ShowBook(Book book)
        {
            Console.WriteLine("////////////////////////////////////////");
            Console.Write($"Id\t{book.Id}");
            Console.Write($"Title\t{book.Title}");
            Console.Write($"Genre\t{book.Genre}");
            Console.Write($"Public year\t{book.Public_year.ToString()}");
            Console.WriteLine("////////////////////////////////////////");
        }
    }


        // Нужно реализовать 
        //Получать список книг определенного жанра и вышедших между определенными годами.
        //Получать количество книг определенного автора в библиотеке.
        //Получать количество книг определенного жанра в библиотеке.
        //Получать булевый флаг о том, есть ли книга определенного автора и с определенным названием в библиотеке.
        //Получать булевый флаг о том, есть ли определенная книга на руках у пользователя.
        //Получать количество книг на руках у пользователя.
        //Получение последней вышедшей книги.
        //Получение списка всех книг, отсортированного в алфавитном порядке по названию.
        //Получение списка всех книг, отсортированного в порядке убывания года их выхода.


        // По красоте надо это дело всё разбить на подсистемы, работать через родительские классы
        // В Servicах будут консольные команды
        // В Repositories будут команды к БД
}
