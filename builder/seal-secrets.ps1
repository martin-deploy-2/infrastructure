Get-ChildItem -Path "$PSScriptRoot/*.yaml.secret" | ForEach-Object {
	"# This file is generated, do not edit manually." `
		> ($_.FullName -replace ".yaml.secret", ".yaml")

	kubeseal `
		--secret-file ($_.FullName) `
		--allow-empty-data `
		--cert "$PSScriptRoot/sealed-cert.pem" `
		--format yaml `
		>> ($_.FullName -replace ".yaml.secret", ".yaml")
}
