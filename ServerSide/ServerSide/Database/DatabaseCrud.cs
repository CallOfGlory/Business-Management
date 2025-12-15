using Azure.Identity;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServerSide.Database
{
    
    public class DatabaseCrud
    {
        private const string _connectionString = "Server=DESKTOP-7BTDA28;Database=BusinessManagement;TrustServerCertificate=true;Trusted_Connection=True;";


        async public Task<List<User>> GetAllUsersAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Users";
            var users = connection.Query<User>(query).AsList();
            return users;
        }

        async public void AddUserAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "INSERT INTO Users (Login, Email, Password, CreatedAt, Company, Full_Name, Phone, Token) VALUES (@Login, @Email, @Password, @CreatedAt, @Company, @Full_Name, @Phone, @Token)";
            connection.Execute(query, user);
        }

        async public Task<bool> CheckUserDails(User user)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password AND Email = @Email";
            int result = Convert.ToInt16(connection.ExecuteScalar(query, new { user.Login, user.Password, user.Email }));
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        async public Task<bool> CheckEmail(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            int result = Convert.ToInt16(connection.ExecuteScalar(query, new { user.Email }));
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        async public Task<bool> UpdateUserEmail(User user, string newEmail)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "UPDATE Users SET Email = @NewEmail WHERE Email = @Email";
            int rowsAffected = connection.Execute(query, new { NewEmail = newEmail, Email = user.Email });
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        async public Task<bool> DeleteUser(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "DELETE FROM Users WHERE Login = @Login AND Password = @Password AND Email = @Email";
            int rowsAffected = connection.Execute(query, new { user.Login, user.Password, user.Email });
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        async public Task<User> GetUserByToken(string token)
        {
            Console.WriteLine("Getting user by token: " + token);
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT Id, Login, Email, Company, Full_Name, Phone FROM Users WHERE Token = @Token";
            User user = connection.QuerySingleOrDefault<User>(query, new { Token = token });
            return user;
        }

        async public Task<User> GetUserByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Users WHERE Email = @Email";
            User user = connection.QuerySingleOrDefault<User>(query, new { Email = email });
            return user;
        }
        async public void UpdateUserTokenByEmail(User user, string email)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "UPDATE Users SET Token = @Token WHERE Email = @Email";
            connection.Execute(query, new { user.Token, Email = email });
        }
    }
}
