param(
    [switch]$Publish = $false,  # Set to $true to actually publish to NuGet
    [switch]$Test = $true       # Always true for testing, set to false for production runs
)

Set-ExecutionPolicy Bypass

# Configuration
$rootPath = [IO.Path]::GetFullPath([IO.Path]::Combine($PSScriptRoot, '..'))
$outputPath = [IO.Path]::Combine($rootPath, 'Output')
$srcPath = [IO.Path]::Combine($rootPath, 'src')

# Project definitions
$projects = @(
    @{ Name = "TestAny.Essentials.Core"; Path = [IO.Path]::Combine($srcPath, "TestAny.Essentials.Core", "TestAny.Essentials.Core.csproj") },
    @{ Name = "TestAny.Essentials.Api"; Path = [IO.Path]::Combine($srcPath, "TestAny.Essentials.Api", "TestAny.Essentials.Api.csproj") },
    @{ Name = "TestAny.Essentials.Api.Extensions"; Path = [IO.Path]::Combine($srcPath, "TestAny.Essentials.Api.Extensions", "TestAny.Essentials.Api.Extensions.csproj") },
    @{ Name = "TestAny.Essentials.Api.Lite"; Path = [IO.Path]::Combine($srcPath, "TestAny.Essentials.Api.Lite", "TestAny.Essentials.Api.Lite.csproj") },
    @{ Name = "Selenium.Essentials"; Path = [IO.Path]::Combine($srcPath, "Selenium.Essentials", "Selenium.Essentials.csproj") }
)

# Functions
function Get-ProjectVersion {
    param([string]$ProjectPath)
    
    if (-not (Test-Path $ProjectPath)) {
        throw "Project file not found: $ProjectPath"
    }
    
    [xml]$projectXml = Get-Content $ProjectPath
    $versionNode = $projectXml.Project.PropertyGroup.Version
    
    if (-not $versionNode) {
        throw "Version not found in project file: $ProjectPath"
    }
    
    return $versionNode
}

function Set-ProjectVersion {
    param([string]$ProjectPath, [string]$NewVersion)
    
    Write-Host "Updating version in: $ProjectPath to $NewVersion" -ForegroundColor Yellow
    
    [xml]$projectXml = Get-Content $ProjectPath
    $versionNode = $projectXml.Project.PropertyGroup.Version
    
    if (-not $versionNode) {
        throw "Version node not found in project file: $ProjectPath"
    }
    
    $versionNode = $NewVersion
    $projectXml.Project.PropertyGroup.Version = $NewVersion
    $projectXml.Save($ProjectPath)
}

function Increment-Version {
    param([string]$Version)
    
    $versionParts = $Version.Split('.')
    if ($versionParts.Length -ne 3) {
        throw "Version format should be Major.Minor.Patch (e.g., 2.0.3), got: $Version"
    }
    
    $major = [int]$versionParts[0]
    $minor = [int]$versionParts[1]
    $patch = [int]$versionParts[2]
    
    # Increment patch version
    $patch++
    
    return "$major.$minor.$patch"
}

function Ensure-OutputDirectory {
    if (-not (Test-Path $outputPath)) {
        Write-Host "Creating output directory: $outputPath" -ForegroundColor Green
        New-Item -ItemType Directory -Path $outputPath -Force | Out-Null
    }
}

