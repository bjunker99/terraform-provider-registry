FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["TerraformProviderRegistry/TerraformProviderRegistry.csproj", "TerraformProviderRegistry/"]
RUN dotnet restore "TerraformProviderRegistry/TerraformProviderRegistry.csproj"
COPY . .
WORKDIR "/src/TerraformProviderRegistry"
RUN dotnet build "TerraformProviderRegistry.csproj" --no-restore -c Release -o /app/build --runtime linux-musl-x64 --self-contained

FROM build AS publish
RUN dotnet publish "TerraformProviderRegistry.csproj" --no-restore -c Release -o /app/publish --runtime linux-musl-x64 --self-contained

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TERRAFORM_PROVIDER_BUCKET_REGION="us-east-1"
ENV TERRAFORM_PROVIDER_BUCKET=""
ENTRYPOINT ["dotnet", "TerraformProviderRegistry.dll"]