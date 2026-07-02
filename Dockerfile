FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PharmaFlow.sln ./
COPY PharmaFlow.Domain/PharmaFlow.Domain.csproj PharmaFlow.Domain/
COPY PharmaFlow.Application/PharmaFlow.Application.csproj PharmaFlow.Application/
COPY PharmaFlow.Infrastructure/PharmaFlow.Infrastructure.csproj PharmaFlow.Infrastructure/
COPY PharmaFlow.Persistence/PharmaFlow.Persistence.csproj PharmaFlow.Persistence/

RUN dotnet restore PharmaFlow.sln

COPY . .
RUN dotnet publish PharmaFlow.Persistence/PharmaFlow.Persistence.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PharmaFlow.Persistence.dll"]
