
Set-ExecutionPolicy Bypass

$score = [IO.Path]::Combine($PSScriptRoot, '..\src\TestAny.Essentials.Core\TestAny.Essentials.Core.csproj')
$sapi = [IO.Path]::Combine($PSScriptRoot, '..\src\TestAny.Essentials.Api\TestAny.Essentials.Api.csproj')
$se = [IO.Path]::Combine($PSScriptRoot, '..\src\Selenium.Essentials\Selenium.Essentials.csproj')

dotnet pack $score
dotnet pack $sapi
dotnet pack $se