using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Entityes
{
    public class User : IEquatable<User>
    {
        
        /// <summary>
        /// Метод сравнения пользовательского класса, 
        /// написал для того чтобы работал Linq при сравнении множеств
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(User other)
        {
            if (other is null)
                return false;

            return this.Id == other.Id;
        }
        public override bool Equals(object obj) => Equals(obj as User);
        public override int GetHashCode() => (Id).GetHashCode();

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        // Навигационное свойство
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
