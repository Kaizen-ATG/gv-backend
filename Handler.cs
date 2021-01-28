using Amazon.Lambda.Core;
using System.Collections;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using MySql.Data.MySqlClient;
using System;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace AwsDotnetCsharp
{
    public class Handler
    {
        static string DB_HOST = System.Environment.GetEnvironmentVariable("DB_HOST");
        static string DB_NAME = System.Environment.GetEnvironmentVariable("DB_NAME");
        static string DB_USER = System.Environment.GetEnvironmentVariable("DB_USER");
        static string DB_PASSWORD = System.Environment.GetEnvironmentVariable("DB_PASSWORD");

        MySqlConnection connection = new MySqlConnection($"server={DB_HOST};user id={DB_USER};password={DB_PASSWORD};port=3306;database={DB_NAME};");
        public APIGatewayProxyResponse GetUser(APIGatewayProxyRequest request)
        {
            int userId = Int32.Parse(request.PathParameters["userId"]);
            LambdaLogger.Log("Getting detials for: " + userId);
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
                Body = System.Text.Json.JsonSerializer.Serialize(users),
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
                Body = System.Text.Json.JsonSerializer.Serialize(users),
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
                Body = System.Text.Json.JsonSerializer.Serialize(offers),
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
            LambdaLogger.Log(requestBody);
            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(requestBody);
            LambdaLogger.Log("Saving user info: " + user.UserName);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO user(username,email,role_id) values(@name,@email,@role)";
            cmd.Parameters.AddWithValue("@name", user.UserName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@role", 501);
            LambdaLogger.Log("Saving user info: " + cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.Close();
            return new APIGatewayProxyResponse
            {
                Body = "User info Saved",
                Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            },
                StatusCode = 200,
            };
        }
        public APIGatewayProxyResponse SaveUserPoints(APIGatewayProxyRequest request)
        {
            string requestBody = request.Body;
            UserPoints pts = Newtonsoft.Json.JsonConvert.DeserializeObject<UserPoints>(requestBody);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO userpoints(green_points,carbon_points,weekGP,weekCP,user_id) values(@greenpts,@carbonpts,@wgpts,@wcpts,@userid)";
            cmd.Parameters.AddWithValue("@greenpts", pts.GreenPoints);
            cmd.Parameters.AddWithValue("@carbonpts", pts.CarbonPoints);
            cmd.Parameters.AddWithValue("@wgpts", pts.WeekGP);
            cmd.Parameters.AddWithValue("@wcpts", pts.WeekCP);
            cmd.Parameters.AddWithValue("@userid", pts.UserId);
            cmd.ExecuteNonQuery();
            connection.Close();
            return new APIGatewayProxyResponse
            {
                Body = "User points Saved",
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

        public RedeemOffers() { }

        public RedeemOffers(string deal_type, int points_required, string deal_code, string description)
        {
            Dealtype = deal_type;
            PointsRequired = points_required;
            Dealcode = deal_code;
            Description = description;
        }
    }
    public class UserPoints
    {
        public int GreenPoints { get; set; }
        public int CarbonPoints { get; set; }
        public int WeekGP { get; set; }
        public int WeekCP { get; set; }
        public int UserId { get; set; }
        public UserPoints() { }
        public UserPoints(int user_id, int green_points, int carbon_points, int weekGP, int weekCP)
        {
            GreenPoints = green_points;
            CarbonPoints = carbon_points;
            WeekGP = weekGP;
            WeekCP = weekCP;
            UserId = user_id;
        }
    }
}