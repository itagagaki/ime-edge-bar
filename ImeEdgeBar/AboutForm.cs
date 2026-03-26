using System.Diagnostics;

namespace ImeEdgeBar;

/// <summary>
/// About dialog showing the application name, version, and GitHub repository link.
/// </summary>
internal partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        using var iconStream = typeof(AboutForm).Assembly.GetManifestResourceStream("ImeEdgeBar.app.ico");
        if (iconStream is not null) Icon = new Icon(iconStream);
        var wa = Screen.FromPoint(Cursor.Position).WorkingArea;
        Location = new Point(wa.Left + (wa.Width - Width) / 2, wa.Top + (wa.Height - Height) / 2);
        _lblVersion.Text = $"Version {Application.ProductVersion.Split('+')[0]}";
    }

    private void _lnkGitHub_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo(_lnkGitHub.Text) { UseShellExecute = true });
    }
}
