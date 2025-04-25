# LifeQuest Project

## Overview
TBD

## Getting Started

### Prerequisites
Before you begin, ensure you have met the following requirements:
- Docker Desktop: [Install Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/) (Select use WSL instead of Hyper-V)
- Visual Studio: [Download Visual Studio](https://visualstudio.microsoft.com/)
- VS Code Insiders
- [React native](https://reactnative.dev/docs/set-up-your-environment)

### Clone the Repository
1. Copy your SSH key to `C:\Users\<user>\.ssh`
2. Clone the repository using the SSH key

## App

### Create app
Create initial app using ```npx @react-native-community/cli init lifequest```

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
    cd ~/Server/LifeQuest.Api
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

In order to be able to quesry the database when connecting from localhost you need to log in to your Visual Stidio with Microsoft account and enable the account on the database in [Azure Portal](https://portal.azure.com/#@janamediceseznam.onmicrosoft.com/resource/subscriptions/dc82971a-f484-4c22-bb31-c36b9805a147/resourceGroups/life-quest/providers/Microsoft.Sql/servers/life-quest-db-server/databases/life-quest-db/queryEditor) using command CREATE USER [user-email] FROM EXTERNAL PROVIDER;

### Package
Need to have docker running locally. The command will create an image without the need to specify docker file.
	```sh
	dotnet publish --os linux --arch x64 /t:PublishContainer
	```

### Push to container registry
The image can be uploaded directly to container registry lifequest
	```sh
	dotnet publish --os linux --arch x64 /t:PublishContainer -p ContainerRegistry=lifequest.azurecr.io
	```
	
you will first need to login ```az acr login --name lifequest```.

### Azure Container
We will deploy the container as Azure container instance. We need to create Azure container instance
- make sure the port on the container is correctly setup
- configure the log analysics workspace for logs
- assign user managed identity to container

To grant container access to the SQL db, create the user for the container MI in the db and grant the MI access to db 
	```sh
	CREATE USER [life-quest-api] FROM EXTERNAL PROVIDER;
	ALTER ROLE db_datareader ADD MEMBER [life-quest-api];
	ALTER ROLE db_datawriter ADD MEMBER [life-quest-api];
	```

## CI/CD

## Azure

## FAQ
1. Error when starting docker Wsl/Service/RegisterDistro/CreateVm/HCS/HCS_E_SERVICE_NOT_AVAILABLE

	```sh
	wsl --set-default-version 2
	sudo dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
	sudo dism.exe /online /enable-feature /featurename:Microsoft-Hyper-V-All /all /norestart
	```
 	restart machine