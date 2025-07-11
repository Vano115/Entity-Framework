
using Entity_Framework.Entityes;
using Entity_Framework.Repositories;
using System.Data.SqlTypes;

namespace Entity_Framework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookRepository bookRepository = new BookRepository();
            AuthorRepository authorRepository = new AuthorRepository();
            UserRepository userRepository = new UserRepository();

            // Наполнение БД

            Author author = new Author() { Name = "Vasily" };
            Author author2 = new Author() { Name = "Eugen" };
            Author author3 = new Author() { Name = "Slava" };
            Author author4 = new Author() { Name = "Tor" };

            // Добавляем авторов в БД
            authorRepository.Add(author);
            authorRepository.Add(author2);
            authorRepository.Add(author3);
            authorRepository.Add(author4);

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

            foreach(var b in books)
            {
                bookRepository.Add(b);
            }

            // Пользователи
            User user = new User { Name = "Kak", Email = "gmail67@gmail.com"};
            User user2 = new User { Name = "Lata", Email = "gmail2@gmail.com" };
            User user3 = new User { Name = "Rezepov", Email = "g@gmail.com" };
            User user4 = new User { Name = "Vishnyakov", Email = "45591@gmail.com" };

            // Добавление пользователей в БД
            userRepository.Add(user);
            userRepository.Add(user2);
            userRepository.Add(user3);
            userRepository.Add(user4);

            // Переопределение сущностям существующих в БД данных
            user = userRepository.FindByEmail(user.Email);
            user2 = userRepository.FindByEmail(user2.Email);
            user3 = userRepository.FindByEmail(user3.Email);
            user4 = userRepository.FindByEmail(user4.Email);

            author = authorRepository.FindByName(author.Name);
            author2 = authorRepository.FindByName(author2.Name);
            author3 = authorRepository.FindByName(author3.Name);
            author4 = authorRepository.FindByName(author4.Name);

            Book book = bookRepository.FindByTitle(books[0].Title);
            Book book2 = bookRepository.FindByTitle(books[3].Title);
            Book book3 = bookRepository.FindByTitle(books[2].Title);

            userRepository.AddUserBooks(user, new List<Book> { book});
            authorRepository.AddAuthorBooks(author, new List<Book> { book3 });



            // Получение списка книг по заданому жанру
            List<Book> genreSelect = bookRepository.FindBooksInGenre("Horror"); ;
            ShowBook(genreSelect);
            Console.WriteLine("---------------------Жанр");
            
            // Получение списка книг по заданому диапазону дат
            List<Book> dateSelect = bookRepository.GetBooksByYears(new DateOnly(1, 1, 1), new DateOnly(1000, 1, 1));
            ShowBook(dateSelect);
            Console.WriteLine("---------------------с 1 по 1000 год");

            // Получение списка по заданому жанру и диапазону дат
            List<Book> genreDateSelect = dateSelect.Intersect(genreSelect).ToList();
            ShowBook(genreDateSelect);
            Console.WriteLine("---------------------Жанр и с 1 по 1000 год");

            int countAuthorBook = authorRepository.GetAuthorsBook(author).Count;
            Console.WriteLine(countAuthorBook);
            Console.WriteLine($"--------------------Количество книг автора {author.Name}");

            List<Book> usersBook = userRepository.FindById(user.Id).Books;
            ShowBook(usersBook);
            Console.WriteLine($"--------------------Книги пользователя {user.Name}");

            string genre = "Horror";
            int countGenreBook = bookRepository.FindBooksInGenre(genre).Count;
            Console.WriteLine(countGenreBook);
            Console.WriteLine($"--------------------Количество книг жанра {genre}");

            string title = "20000";
            string title2 = "tshth";
            bool checkBookTitle = bookRepository.FindByTitle(title) != null;
            bool checkBookTitle2 = bookRepository.FindByTitle(title) == null;
            Console.WriteLine(checkBookTitle + "\tПроверка существующего");
            Console.WriteLine(checkBookTitle2 + "\tПроверка не существующего");
            Console.WriteLine($"--------------------True or False на наличие книг {title} and {title2}");

            string authorForFind = "Tor";
            string authorForNotFind = "ajgnn";
            bool checkAuthor = authorRepository.FindByName(authorForFind) != null;
            bool checkAuthor2 = authorRepository.FindByName(authorForNotFind) != null;
            Console.WriteLine($"--------------------True or False на наличие авторов {authorForFind} and {authorForNotFind}");

            int countUserBook = userRepository.ListBooks(user).Count;
            int countUserBook2 = userRepository.ListBooks(user3).Count;
            Console.WriteLine($"Количество книг у пользователя {user.Name}: {countUserBook}");
            Console.WriteLine($"Количество книг у пользователя {user3.Name}: {countUserBook2}");
            Console.WriteLine($"--------------------Количество книг пользователя");

            Book moreLateBook = bookRepository.GetAllTable().MaxBy(p => p.Public_year);
            Console.WriteLine($"--------------------Самая поздняя выпущеная книга: {moreLateBook.Title}");

            List<Book> sortedByTitle = bookRepository.GetAllTable().OrderBy(p => p.Title).ToList();
            ShowBook(sortedByTitle);
            Console.WriteLine($"--------------------Список книг в алфивитном порядке");

            List<Book> sortedByYear = bookRepository.GetAllTable().OrderByDescending(p => p.Public_year).ToList();
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
}
