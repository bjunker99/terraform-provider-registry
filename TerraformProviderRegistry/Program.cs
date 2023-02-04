using TerraformProviderRegistry;
using TerraformProviderRegistry.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

IServiceConfiguration serviceConfig = new ServiceConfiguration();
builder.Services.AddSingleton(serviceConfig);

var container_value = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

if (string.IsNullOrEmpty(container_value))
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
}

var app = builder.Build();

if (!string.IsNullOrEmpty(container_value))
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.MapControllers();

app.Run();
