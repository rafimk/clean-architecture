IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgress = builder.AddPostgres("ca-db")
                                                        .WithDataVolume()
                                                        .WithPgAdmin();

IResourceBuilder<SeqResource> seq = builder.AddSeq("seq")
                                        .ExcludeFromManifest()
                                        .WithLifetime(ContainerLifetime.Persistent)
                                        .WithEnvironment("ACCEPT_EULA", "Y");

builder.AddProject<Projects.Web_Api>("web-api")
        .WithReference(postgress)
        .WaitFor(postgress)
        .WithReference(seq)
        .WaitFor(seq);


await builder.Build().RunAsync();
