# ReadingIsGood Project

## Technologies Used
- **Backend:** .NET 7, C#
- **Database:** PostgreSQL
- **Containerization:** Docker
- **Authentication:** JWT

## Getting Started

### Prerequisites
Before you begin, ensure you have the following installed:
- [.NET 7 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

### Setup

1. **Clone the Repository**

>      git clone https://github.com/devonuryilmaz/ReadingIsGood.git

>      cd ReadingIsGood

2.  **Configure Docker**  
    To run the entire application using Docker, ensure you have Docker installed and then follow these steps:
    
    -   Navigate to the project root where your `docker-compose.yml` file is located.
    -   Run the following command to start up the services:
    
>   `docker-compose up -d` 


### Docker Services

-   **API**: Available at [http://localhost:8081](http://localhost:8081)
-   **PostgreSQL**: Database service, available at `localhost:5432`

### Database Connection String

The default connection string for PostgreSQL is configured in `appsettings.json` as follows:

>    `"DefaultConnection": "Host=postgres;Database=readingisgood;Username=postgres;Password=postgres"`
