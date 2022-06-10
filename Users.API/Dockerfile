#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Users.API/Users.API.csproj", "Users.API/"]
COPY ["Users.DataAccess/Users.DataAccess.csproj", "Users.DataAccess/"]
COPY ["Users.Core/Users.Core.csproj", "Users.Core/"]
COPY ["Users.ApplicationServices.Shared/Users.ApplicationServices.Shared.csproj", "Users.ApplicationServices.Shared/"]
COPY ["Users.ApplicationServices/Users.ApplicationServices.csproj", "Users.ApplicationServices/"]
RUN dotnet restore "Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/Users.API"
RUN dotnet build "Users.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.API.dll"]