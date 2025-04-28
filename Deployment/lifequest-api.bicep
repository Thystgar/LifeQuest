param location string 
param environment string

targetScope = 'resourceGroup'

resource containerIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' existing =  {
  name: 'lifequest-${environment}-api'
  scope: resourceGroup('lifequest-identity')
}

resource container 'Microsoft.ContainerInstance/containerGroups@2024-10-01-preview' = {
  name: 'lifequest-${environment}-container'
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${containerIdentity.id}': {}
    }
  }
  properties: {
    containers: [
      {
        name: 'lifequest-api-container'
        properties: {
          image: 'lifequest.azurecr.io/lifequest-api:latest'
          ports: [
            {
              port: 8080
            }
          ]
          resources: {
            requests: {
              cpu: 1
              memoryInGB: 1
            }
          }
          environmentVariables: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: environment
            }
          ]
        }
      }
    ]
    imageRegistryCredentials: [
      {
        server: 'lifequest.azurecr.io'
        identity: containerIdentity.id
      }
    ]
    osType: 'Linux'
    restartPolicy: 'Always'
  }
}
