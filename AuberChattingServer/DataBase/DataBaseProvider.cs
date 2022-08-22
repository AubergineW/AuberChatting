using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuberChattingServer.DataBase
{
    public static class DataBaseProvider
    {
        public static IDataBase GetDataBase()
        {
            return new MySQLDataAccess();
        }
    }
}
