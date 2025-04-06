using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var sut = builder.AddProject<Projects.OA2MCP_TestMe>("oa2mcp-testme");

var inspector = builder
    .AddNpmApp("inspector-on-6274", "../MCPInspector", "OA2MCP_TestMe")
    .WaitFor(sut)
    .WithEnvironment("SERVER_PORT", "5246")
    //.WithHttpEndpoint(6274)
    .WithExternalHttpEndpoints()
    ;
   
builder.Build().Run();
