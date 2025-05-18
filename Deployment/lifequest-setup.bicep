param environments array
param location string

// create resource groups
resource resourceGroups 'Microsoft.Resources/resourceGroups@2022-09-01' = [for environment in environments: {
  name: 'lifequest-${environment}'
  location: location
}]

resource sharedResourceGroup 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'lifequest-shared'
  location: location
}

resource dbResourceGroups 'Microsoft.Resources/resourceGroups@2022-09-01' = [for environment in environments: {
  name: 'lifequest-${environment}-db'
  location: location
}]

module dbIdentity 'modules/identity-module.bicep' = {
  name: 'dbIdentity'
  scope: sharedResourceGroup
  params: {
    name: 'lifequest-db'
  }
}

// create all container identities in separate resource group so if the compute needs to be deleted they are preserved along with any access
module containerIdentity 'modules/identity-module.bicep' = [for (environment, i) in environments: {
  name: 'containerIdentity'
  scope: resourceGroups[i]
  params: {
    name: 'lifequest-${environment}-api'
  }
}]

// Create all resources relevant to logging and grant the container access
module logsModule 'modules/logs-module.bicep' = {
  name: 'logsModule'
  scope: sharedResourceGroup
  params: {
    environments: environments
  }
}

// create Azure container registry and grant the container identity access
module acrModule 'modules/acr-module.bicep' = {
  name: 'acrModule'
  scope: sharedResourceGroup
  params: {
    environments: environments
  }
}

targetScope = 'subscription'


