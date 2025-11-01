using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace YouNeedToChillMihail;

public class Settings
{
    [MaintainOrder]

    [SettingName("Health Nerf Percent")]
    [Tooltip("The percentage of health to nerf all Mihail Mobs by\n(You can also set this negative to buff them instead but I don't know why you would do that)")]
    public float HealthNerfPercent { get; set; } = 0.4f;
    public float HealthMultiplier => 1f - HealthNerfPercent;
    
    [SettingName("Damage Nerf Percent")]
    [Tooltip("The percentage of damage to nerf all Mihail Mobs by\n(You can also set this negative to buff them instead but I don't know why you would do that)")]
    public float DamageNerfPercent { get; set; } = 0.5f;
    public float DamageMultiplier => 1f - DamageNerfPercent;
    
    [SettingName("Ignored mods")]
    [Tooltip("A list of mods that should not be patched. Each entry is a string matching a plugin name.")]
    public List<string> Ignored
    {
        get => _ignored;
        set => _ignored = value;
    }
    private List<string> _ignored = [];

    [SettingName("Blacklisted EditorIDs")]
    [Tooltip("Each entry is a comma separated list of partial Weapon EditorIDs, case insensitive.")]
    public List<string> Blacklist
    {
        get => _blacklist.Select(filter => string.Join(",", filter)).ToList();
        set => _blacklist = ParseIdFilters(value);
    }
    private List<string[]> _blacklist = [];
    
    private HashSet<ModKey>? _ignoredMods;
    public HashSet<ModKey> IgnoredMods
    {
        get
        {
            var modKeys = new List<ModKey>();
            IList<ModKey> modFiles = Program.State.LoadOrder.Keys.ToList();
            foreach (var modFilter in Ignored.Where(modFilter => modFilter != ""))
                modKeys.AddRange(modFiles.Where(modKey => modKey.FileName.String.Contains(modFilter, StringComparison.OrdinalIgnoreCase)));
            _ignoredMods = new HashSet<ModKey>(modKeys);
            return _ignoredMods;
        }
    }
    
    private static List<string[]> ParseIdFilters(IEnumerable<string> filterData)
    {
        List<string[]> result = [];
        foreach (var filterDataItem in filterData)
        {
            if (string.IsNullOrEmpty(filterDataItem))
                continue;

            var filterElements = filterDataItem.Split(',');
            if (filterElements.Length > 0)
                result.Add(filterElements);
        }

        return result;
    }
}