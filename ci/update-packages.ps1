
param(
    [string]$ProjectDir = ".",
    [string]$Name,
    [Parameter(Mandatory=$true)]
    [string]$RepoName
)
try {
    ./dotnet/run-update-dependencies.ps1 -RepoName $RepoName -ProjectDir $ProjectDir -Name $Name
}
catch {
    Write-Output "An error occurred:"
    Write-Output $_
    Write-Output "========= ========= ========="
}
finally {
    Write-Output "LASTEXITCODE = $LASTEXITCODE"
}

exit $LASTEXITCODE