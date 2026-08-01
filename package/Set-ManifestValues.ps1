<#
.SYNOPSIS
    Replaces __PARTNER_CENTER_ID__ and __VERSION__ placeholders in the MSIX manifest template.

.DESCRIPTION
    Reads AppxManifest.xml.template, substitutes the publisher identity and package
    version, and writes the result to AppxManifest.xml ready for makeappx.exe.

.PARAMETER Version
    App package version. Accepts 3-part (1.4.22) or 4-part (1.4.22.0) — the 4th
    part is forced to 0 if omitted, since Store packages require it to be 0.

.PARAMETER PublisherId
    The Publisher value from Partner Center > Product identity, e.g.
    "CN=A1B2C3D4-1234-5678-9ABC-DEF012345678".

.PARAMETER TemplatePath
    Path to the manifest template. Defaults to .\package\AppxManifest.xml.template

.PARAMETER OutputPath
    Path to write the resolved manifest. Defaults to .\package\AppxManifest.xml

.EXAMPLE
    .\Set-ManifestValues.ps1 -Version "1.4.22" -PublisherId "CN=A1B2C3D4-1234-5678-9ABC-DEF012345678"

.EXAMPLE
    # Typical CI usage, pulling from pipeline variables/secrets
    .\Set-ManifestValues.ps1 -Version $env:BUILD_VERSION -PublisherId $env:PARTNER_CENTER_PUBLISHER_ID
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [Parameter(Mandatory = $true)]
    [string]$PublisherId,

    [string]$TemplatePath = ".\AppxManifest.xml.template",

    [string]$OutputPath = ".\AppxManifest.xml"
)

$ErrorActionPreference = "Stop"

# --- Validate version -------------------------------------------------
# Accept 3-part (1.4.22) or 4-part (1.4.22.0); normalize to 4-part with
# trailing 0, since Store submissions require the 4th part to be 0.
if ($Version -notmatch '^\d+\.\d+\.\d+(\.\d+)?$') {
    throw "Version '$Version' is not a valid format. Expected Major.Minor.Build[.Revision], e.g. 1.4.22 or 1.4.22.0"
}

$parts = $Version.Split('.')
if ($parts.Count -eq 3) {
    $Version = "$Version.0"
}
elseif ($parts.Count -eq 4 -and $parts[3] -ne '0') {
    Write-Warning "Revision part of version is '$($parts[3])', not 0. Store packages require the 4th version part to be 0 forcing it."
    $Version = "$($parts[0]).$($parts[1]).$($parts[2]).0"
}

# --- Validate publisher -------------------------------------------------
if ([string]::IsNullOrWhiteSpace($PublisherId)) {
    throw "PublisherId is empty. Pass the exact 'Publisher' value from Partner Center > Product identity (e.g. 'CN=A1B2C3D4-1234-5678-9ABC-DEF012345678')."
}

if ($PublisherId -notmatch '^CN=') {
    Write-Warning "PublisherId '$PublisherId' does not start with 'CN='. Partner Center publisher values are normally in the form 'CN=<guid>'. Continuing, but double-check this value if the Store submission fails validation."
}

# --- Resolve template ----------------------------------------------------
if (-not (Test-Path $TemplatePath)) {
    throw "Template not found at '$TemplatePath'. Pass -TemplatePath if it lives elsewhere."
}

$content = Get-Content -Path $TemplatePath -Raw

if ($content -notmatch '__VERSION__') {
    Write-Warning "Placeholder '__VERSION__' not found in template — nothing to replace for version."
}
if ($content -notmatch '__PARTNER_CENTER_ID__') {
    Write-Warning "Placeholder '__PARTNER_CENTER_ID__' not found in template — nothing to replace for publisher."
}

$content = $content -replace '__VERSION__', $Version
$content = $content -replace '__PARTNER_CENTER_ID__', $PublisherId

# --- Write output ----------------------------------------------------
$outputDir = Split-Path -Path $OutputPath -Parent
if ($outputDir -and -not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

Set-Content -Path $OutputPath -Value $content -NoNewline -Encoding UTF8

Write-Host "Wrote manifest to '$OutputPath'"
Write-Host "  Version:   $Version"
Write-Host "  Publisher: $PublisherId"

# --- Sanity check: no placeholders left behind ----------------------------
$final = Get-Content -Path $OutputPath -Raw
if ($final -match '__[A-Z_]+__') {
    throw "Unresolved placeholder(s) remain in '$OutputPath'. Check the template for typos in placeholder names."
}
