param environment string

resource containerIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: 'lifequest-${environment}-api'
  location: resourceGroup().location
}

output containerIdentity object = containerIdentity
