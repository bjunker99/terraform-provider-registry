using TerraformProviderRegistry;
using TerraformProviderRegistry.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

IServiceConfiguration serviceConfig = new ServiceConfiguration();
builder.Services.AddSingleton(serviceConfig);

var app = builder.Build();

string? aws_region = Environment.GetEnvironmentVariable("AWS_REGION");

if (aws_region != null)
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
    app.UseHttpsRedirection();
}

app.UseRouting();
app.MapControllers();

app.Run();