#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["UMS.csproj", "UMS/"]
RUN dotnet restore "UMS.csproj"
COPY . .
WORKDIR "/src/UMS"
RUN dotnet build "UMS/UMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UMS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "UMS.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet UMS.dll
