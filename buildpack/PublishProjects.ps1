Set-ExecutionPolicy Bypass

$apiKey = [System.Environment]::GetEnvironmentVariable('NugetApiKey', 'User')
$packageVersion = '.1.0.5.16.nupkg'

$score = [IO.Path]::Combine($PSScriptRoot, '..\src\TestAny.Essentials.Core\bin\Debug\TestAny.Essentials.Core' + $packageVersion)
$sapi = [IO.Path]::Combine($PSScriptRoot, '..\src\TestAny.Essentials.Api\bin\Debug\TestAny.Essentials.Api' + $packageVersion)
$se = [IO.Path]::Combine($PSScriptRoot, '..\src\Selenium.Essentials\bin\Debug\Selenium.Essentials' + $packageVersion)

Get-ChildItem -Path $score -ErrorAction Stop
Get-ChildItem -Path $sapi -ErrorAction Stop
Get-ChildItem -Path $se -ErrorAction Stop

dotnet nuget push $score --api-key $apiKey --source https://api.nuget.org/v3/index.json
dotnet nuget push $sapi --api-key $apiKey --source https://api.nuget.org/v3/index.json
dotnet nuget push $se --api-key $apiKey --source https://api.nuget.org/v3/index.json