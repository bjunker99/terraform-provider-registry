var builder = WebApplication.CreateBuilder(args);

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/.well-known/terraform.json", async context =>
    {
        string serviceResponse = TerraformProviderRegistry.TerraformProviderService.serviceDiscovery();
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(serviceResponse);
    });

    endpoints.MapGet("/terraform/providers/v1/{namespace}/{name}/{version}/download/{os}/{arch}", async context =>
    {
        string? bucketName = System.Environment.GetEnvironmentVariable("TERRAFORM_PROVIDER_BUCKET");

        string? name_space = context.Request.RouteValues["namespace"] as string;
        string? name = context.Request.RouteValues["name"] as string;
        string? version = context.Request.RouteValues["version"] as string;
        string? os = context.Request.RouteValues["os"] as string;
        string? arch = context.Request.RouteValues["arch"] as string;

        var tps = new TerraformProviderRegistry.TerraformProviderService(bucketName);
        string? response = tps.providerPackage(name_space, name, version, os, arch);

        if (response == null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Provider not found.");
            return;
        }

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(response);
    });

    endpoints.MapGet("/terraform/providers/v1/{namespace}/{name}/versions", async context =>
    {

        string? bucketName = System.Environment.GetEnvironmentVariable("TERRAFORM_PROVIDER_BUCKET");

        string? name_space = context.Request.RouteValues["namespace"] as string;
        string? name = context.Request.RouteValues["name"] as string;

        var tps = new TerraformProviderRegistry.TerraformProviderService(bucketName);
        string? response = tps.versions(name_space, name);

        if (response == null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Provider not found.");
            return;
        }

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(response);
    });
});

app.Run();