
param(
    [string]$ProjectDir = ".",
    [string]$Name,
    [Parameter(Mandatory=$true)]
    [string]$RepoName
)
try {
    ./dotnet/run-update-dependencies.ps1 -RepoName $RepoName -ProjectDir $ProjectDir -Name $Name -Debug
}
catch {
    Write-Output "An error occurred:"
    Write-Output $_
}
finally {
    Write-Output "░░░░░░░░░ ░░░░░░░░░ ░░░░░░░░░ ░░░░░░░░░ ░░░░░░░░░"
    Write-Output "In [update-packages.ps1]:"
    Write-Output "LASTEXITCODE = $LASTEXITCODE"
}

exit $LASTEXITCODE