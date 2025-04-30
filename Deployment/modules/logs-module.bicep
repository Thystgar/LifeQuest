param environments array

resource logWorspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: 'lifequest-logs'
  location: resourceGroup().location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    features: {
      legacy: 0
      searchVersion: 1
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

// grant Monitoring Metrics Publisher access to log workspace
resource containerIdentities 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' existing = [for environment in environments: {
  name: 'lifequest-${environment}-api'
  scope: resourceGroup('lifequest-${environment}')
}]

resource container_identity_access 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for (environment, i) in environments: {
  name: guid(logWorspace.id, containerIdentities[i].id, '7f951dda-4ed3-4680-a7ca-43fe172d538d')
  scope: logWorspace
  properties: {
    principalId: containerIdentities[i].properties.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')
    principalType: 'ServicePrincipal'
  }
}]
