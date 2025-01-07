# Use the official .NET image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BudgetExpenseSystem.Api/BudgetExpenseSystem.Api.csproj", "BudgetExpenseSystem.Api/"]
RUN dotnet restore "BudgetExpenseSystem.Api/BudgetExpenseSystem.Api.csproj"
COPY . .
WORKDIR "/src/BudgetExpenseSystem.Api"
RUN dotnet build "BudgetExpenseSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetExpenseSystem.Api.csproj" -c Release -o /app/publish

# Use the base image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetExpenseSystem.Api.dll"]
