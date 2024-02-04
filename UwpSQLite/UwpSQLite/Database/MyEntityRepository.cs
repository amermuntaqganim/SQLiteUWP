using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSQLite.Database
{
    public class MyEntityRepository : GenericRepository<MyEntity>
    {
        public MyEntityRepository() : base()
        {
            // You can add specific initialization for MyEntityRepository here
        }

        public override void Insert(MyEntity entity)
        {
            var command = CreateCommand($"INSERT INTO {typeof(MyEntity).Name} (Name) VALUES (@Name)");
            AddParameter(command, "@Name", entity.Name);
            command.ExecuteNonQuery();
        }

        public override void Update(MyEntity entity)
        {
            var command = CreateCommand($"UPDATE {typeof(MyEntity).Name} SET Name = @Name WHERE Id = @Id");
            AddParameter(command, "@Name", entity.Name);
            AddParameter(command, "@Id", entity.Id);
            command.ExecuteNonQuery();
        }

        public override void Delete(int id)
        {
            var command = CreateCommand($"DELETE FROM {typeof(MyEntity).Name} WHERE Id = @Id");
            AddParameter(command, "@Id", id);
            command.ExecuteNonQuery();
        }

        public override List<MyEntity> GetAll()
        {
            var result = new List<MyEntity>();

            var command = CreateCommand($"SELECT * FROM {typeof(MyEntity).Name}");

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new MyEntity
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString()
                    });
                }
            }

            return result;
        }

        public override MyEntity GetById(int id)
        {
            var command = CreateCommand($"SELECT * FROM {typeof(MyEntity).Name} WHERE Id = @Id");
            AddParameter(command, "@Id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new MyEntity
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
