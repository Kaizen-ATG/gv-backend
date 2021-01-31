![alt text](https://github.com/Kaizen-ATG/gv-common/blob/main/gv-logo.png "Green Vibe Logo")

# Greenvibe - Backend

This is the back end API for the Greenvibe Application, built throughout the Tech Returners Your Journey Into Tech course. It is consumed by a front-end React application, available here and connects to an RDS Database.

The hosted version of the application is available here: https://github.com/Kaizen-ATG 


## Technology Used
This project uses the following technology:
Serverless Framework
JavaScript 
MySQL
Mysql library
AWS Lambda and API Gateway
AWS RDS
ESLint
Endpoints
The API exposes the following endpoints:

GET/userinfo
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/users/{userId}/user
Responds with JSON containing all Users in the Database.

GET/users
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/users



GET/redeempoints
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/redeemoffers

POST/userinfo
POST - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/saveuser 

{
    "UserName": "testuser",
    "Email": "123new@gmail.com"
}
 
POST /userpoints
POST - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/saveuserpoints

{
    "green_points": 20,
    "carbon_points": 35,
    "weekGP": 15,
    "WeekCP": 25,
    "user_id": 11
}


Delete/coupon
DELETE - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/deletecoupon/{dealCode}

{
    "deal_code": "STAR12"
}





