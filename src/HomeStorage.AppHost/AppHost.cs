var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL runs as a container both locally and when deployed to Azure
// Container Apps. This keeps cost at (or near) zero instead of provisioning a
// managed Azure Database for PostgreSQL. The data volume persists data across
// restarts.
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var database = postgres.AddDatabase("homestorage");

var api = builder.AddProject<Projects.HomeStorage_Api>("api")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.HomeStorage_Web>("web")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
