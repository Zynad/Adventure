FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY nuget.config .
COPY Adventure.slnx .
COPY Adventure.Domain/Adventure.Domain.csproj Adventure.Domain/
COPY Adventure.Application/Adventure.Application.csproj Adventure.Application/
COPY Adventure.Infrastructure/Adventure.Infrastructure.csproj Adventure.Infrastructure/
COPY Adventure.Api/Adventure.Api.csproj Adventure.Api/
RUN dotnet restore
COPY . .
RUN dotnet publish Adventure.Api/Adventure.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
RUN mkdir -p /app/data
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Adventure.Api.dll"]
