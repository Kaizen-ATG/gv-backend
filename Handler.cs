using Amazon.Lambda.Core;
using System.Collections;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using MySql.Data.MySqlClient;
using System;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace AwsDotnetCsharp
{
    public class Handler
    {
        private string DB_HOST = System.Environment.GetEnvironmentVariable("DB_HOST");
        private string DB_NAME = System.Environment.GetEnvironmentVariable("DB_NAME");
        private string DB_USER = System.Environment.GetEnvironmentVariable("DB_USER");
        private string DB_PASSWORD = System.Environment.GetEnvironmentVariable("DB_PASSWORD");

        public APIGatewayProxyResponse GetUsers(APIGatewayProxyRequest request)
        {
            int userId = Int32.Parse(request.PathParameters["userId"]);
            LambdaLogger.Log("Getting detials for: " + userId);

            MySqlConnection connection = new MySqlConnection($"server={DB_HOST};user id={DB_USER};password={DB_PASSWORD};port=3306;database={DB_NAME};");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `user` WHERE `user_id` = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);

            MySqlDataReader reader = cmd.ExecuteReader();
            ArrayList users = new ArrayList();

            while (reader.Read())
            {
                User user = new User(reader.GetInt16("user_id"), reader.GetString("username"), reader.GetString("email"),reader.GetInt16("role_id"));
                users.Add(user);
            }

            connection.Close();

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(users),
                Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            },
                StatusCode = 200,
            };
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int  Role { get; set; }

        public User() { }

        public User(int user_id, string username, string email,int role_id)
        {
            UserId = user_id;
            UserName = username;
            Email = email;
            Role=role_id;
        }
    }
}