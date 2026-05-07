using System.Text.Json;

namespace ImeEdgeBar;

internal enum EdgePosition { Top, Bottom, Left, Right }

/// <summary>
/// Application settings persisted as JSON in the user's AppData folder.
/// </summary>
internal class AppSettings
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ImeEdgeBar", "settings.json");

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    /// <summary>Edge of the monitor on which the bar is displayed.</summary>
    public EdgePosition Position { get; set; } = EdgePosition.Top;

    /// <summary>
    /// Thickness of the bar in pixels.
    /// For Top/Bottom this is the height; for Left/Right this is the width.
    /// </summary>
    public int BarThickness { get; set; } = 8;

    /// <summary>Background color (ARGB) when IME is ON.</summary>
    public int ImeOnColorArgb { get; set; } = Color.Green.ToArgb();

    /// <summary>Window opacity (0.0–1.0) when IME is ON.</summary>
    public double ImeOnOpacity { get; set; } = 1.0;

    /// <summary>Background color (ARGB) when IME is OFF.</summary>
    public int ImeOffColorArgb { get; set; } = Color.Red.ToArgb();

    /// <summary>Window opacity (0.0–1.0) when IME is OFF.</summary>
    public double ImeOffOpacity { get; set; } = 1.0;

    /// <summary>Whether to draw the mouse pointer position indicator (arrow) on the bar.</summary>
    public bool ShowMousePointerIndicator { get; set; } = true;

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
        }
        catch { }
        return new AppSettings();
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, JsonOptions));
        }
        catch { }
    }
}
