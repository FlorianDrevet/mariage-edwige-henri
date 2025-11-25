var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDbGate()
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var postgresdb = postgres.AddDatabase("postgresdb");

var api = builder.AddProject<Projects.Mariage_Api>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

var frontend = builder.AddJavaScriptApp("frontend", "./../../front")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
