param environments array

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2024-11-01-preview' = {
  name: 'lifequest'
  location: resourceGroup().location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

resource containerIdentities 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' existing = [for environment in environments: {
  name: 'lifequest-${environment}-api'
  scope: resourceGroup('lifequest-${environment}')
}]

resource container_identity_access 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for (environment, i) in environments: {
  name: guid(containerRegistry.id, containerIdentities[i].id, '7f951dda-4ed3-4680-a7ca-43fe172d538d')
  scope: containerRegistry
  properties: {
    principalId: containerIdentities[i].properties.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')
    principalType: 'ServicePrincipal'
  }
}]
