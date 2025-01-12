
# Budget Expense System

## Description

Budget Expense System is an application helping users manage their finances, creating budgets, improving organization and reaching their financial goals. Built using .NET 8, SignalR, Hangfire and MySql, it provides a RESTful Web API for data management, real-time updates, and efficient background processing.

## Features

- Manage accounts, budgets, categories, and transactions
- Schedule one-time and recurring transactions
- Receive real-time notifications
- Ensure data integrity with unit of work pattern
- Daily database backup

## Technologies

- C#
- .NET Core
- Entity Framework 
- MySQL
- Hangfire
- SignalR

## Set up

#### Docker build:
Make sure that docker, docker-compose has been installed in your host.

- Navigate to the project root directory
- Run the following command to build and start the containers:
   ```sh
    docker-compose up --build
    ```
- After the containers are up, open your browser and go to `localhost:8080/swagger/index.html` to access the application.

#### Native build:
1. Clone the repository:
    ```sh
    git clone https://github.com/SofijaVvv/BudgetExpenseSystem.git
    ```
- Navigate to the project root directory

2. Set up the database:
- Create a MySql database named `budget_expense_system`
- Update the connection string in `appsettings.json`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=budget_expense_system;User=root;Password=yourpassword;"
  }
  ```
3. Apply migrations:
    ```sh
    dotnet ef database update
    ```

4. Seed the database:
    ```sh
    dotnet run --project BudgetExpenseSystem.Api --seed
    ```
5. Build the project:
    ```sh
    dotnet build
    ```

## Default Users

You can log in with the following test accounts:

| Role  | Email            | Password |
|-------|----------------|----------|
| Admin | admin@gmail.com | admin123 |
| User  | user@gmail.com  | user123  |
