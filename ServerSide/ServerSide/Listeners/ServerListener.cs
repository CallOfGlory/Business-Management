using Microsoft.EntityFrameworkCore;
using ServerSide.Database;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServerSide.Listeners
{
    public class ServerListener
    {
        private HttpListener _httpListener = new HttpListener();
        private DatabaseCrud databaseManager;

        public ServerListener(DatabaseCrud database)
        {
            databaseManager = database;
        }

        public void AddPrefix(string prefix)
        {
            _httpListener.Prefixes.Add(prefix);
        }

        public async Task StartListeningAsync()
        {
            _httpListener.Start();
            Console.WriteLine("Server is listening...");
            while (true)
            {
                var context = await _httpListener.GetContextAsync();
                Task _ = Task.Run(() => ProcessRequest(context));
            }
        }
        private async Task ProcessRequest(HttpListenerContext context)
        {

            Console.WriteLine("Received request: " + context.Request.RawUrl);

            if(context.Request.RawUrl == "/login")
            {
                string d = context.Request.Headers.ToString();
                using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding ?? Encoding.UTF8);
                string body = reader.ReadToEnd();
                Console.WriteLine("Request Body: " + body);
                User dto = JsonSerializer.Deserialize<User>(body);
                await Verify_login(context, dto);
            }

            if(context.Request.RawUrl == "/register")
            {
                using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding ?? Encoding.UTF8);
                string body = reader.ReadToEnd();
                Console.WriteLine("Request: " + body);
                User dto;
                try
                {
                    dto = JsonSerializer.Deserialize<User>(body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deserializing request body: " + ex.Message);
                    SendResponse(context, "Invalid request format");
                    return;
                }
                await Verify_register(context, dto);

            }
        }

        private async Task Verify_login(HttpListenerContext context, User dto)
        {

            bool data = await databaseManager.CheckUserDails(dto);
            if (data)
            {
                SendResponse(context, "True");
            }
            else
            {
                SendResponse(context, "False");
            }
        }

        private async Task Verify_register(HttpListenerContext context, User dto)
        {
            Console.WriteLine("Verifying registration for email: " + dto.Email);
            bool data = await databaseManager.CheckEmail(dto);
            if (data)
            {
                Console.WriteLine("User already exists");
                SendResponse(context, "User already exists");
            }
            else
            {
                dto.CreatedAt = DateTime.Now;
                databaseManager.AddUserAsync(dto);
                SendResponse(context, "User registered successfully");
            }
        }

        private void SendResponse(HttpListenerContext context, string responseString)
        {
            Console.WriteLine("Sending response: " + responseString);
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}
