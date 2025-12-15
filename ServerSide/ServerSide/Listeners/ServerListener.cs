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
        private TokenModule _tokenModule;

        public ServerListener(DatabaseCrud database, TokenModule tokenModule)
        {
            databaseManager = database;
            _tokenModule = tokenModule;
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

            //if (context.Request.HttpMethod == "OPTIONS")
            //{
            //    AddCorsHeaders(context);
            //    context.Response.StatusCode = 204; // No Content
            //    context.Response.Close();
            //    return;
            //}

            if (context.Request.RawUrl == "/login")
            {
                string d = context.Request.Headers.ToString();
                using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding ?? Encoding.UTF8);
                string body = reader.ReadToEnd();
                Console.WriteLine("Request Body: " + body);
                User dto = JsonSerializer.Deserialize<User>(body);
                await Verify_login(context, dto);
            }

            if (context.Request.RawUrl == "/register")
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

            if (context.Request.RawUrl == "/login_token")
            {
                using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding ?? Encoding.UTF8);
                string body = reader.ReadToEnd();
                Console.WriteLine("Request: " + body);
                User user = await databaseManager.GetUserByToken(body);

                if (user != null)
                {
                    string responseJson = JsonSerializer.Serialize(user);
                    SendResponse(context, responseJson);
                }
                else
                {
                    SendResponse(context, "Invalid token");
                }
            }

            if (context.Request.RawUrl == "/check_auth")
            {
                await Verify_Access(context);
            }
        }

        private async Task Verify_login(HttpListenerContext context, User dto)
        {
            bool data = await databaseManager.CheckUserDails(dto);
            if (data)
            {

                User user = await databaseManager.GetUserByEmail(dto.Email);
                string token = _tokenModule.GenerateToken(user.Id);
                user.Token = token;
                databaseManager.UpdateUserTokenByEmail(user, dto.Email);

                CookieManager.SetHttpOnlyCookie(context.Response, "AuthToken", token, 7);

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
                SendResponse(context, "False");
            }
            else
            {
                dto.CreatedAt = DateTime.Now;
                string token = _tokenModule.GenerateToken(dto.Id);
                dto.Token = token;
                databaseManager.AddUserAsync(dto);

                CookieManager.SetHttpOnlyCookie(context.Response, "AuthToken", token, 7);

                SendResponse(context, "True");
            }
        }

        private async Task Verify_Access(HttpListenerContext context)
        {
            Console.WriteLine("Verifying access with cookies");
            CookieCollection cookies = context.Request.Cookies;
            Console.WriteLine("Found cookie: " + cookies.Count());
            if (cookies.Count == 0)
            {
                SendResponse(context, "False");
                return;
            }
            foreach (Cookie cookie in cookies)
            {
                Console.WriteLine("Found cookie: " + cookie.Name + "Content: " + cookie.Value);
                if (cookie.Name == "AuthToken")
                {
                    string token = cookie.Value;
                    bool isValid = _tokenModule.ValidateToken(token);
                    if (isValid)
                    {
                        User user = await databaseManager.GetUserByToken(token);
                        string responseJson = JsonSerializer.Serialize(user);
                        SendResponse(context, responseJson);
                        return;
                    }
                    else
                    {
                        SendResponse(context, "False");
                        return;
                    }
                }
            }
            SendResponse(context, "False");
        }


        private void SendResponse(HttpListenerContext context, string responseString)
        {
            AddCorsHeaders(context);
            Console.WriteLine("Sending response: " + responseString);

            CookieCollection responseCookies = context.Response.Cookies;
            foreach (Cookie cookie in responseCookies)
            {
                Console.WriteLine("Response cookie: " + cookie.Name + " = " + cookie.Value);
            }
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        private void AddCorsHeaders(HttpListenerContext context)
        {
            try
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5500");

                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");

                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, Cookie");

                context.Response.Headers.Add("Access-Control-Expose-Headers", "Set-Cookie");

                Console.WriteLine("CORS headers added");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding CORS headers: {ex.Message}");
            }
        }
    }

}
