FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5206

ENV ASPNETCORE_URLS=http://+:5206

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["ExpensesTracker/ExpensesTracker.csproj", "ExpensesTracker/"]
RUN dotnet restore "ExpensesTracker/ExpensesTracker.csproj"
COPY . .
WORKDIR "/src/ExpensesTracker"
RUN dotnet build "ExpensesTracker.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ExpensesTracker.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesTracker.dll"]
