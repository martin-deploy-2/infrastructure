Get-ChildItem -Path "$PSScriptRoot/*.yaml.secret" | ForEach-Object {
	kubeseal `
		--secret-file ($_.FullName) `
		--sealed-secret-file ($_.FullName -replace ".yaml.secret", ".yaml") `
		--allow-empty-data `
		--cert "$PSScriptRoot/sealed-cert.pem" `
		--format yaml
}
