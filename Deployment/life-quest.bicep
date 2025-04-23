param serverName string
param adminUser string = 'janamedice_seznam.cz#EXT#@janamediceseznam.onmicrosoft.com'
param databaseName string
param location string 

targetScope = 'resourceGroup'

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: serverName
  location: location
  properties: {
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      login: adminUser
      sid: '276b2d9d-0e24-4f07-89e3-67c304ffb608'
      tenantId: 'e9c8307d-b05f-4d2b-8132-5d4711a339fa'
      azureADOnlyAuthentication: true
    }
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  parent: sqlServer
  name: databaseName
  location: location
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
