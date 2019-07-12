FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Finalaplication/Finalaplication.csproj Finalaplication/
RUN dotnet restore Finalaplication/Finalaplication.csproj
COPY . .
WORKDIR /src/Finalaplication
RUN dotnet build Finalaplication.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Finalaplication.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Finalaplication.dll"]
