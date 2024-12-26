# Restaurant Table Booking App API

## Overview
The **Restaurant Table Booking App API** is a robust backend solution for managing restaurant reservations and table bookings. This project offers comprehensive functionality for restaurants to streamline their booking processes, optimize table usage, and enhance customer experience.

## Features
- **User Management:** Manage users with secure authentication and authorization.
- **Table Management:** Efficiently handle table availability, reservations, and status.
- **Reservation System:** Allow customers to book, modify, and cancel reservations.
- **Admin Dashboard:** Provide administrators with tools to oversee bookings and manage tables.
- **Real-time Updates:** Reflect live booking status and availability.
- **API Documentation:** Detailed Swagger documentation for all API endpoints.

## Technologies Used
- **Backend Framework:** ASP.NET Core
- **Database:** SQL Server
- **Authentication:** Azure AD B2C for secure login and identity management
- **Documentation:** Swagger/OpenAPI
- **Deployment:** Azure App Service

## Installation
### Prerequisites
- .NET SDK (version X.X or higher)
- SQL Server
- Azure AD B2C setup (for authentication)
- Azure App Service subscription

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/HuySG/HSG.RestaurantTableBookingApp.API.git
   cd HSG.RestaurantTableBookingApp.API
   ```

2. Set up the database:
   - Create a new SQL Server database.
   - Update the `appsettings.json` file with your database connection string.

3. Configure Azure AD B2C:
   - Create an Azure AD B2C tenant and application.
   - Update the `appsettings.json` file with Azure AD B2C settings (e.g., tenant ID, client ID, policy names).

4. Restore dependencies and build the project:
   ```bash
   dotnet restore
   dotnet build
   ```

5. Apply migrations and seed data (if applicable):
   ```bash
   dotnet ef database update
   ```

6. Deploy to Azure App Service:
   - Create an App Service in your Azure portal.
   - Publish the application to Azure using Visual Studio or the Azure CLI:
     ```bash
     az webapp up --name <YourAppServiceName> --resource-group <YourResourceGroup> --plan <YourAppServicePlan>
     ```
   - Update the App Service settings with your `appsettings.json` configurations (e.g., connection strings, Azure AD B2C settings).

7. Access the Swagger documentation at `https://hsg-table-booking-app-api-ebdsbbbsgrfpckbm.southeastasia-01.azurewebsites.net/swagger`.

## Usage
### Endpoints Overview
| Method | Endpoint                     | Description                          |
|--------|------------------------------|--------------------------------------|
| POST   | /api/users/register          | Register a new user                  |
| POST   | /api/users/login             | Authenticate user via Azure AD B2C   |
| GET    | /api/tables                  | Retrieve all tables                  |
| POST   | /api/reservations            | Create a new reservation             |
| GET    | /api/reservations            | List all reservations                |
| PUT    | /api/reservations/{id}       | Modify an existing reservation       |
| DELETE | /api/reservations/{id}       | Cancel a reservation                 |

## Contributing
Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/YourFeatureName`).
3. Commit your changes (`git commit -m 'Add your message here'`).
4. Push to the branch (`git push origin feature/YourFeatureName`).
5. Create a Pull Request.

## License
This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

## Acknowledgments
- Thanks to the contributors and supporters of this project.
- Special recognition for open-source tools and libraries used in the development of this API.
- Integration with Azure AD B2C for secure authentication.

---
For more information or inquiries, please contact [HuySG](https://github.com/HuySG).

