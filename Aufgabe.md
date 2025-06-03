# 🛠 Übungsaufgabe: Arbeiten mit dem PlanManager + API

Ziel: Du erweiterst eine funktionierende .NET-Minimal-API mit dem `PlanManager`, um Stundenpläne zu verwalten.

---

## 📦 Voraussetzungen

- Projekt enthält `PlanManager.cs` und `Program.cs`
- Es existiert bereits ein vorgegebener Stundenplan (z. B. für KW 23)
- Der folgende Endpoint ist bereits vorhanden:
  - `GET /plan/all` → gibt alle gespeicherten Pläne zurück

---

## 🚀 Schritt 0 – Projekt starten mit Auto-Reload

Lade die nootwendigen Bibliotheken

```bash
dotnet add package Swashbuckle.AspNetCore
```

Nutze folgenden Befehl, damit dein Projekt automatisch bei Codeänderungen neu startet:

```bash
dotnet watch run
```

---

## ✅ Aufgabe 1 – API: Stundenplan für eine bestimmte Woche abrufen

Implementiere:

```http
GET /plan?kw=23
```

- Übergabe per Query-Parameter `kw`
- Verwende `planManager.GetPlan(kw)`
- Rückgabe: Liste aller Stunden mit Index in der KW

Beispielausgabe:
```json
[
  { "index": 0, "stunde": { "fach": "Mathe", "tag": "Montag", "zeit": "08:00" } },
  { "index": 1, "stunde": { "fach": "Informatik", "tag": "Dienstag", "zeit": "10:00" } }
]
```

---

## ✅ Aufgabe 2 – API: Nur einen bestimmten Tag abrufen

Implementiere:

```http
GET /plan/day?kw=23&tag=Dienstag
```

- Nutze `planManager.GetDayPlan(kw, tag)`
- Rückgabe: Nur Stunden am angegebenen Tag

---

## ✅ Aufgabe 3 – API: Einen Tag in eine KW schreiben

Implementiere:

```http
POST /plan/tag
Content-Type: application/json

{
  "kw": 24,
  "fach": "Physik",
  "tag": "Montag",
  "zeit": "09:00"
}
```

- Lies JSON aus dem Request-Body ein
- Verwende `planManager.AddStunde(...)`
- Rückgabe: Index der neu hinzugefügten Stunde

---

## ✅ Aufgabe 4 – API: Einen Eintrag löschen

Implementiere:

```http
DELETE /plan/tag?kw=23&index=1
```

- Verwende `planManager.DeleteStunde(kw, index)`
- Rückgabe: `{ "status": "gelöscht" }` oder Fehlermeldung

---

## 🌟 Bonus – API für Sync mit anderem Server

Implementiere:

```http
GET /sync?kw=23&remote=http://anderer-server:5224
```

- Verwende `HttpClient`, um von einem anderen Server `GET /plan?kw=...` zu holen
- Deserialisiere die Liste von Stunden
- Füge die Stunden mit `planManager.MergePlan(...)` ein
- Rückgabe: Übersicht mit `eigene`, `fremde`, `gesamt` Stunden

---

Viel Erfolg beim Entwickeln! 🚀
