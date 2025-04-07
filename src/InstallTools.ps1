cls
dotnet new tool-manifest
#dotnet tool install --local coverlet.console
#dotnet tool install --local csharpier
#dotnet tool install --local csharptotypescript.clitool
#dotnet tool install --local docfx
#dotnet tool install --local dotnet-aop
dotnet tool install --local dotnet-depends
dotnet tool install --local dotnet-ef
dotnet tool install --local dotnet-outdated-tool
dotnet tool install --local dotnet-project-licenses
dotnet tool install --local dotnet-property
dotnet tool install --local dotnet-repl
#dotnet tool install --local dotnet-reportgenerator-globaltool
#dotnet tool install --local dotnetsay
#dotnet tool install --local dotnet-sql-cache
dotnet tool install --local dotnetthx
#dotnet tool install --local gitversion.tool
#dotnet tool install --local grate
#dotnet tool install --local mapster.tool
dotnet tool install --local microsoft.dotnet-httprepl
#dotnet tool install --local moniker.cli
dotnet tool install --local netpackageanalyzerconsole
#dotnet tool install --local nswag.consolecore
dotnet tool install --local powershell
dotnet tool install --local run-script
#dotnet tool install --local strawberryshake.tools
dotnet tool install --local watch2
#dotnet tool install --local xunit-cli
# add global json for run script
$file = "global.json"
if (-not (Test-Path -Path $file)) {
$multiLineText = @"
{
  `"scripts`": {
    `"build`": `"dotnet build --configuration Release`",
    `"test`": `"dotnet test --configuration Release`",
    `"ci": `"dotnet r build && dotnet r test`"    
  }
}
"@


New-Item $file -ItemType File -Value $multiLineText

}

foreach ($path in Get-ChildItem -Recurse "**/*.csproj") {
	$folder= Split-Path -parent $path
	$xml = [xml]::new()
	$xml.PreserveWhitespace = $true
	$xml.Load($path)
	$propertyGroups = $xml.SelectSingleNode("//Project/PropertyGroup/TreatWarningsAsErrors")
	if ($node) {
		# Write-Output "Project already has TreatWarningsAsErrors set to $($node.InnerText)"
		$node.InnerText="true"		
	}
	else{
		# Construct a new node and fill in its value
		$newNode = $xml.CreateDocumentFragment()
		$newNode.InnerXml = "<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`r`n"

		# Append child and save the results to a variable
		# If you forget, PowerShell prints the return value to console!
		$_ = $xml.Project.PropertyGroup.AppendChild($newNode)
	}
	$xml.Save($path)

# add nuget package
	Push-Location $folder
	$addPack = & dotnet add package RSCG_TimeBombComment 


	if ($?) {
		Write-Output "Successfully add package ${path}"
	} else {
		Write-Output "Unable to add package ${path} with the updated property setting."
	}
	Pop-Location
		
}
