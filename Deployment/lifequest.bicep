param location string = 'northeurope'
param resourceGroupName string = 'lifequest-shared'
param deploymentTemplate string = 'lifequest-shared.bicep'
param deploymentParams string = 'lifequest-shared.bicepparam'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: location
}

targetScope = 'subscription'

module sharedDeployment 'lifequest-shared.bicep' = {
  name: 'DeployLifeQuestShared'
  scope: resourceGroup
  params: {
    // Add parameters required by the lifequest-shared.bicep template here
  }
}
