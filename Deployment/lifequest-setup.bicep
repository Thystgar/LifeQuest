param location string
param environments array

targetScope = 'subscription'

resource sharedResourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'lifequest-shared'
  location: location
}

resource envResourceGroups 'Microsoft.Resources/resourceGroups@2021-04-01' = [for environment in environments: {
  name: 'lifequest-${environment}'
  location: location
}]



