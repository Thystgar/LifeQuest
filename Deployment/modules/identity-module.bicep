param name string

resource containerIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: name
  location: resourceGroup().location
}

output principalId string = containerIdentity.properties.principalId
