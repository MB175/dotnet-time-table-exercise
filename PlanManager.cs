using System;
using System.Collections.Generic;
using System.Linq;


public record Stunde
{
    public string Fach { get; set; } = "";
    public string Tag { get; set; } = "";
    public string Zeit { get; set; } = "";
}

public record StundeMitKW(int KW, string Fach, string Tag, string Zeit);

public record StundeMitIndex(int Index, Stunde Stunde);

public record StundeAenderungMitIndex(int KW, int Index, Stunde Stunde);


public class PlanManager
{
    private readonly Dictionary<int, Dictionary<int, Stunde>> _plaene;

    public PlanManager(Dictionary<int, Dictionary<int, Stunde>>? initial = null)
    {
        _plaene = initial != null
            ? new Dictionary<int, Dictionary<int, Stunde>>(initial)
            : new Dictionary<int, Dictionary<int, Stunde>>();
    }

    // 📥 Neue Stunde einfügen
    public int AddStunde(int kw, Stunde stunde)
    {
        if (!_plaene.ContainsKey(kw))
            _plaene[kw] = new();

        var index = _plaene[kw].Count > 0
            ? _plaene[kw].Keys.Max() + 1
            : 0;

        _plaene[kw][index] = stunde;
        return index;
    }

    // 📤 Alle Stunden mit Index einer KW abrufen
    public IEnumerable<StundeMitIndex> GetPlan(int kw)
    {
        if (_plaene.TryGetValue(kw, out var plan))
            return plan.Select(p => new StundeMitIndex(p.Key, p.Value));

        return Enumerable.Empty<StundeMitIndex>();
    }

    // 📅 Stunden eines bestimmten Tags abrufen
    public IEnumerable<StundeMitIndex> GetDayPlan(int kw, string tag)
    {
        return GetPlan(kw)
            .Where(p => p.Stunde.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));
    }

    // ❌ Stunde löschen
    public bool DeleteStunde(int kw, int index)
    {
        return _plaene.TryGetValue(kw, out var plan) && plan.Remove(index);
    }

    // 🔁 Stunde ändern
    public bool UpdateStunde(int kw, int index, Stunde neueStunde)
    {
        if (_plaene.TryGetValue(kw, out var plan) && plan.ContainsKey(index))
        {
            plan[index] = neueStunde;
            return true;
        }
        return false;
    }

    // 🔄 Alle Stunden als Dictionary (für /plan/all)
    public Dictionary<int, Dictionary<int, Stunde>> GetRaw()
    {
        return _plaene;
    }

    // 📊 Nur Stundenwerte (ohne Index) einer KW
    public List<Stunde> GetAllStunden(int kw)
    {
        return _plaene.TryGetValue(kw, out var plan)
            ? plan.Values.ToList()
            : new List<Stunde>();
    }

    // 🔗 Fremdplan in lokale KW mergen
    public void MergePlan(int kw, IEnumerable<Stunde> neueStunden)
    {
        if (!_plaene.ContainsKey(kw))
            _plaene[kw] = new();

        var plan = _plaene[kw];
        int nextIndex = plan.Count > 0 ? plan.Keys.Max() + 1 : 0;

        foreach (var stunde in neueStunden)
        {
            plan[nextIndex++] = stunde;
        }
    }
}
