using System.Globalization;

public static class NumberFormatter
{
    public static string FormatCount(long n)
    {
        if (n >= 1000000) return (n / 1000000f).ToString("0.#", CultureInfo.InvariantCulture) + "M";
        if (n >= 1000) return (n / 1000f).ToString("0.#", CultureInfo.InvariantCulture) + "k";
        return n.ToString(CultureInfo.InvariantCulture);
    }
}
