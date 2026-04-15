@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param aca_wedding_env_outputs_azure_container_apps_environment_default_domain string

param aca_wedding_env_outputs_azure_container_apps_environment_id string

@secure()
param postgres_password_value string

param aca_wedding_env_outputs_volumes_postgres_0 string

resource postgres 'Microsoft.App/containerApps@2025-01-01' = {
  name: 'postgres'
  location: location
  properties: {
    configuration: {
      secrets: [
        {
          name: 'postgres-password'
          value: postgres_password_value
        }
      ]
      activeRevisionsMode: 'Single'
      ingress: {
        external: false
        targetPort: 5432
        transport: 'tcp'
      }
    }
    environmentId: aca_wedding_env_outputs_azure_container_apps_environment_id
    template: {
      containers: [
        {
          image: 'docker.io/library/postgres:17.6'
          name: 'postgres'
          env: [
            {
              name: 'POSTGRES_HOST_AUTH_METHOD'
              value: 'scram-sha-256'
            }
            {
              name: 'POSTGRES_INITDB_ARGS'
              value: '--auth-host=scram-sha-256 --auth-local=scram-sha-256'
            }
            {
              name: 'POSTGRES_USER'
              value: 'postgres'
            }
            {
              name: 'POSTGRES_PASSWORD'
              secretRef: 'postgres-password'
            }
          ]
          volumeMounts: [
            {
              volumeName: 'v0'
              mountPath: '/var/lib/postgresql/data'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
      }
      volumes: [
        {
          name: 'v0'
          storageType: 'AzureFile'
          storageName: aca_wedding_env_outputs_volumes_postgres_0
        }
      ]
    }
  }
}