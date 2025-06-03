using System;
using System.Collections.Generic;
using System.Linq;



// 📦 Models als record types (inline embedded)

public record Stunde
{
    public string Fach { get; set; } = "";
    public string Tag { get; set; } = "";
    public string Zeit { get; set; } = "";
}

record StundeMitKW(int KW, string Fach, string Tag, string Zeit);

record StundeAenderung(int KW, Stunde Alt, Stunde Neu);


public class PlanManager
{
    private readonly Dictionary<int, Dictionary<int, Stunde>> _plaene;

        // ✅ Konstruktor mit optionalem Initialwert
    public PlanManager(Dictionary<int, Dictionary<int, Stunde>>? initial = null)
    {
        _plaene = initial != null
            ? new Dictionary<int, Dictionary<int, Stunde>>(initial)
            : new Dictionary<int, Dictionary<int, Stunde>>();
    }


    public IEnumerable<(int Index, Stunde Stunde)> GetPlan(int kw)
    {
        if (_plaene.TryGetValue(kw, out var plan))
            return plan.Select(p => (p.Key, p.Value));

        return Enumerable.Empty<(int, Stunde)>();
    }

    public IEnumerable<(int Index, Stunde Stunde)> GetDayPlan(int kw, string tag)
    {
        return GetPlan(kw)
            .Where(p => p.Stunde.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));
    }

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

    public bool DeleteStunde(int kw, int index)
    {
        return _plaene.TryGetValue(kw, out var plan) && plan.Remove(index);
    }

    public bool UpdateStunde(int kw, int index, Stunde neueStunde)
    {
        if (_plaene.TryGetValue(kw, out var plan) && plan.ContainsKey(index))
        {
            plan[index] = neueStunde;
            return true;
        }
        return false;
    }

    public Dictionary<int, Dictionary<int, Stunde>> GetRaw()
    {
        return _plaene;
    }

    public List<Stunde> GetAllStunden(int kw)
    {
        return _plaene.TryGetValue(kw, out var plan)
            ? plan.Values.ToList()
            : new List<Stunde>();
    }

    public void MergePlan(int kw, IEnumerable<Stunde> neueStunden)
    {
        if (!_plaene.ContainsKey(kw))
            _plaene[kw] = new();

        var plan = _plaene[kw];
        int nextIndex = plan.Count > 0 ? plan.Keys.Max() + 1 : 0;

        foreach (var s in neueStunden)
        {
            plan[nextIndex++] = s;
        }
    }
}
