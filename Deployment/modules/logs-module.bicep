param location string 
param identities array

resource logWorspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: 'lifequest-logs'
  location: location
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

resource containerAccess 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for identity in identities: {
  name: guid(logWorspace.id, identity, '3913510d-42f4-4e42-8a64-420c390055eb')
  scope: logWorspace
  properties: {
    principalId: identity
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '3913510d-42f4-4e42-8a64-420c390055eb')
    principalType: 'ServicePrincipal'
  }
}]
