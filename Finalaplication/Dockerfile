FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Finalaplication.csproj", "Finalaplication/"]
RUN dotnet restore "Finalaplication/Finalaplication.csproj"

WORKDIR "/src/Finalaplication"
COPY . ./
COPY Controllers/* /app/Controllers/
COPY Resources/* /app/Resources/

COPY Properties appsettings.json Program.cs Startup.cs web.config /app/
RUN dotnet build "Finalaplication.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Finalaplication.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Finalaplication.dll"]