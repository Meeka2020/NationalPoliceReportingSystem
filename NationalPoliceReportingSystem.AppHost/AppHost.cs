var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NPRSApp_Maui>("nprsapp-maui");

builder.Build().Run();
