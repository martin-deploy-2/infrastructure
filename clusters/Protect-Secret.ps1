Get-ChildItem -Path "$PSScriptRoot" -Directory -Name | ForEach-Object {
	$Cluster = "$PSScriptRoot/$_"
	Write-Host "+ Cluster: $Cluster" -ForegroundColor "Blue"

	Get-ChildItem -Path "$Cluster/namespaces" -Directory -Name | ForEach-Object {
		$Namespace = "$Cluster/namespaces/$_"
		Write-Host "  + Namespace: $Namespace" -ForegroundColor "Green"

		Get-ChildItem -Path "$Namespace/sealed-secrets/*.yaml.secret" File -Name | ForEach-Object {
			$Secret = "$Namespace/sealed-secrets/$_"
			$SealedSecret = $Secret -replace ".yaml.secret", ".yaml"

			Write-Host "    + Secret: $Secret" -ForegroundColor "Gray"
			Write-Host "      Sealed: $SealedSecret" -ForegroundColor "Gray"

			"" > $SealedSecret
			"# This file is generated, do not edit manually." >> $SealedSecret
			"# When editing ``$Secret``, run ``Protect-Secret.ps1`` to regenerate." >> $SealedSecret

			kubeseal `
				--secret-file $Secret `
				--allow-empty-data `
				--cert "$Cluster/sealed-secrets-cert.pem" `
				--format yaml `
				>> $SealedSecret
		}
	}
}
