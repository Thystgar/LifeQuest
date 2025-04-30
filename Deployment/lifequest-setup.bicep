param environments array
param location string

// create resource groups
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

resource dbResourceGroups 'Microsoft.Resources/resourceGroups@2022-09-01' = [for environment in environments: {
  name: 'lifequest-${environment}-db'
  location: location
}]

// create all container identities in separate resource group so if the compute needs to be deleted they are preserved along with any access
module identityModule 'modules/identity-module.bicep' = {
  name: 'identityModule'
  scope: identityResourceGroup
  params: {
    location: location
    environments: environments
  }
}

// Create all resources relevant to logging and grant the container access
module logsModule 'modules/logs-module.bicep' = {
  name: 'logsModule'
  scope: sharedResourceGroup
  params: {
    location: location
    identities: identityModule.outputs.principalIds
  }
}

// create Azure container registry and grant the container identity access
module acrModule 'modules/acr-module.bicep' = {
  name: 'acrModule'
  scope: sharedResourceGroup
  params: {
    location: location
    identities: identityModule.outputs.principalIds
  }
}

targetScope = 'subscription'


