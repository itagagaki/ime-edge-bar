namespace ImeEdgeBar;

internal partial class SettingsForm : Form
{
    public EdgePosition Position =>
        _rdoBottom.Checked ? EdgePosition.Bottom :
        _rdoLeft.Checked   ? EdgePosition.Left   :
        _rdoRight.Checked  ? EdgePosition.Right  :
                             EdgePosition.Top;

    public int BarThickness   => (int)_numThickness.Value;
    public int ImeOnColorArgb  => _pnlImeOnColor.BackColor.ToArgb();
    public double ImeOnOpacity => _trkImeOnOpacity.Value / 100.0;
    public int ImeOffColorArgb  => _pnlImeOffColor.BackColor.ToArgb();
    public double ImeOffOpacity => _trkImeOffOpacity.Value / 100.0;

    public SettingsForm(AppSettings settings)
    {
        InitializeComponent();

        // Centre on the working area of the monitor that holds the cursor
        var wa = Screen.FromPoint(Cursor.Position).WorkingArea;
        Location = new Point(
            wa.Left + (wa.Width  - Width)  / 2,
            wa.Top  + (wa.Height - Height) / 2);

        // Populate position
        _rdoTop.Checked    = settings.Position == EdgePosition.Top;
        _rdoBottom.Checked = settings.Position == EdgePosition.Bottom;
        _rdoLeft.Checked   = settings.Position == EdgePosition.Left;
        _rdoRight.Checked  = settings.Position == EdgePosition.Right;

        // Populate thickness
        _numThickness.Value = Math.Clamp(settings.BarThickness, 4, 30);

        // Populate IME ON
        _pnlImeOnColor.BackColor   = Color.FromArgb(255, Color.FromArgb(settings.ImeOnColorArgb));
        _trkImeOnOpacity.Value      = (int)Math.Round(Math.Clamp(settings.ImeOnOpacity * 100, 1, 100));

        // Populate IME OFF
        _pnlImeOffColor.BackColor  = Color.FromArgb(255, Color.FromArgb(settings.ImeOffColorArgb));
        _trkImeOffOpacity.Value     = (int)Math.Round(Math.Clamp(settings.ImeOffOpacity * 100, 1, 100));

        UpdateOpacityLabels();

        _btnImeOnColor.Click  += (_, _) => PickColor(_pnlImeOnColor);
        _btnImeOffColor.Click += (_, _) => PickColor(_pnlImeOffColor);
        _trkImeOnOpacity.ValueChanged  += (_, _) => UpdateOpacityLabels();
        _trkImeOffOpacity.ValueChanged += (_, _) => UpdateOpacityLabels();
    }

    private void PickColor(Panel panel)
    {
        using var dlg = new ColorDialog { Color = panel.BackColor, FullOpen = true };
        if (dlg.ShowDialog(this) == DialogResult.OK)
            panel.BackColor = dlg.Color;
    }

    private void UpdateOpacityLabels()
    {
        _lblImeOnOpacityValue.Text  = $"{_trkImeOnOpacity.Value}%";
        _lblImeOffOpacityValue.Text = $"{_trkImeOffOpacity.Value}%";
    }
}
