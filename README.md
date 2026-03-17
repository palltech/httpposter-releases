# httpPostGet

A lightweight desktop tool for testing HTTP requests. A simple, portable alternative to tools like Postman — designed for quick use on-site without installation.

## Features

- Send HTTP requests — GET, POST, PUT, DELETE, PATCH
- Header editor (key/value, collapsible section)
- Raw request body with Content-Type selection
- JDF (`application/vnd.cip4-jdf+xml`) and JMF (`application/vnd.cip4-jmf+xml`) support
- Response viewer (status, headers, body with automatic JSON/XML pretty-print)
- URL directory with optional description
- Saved requests (store a full request under a custom name)
- History of last 10 sent requests

## Download

### v1.0

| Variant | Requirements |
|---|---|
| [httpPostGet-v1.0-standalone.zip](../../releases/download/v1.0/httpPostGet-v1.0-standalone.zip) | None — runs on any Windows 10/11 x64 |
| [httpPostGet-v1.0-dependent.zip](../../releases/download/v1.0/httpPostGet-v1.0-dependent.zip) | [.NET 9 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/9.0/runtime) |

> Not sure which to pick? Download the standalone version — it works out of the box.

## Technology

- [Avalonia UI 11](https://avaloniaui.net/) — cross-platform desktop UI framework
- .NET 9, C#
- CommunityToolkit.Mvvm (MVVM pattern)
- System.Text.Json (data storage)

## Running (development)

```bash
dotnet run --project HttpPostGet.csproj
```

## Publishing (self-contained, no dependencies)

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

Produces a single portable `HttpPostGet.exe` with no .NET runtime installation required.

## Data

The app stores data in `data/data.json` next to the executable:

```
httpPostGet.exe
data/
  data.json    ← URL directory, saved requests, history
```

---

Palltech httpPostGet v1.0  ©  2026
