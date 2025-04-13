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
- Install packages (navigate to App folder):
    ```sh
    npm install -g expo-cli
    npm install expo
	npm install expo-router
    ```
- Expo framework for React native: [Expo framework](https://docs.expo.dev/get-started/create-a-project/)
    ```sh
    npx create-expo-app@latest
    ```
- Expo app on Google play to test on mobile device

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

In order to be able to quesry the database when connecting from localhost you need to log in to your Visual Stidio with Microsoft account and enable the account on the database in [Azure Portal](https://portal.azure.com/#@janamediceseznam.onmicrosoft.com/resource/subscriptions/dc82971a-f484-4c22-bb31-c36b9805a147/resourceGroups/life-quest/providers/Microsoft.Sql/servers/life-quest-db-server/databases/life-quest-db/queryEditor) using command CREATE USER [user-email] FROM EXTERNAL PROVIDER;

### Package
Need to have docker running locally. The command will create an image without the need to specify docker file.
	```sh
	dotnet publish --os linux --arch x64 /t:PublishContainer
	```

### Push to container registry
The image can be uploaded directly to [container registry](https://portal.azure.com/#@janamediceseznam.onmicrosoft.com/resource/subscriptions/dc82971a-f484-4c22-bb31-c36b9805a147/resourceGroups/life-quest/providers/Microsoft.ContainerRegistry/registries/lifequest/overview)
	```sh
	dotnet publish --os linux --arch x64 /t:PublishContainer -p ContainerRegistry=lifequest.azurecr.io
	```
	
Uploading might need ```docker login```. It can also be uploaded from docker:
A new lifequest.azurecr.io Azure container registry have been created for storing the container. To push the container from docker to the registry:
	```sh
	docker login
	docker tag life-quest-api lifequest.azurecr.io/life-quest-api #tag the image for upload
	docker push lifequest.azurecr.io/lifequest-api #upload the image
	```

### Run locally

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