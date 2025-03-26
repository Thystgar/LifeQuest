# LifeQuest Project

## Overview
TBD

## Getting Started

### Prerequisites
Before you begin, ensure you have met the following requirements:
- Docker Desktop: [Install Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/) (Select use WSL instead of Hyper-V)
- Visual Studio: [Download Visual Studio](https://visualstudio.microsoft.com/)
- VS Code
- Node.js
- Expo framework for React native: [Expo framework](https://docs.expo.dev/get-started/create-a-project/)
    ```sh
    npx create-expo-app@latest
    ```
- Expo app on Google play

### Clone the Repository
1. Copy your SSH key to `C:\Users\<user>\.ssh`
2. Clone the repository using the SSH key

## App

### Run locally
Go to folder App and run
	```sh
	npm run android
	```

## Server

### Setup DB migrations
[Install EF tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
[Create and apply migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
	```sh
    dotnet tool install --global dotnet-ef
    cd C:/LifeQuest/Server
    dotnet add package Microsoft.EntityFrameworkCore.Design
	```

Create migration
	```sh
    dotnet ef migrations add InitialCreate
	```

Apply migration
	```sh
    dotnet ef database update
	```

### Connect to db
#### Connect to local db 
Data Source=127.0.0.1;Initial Catalog=life-quest-db;User ID=sa;Password=P@ssw0rd!;Encrypt=False;Trust Server Certificate=True
#### Connect to azure db 
Add your IP to have access to [Azure SQL Server](https://portal.azure.com/#@janamediceseznam.onmicrosoft.com/resource/subscriptions/dc82971a-f484-4c22-bb31-c36b9805a147/resourceGroups/life-quest/providers/Microsoft.Sql/servers/life-quest-db-server/networking). In the networking panel under firewall rules add your client IPv4 address and save.

### Run locally

## CI/CD

## Azure