# Product Management System

A modern ASP.NET Core application for managing product inventory with a clean, responsive UI and optimized performance.



## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository
   ```
   git clone https://github.com/yourusername/product-management.git
   ```

2. Navigate to the project directory
   ```
   cd product-management
   ```

3. Restore dependencies
   ```
   dotnet restore
   ```

4. Update the database
   ```
   dotnet ef database update
   ```

5. Run the application
   ```
   dotnet run
   ```

6. Open your browser and navigate to `https://localhost:5001`

### Default Login

- Email: admin@example.com
- Password: Admin123!

## Using the Product Grid

The product grid provides a comprehensive view of all products with the following features:

### Grid Features

- **Sorting**: Click on column headers to sort
- **Filtering**: Use the filter row to filter by any column
- **Searching**: Use the search box to search across all columns
- **Inline Editing**: Click the edit button to edit a row directly
- **Deletion**: Remove products with the delete button

### Pagination

The pagination component is separated from the grid for better usability:

- **Page Size**: Select how many items to display per page
- **Navigation**: Use the buttons to navigate between pages
- **Page Info**: View current page information

## Architecture

This application follows a clean architecture pattern:

- **Controllers**: Handle HTTP requests and responses
- **Services**: Contain business logic
- **Data Access**: Entity Framework Core for database operations
- **Views**: Razor views for UI rendering

## Performance Optimizations

- **Caching**: Memory caching for frequently accessed data
- **Asynchronous Operations**: All database operations are async
- **Minimal Data Transfer**: Only necessary data is sent to the client

## License

This project is licensed under the MIT License - see the LICENSE file for details.