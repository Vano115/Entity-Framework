using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Configuration
{
    internal class ConnectionString
    {
        // Строка соединения с БД
        internal static string MsSqlConnection =>
            @"Data Source=DESKTOP-0UOOKMK;Database=Entity;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
