FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY HygeaDirectory.sln ./
COPY src/DirectoryMS.Domain/DirectoryMS.Domain.csproj src/DirectoryMS.Domain/
COPY src/DirectoryMS.Application/DirectoryMS.Application.csproj src/DirectoryMS.Application/
COPY src/DirectoryMS.Infrastructure/DirectoryMS.Infrastructure.csproj src/DirectoryMS.Infrastructure/
COPY src/DirectoryMS/DirectoryMS.csproj src/DirectoryMS/

RUN dotnet restore src/DirectoryMS/DirectoryMS.csproj

COPY . .
RUN dotnet publish src/DirectoryMS/DirectoryMS.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "DirectoryMS.dll"]
