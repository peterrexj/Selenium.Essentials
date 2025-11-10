# Selenium.Essentials Build Scripts

This directory contains automated scripts for building, versioning, and publishing the Selenium.Essentials NuGet packages.

## Scripts Overview

### 1. `AutoVersionPackPublish.ps1` (Recommended)
**The comprehensive automated solution for version management, packing, and publishing.**

This script automatically:
- Reads the current version from project files
- Increments the patch version (e.g., 2.0.3 → 2.0.4)
- Updates all project files with the new version
- Packs all projects into NuGet packages
- Optionally publishes to NuGet.org

### 2. `PackProjects.ps1` (Legacy)
Simple script that packs all projects using the current version in project files.

### 3. `PublishProjects.ps1` (Legacy)
Simple script that publishes pre-built packages with hardcoded version numbers.

## Using AutoVersionPackPublish.ps1

### Basic Usage (Test Mode - Safe)
```powershell
# This will increment version, update projects, and pack - but NOT publish
.\AutoVersionPackPublish.ps1
```

### Pack Only (No Publishing)
```powershell
# Explicitly disable publishing
.\AutoVersionPackPublish.ps1 -Publish:$false
```

### Full Production Run (Pack + Publish)
```powershell
# WARNING: This will publish to NuGet.org!
.\AutoVersionPackPublish.ps1 -Test:$false -Publish:$true
```

## Safety Features

### Test Mode Protection
- **Default**: `Test = $true` - Prevents accidental publishing
- The script will pack packages but skip publishing
- Must explicitly set `-Test:$false` to enable publishing

### Publish Flag
- **Default**: `Publish = $false` - Publishing is opt-in
- Must explicitly set `-Publish:$true` to publish packages

### Both flags must be set for actual publishing:
```powershell
.\AutoVersionPackPublish.ps1 -Test:$false -Publish:$true
```

## Prerequisites

### For Packing
- .NET SDK installed
- All project files must have `<Version>` property

### For Publishing
- NuGet API key set as environment variable: `NugetApiKey`
- Set the environment variable:
  ```powershell
  [System.Environment]::SetEnvironmentVariable('NugetApiKey', 'your-api-key-here', 'User')
  ```

## What the Script Does

### Step 1: Version Reading
- Reads current version from `TestAny.Essentials.Core.csproj`
- Validates version format (Major.Minor.Patch)

### Step 2: Version Increment
- Increments the patch version by 1
- Example: `2.0.3` → `2.0.4`

### Step 3: Version Update
- Updates all 5 project files with the new version:
  - TestAny.Essentials.Core
  - TestAny.Essentials.Api
  - TestAny.Essentials.Api.Extensions
  - TestAny.Essentials.Api.Lite
  - Selenium.Essentials

### Step 4: Packing
- Creates NuGet packages for all projects
- Outputs to `../Output/` directory
- Uses Release configuration

### Step 5: Verification
- Verifies all expected packages were created
- Lists all generated packages

### Step 6: Publishing (Optional)
- Only runs if both `-Test:$false` and `-Publish:$true`
- Publishes all packages to NuGet.org
- Uses API key from environment variable

## Output

The script provides detailed colored output showing:
- Current and new version numbers
- Progress for each step
- Success/failure status
- Summary of operations performed

### Example Output
```
=== Selenium.Essentials Automated Version Management & Publishing ===
Test Mode: True
Publish Mode: False

Reading current version from: TestAny.Essentials.Core
Current version: 2.0.3
New version: 2.0.4

Updating all project versions...
All projects updated to version 2.0.4

Packing all projects...
All projects packed successfully

Verifying packages...
✓ Found: TestAny.Essentials.Core.2.0.4.nupkg
✓ Found: TestAny.Essentials.Api.2.0.4.nupkg
✓ Found: TestAny.Essentials.Api.Extensions.2.0.4.nupkg
✓ Found: TestAny.Essentials.Api.Lite.2.0.4.nupkg
✓ Found: Selenium.Essentials.2.0.4.nupkg

SKIPPING PUBLISH - Test mode is enabled

=== SUMMARY ===
Version updated: 2.0.3 → 2.0.4
Projects packed: 5
Packages created in: C:\path\to\Selenium.Essentials\Output
Packages published: NO

SUCCESS: All operations completed successfully!
```

## Error Handling

The script includes comprehensive error handling:
- Validates all project files exist
- Checks version format
- Verifies successful packing
- Confirms packages were created
- Validates NuGet API key for publishing

If any step fails, the script will:
- Display a clear error message
- Show the stack trace
- Exit with error code 1

## Best Practices

1. **Always test first**: Run without publish flags to verify everything works
2. **Check output**: Review the generated packages before publishing
3. **Version control**: Commit the version changes after successful publishing
4. **API key security**: Keep your NuGet API key secure and rotate regularly

## Troubleshooting

### "Project file not found"
- Ensure you're running from the `buildpack` directory
- Verify all project files exist in expected locations

### "Version not found in project file"
- Ensure all `.csproj` files have a `<Version>` property
- Check XML format is valid

### "NuGet API key not found"
- Set the environment variable: `$env:NugetApiKey = "your-key"`
- Or use: `[System.Environment]::SetEnvironmentVariable('NugetApiKey', 'your-key', 'User')`

### "Failed to pack project"
- Check for compilation errors
- Ensure all dependencies are restored
- Verify .NET SDK is installed
