using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ServerSide.Database
{
    public class User
    {
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string Login { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        [MinLength(6)]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("phone")]
        [MaxLength(30)]
        public string Phone { get; set; }

        [JsonPropertyName("company")]
        [MaxLength(100)]
        public string Company { get; set; }

        [JsonPropertyName("full_name")]
        [MaxLength(200)]
        public string Full_Name { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        public void Print()
        {
            Console.WriteLine($"Username: {Login}, Password: {Password}, Email: {Email}");
        }

        public void PrintFull()
        {
            Console.WriteLine($"ID: {Id}, Username: {Login}, Password: {Password}, Email: {Email}, CreatedAt: {CreatedAt}, Phone: {Phone}, Company: {Company}, Full Name: {Full_Name}");
        }
    }
}
