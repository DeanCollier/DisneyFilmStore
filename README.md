# Disney Film Store
### Dean Collier, Mariah Page, Tamara Erby - Blue Badge 2021
 
## About
This is a collaborative project that uses ASP.NET Web API to create an N-tier architecture Disney Film Store application on a local database.  C# was the main language used to create the models, logic, and API along with some SQL for database querying. 
 
## Technologies
1. Entity Framework
2. .NET Framework with Owin for ASP.NET
 
## Table of Contents
1. General Project Layout
2. Database Entity Detail
3. Git Pull Documentation
4. Postman User Documentation
 
### 1. General Project Layout
1. Data - houses object classes and database requirements
2. Models - sets data models to exchange data with the user
3. Services - performs logic on database objects
4. WebAPI - uses HTTP methods to Post, Get, Put, and Delete information
 
### 2. Database Entity Detail
- Film
  - Description: Disney films available for users to purchase.
  - FilmId (Primary Key): Uniquely identifies each Film with an integer
  - Title: Title of the Disney film
  - Genre: Genre of the film
  - YearReleased: release year of the film
  - MemberCost: cost of the film if the user is a member of the store service
  - NonMemberCost: regular cost of film
 
- Customer
  - Description: Contains details on specific customers
  - Id (Primary Key): Uniquely identifies each Customer with an integer
  - UserId: A Guid that identifies the user based on a token
  - FirstName & LastName: Name of customer
  - Email
  - Address
  - Member: boolean that specifies if the customer is a member of the store service
 
- Order
  - Description: Contains information about individual orders from the store
  - OrderId (Primary Key): Uniquely identifies each Order with an integer
  - OrderDate (can change if order is edited)
  - TotalOrderCost: Sums the cost of all the films in the order
  - CustomerId (Foreign Key): Identifies the Customer who submitted the order
  - Customer: Virtual entity of the Customer
 
- FilmOrder
  - Description: This entity acts as a joining table between Films and Orders without storing virtual versions within one another
  - Id (Primary Key): Uniquely identifies each FilmOrder with an integer
  - OrderId (Foreign Key): Identifies the Order for each FilmOrder
  - Order: Virtual entity of the Order
  - FilmId (Foreign Key): Identifies the Film for each FilmOrder
  - Film: Virtual entity of the Film
 
- ShippingInformation
  - Description: Contains the shipping information for each order based on the customer of the order
  - Id (Primary Key): Uniquely identifies each ShippingInformation with an integer
  - UserId: Guid that prevents shipping information from being viewed by users that did not create the order
  - OrderId (Foreign Key): Identifies the Order for each shipment
  - Order: Virtual entity of the Order
  - CustomerId: The customer Id for the Customer that submitting the Order
 
  ### 3. Git Pull Documentation
  Clone the repository to your local machine with this git command:
  ```
  git clone https://github.com/mariahlynnpage/DisneyFilmStoreFinal.git
  ```
  You did it!
 
  ### 4. Postman User Documentation
  Postman has a great resource page here if you want to use Postman to play with the application: 
   - [Postman Resources](https://learning.postman.com/docs/getting-started/introduction/)
  
