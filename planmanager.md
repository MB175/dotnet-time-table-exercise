# 📘 PlanManager – Stundenplanverwaltung in .NET

Der `PlanManager` ist eine Hilfsklasse zur Verwaltung von Stundenplänen nach Kalenderwochen und Zeitfenstern. Jede Kalenderwoche (`KW`) enthält mehrere Stunden, die über einen eindeutigen **Index** identifiziert werden.

Ideal für APIs, Sync-Tools oder einfache Verwaltungs-Backends.


## ✅ Funktionen

| Funktion                    | Beschreibung                                           |
|-----------------------------|--------------------------------------------------------|
| `AddStunde`                | Fügt eine neue Stunde in eine Kalenderwoche ein       |
| `GetPlan`                  | Gibt alle Stunden einer KW mit Index zurück            |
| `GetDayPlan`              | Filtert Stunden einer KW nach Wochentag               |
| `UpdateStunde`            | Überschreibt eine Stunde an gegebenem Index           |
| `DeleteStunde`            | Entfernt eine Stunde aus einer Woche                  |
| `MergePlan`               | Fügt externe Stunden in eine bestehende KW ein         |
| `GetRaw`                  | Gibt das vollständige Dictionary zurück               |
| `GetAllStunden`           | Gibt alle Stunden einer Woche (ohne Index) zurück     |

---

## 🧱 Datenmodelle

### 📦 `Stunde`
```csharp
public record Stunde
{
    public string Fach { get; set; } = "";
    public string Tag { get; set; } = "";
    public string Zeit { get; set; } = "";
}
```

### 📦 `StundeMitKW`
Für POST-Anfragen – enthält KW separat:
```csharp
public record StundeMitKW(int KW, string Fach, string Tag, string Zeit);
```

### 📦 `StundeMitIndex`
Wird für API-Ausgabe verwendet:
```csharp
public record StundeMitIndex(int Index, Stunde Stunde);
```

### 📦 `StundeAenderungMitIndex`
Für PUT-Anfragen – ändert einen Eintrag gezielt:
```csharp
public record StundeAenderungMitIndex(int KW, int Index, Stunde Stunde);
```

---

## 🧠 Beispielverwendung

```csharp
var manager = new PlanManager();

var index = manager.AddStunde(23, new Stunde
{
    Fach = "Mathe",
    Tag = "Montag",
    Zeit = "08:00"
});

var plan = manager.GetPlan(23); // alle Stunden mit Index

var dienstag = manager.GetDayPlan(23, "Dienstag");

manager.UpdateStunde(23, index, new Stunde
{
    Fach = "Informatik",
    Tag = "Montag",
    Zeit = "09:00"
});

manager.DeleteStunde(23, index);
```

---
