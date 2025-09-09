# FiveMicroservicesSample

Five minimal-API microservices (.NET 9) with Docker, docker-compose, and Azure API Management (APIM) import scripts.

## Services
- WeatherService (port 5001)
- CountryService (port 5002)
- StateService (port 5003)
- DistrictService (port 5004)
- PostalService (port 5005)

## Quick Start (Docker Compose)
```bash
docker compose up --build
```
Then browse:
- http://localhost:5001/swagger
- http://localhost:5002/swagger
- http://localhost:5003/swagger
- http://localhost:5004/swagger
- http://localhost:5005/swagger
```

## Build locally
```bash
dotnet build **/*.csproj
```

## Azure API Management (APIM)
Use `apim/apim-setup.ps1` to create/import APIs from OpenAPI.
Prereqs: Azure CLI logged in (`az login`), an existing APIM instance.

Example:
```powershell
cd apim
# Set your variables
$resourceGroup="my-rg"
$apimName="my-apim"
$serviceSuffix="dev"
./apim-setup.ps1 -ResourceGroup $resourceGroup -ApimName $apimName -ServiceSuffix $serviceSuffix
```
This will create 5 APIs in APIM, each with a backend pointing to your local/dev URLs (adjust as needed).