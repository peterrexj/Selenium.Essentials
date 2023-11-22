Set-ExecutionPolicy Bypass

$apiKey = [System.Environment]::GetEnvironmentVariable('NugetApiKey', 'User')
$packageVersion = '.1.0.5.27.nupkg'

$score = [IO.Path]::Combine($PSScriptRoot, '..\Output\TestAny.Essentials.Core' + $packageVersion)
$sapi = [IO.Path]::Combine($PSScriptRoot, '..\Output\TestAny.Essentials.Api' + $packageVersion)
$sapiLite = [IO.Path]::Combine($PSScriptRoot, '..\Output\TestAny.Essentials.Api.Lite' + $packageVersion)
$se = [IO.Path]::Combine($PSScriptRoot, '..\Output\Selenium.Essentials' + $packageVersion)

Get-ChildItem -Path $score -ErrorAction Stop
Get-ChildItem -Path $sapi -ErrorAction Stop
Get-ChildItem -Path $sapiLite -ErrorAction Stop
Get-ChildItem -Path $se -ErrorAction Stop

dotnet nuget push $score --api-key $apiKey --source https://api.nuget.org/v3/index.json
dotnet nuget push $sapi --api-key $apiKey --source https://api.nuget.org/v3/index.json
dotnet nuget push $sapiLite --api-key $apiKey --source https://api.nuget.org/v3/index.json
dotnet nuget push $se --api-key $apiKey --source https://api.nuget.org/v3/index.json