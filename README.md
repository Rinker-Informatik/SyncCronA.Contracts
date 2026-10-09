# SyncCronA.Contracts

`SyncCronA.Contracts` enthält die gemeinsamen .NET-Verträge für die Kommunikation zwischen der SyncCronA-Control-Plane und ihren Agents. Die Bibliothek stellt die aus Protobuf generierten Nachrichtentypen und gRPC-Dienste bereit, damit beide Seiten dasselbe Protokoll verwenden.

## Enthaltene Verträge

- **Host-Agents:** Registrierung, Heartbeats, Befehle und deren Ergebnisse über einen bidirektionalen gRPC-Stream.
- **Application-Agents:** Registrierung von Handlern, Heartbeats, Ausführung und Abbruch von Handlern sowie Statusmeldungen und deren Bestätigung.
- **Enrollment:** Antworttypen für Bootstrap und Zertifikate sowie `EnrollmentProof.Canonicalize` zur einheitlichen Byte-Darstellung eines Enrollment-Nachweises.

Die Protobuf-Verträge liegen unter [`dotnet/SyncCronA.Contracts/Protos`](https://github.com/Rinker-Informatik/SyncCronA.Contracts/tree/main/dotnet/SyncCronA.Contracts/Protos). Die generierten C#-Typen verwenden die Namespaces `SyncCronA.Contracts.HostAgents` und `SyncCronA.Contracts.ApplicationAgents`; die Enrollment-Typen liegen in `SyncCronA.Contracts.Enrollment`.

## Verwendung

Das NuGet-Paket `SyncCronA.Contracts` in allen .NET-Projekten einbinden, die mit diesen Nachrichten oder Diensten arbeiten:

```bash
dotnet add package SyncCronA.Contracts
```

Das Paket ist für .NET 10 gebaut.