# Main execution
try {
    Write-Host "=== Selenium.Essentials Automated Version Management & Publishing ===" -ForegroundColor Cyan
    Write-Host "Test Mode: $Test" -ForegroundColor $(if ($Test) { "Yellow" } else { "Green" })
    Write-Host "Publish Mode: $Publish" -ForegroundColor $(if ($Publish) { "Green" } else { "Yellow" })
    Write-Host ""
    
    # Step 1: Read current version from the first project (they should all be the same)
    $referenceProject = $projects[0]
    Write-Host "Reading current version from: $($referenceProject.Name)" -ForegroundColor Green
    $currentVersion = Get-ProjectVersion -ProjectPath $referenceProject.Path
    Write-Host "Current version: $currentVersion" -ForegroundColor White
    
    # Step 2: Increment version
    $newVersion = Increment-Version -Version $currentVersion
    Write-Host "New version: $newVersion" -ForegroundColor Green
    Write-Host ""
    
    # Step 3: Update all project versions
    Write-Host "Updating all project versions..." -ForegroundColor Green
    foreach ($project in $projects) {
        Set-ProjectVersion -ProjectPath $project.Path -NewVersion $newVersion
    }
    Write-Host "All projects updated to version $newVersion" -ForegroundColor Green
    Write-Host ""
    
    # Step 4: Ensure output directory exists
    Ensure-OutputDirectory
    
    # Step 5: Pack all projects
    Write-Host "Packing all projects..." -ForegroundColor Green
    foreach ($project in $projects) {
        Write-Host "Packing: $($project.Name)" -ForegroundColor Yellow
        $packResult = dotnet pack $project.Path --output $outputPath --configuration Release
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to pack project: $($project.Name)"
        }
    }
    Write-Host "All projects packed successfully" -ForegroundColor Green
    Write-Host ""
    
    # Step 6: Verify packages exist
    Write-Host "Verifying packages..." -ForegroundColor Green
    $packagePaths = @()
    foreach ($project in $projects) {
        $packagePath = [IO.Path]::Combine($outputPath, "$($project.Name).$newVersion.nupkg")
        if (-not (Test-Path $packagePath)) {
            throw "Package not found: $packagePath"
        }
        $packagePaths += $packagePath
        Write-Host "✓ Found: $($project.Name).$newVersion.nupkg" -ForegroundColor Green
    }
    Write-Host ""
    
    # Step 7: Publish (if enabled and not in test mode)
    if ($Publish -and -not $Test) {
        Write-Host "Publishing packages to NuGet..." -ForegroundColor Green
        
        $apiKey = [System.Environment]::GetEnvironmentVariable('NugetApiKey', 'User')
        if (-not $apiKey) {
            throw "NuGet API key not found. Please set the 'NugetApiKey' environment variable."
        }
        
        foreach ($packagePath in $packagePaths) {
            $packageName = [IO.Path]::GetFileName($packagePath)
            Write-Host "Publishing: $packageName" -ForegroundColor Yellow
            
            $publishResult = dotnet nuget push $packagePath --api-key $apiKey --source https://api.nuget.org/v3/index.json
            if ($LASTEXITCODE -ne 0) {
                throw "Failed to publish package: $packageName"
            }
            Write-Host "✓ Published: $packageName" -ForegroundColor Green
        }
        
        Write-Host "All packages published successfully!" -ForegroundColor Green
    }
    elseif ($Test) {
        Write-Host "SKIPPING PUBLISH - Test mode is enabled" -ForegroundColor Yellow
        Write-Host "To publish, run with: -Test:`$false -Publish:`$true" -ForegroundColor Yellow
    }
    elseif (-not $Publish) {
        Write-Host "SKIPPING PUBLISH - Publish flag not set" -ForegroundColor Yellow
        Write-Host "To publish, run with: -Publish:`$true -Test:`$false" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "=== SUMMARY ===" -ForegroundColor Cyan
    Write-Host "Version updated: $currentVersion → $newVersion" -ForegroundColor White
    Write-Host "Projects packed: $($projects.Count)" -ForegroundColor White
    Write-Host "Packages created in: $outputPath" -ForegroundColor White
    if ($Publish -and -not $Test) {
        Write-Host "Packages published: YES" -ForegroundColor Green
    } else {
        Write-Host "Packages published: NO" -ForegroundColor Yellow
    }
    Write-Host ""
    Write-Host "SUCCESS: All operations completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host ""
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Stack Trace:" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    exit 1
}
