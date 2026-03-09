# httpPoster

Desktopová aplikace pro testování HTTP requestů. Jednoduchá, přenositelná alternativa k nástrojům jako Postman — navržena pro rychlé použití přímo u zákazníka bez nutnosti instalace.

## Funkce

- Odesílání HTTP requestů (GET, POST, PUT, DELETE, PATCH)
- Editor hlaviček (key/value, sbalitelná sekce)
- Raw tělo requestu s výběrem Content-Type
- Podpora JDF (`application/vnd.cip4-jdf+xml`) a JMF (`application/vnd.cip4-jmf+xml`)
- Zobrazení odpovědi (status, hlavičky, tělo s automatickým pretty-print JSON)
- Adresář URL s volitelným popisem
- Oblíbené requesty (uložení celého requestu pod vlastním názvem)
- Historie posledních 10 odeslaných requestů

## Technologie

- [Avalonia UI 11](https://avaloniaui.net/) — cross-platform desktop UI framework
- .NET 9, C#
- CommunityToolkit.Mvvm (MVVM pattern)
- System.Text.Json (ukládání dat)

## Spuštění (development)

```bash
dotnet run --project HttpPoster.csproj
```

## Publikování (self-contained, bez závislostí)

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

Výsledkem je jeden přenositelný `HttpPoster.exe` bez nutnosti instalace .NET runtime.

## Data

Aplikace ukládá data do souboru `data/data.json` vedle spustitelného souboru:

```
httpPoster.exe
data/
  data.json    ← adresář URL, oblíbené requesty, historie
```

---

Palltech httpPoster v0.1  ©  2026
