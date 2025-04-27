param roleId string 
param principalId string

targetScope = 'resourceGroup'

resource container_registry 'Microsoft.ContainerRegistry/registries@2024-11-01-preview' existing = {
  name: 'lifequest'
}

resource container_identity_access 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(container_registry.id, principalId, roleId)
  scope: container_registry
  properties: {
    principalId: principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleId)
    principalType: 'ServicePrincipal'
  }
}
