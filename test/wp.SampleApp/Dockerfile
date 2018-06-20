FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY test/wp.SampleApp/wp.SampleApp.csproj test/wp.SampleApp/
RUN dotnet restore test/wp.SampleApp/wp.SampleApp.csproj
COPY . .
WORKDIR /src/test/wp.SampleApp
RUN dotnet build wp.SampleApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish wp.SampleApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "wp.SampleApp.dll"]
