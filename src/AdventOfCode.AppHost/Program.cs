var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AdventOfCode_ApiService>("apiservice")
    .WithHttpEndpoint(port: 1100, name: "http");

builder.Build().Run();
