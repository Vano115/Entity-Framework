using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Entityes
{
    public class Book : IEquatable<Book>
    {
        /// <summary>
        /// Метод сравнения пользовательского класса, 
        /// написал для того чтобы работал Linq при сравнении множеств
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Book other)
        {
            if (other is null)
                return false;

            return this.Id == other.Id;
        }
        public override bool Equals(object obj) => Equals(obj as Book);
        public override int GetHashCode() => (Id).GetHashCode();

        public int Id { get; set; }
        public string? Title { get; set; }
        public DateOnly? Public_year { get; set; }

        public string? Genre { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();

        public List<User> Users { get; set; } = new List<User>();
    }
}
