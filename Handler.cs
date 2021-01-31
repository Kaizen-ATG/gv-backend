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
            string userId = request.PathParameters["userId"].ToString();
            LambdaLogger.Log("Getting detials for: " + userId);
            using (connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM `user` WHERE `user_id` = @userId";
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();
                ArrayList users = new ArrayList();
                while (reader.Read())
                {
                    User user = new User(reader.GetString("user_id"), reader.GetString("username"), reader.GetString("email"), reader.GetInt16("role_id"));
                    users.Add(user);
                }
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
        }
        public APIGatewayProxyResponse GetUsers(APIGatewayProxyRequest request)
        {
            LambdaLogger.Log("Getting list of users");
            using (connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM user";
                MySqlDataReader reader = cmd.ExecuteReader();
                ArrayList users = new ArrayList();
                while (reader.Read())
                {
                    User user = new User(reader.GetString("user_id"), reader.GetString("username"), reader.GetString("email"), reader.GetInt16("role_id"));
                    users.Add(user);
                }
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
        }
        public APIGatewayProxyResponse GetRedeemOffers(APIGatewayProxyRequest request)
        {
            LambdaLogger.Log("Getting detials for RedeemOffers");
            using (connection)
            {
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
        }
        public APIGatewayProxyResponse SaveUser(APIGatewayProxyRequest request)
        {
            LambdaLogger.Log("Saving New User");
            string requestBody = request.Body;
            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(requestBody);
            using (connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO user(user_id,username,email,role_id) values(@id,@name,@email,@role)";
                cmd.Parameters.AddWithValue("@id", user.UserId);
                cmd.Parameters.AddWithValue("@name", user.UserName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@role", 501);
                cmd.ExecuteNonQuery();
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
        }
        public APIGatewayProxyResponse SaveUserPoints(APIGatewayProxyRequest request)
        {
            LambdaLogger.Log("Saving User Points");
            string requestBody = request.Body;
            UserPoints pts = Newtonsoft.Json.JsonConvert.DeserializeObject<UserPoints>(requestBody);
            using (connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO userpoints(green_points,carbon_points,weekGP,weekCP,user_id) values(@greenpts,@carbonpts,@wgpts,@wcpts,@userid)";
                cmd.Parameters.AddWithValue("@greenpts", pts.GreenPoints);
                cmd.Parameters.AddWithValue("@carbonpts", pts.CarbonPoints);
                cmd.Parameters.AddWithValue("@wgpts", pts.WeekGP);
                cmd.Parameters.AddWithValue("@wcpts", pts.WeekCP);
                cmd.Parameters.AddWithValue("@userid", pts.UserId);
                cmd.ExecuteNonQuery();
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
        public APIGatewayProxyResponse DeleteCoupon(APIGatewayProxyRequest request)
        {
            string dealCode = request.PathParameters["dealCode"].ToString();
            LambdaLogger.Log("Deleting Coupon after using");
            using (connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"DELETE FROM `redeemoffers` WHERE `deal_code` = @dealCode";
                cmd.Parameters.AddWithValue("@dealCode", dealCode);
                cmd.ExecuteNonQuery();
                return new APIGatewayProxyResponse
                {
                    Body = "Coupon deleted",
                    Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            },
                    StatusCode = 200,
                };
            }
        }
    }

    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public User() { }

        public User(string userid, string username, string email, int roleid)
        {
            UserId = userid;
            UserName = username;
            Email = email;
            Role = roleid;
        }
    }
    public class RedeemOffers
    {
        public string Dealtype { get; set; }
        public int PointsRequired { get; set; }
        public string Dealcode { get; set; }
        public string Description { get; set; }

        public RedeemOffers() { }

        public RedeemOffers(string dealtype, int pointsrequired, string dealCode, string description)
        {
            Dealtype = dealtype;
            PointsRequired = pointsrequired;
            Dealcode = dealCode;
            Description = description;
        }
    }
    public class UserPoints
    {
        public int GreenPoints { get; set; }
        public int CarbonPoints { get; set; }
        public int WeekGP { get; set; }
        public int WeekCP { get; set; }
        public string UserId { get; set; }
        public UserPoints() { }
        public UserPoints(string userid, int greenpoints, int carbonpoints, int weekGP, int weekCP)
        {
            GreenPoints = greenpoints;
            CarbonPoints = carbonpoints;
            WeekGP = weekGP;
            WeekCP = weekCP;
            UserId = userid;
        }
    }
}