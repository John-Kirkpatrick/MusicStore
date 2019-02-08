FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["src/Services/Artists/Artists.API/Artists.API.csproj", "src/Services/Artists/Artists.API/"]
RUN dotnet restore "src/Services/Artists/Artists.API/Artists.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Artists/Artists.API"
RUN dotnet build "Artists.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Artists.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Artists.API.dll"]