### How to build the installer file (.msix)

1. Copy `recall.exe` from the GitHub release to `/package`
2. CD to `/package` and run `.\Set-ManifestValues.ps1 -Version {__VERSION__} -PublisherId {__Partner__Center__ ID__}` e.g. `.\Set-ManifestValues.ps1 -Version 1.0.4.0 -PublisherId CN=random-partner-center-id-guid`
3. CD to `/` and run `& "C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64\makeappx.exe" pack /d .\package /p .\msix-package\recall.msix /overwrite` 