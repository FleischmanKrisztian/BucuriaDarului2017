FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Finalaplication.csproj", "Finalaplication/"]
RUN dotnet restore "Finalaplication/Finalaplication.csproj"

WORKDIR "/src/Finalaplication"
COPY . ./
RUN dotnet build "Finalaplication.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Finalaplication.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Finalaplication.dll"]