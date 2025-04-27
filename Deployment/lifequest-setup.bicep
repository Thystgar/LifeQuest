param environments array
param location string

resource resourceGroups 'Microsoft.Resources/resourceGroups@2022-09-01' = [for environment in environments: {
  name: 'lifequest-${environment}'
  location: location
}]

resource identityResourceGroup 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'lifequest-identity'
  location: location
}

resource sharedResourceGroup 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'lifequest-shared'
  location: location
}

module identityModule 'modules/identity-module.bicep' = {
  name: 'identityModule'
  scope: identityResourceGroup
  params: {
    location: location
    environments: environments
  }
}

module acrModule 'modules/acr-module.bicep' = {
  name: 'acrModule'
  scope: sharedResourceGroup
  params: {
    location: location
    identities: identityModule.outputs.principalIds
  }
}

targetScope = 'subscription'


