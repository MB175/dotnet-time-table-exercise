# 📘 PlanManager – Stundenplanverwaltung in .NET

Der `PlanManager` ist eine Hilfsklasse zur Verwaltung von Stundenplänen nach Kalenderwochen und Zeitfenstern. Jede Kalenderwoche (`KW`) enthält mehrere Stunden, die über einen eindeutigen **Index** identifiziert werden.

Ideal für APIs, Sync-Tools oder einfache Verwaltungs-Backends.

---

## 🧱 Datenstruktur

```csharp
Dictionary<int, Dictionary<int, Stunde>>
```

- `int` (KW) → Kalenderwoche  
- `int` (Index) → eindeutiger Index pro Stunde in dieser KW  
- `Stunde` → Objekt mit Fach, Tag, Zeit

---

## 📦 Model: `Stunde`

```csharp
public record Stunde
{
    public string Fach { get; set; } = "";
    public string Tag { get; set; } = "";
    public string Zeit { get; set; } = "";
}
```

---

## 🏗 Konstruktor

```csharp
new PlanManager(Dictionary<int, Dictionary<int, Stunde>>? initial = null)
```

> Erstellt einen leeren Manager oder lädt direkt Pläne vor.

```csharp
var manager = new PlanManager(); // leer

var preload = new Dictionary<int, Dictionary<int, Stunde>> { ... };
var manager2 = new PlanManager(preload); // mit initialem Plan
```

---

## 📥 AddStunde

```csharp
int AddStunde(int kw, Stunde stunde)
```

> Fügt eine neue Stunde in die Kalenderwoche ein. Gibt den neuen Index zurück.

```csharp
int index = manager.AddStunde(23, new Stunde { Fach = "Bio", Tag = "Dienstag", Zeit = "10:00" });
```

---

## 📤 GetPlan

```csharp
IEnumerable<(int Index, Stunde Stunde)> GetPlan(int kw)
```

> Gibt alle Stunden einer KW mit Index zurück.

```csharp
var alle = manager.GetPlan(23);
```

---

## 📅 GetDayPlan

```csharp
IEnumerable<(int Index, Stunde Stunde)> GetDayPlan(int kw, string tag)
```

> Gibt nur Stunden eines bestimmten Tages (z. B. `"Dienstag"`) zurück.

```csharp
var dienstag = manager.GetDayPlan(23, "Dienstag");
```

---

## ❌ DeleteStunde

```csharp
bool DeleteStunde(int kw, int index)
```

> Löscht eine Stunde über Index und KW. Gibt `true` zurück bei Erfolg.

```csharp
manager.DeleteStunde(23, 1);
```

---

## 🔁 UpdateStunde

```csharp
bool UpdateStunde(int kw, int index, Stunde neueStunde)
```

> Aktualisiert eine existierende Stunde.

```csharp
manager.UpdateStunde(23, 1, new Stunde { Fach = "Geo", Tag = "Dienstag", Zeit = "11:00" });
```

---

## 🧾 GetAllStunden

```csharp
List<Stunde> GetAllStunden(int kw)
```

> Gibt nur die Stunde-Objekte (ohne Index) zurück.

```csharp
var nurStunden = manager.GetAllStunden(23);
```

---

## 🧱 GetRaw

```csharp
Dictionary<int, Dictionary<int, Stunde>> GetRaw()
```

> Gibt die komplette interne Datenstruktur zurück.

```csharp
var alleDaten = manager.GetRaw();
```

---

## 🔗 MergePlan

```csharp
void MergePlan(int kw, IEnumerable<Stunde> neueStunden)
```

> Fügt mehrere Stunden in eine KW ein, mit automatisch fortlaufenden Indizes.

```csharp
manager.MergePlan(23, new[] {
    new Stunde { Fach = "Spanisch", Tag = "Freitag", Zeit = "13:00" }
});
```

---

## 📋 Beispielnutzung

```csharp
var manager = new PlanManager();

int index = manager.AddStunde(24, new Stunde { Fach = "Bio", Tag = "Freitag", Zeit = "12:00" });

var alle = manager.GetPlan(24);

manager.UpdateStunde(24, index, new Stunde { Fach = "Biologie", Tag = "Freitag", Zeit = "12:00" });

manager.DeleteStunde(24, index);
```

---

