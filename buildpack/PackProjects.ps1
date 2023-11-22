
Set-ExecutionPolicy Bypass

$pjOutput = [IO.Path]::Combine($PSScriptRoot, '..\Output\')

$score = [IO.Path]::Combine($PSScriptRoot, '..\src\TestAny.Essentials.Core\TestAny.Essentials.Core.csproj')
$sapi = [IO.Path]::Combine($PSScriptRoot, '..\src\TestAny.Essentials.Api\TestAny.Essentials.Api.csproj')
$sapiLite = [IO.Path]::Combine($PSScriptRoot, '..\src\TestAny.Essentials.Api.Lite\TestAny.Essentials.Api.Lite.csproj')
$se = [IO.Path]::Combine($PSScriptRoot, '..\src\Selenium.Essentials\Selenium.Essentials.csproj')

dotnet pack $score --output $pjOutput
dotnet pack $sapi --output $pjOutput
dotnet pack $sapiLite --output $pjOutput
dotnet pack $se --output $pjOutput