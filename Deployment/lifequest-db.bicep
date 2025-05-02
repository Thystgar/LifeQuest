param environment string

targetScope = 'resourceGroup'

resource dbIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: 'lifequest-${environment}-db'
  location: resourceGroup().location
}

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: 'lifequest-${environment}-db-server'
  location: resourceGroup().location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${dbIdentity.id}': {}
    }
  }
  properties: {
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    primaryUserAssignedIdentityId: dbIdentity.id
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'Group'
      login: 'GitHub'
      sid: '12ac0b55-3a84-4933-b5f0-9e7bbdce84b8'
      tenantId: 'e9c8307d-b05f-4d2b-8132-5d4711a339fa'
      azureADOnlyAuthentication: true
    }
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  parent: sqlServer
  name: 'lifequest-${environment}-db'
  location: resourceGroup().location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
  properties: {
    zoneRedundant: false
  }
}

resource servers_life_quest_db_server_name_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2024-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource servers_life_quest_db_server_name_ClientIPAddress_2025_3_26_21_19_23 'Microsoft.Sql/servers/firewallRules@2024-05-01-preview' = {
  parent: sqlServer
  name: 'LocalDebugging'
  properties: {
    startIpAddress: '86.45.200.157'
    endIpAddress: '86.45.200.157'
  }
}
