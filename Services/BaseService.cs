using Entity_Framework.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Services
{
    public class BaseService
    {
        public BaseService() { }

        /// <summary>
        /// Проверка корректности введённого имени
        /// </summary>
        /// <param name="name"> Строка с введёным пользователем значением</param>
        /// <returns>True если строка прошла проверку, иначе False</returns>
        internal bool TextChecker(string name)
        {
            if (name.IsNullOrEmpty()) return false;

            return true;
        }
    }
}
