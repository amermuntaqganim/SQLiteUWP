using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UwpSqliteTestOne
{
    public class ImageManager
    {

        public static ImageManager Instance = Singleton<ImageManager>.Instance;
        private ImageManager() { }

        /* public async Task DownloadAndSaveImages(List<string> imageUrls)
         {
             using (var connection = DbManager.Instance.GetConnection())
             {
                 await connection.OpenAsync();
                 foreach (var url in imageUrls)
                 {
                     byte[] imageData = await DownloadImage(url);
                     await SaveImageToDatabase(url, imageData);
                 }
             }
         }*/

        public async Task DownloadAndSaveImages(List<string> imageUrls)
        {

            var tasks = new List<Task>();
            foreach (var url in imageUrls)
            {
                tasks.Add(DownloadAndSaveImageAsync(url));
            }
            await Task.WhenAll(tasks);

        }

        private async Task DownloadAndSaveImageAsync(string url)
        {
            try
            {
                byte[] imageData = await DownloadImage(url);
                await SaveImageToDatabase(url, imageData);
            }
            catch (Exception ex)
            {
                // Handle download or insertion errors
                Console.WriteLine($"Error downloading or saving image from {url}: {ex.Message}");
            }
        }

        private async Task<byte[]> DownloadImage(string url)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetByteArrayAsync(url);
            }
        }

        private async Task SaveImageToDatabase( string url, byte[] imageData)
        {

            using (var connection = DbManager.Instance.GetConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Images (Url, ImageData) VALUES (@Url, @ImageData);";
                        command.Parameters.AddWithValue("@Url", url);
                        command.Parameters.AddWithValue("@ImageData", imageData);
                        await command.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                }
            }

        }

        public async Task<byte[]> GetImageFromDatabase(string url)
        {
            using (var connection = DbManager.Instance.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT ImageData FROM Images WHERE Url = @Url;";
                    command.Parameters.AddWithValue("@Url", url);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (byte[])reader["ImageData"];
                        }
                    }
                }
            }
            return null; // Return null if image not found
        }

    }
}
