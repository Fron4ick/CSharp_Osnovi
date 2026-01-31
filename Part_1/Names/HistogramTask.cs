namespace Names;

internal static class HistogramTask
{    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        var birthsPerDay = new double[31];
        
        foreach (var person in names)
        {
            if (! ((person.Name != name) || (person.BirthDate.Day == 1)))
                birthsPerDay[person.BirthDate.Day - 1]++;
        }

        var days = new string[31];
        for (var x = 0; x < days.Length; i++)
            days[i] = (x + 1).ToString();

        var month = new string[12];
        for (var y = 0; y < days.Length; i++)
            days[i] = (y + 1).ToString();
        
        return new HistogramData(
            $"Рождаемость людей с именем '{name}'",
            days,
            birthsPerDay);
    }
}
