using DataAccessLibrary.Manager;
using DataAccessLibrary.Repository;
using DataAccessLibrary.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using UwpSQLite.Database;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpSQLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int Counter = 0;
        int updateCounter = 0;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void InsertClick(object sender, RoutedEventArgs e)
        {
            Counter++;
            // Assuming you have a MyEntity object
            MyEntity myEntity = new MyEntity
            {
                Name = "SampleEntity"+Counter
            };

            DbManager.Instance.CreateTable<MyEntity>();

            // Create an instance of MyEntityRepository
            using (var myEntityRepository = new MyEntityRepository())
            {
                // Insert a new entity
                myEntityRepository.Insert(myEntity);

                // Retrieve all entities
                List<MyEntity> allEntities = myEntityRepository.GetAll();
                foreach (var entity in allEntities)
                {
                    Console.WriteLine($"Entity Id: {entity.Id}, Name: {entity.Name}");
                }

                // Update an entity
/*                MyEntity entityToUpdate = myEntityRepository.GetById(1);
                if (entityToUpdate != null)
                {
                    entityToUpdate.Name = "UpdatedEntity";
                    myEntityRepository.Update(entityToUpdate);
                }

                // Delete an entity
                int entityIdToDelete = 2; // Replace with the actual Id of the entity you want to delete
                myEntityRepository.Delete(entityIdToDelete);*/
            }
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            updateCounter++;
            // Update an entity

            // Create an instance of MyEntityRepository
            using (var myEntityRepository = new MyEntityRepository())
            {


                // Update an entity
                MyEntity entityToUpdate = myEntityRepository.GetById(1);
                if (entityToUpdate != null)
                {
                    entityToUpdate.Name = "UpdatedEntity"+updateCounter;
                    myEntityRepository.Update(entityToUpdate);
                }

            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            using (var myEntityRepository = new MyEntityRepository())
            {


                // Delete an entity
                int entityIdToDelete = 2; // Replace with the actual Id of the entity you want to delete
                myEntityRepository.Delete(entityIdToDelete); 

            }
        }

        private void InsertButtonClick(object sender, RoutedEventArgs e)
        {
            Counter++;
            // Assuming you have a MyEntity object
            Users user = new Users
            {
                Name = "User " + Counter,
                Age = Counter
            };  

            SQLiteDBManager.Instance.CreateTable<Users>();

            // Create an instance of MyEntityRepository
            using (var usersRepository = new UsersRepository())
            {
                // Insert a new entity
                usersRepository.Insert(user);

                // Retrieve all entities
                List<Users> allUsers = usersRepository.GetAll();
                foreach (var entity in allUsers)
                {
                    LogManager.WriteLogs($"Entity Id: {entity.Id}, Name: {entity.Name}");
                }
            }
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            updateCounter++;
            // Update an entity

            // Create an instance of MyEntityRepository
            using (var userRepository = new UsersRepository())
            {
                // Update an entity
                Users usersToUpdate = userRepository.GetById(1);
                if (usersToUpdate != null)
                {
                    usersToUpdate.Name = "UpdatedEntity" + updateCounter;
                    userRepository.Update(usersToUpdate);
                }

            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            using (var usersRepository = new UsersRepository())
            {


                // Delete an entity
                int userIdToDelete = 2; // Replace with the actual Id of the entity you want to delete
                usersRepository.Delete(userIdToDelete);

            }
        }
    }
}
