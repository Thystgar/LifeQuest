param environments array
param location string

resource resourceGroups 'Microsoft.Resources/resourceGroups@2022-09-01' = [for environment in environments: {
  name: 'lifequest-${environment}'
  location: location
}]

module identityModule 'modules/identity-module.bicep' = {
  name: 'identityModule'
  scope: resourceGroup('lifequest-identity')
  params: {
    location: location
    environments: environments
  }
}

module acrModule 'modules/acr-module.bicep' = {
  name: 'acrModule'
  scope: resourceGroup('lifequest-shared')
  params: {
    location: location
    identities: identityModule.outputs.principalIds
  }
}

targetScope = 'subscription'


