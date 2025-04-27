param location string
param environments array

resource containerIdentities 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = [for environment in environments: {
  name: 'lifequest-${environment}-api'
  location: location
}]

output principalIds array = [for (environment, i) in environments: containerIdentities[i].properties.principalId]
