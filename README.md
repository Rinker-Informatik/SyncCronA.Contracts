# SyncCronA.Contracts

## CI und NuGet-Veröffentlichung

Bei Pushes auf `main` und bei Pull Requests führt [`.NET CI`](.github/workflows/dotnet-ci.yml) Clean, Restore, Build und Tests mit .NET 10 aus.

Für die Veröffentlichung auf NuGet.org [Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing) einrichten:

1. Auf NuGet.org unter **Trusted Publishing** eine GitHub-Policy mit Repository Owner `Rinker-Informatik`, Repository `SyncCronA.Contracts` und Workflow File `publish-nuget.yml` anlegen. Als Policy Owner das NuGet.org-Konto oder die NuGet.org-Organisation auswählen, unter der das Paket erscheinen soll. Als Scope **Push new packages and package versions** und als Paketmuster `SyncCronA.Contracts` wählen. Das optionale Feld **Environment** leer lassen.
2. In GitHub unter **Settings → Secrets and variables → Actions → Variables** die Repository- oder Organisationsvariable `NUGET_USER` mit dem NuGet.org-Benutzernamen der Person anlegen, die die Policy erstellt hat (bei einer Organisations-Policy: ein Mitglied der Organisation). Bei einer Organisationsvariable muss `SyncCronA.Contracts` Zugriff darauf haben. Eine E-Mail-Adresse oder der GitHub-Benutzername ist hier nicht gemeint. Ein dauerhaftes API-Key-Secret wird nicht benötigt.
3. Einen Versionstag erstellen und pushen, zum Beispiel:

```bash
git tag v1.0.0
git push origin v1.0.0
```

Der [Publish-Workflow](.github/workflows/publish-nuget.yml) führt zuerst die CI-Prüfung aus. Nur bei Erfolg erstellt er `SyncCronA.Contracts.1.0.0.nupkg`, meldet sich über GitHub OIDC bei NuGet.org an und veröffentlicht das Paket. Tags wie `v1.0.0-rc.1` sind ebenfalls möglich.
