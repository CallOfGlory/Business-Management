using System;
using System.Threading.Tasks;
using ServerSide.Database;
using ServerSide.Listeners;

namespace ServerSide
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TokenModule tokenModule = new TokenModule();
            DatabaseCrud databaseManager = new DatabaseCrud();
            User user = new User
            {
                Login = "exampleUser",
                Email = "test@gmail.com",
                Password = "securePassword123",
                CreatedAt = DateTime.UtcNow
            };

            ////databaseManager.AddUserAsync(user);
            //await Task.Delay(2000); // optional short delay if needed for downstream async operations

            //var users = await databaseManager.GetAllUsersAsync();
            //foreach (var u in users)
            //{
            //    Console.WriteLine($"ID: {u.Id}, Login: {u.Login}, Email: {u.Email}, CreatedAt: {u.CreatedAt}");
            //}

            bool data = await databaseManager.CheckUserDails(user);
            Console.WriteLine($"User exists: {data}");

            //bool uodateEmail = await databaseManager.UpdateUserEmail(user, "test@gmail.com");
            //Console.WriteLine($"Email updated: {uodateEmail}");

            ServerListener serverListener = new ServerListener(databaseManager, tokenModule);
            serverListener.AddPrefix("http://localhost:8080/");
            await serverListener.StartListeningAsync();
        }
    }
}