targetScope = 'subscription'

param resourceGroupName string

param location string

param principalId string

resource rg 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
}

module wedding_acr 'wedding-acr/wedding-acr.bicep' = {
  name: 'wedding-acr'
  scope: rg
  params: {
    location: location
  }
}

module aca_wedding_env 'aca-wedding-env/aca-wedding-env.bicep' = {
  name: 'aca-wedding-env'
  scope: rg
  params: {
    location: location
    wedding_acr_outputs_name: wedding_acr.outputs.name
    userPrincipalId: principalId
  }
}

output wedding_acr_name string = wedding_acr.outputs.name

output wedding_acr_loginServer string = wedding_acr.outputs.loginServer

output aca_wedding_env_AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID string = aca_wedding_env.outputs.AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID

output aca_wedding_env_AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = aca_wedding_env.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN

output aca_wedding_env_AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = aca_wedding_env.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_ID

output aca_wedding_env_volumes_postgres_0 string = aca_wedding_env.outputs.volumes_postgres_0

output aca_wedding_env_AZURE_CONTAINER_REGISTRY_ENDPOINT string = aca_wedding_env.outputs.AZURE_CONTAINER_REGISTRY_ENDPOINT