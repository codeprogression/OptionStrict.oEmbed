push-location src\packages
$xml = [xml](get-content repositories.config)
$repositories = $xml.SelectNodes("repositories/repository")
$repositories | foreach {   
	"Processing " + $_.path
	"..\..\nuget.exe i " + $_.path  + " -source 'https://go.microsoft.com/fwlink/?LinkID=206669' -o . " | invoke-expression  
}	
pop-location