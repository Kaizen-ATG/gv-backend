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

        public APIGatewayProxyResponse GetUser(APIGatewayProxyRequest request)
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
                User user = new User(reader.GetInt16("user_id"), reader.GetString("username"), reader.GetString("email"), reader.GetInt16("role_id"));
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
        public APIGatewayProxyResponse GetUsers(APIGatewayProxyRequest request)
        {
            LambdaLogger.Log("Getting list of users");

            MySqlConnection connection = new MySqlConnection($"server={DB_HOST};user id={DB_USER};password={DB_PASSWORD};port=3306;database={DB_NAME};");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM user";

            MySqlDataReader reader = cmd.ExecuteReader();
            ArrayList users = new ArrayList();

            while (reader.Read())
            {
                User user = new User(reader.GetInt16("user_id"), reader.GetString("username"), reader.GetString("email"), reader.GetInt16("role_id"));
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
        public APIGatewayProxyResponse GetRedeemOffers(APIGatewayProxyRequest request)
        {
            LambdaLogger.Log("Getting detials for RedeemOffers");
            MySqlConnection connection = new MySqlConnection($"server={DB_HOST};user id={DB_USER};password={DB_PASSWORD};port=3306;database={DB_NAME};");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM redeemoffers";
            MySqlDataReader reader = cmd.ExecuteReader();
            ArrayList offers = new ArrayList();

            while (reader.Read())
            {
                RedeemOffers offer = new RedeemOffers
                                    (reader.GetString("deal_type"), reader.GetInt16("points_required"), reader.GetString("deal_code"), reader.GetString("description"));
                offers.Add(offer);
            }

            connection.Close();

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(offers),
                Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            },
                StatusCode = 200,
            };
        }
        public APIGatewayProxyResponse SaveUser(APIGatewayProxyRequest request)
        {
            string requestBody = request.Body;
            User u = JsonSerializer.Deserialize<User>(requestBody);
            LambdaLogger.Log("Saving user info: " + u.UserId);
            
            return new APIGatewayProxyResponse
            {
                Body = "user info Saved",
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
        public int Role { get; set; }

        public User() { }

        public User(int user_id, string username, string email, int role_id)
        {
            UserId = user_id;
            UserName = username;
            Email = email;
            Role = role_id;
        }
    }
    public class RedeemOffers
    {
        public string Dealtype { get; set; }
        public int PointsRequired { get; set; }
        public string Dealcode { get; set; }
        public string Description { get; set; }

        //public RedeemOffers() { }

        public RedeemOffers(string deal_type, int points_required, string deal_code, string description)
        {
            Dealtype = deal_type;
            PointsRequired = points_required;
            Dealcode = deal_code;
            Description = description;
        }
    }
}