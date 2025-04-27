param location string 
param environment string

targetScope = 'resourceGroup'


module sqlDatabaseModule 'modules/sqlDatabase-module.bicep' = {
  name: 'sqlDatabaseModule'
  scope: resourceGroup('life-quest-shared')
  params: {
    location: location
    environment: environment
  }
}

resource container_identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: 'lifequest-${environment}-api'
  location: location
}

module roleAssignmentModule 'modules/acrRoleAssignment-module.bicep' = {
  name: 'roleAssignmentModule'
  scope: resourceGroup('life-quest-shared')
  params: {
    roleId: '7f951dda-4ed3-4680-a7ca-43fe172d538d'
    principalId: container_identity.properties.principalId
  }
}

resource container 'Microsoft.ContainerInstance/containerGroups@2024-10-01-preview' = {
  name: 'lifequest-${environment}-container'
  location: location
  dependsOn: [
    roleAssignmentModule
  ]
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${container_identity.id}': {}
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
        identity: container_identity.id
      }
    ]
    osType: 'Linux'
    restartPolicy: 'Always'
  }
}
