## General information
The Picc d.o.o. app was implemented in .NET (C#) using Entity Framework and MSSQL. For the purpose of the application, database with two tables was created. 
One of them is table Vehicle that is used to store information about vehicle such as type, color etc. Another one is VehicleShoppingArticle that stores information about price and contact phone number.
Tables are related through vehicle ID that represents ID in the Vehicle table (PK) and VehicleID in VehicleShoppingArticle (FK).
Database was used to create a model for the application (emdx). So, application has an entity that represents the database, and two classes that represent Vehicles and VehicleShoppingArticles.

## Application logic 
Logic of the application is based on the main menu that gets called constantly unless user decides to exit app. User has options to add, search, update or delete records from the database.
*	Adding records – Firstly, record in the Vehicle table is saved and, after that, a record in the VehicleShoppingArticle table is created with corresponding VehicleID. Here are the fields that have some constraints that need to be fulfilled:
1. Mileage should be a number greater than 0
2. Year should have four digits and be equal or less than the current year
3. Price should be a greater than 0
4. Contact phone number should be compatible with the following regex (standard US phone number format) - \(?\d{3}\)?-? *\d{3}-? *-?\d{4} 

*	Searching records – User has possibility to search using mileage, type and price filters

*	Updating records – User is prompted to enter a vehicle ID and after that type or description of the vehicle can be updated.

*	Deleting records - User is prompted to enter a vehicle ID to remove it from the database.


## Potential improvements 
For the possible improvements, a nice UI would make the app more convenient to use. 
Also, it may be good to add some additional messages for the user to serve as instructions to add and retrieve data from the database faster, for example, noting what kind of input every field in expecting before user enters a value.
One of the useful improvements could be adding back option for users to go back to previous steps. 
