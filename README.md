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
Responds with JSON containing single user info in the Database.
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/users/{userId}/user


GET/users
Responds with JSON containing all Users in the Database.
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/users

GET/redeempoints
Responds with JSON containing info on redeem points 
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/redeemoffers

POST/userinfo
POST - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/saveuser 

{
    "userid": "4ec20f9f-4eec-43b8-88ce-2d7f9af7cd66",
    "username": "Osama",
    "email": "test3@gmail.com"
}
 
POST /userpoints
POST - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/saveuserpoints

{
    "userid": "4ec20f9f-4eec-43b8-77ce-2d7f9af7cd66",
    "greenpoints": 200,
    "carbonpoints": 200,
    "weekGP": 80,
    "weekCP": 100
}


Delete/coupon
Deletes the coupon from the database that is availed by the user
DELETE - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/deletecoupon/{dealCode}

