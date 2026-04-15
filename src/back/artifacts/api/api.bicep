@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param aca_wedding_env_outputs_azure_container_apps_environment_default_domain string

param aca_wedding_env_outputs_azure_container_apps_environment_id string

param api_containerimage string

param api_containerport string

@secure()
param postgres_password_value string

param aca_wedding_env_outputs_azure_container_registry_endpoint string

param aca_wedding_env_outputs_azure_container_registry_managed_identity_id string

resource api 'Microsoft.App/containerApps@2025-02-02-preview' = {
  name: 'api'
  location: location
  properties: {
    configuration: {
      secrets: [
        {
          name: 'connectionstrings--postgresdb'
          value: 'Host=postgres;Port=5432;Username=postgres;Password=${postgres_password_value};Database=postgresdb'
        }
        {
          name: 'postgresdb-password'
          value: postgres_password_value
        }
        {
          name: 'postgresdb-uri'
          value: 'postgresql://postgres:${uriComponent(postgres_password_value)}@postgres:5432/postgresdb'
        }
      ]
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: int(api_containerport)
        transport: 'http'
      }
      registries: [
        {
          server: aca_wedding_env_outputs_azure_container_registry_endpoint
          identity: aca_wedding_env_outputs_azure_container_registry_managed_identity_id
        }
      ]
      runtime: {
        dotnet: {
          autoConfigureDataProtection: true
        }
      }
    }
    environmentId: aca_wedding_env_outputs_azure_container_apps_environment_id
    template: {
      containers: [
        {
          image: api_containerimage
          name: 'api'
          env: [
            {
              name: 'OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EXCEPTION_LOG_ATTRIBUTES'
              value: 'true'
            }
            {
              name: 'OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EVENT_LOG_ATTRIBUTES'
              value: 'true'
            }
            {
              name: 'OTEL_DOTNET_EXPERIMENTAL_OTLP_RETRY'
              value: 'in_memory'
            }
            {
              name: 'ASPNETCORE_FORWARDEDHEADERS_ENABLED'
              value: 'true'
            }
            {
              name: 'HTTP_PORTS'
              value: api_containerport
            }
            {
              name: 'ConnectionStrings__postgresdb'
              secretRef: 'connectionstrings--postgresdb'
            }
            {
              name: 'POSTGRESDB_HOST'
              value: 'postgres'
            }
            {
              name: 'POSTGRESDB_PORT'
              value: '5432'
            }
            {
              name: 'POSTGRESDB_USERNAME'
              value: 'postgres'
            }
            {
              name: 'POSTGRESDB_PASSWORD'
              secretRef: 'postgresdb-password'
            }
            {
              name: 'POSTGRESDB_URI'
              secretRef: 'postgresdb-uri'
            }
            {
              name: 'POSTGRESDB_JDBCCONNECTIONSTRING'
              value: 'jdbc:postgresql://postgres:5432/postgresdb'
            }
            {
              name: 'POSTGRESDB_DATABASENAME'
              value: 'postgresdb'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
      }
    }
  }
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${aca_wedding_env_outputs_azure_container_registry_managed_identity_id}': { }
    }
  }
}