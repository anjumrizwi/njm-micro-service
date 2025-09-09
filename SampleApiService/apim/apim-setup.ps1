param(
    [Parameter(Mandatory=$true)] [string] $ResourceGroup,
    [Parameter(Mandatory=$true)] [string] $ApimName,
    [Parameter(Mandatory=$false)] [string] $ServiceSuffix = "dev"
)

$services = @(
    @{ name="WeatherService";  url="http://localhost:5001"; display="Weather API" },
    @{ name="CountryService";  url="http://localhost:5002"; display="Countries API" },
    @{ name="StateService";    url="http://localhost:5003"; display="States API" },
    @{ name="DistrictService"; url="http://localhost:5004"; display="Districts API" },
    @{ name="PostalService";   url="http://localhost:5005"; display="Postal API" }
)

foreach ($s in $services) {
    $apiId = ($s.name + "-" + $ServiceSuffix).ToLower()
    Write-Host "Importing $($s.display) as $apiId ..."

    # Export the OpenAPI from running service or fall back to minimal import
    # For quick start we create a blank API and set backend service URL
    az apim api create `
        --resource-group $ResourceGroup `
        --service-name $ApimName `
        --api-id $apiId `
        --display-name $($s.display) `
        --path $($s.name.ToLower()) `
        --protocols https

    # set backend service url
    az apim api update `
        --resource-group $ResourceGroup `
        --service-name $ApimName `
        --api-id $apiId `
        --set serviceUrl=$($s.url)

    # Add a basic policy to set backend service URL and allow CORS (dev only)
    $policy = @"
<policies>
  <inbound>
    <base />
    <cors allow-credentials="true">
      <allowed-origins>
        <origin>*</origin>
      </allowed-origins>
      <allowed-methods>
        <method>*</method>
      </allowed-methods>
      <allowed-headers>
        <header>*</header>
      </allowed-headers>
      <expose-headers>
        <header>*</header>
      </expose-headers>
      <max-age>120</max-age>
    </cors>
  </inbound>
  <backend>
    <base />
  </backend>
  <outbound>
    <base />
  </outbound>
  <on-error>
    <base />
  </on-error>
</policies>
"@

    $policyFile = New-TemporaryFile
    Set-Content -Path $policyFile -Value $policy -Encoding UTF8
    az apim api policy update `
        --resource-group $ResourceGroup `
        --service-name $ApimName `
        --api-id $apiId `
        --policy-file $policyFile
    Remove-Item $policyFile
}
Write-Host "Done. Remember to run the services (docker compose up) or point serviceUrl to your deployed endpoints."