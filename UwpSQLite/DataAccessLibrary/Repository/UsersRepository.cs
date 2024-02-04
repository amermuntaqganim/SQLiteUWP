using DataAccessLibrary.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Repository
{
    public class UsersRepository : GenericRepository<Users>
    {
        public UsersRepository() : base()
        {
            // You can add specific initialization for MyEntityRepository here
        }

        public override void Insert(Users user)
        {
            var command = CreateCommand($"INSERT INTO {typeof(Users).Name} (Name, Age) VALUES (@Name, @Age)");
            AddParameter(command, "@Name", user.Name);
            AddParameter(command, "@Age", user.Age);
            command.ExecuteNonQuery();
        }

        public override void Update(Users user)
        {
            var command = CreateCommand($"UPDATE {typeof(Users).Name} SET Name = @Name WHERE Id = @Id");
            AddParameter(command, "@Name", user.Name);
            AddParameter(command, "@Id", user.Id);
            command.ExecuteNonQuery();
        }

        public override void Delete(int id)
        {
            var command = CreateCommand($"DELETE FROM {typeof(Users).Name} WHERE Id = @Id");
            AddParameter(command, "@Id", id);
            command.ExecuteNonQuery();
        }

        public override List<Users> GetAll()
        {
            var result = new List<Users>();

            var command = CreateCommand($"SELECT * FROM {typeof(Users).Name}");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Users
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString()
                    });
                }
            }

            return result;
        }

        public override Users GetById(int id)
        {
            var command = CreateCommand($"SELECT * FROM {typeof(Users).Name} WHERE Id = @Id");
            AddParameter(command, "@Id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Users
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString()
                    };
                }
            }

            return null;
        }

    }
}
