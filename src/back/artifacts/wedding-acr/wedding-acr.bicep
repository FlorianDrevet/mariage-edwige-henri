@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

resource wedding_acr 'Microsoft.ContainerRegistry/registries@2025-04-01' = {
  name: take('weddingacr${uniqueString(resourceGroup().id)}', 50)
  location: location
  sku: {
    name: 'Basic'
  }
  tags: {
    'aspire-resource-name': 'wedding-acr'
  }
}

output name string = wedding_acr.name

output loginServer string = wedding_acr.properties.loginServer