# EmployeeDB SQL Project

This project demonstrates a simple **Employee Management Database** using SQL Server.  
It includes creating a database, tables, inserting data, and querying with joins.

After creating the database run the scaffold command to implement database first approach:

**dotnet ef dbcontext scaffold "YOUR_DATABASE_CONNECTION_STRING_HERE" Microsoft.EntityFrameworkCore.SqlServer -o Models**

---

## SQL Script

You can run the following SQL script to create and populate the database:

```sql
-- Create Database
CREATE DATABASE EmployeeDB;
USE EmployeeDB;

-- Create Gender Table
CREATE TABLE Gender(
    GenderID INT PRIMARY KEY IDENTITY(1,1),
    GenderName NVARCHAR(50) NOT NULL
);

-- View Gender Table
SELECT * FROM Gender;

-- Insert Sample Data into Gender Table
INSERT INTO Gender(GenderName) VALUES ('Male'),('Female'),('Others');

-- Create Employee Table
CREATE TABLE Employee(
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeName NVARCHAR(125) NOT NULL,
    GenderID INT NOT NULL,
    FOREIGN KEY (GenderID) REFERENCES Gender(GenderID)
);

-- View Tables
SELECT * FROM Employee;
SELECT * FROM Gender;

-- Join Employee with Gender
SELECT e.EmployeeName, g.GenderName 
FROM Employee e
INNER JOIN Gender g
ON e.GenderID = g.GenderID;
