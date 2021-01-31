greenvibe -Application - Backend

This is the back end API of a greenvibe Application, built throughout the Tech Returners Your Journey Into Tech course. It is consumed by a front end React application, available here and connects to an RDS Database.

The hosted version of the application is available here: https://github.com/Kaizen-ATG 
Technology Used
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

GET /userinfo
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/users/{userId}/user
Responds with JSON containing all Users in the Database.

GET /users
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/users

GET /redeempoints
GET - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/redeemoffers

POST /userinfo
 POST - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/saveuser
 
POST /userpoints
POST - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/saveuserpoints

Delete/coupon
DELETE - https://eumdh35gzh.execute-api.eu-west-2.amazonaws.com/deletecoupon




