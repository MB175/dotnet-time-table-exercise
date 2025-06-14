# 🧰 Hilfe & Tipps zur API-Entwicklung

Dieses Dokument hilft dir bei der Entwicklung deiner Minimal API mit .NET sowie beim Testen mit `httpyac`.

---

## 📬 HTTP-Methoden richtig nutzen in Minimal APIs

### 🔸 `POST`-Daten lesen (JSON aus Body)

```csharp
app.MapPost("/plan/tag", async (HttpRequest request) =>
{
    var body = await request.ReadFromJsonAsync<StundeMitKW>();
    if (body == null)
        return Results.BadRequest(new { fehler = "Ungültiger Body" });

    // Weiterverarbeitung …
});
```

📄 Microsoft Docs:
- [ReadFromJsonAsync](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.httprequestjsonextensions.readfromjsonasync)

---

### 🔸 `DELETE` mit Query-Parametern

```csharp
app.MapDelete("/plan/tag", (int? kw, int? index) =>
{
    if (kw == null || index == null)
        return Results.BadRequest(new { fehler = "Fehlender Parameter" });

    // Weiterverarbeitung …
});
```

📄 Microsoft Docs:
- [Minimal APIs – Parameter Binding](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis#parameter-binding)

---

### 🔸 `GET` mit mehreren Parametern

```csharp
app.MapGet("/plan/day", (int? kw, string? tag) =>
{
    if (kw == null || string.IsNullOrWhiteSpace(tag))
        return Results.BadRequest(new { fehler = "kw oder tag fehlt" });

    // Weiterverarbeitung …
});
```

---

## 🧪 HTTP-API testen mit `httpyac`



### 📁 Beispiel-Datei: `api.http`

```http
### 🟢 Abruf aller Stunden einer Woche
GET http://localhost:5224/plan?kw=23
Accept: application/json

### 🔵 Neue Stunde hinzufügen
POST http://localhost:5224/plan/tag
Content-Type: application/json

{
  "kw": 24,
  "fach": "Physik",
  "tag": "Mittwoch",
  "zeit": "10:15"
}

### 🔴 Eintrag löschen
DELETE http://localhost:5224/plan/tag?kw=24&index=0
```

👉 Du kannst alle Blöcke in VS Code mit [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) oder `httpyac` ausführen.

---

## 📥 Erklärung: `ReadFromJsonAsync<T>()`

```csharp
var body = await request.ReadFromJsonAsync<StundeMitKW>();
```

### 🔍 Was passiert hier?

Diese Zeile liest den HTTP-Request-Body und wandelt ihn automatisch in ein C#-Objekt um. In diesem Fall: `StundeMitKW`.

---

### 🧠 Beispiel

Wenn der Client folgendes JSON sendet:

```json
{
  "kw": 24,
  "fach": "Physik",
  "tag": "Montag",
  "zeit": "09:00"
}
```

Dann wird daraus automatisch folgendes C#-Objekt erzeugt:

```csharp
new StundeMitKW(24, "Physik", "Montag", "09:00");
```

---

### ✅ Voraussetzung

- Der HTTP-Header muss `Content-Type: application/json` enthalten
- Das JSON muss zum Aufbau des Record-Typs passen

---

### 📘 Hintergrund

- Methode: `HttpRequest.ReadFromJsonAsync<T>()`
- Kommt aus dem Namespace: `System.Net.Http.Json`
- Gibt ein `Task<T>` zurück → deshalb muss `await` verwendet werden
- Spart Zeit, weil kein manuelles Parsen notwendig ist

---
---

## 💡 Weitere Tipps

- Endpunkte sollten immer sinnvoll prüfen, ob Parameter fehlen
- Verwende `Results.BadRequest(...)`, `Results.Ok(...)` oder `Results.NotFound(...)` je nach Ergebnis
- Nutze `dotnet watch run` beim Entwickeln für automatisches Neustarten

---

## 📚 Ressourcen

- [Minimal APIs in .NET](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [HttpRequest.ReadFromJsonAsync](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.httprequestjsonextensions.readfromjsonasync)
- [httpyac](https://github.com/AnWeber/httpyac)
- [REST Client für VS Code](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

---
