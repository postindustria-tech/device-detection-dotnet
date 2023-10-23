param(
    [Parameter(Mandatory=$true)]
    [string]$RepoName,
    [string]$OrgName,
    [string]$ProjectDir = ".",
    [string]$Name = "Release_x64",
    [string]$Configuration = "Release",
    [string]$Arch = "x64"
)

. ./$RepoName/ci/run-performance-tests-console.ps1 -RepoName $RepoName -OrgName $OrgName -Name $Name -Configuration $Configuration -Arch $Arch

# if ($LASTEXITCODE -ne 0) {
#     exit $LASTEXITCODE
# }

# Write-Output ""
# Write-Output "===== CONSOLE / WEB DELIMITER ====="
# Write-Output ""

# . ./$RepoName/ci/run-performance-tests-web.ps1 -RepoName $RepoName -ProjectDir $ProjectDir -Name $Name -Configuration $Configuration -Arch $Arch

exit $LASTEXITCODE
