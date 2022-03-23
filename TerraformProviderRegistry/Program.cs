using TerraformProviderRegistry;
using TerraformProviderRegistry.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

IServiceConfiguration serviceConfig = new ServiceConfiguration();
builder.Services.AddSingleton(serviceConfig);

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();