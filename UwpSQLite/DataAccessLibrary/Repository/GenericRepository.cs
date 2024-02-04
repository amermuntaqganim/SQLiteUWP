using DataAccessLibrary.Interface;
using DataAccessLibrary.Manager;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Repository
{
    public abstract class GenericRepository<T> : IDisposable, IRepository<T> where T : class
    {

        protected SqliteConnection connection;

        protected GenericRepository()
        {
            // Set up your database connection here
            connection = SQLiteDBManager.Instance.GetSqliteConnection();
            connection.Open();
        }

        public abstract void Insert(T entity);
        public abstract void Update(T entity);
        public abstract void Delete(int id);
        public abstract List<T> GetAll();
        public abstract T GetById(int id);

        protected SqliteCommand CreateCommand(string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        protected void AddParameter(SqliteCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }

}

