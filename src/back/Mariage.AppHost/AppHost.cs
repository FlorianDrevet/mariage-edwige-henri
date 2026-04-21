var builder = DistributedApplication.CreateBuilder(args);

var acr = builder.AddAzureContainerRegistry("wedding-acr");

var acaEnv = builder.AddAzureContainerAppEnvironment("aca-wedding-env")
    .WithAzureContainerRegistry(acr);

var postgres = builder.AddPostgres("postgres")
    .WithDbGate()
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var postgresdb = postgres.AddDatabase("postgresdb");

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(emulator => emulator
        .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent));

var blobs = storage.AddBlobs("blobs");

var api = builder.AddProject<Projects.Mariage_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgresdb)
    .WaitFor(postgresdb)
    .WithReference(blobs)
    .WaitFor(blobs)
    .WithEnvironment(ctx =>
    {
        ctx.EnvironmentVariables["BlobSettings__ConnectionString"] = blobs;
        ctx.EnvironmentVariables["BlobSettings__ConnectionStringPictures"] = blobs;
    });

var frontend = builder.AddJavaScriptApp("frontend", "./../../front", "dev")
    .WithNpm()
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(port: 4010, targetPort: 4010, env: "NG_PORT", isProxied: false);

builder.Build().Run();
