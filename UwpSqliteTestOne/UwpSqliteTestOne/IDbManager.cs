using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSqliteTestOne
{
    public interface IDbManager
    {
        Task InitializeDatabase();
        void Open();
        void Close();
        SqliteConnection GetConnection();
    }
}
