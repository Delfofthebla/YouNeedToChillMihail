using Noggog;

namespace YouNeedToChillMihail;

public static class Extensions
{
    public static bool HasValueAndContainsId(this string str, string rhs)
    {
        if (str.IsNullOrWhitespace() || rhs.IsNullOrWhitespace())
            return false;
        
        return str.Trim().Contains(rhs.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
