using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;

namespace ImeEdgeBar;

/// <summary>
/// Screen-edge bar that tracks the mouse pointer position and reflects the IME state
/// via background colour.  An arrow drawn inside the bar points at the mouse X (or Y)
/// coordinate so the user can see where on screen the pointer currently is.
/// </summary>
public partial class MainForm : Form
{
    private readonly System.Windows.Forms.Timer _pollTimer;
    private readonly NotifyIcon _notifyIcon;
    private const int ToggleVisibilityHotkeyId = 1;
    private const int WS_EX_NOACTIVATE = 0x08000000;
    private const int WS_EX_TOOLWINDOW = 0x00000080;
    private const int WS_EX_LAYERED    = 0x00080000;

    private bool _imeOn;
    private double _bgOpacity = 1.0;
    private Point _mousePos;
    private Screen? _currentScreen;
    private IntPtr _currentTrayIconHandle = IntPtr.Zero;

    private readonly AppSettings _settings = AppSettings.Load();
    private SettingsForm? _settingsForm;

    public MainForm()
    {
        InitializeComponent();

        _pollTimer = new System.Windows.Forms.Timer { Interval = 50 };
        _pollTimer.Tick += PollTimer_Tick;

        _notifyIcon = new NotifyIcon { Text = "IME Edge Bar", Visible = true };
        UpdateTrayIcon();

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("表示 / 非表示  (Ctrl+Alt+B)", null, (_, _) => ToggleVisibility());
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add("設定...", null, (_, _) => OpenSettings());
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add("終了", null, (_, _) => ExitApplication());
        _notifyIcon.ContextMenuStrip = contextMenu;

        _mousePos = Cursor.Position;
        _currentScreen = Screen.FromPoint(_mousePos);
        ApplySettings();

        _pollTimer.Start();
    }

    // -----------------------------------------------------------------------
    // Window style: never steal focus, hide from Alt+Tab
    // -----------------------------------------------------------------------

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW | WS_EX_LAYERED;
            return cp;
        }
    }

    // -----------------------------------------------------------------------
    // Hotkey
    // -----------------------------------------------------------------------

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        NativeMethods.RegisterHotKey(Handle, ToggleVisibilityHotkeyId,
            NativeMethods.MOD_CONTROL | NativeMethods.MOD_ALT, (uint)Keys.B);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        RepositionWindow();
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == NativeMethods.WM_HOTKEY && m.WParam.ToInt32() == ToggleVisibilityHotkeyId)
            ToggleVisibility();
        base.WndProc(ref m);
    }

    // -----------------------------------------------------------------------
    // Poll timer (50 ms): update mouse position, monitor, and IME state
    // -----------------------------------------------------------------------

    private void PollTimer_Tick(object? sender, EventArgs e)
    {
        var newMousePos = Cursor.Position;
        var newScreen = Screen.FromPoint(newMousePos);

        bool screenChanged = _currentScreen?.DeviceName != newScreen.DeviceName;
        bool mouseMoved = newMousePos != _mousePos;

        _mousePos = newMousePos;
        _currentScreen = newScreen;

        if (screenChanged)
            RepositionWindow();

        bool newImeState = GetImeOpenStatus();
        if (newImeState != _imeOn)
        {
            _imeOn = newImeState;
            UpdateAppearance();
            UpdateTrayIcon();
        }

        if (mouseMoved)
            RefreshLayered();
    }

    // -----------------------------------------------------------------------
    // IME state detection
    // -----------------------------------------------------------------------

    private bool GetImeOpenStatus()
    {
        IntPtr foreground = NativeMethods.GetForegroundWindow();
        if (foreground == IntPtr.Zero || IsOwnWindow(foreground)) return _imeOn;

        IntPtr imeWnd = NativeMethods.ImmGetDefaultIMEWnd(foreground);
        if (imeWnd == IntPtr.Zero) return false;

        IntPtr result = NativeMethods.SendMessage(
            imeWnd,
            NativeMethods.WM_IME_CONTROL,
            (IntPtr)NativeMethods.IMC_GETOPENSTATUS,
            IntPtr.Zero);
        return result != IntPtr.Zero;
    }

    private bool IsOwnWindow(IntPtr hWnd)
    {
        NativeMethods.GetWindowThreadProcessId(hWnd, out uint pid);
        return pid == (uint)Environment.ProcessId;
    }

    // -----------------------------------------------------------------------
    // Window placement
    // -----------------------------------------------------------------------

    private void RepositionWindow()
    {
        if (_currentScreen == null) return;

        var wa = _currentScreen.WorkingArea;
        int t = Math.Max(1, _settings.BarThickness);

        Bounds = _settings.Position switch
        {
            EdgePosition.Top    => new Rectangle(wa.Left, wa.Top,          wa.Width,  t),
            EdgePosition.Bottom => new Rectangle(wa.Left, wa.Bottom - t,   wa.Width,  t),
            EdgePosition.Left   => new Rectangle(wa.Left, wa.Top,          t,         wa.Height),
            EdgePosition.Right  => new Rectangle(wa.Right - t, wa.Top,     t,         wa.Height),
            _                   => new Rectangle(wa.Left, wa.Top,          wa.Width,  t),
        };
        RefreshLayered();
    }

    private void ApplySettings()
    {
        _currentScreen = Screen.FromPoint(Cursor.Position);
        RepositionWindow();
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (_imeOn)
        {
            BackColor = Color.FromArgb(255, Color.FromArgb(_settings.ImeOnColorArgb));
            _bgOpacity = Math.Clamp(_settings.ImeOnOpacity, 0.01, 1.0);
        }
        else
        {
            BackColor = Color.FromArgb(255, Color.FromArgb(_settings.ImeOffColorArgb));
            _bgOpacity = Math.Clamp(_settings.ImeOffOpacity, 0.01, 1.0);
        }
        RefreshLayered();
    }

    // -----------------------------------------------------------------------
    // Painting: background colour is set via BackColor; OnPaint draws the arrow
    // -----------------------------------------------------------------------

    protected override void OnPaint(PaintEventArgs e) { }
    protected override void OnPaintBackground(PaintEventArgs e) { }

    /// <summary>
    /// Draws a filled triangle inside the bar at the current mouse coordinate.
    /// Arrow direction depends on the edge the bar is attached to:
    ///   Top → ↓   Bottom → ↑   Left → →   Right → ←
    /// </summary>
    private void DrawArrow(Graphics g)
    {
        if (_currentScreen == null) return;

        var wa = _currentScreen.WorkingArea;
        int w = ClientSize.Width;
        int h = ClientSize.Height;

        // Convert absolute mouse coordinate to position along the bar
        int pos = _settings.Position switch
        {
            EdgePosition.Top or EdgePosition.Bottom => _mousePos.X - wa.Left,
            _                                       => _mousePos.Y - wa.Top,
        };

        // Arrow size equals the bar's narrow dimension so it fills the cross-section
        int s = _settings.Position switch
        {
            EdgePosition.Top or EdgePosition.Bottom => h,
            _                                       => w,
        };
        int half = s / 2;

        Point[] pts = _settings.Position switch
        {
            // Top: apex at bottom, base at top → ↓
            EdgePosition.Top    => [new(pos - half, 0),  new(pos + half, 0),  new(pos, h)],
            // Bottom: apex at top, base at bottom → ↑
            EdgePosition.Bottom => [new(pos, 0),          new(pos - half, h),  new(pos + half, h)],
            // Left: apex at right, base at left → →
            EdgePosition.Left   => [new(0, pos - half),  new(0, pos + half),  new(w, pos)],
            // Right: apex at left, base at right → ←
            EdgePosition.Right  => [new(w, pos - half),  new(w, pos + half),  new(0, pos)],
            _                   => [new(pos - half, 0),  new(pos + half, 0),  new(pos, h)],
        };

        using var brush = new SolidBrush(Color.FromArgb(255, GetContrastColor(BackColor)));
        g.FillPolygon(brush, pts);
    }

    /// <summary>Returns white or near-black depending on background luminance.</summary>
    private static Color GetContrastColor(Color bg)
    {
        float luminance = (0.299f * bg.R + 0.587f * bg.G + 0.114f * bg.B) / 255f;
        return luminance > 0.45f ? Color.FromArgb(180, 0, 0, 0) : Color.FromArgb(200, 255, 255, 255);
    }

    // -----------------------------------------------------------------------
    // Layered window painting (per-pixel alpha)
    // -----------------------------------------------------------------------

    /// <summary>
    /// Repaints the window via UpdateLayeredWindow so the background respects
    /// the configured opacity while the arrow is always fully opaque (alpha = 255).
    /// </summary>
    private void RefreshLayered()
    {
        if (!IsHandleCreated || ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

        int w = ClientSize.Width;
        int h = ClientSize.Height;

        using var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            byte bgAlpha = (byte)Math.Clamp((int)(_bgOpacity * 255), 1, 255);
            using (var bgBrush = new SolidBrush(Color.FromArgb(bgAlpha, BackColor)))
                g.FillRectangle(bgBrush, 0, 0, w, h);

            DrawArrow(g);
        }

        PremultiplyAlpha(bmp);

        IntPtr screenDc  = NativeMethods.GetDC(IntPtr.Zero);
        IntPtr memDc     = NativeMethods.CreateCompatibleDC(screenDc);
        IntPtr hBitmap   = IntPtr.Zero;
        IntPtr oldBitmap = IntPtr.Zero;
        try
        {
            hBitmap  = bmp.GetHbitmap(Color.FromArgb(0));
            oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

            var pptDst = new NativeMethods.POINT { x = Left, y = Top };
            var psize  = new NativeMethods.SIZE  { cx = w, cy = h };
            var pptSrc = new NativeMethods.POINT { x = 0, y = 0 };
            var blend  = new NativeMethods.BLENDFUNCTION
            {
                BlendOp             = NativeMethods.AC_SRC_OVER,
                BlendFlags          = 0,
                SourceConstantAlpha = 255,
                AlphaFormat         = NativeMethods.AC_SRC_ALPHA,
            };

            NativeMethods.UpdateLayeredWindow(
                Handle, screenDc,
                ref pptDst, ref psize,
                memDc, ref pptSrc,
                0, ref blend, NativeMethods.ULW_ALPHA);
        }
        finally
        {
            NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
            if (hBitmap != IntPtr.Zero)
            {
                NativeMethods.SelectObject(memDc, oldBitmap);
                NativeMethods.DeleteObject(hBitmap);
            }
            NativeMethods.DeleteDC(memDc);
        }
    }

    /// <summary>
    /// Converts a 32bppArgb bitmap from straight alpha to premultiplied alpha,
    /// which is required by UpdateLayeredWindow (AC_SRC_ALPHA).
    /// </summary>
    private static void PremultiplyAlpha(Bitmap bmp)
    {
        var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        var data = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        try
        {
            int total = Math.Abs(data.Stride) * bmp.Height;
            byte[] pixels = new byte[total];
            Marshal.Copy(data.Scan0, pixels, 0, total);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte a = pixels[i + 3];
                if (a == 255) continue;
                if (a == 0)   { pixels[i] = pixels[i + 1] = pixels[i + 2] = 0; continue; }
                pixels[i]     = (byte)(pixels[i]     * a / 255);
                pixels[i + 1] = (byte)(pixels[i + 1] * a / 255);
                pixels[i + 2] = (byte)(pixels[i + 2] * a / 255);
            }

            Marshal.Copy(pixels, 0, data.Scan0, total);
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }

    // -----------------------------------------------------------------------
    // Tray icon
    // -----------------------------------------------------------------------

    private void UpdateTrayIcon()
    {
        var color = _imeOn
            ? Color.FromArgb(255, Color.FromArgb(_settings.ImeOnColorArgb))
            : Color.FromArgb(255, Color.FromArgb(_settings.ImeOffColorArgb));

        IntPtr newHandle;
        using (var bmp = new Bitmap(16, 16))
        {
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(color);
            }
            newHandle = bmp.GetHicon();
        }

        _notifyIcon.Icon = Icon.FromHandle(newHandle);
        if (_currentTrayIconHandle != IntPtr.Zero)
            NativeMethods.DestroyIcon(_currentTrayIconHandle);
        _currentTrayIconHandle = newHandle;
        _notifyIcon.Text = _imeOn ? "IME Edge Bar — IME ON" : "IME Edge Bar — IME OFF";
    }

    // -----------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------

    private void ToggleVisibility()
    {
        if (Visible)
        {
            Hide();
        }
        else
        {
            Show();
            RepositionWindow();
        }
    }

    private void OpenSettings()
    {
        if (_settingsForm != null && !_settingsForm.IsDisposed)
        {
            _settingsForm.Activate();
            return;
        }

        _settingsForm = new SettingsForm(_settings);
        if (_settingsForm.ShowDialog() == DialogResult.OK)
        {
            _settings.Position       = _settingsForm.Position;
            _settings.BarThickness   = _settingsForm.BarThickness;
            _settings.ImeOnColorArgb = _settingsForm.ImeOnColorArgb;
            _settings.ImeOnOpacity   = _settingsForm.ImeOnOpacity;
            _settings.ImeOffColorArgb = _settingsForm.ImeOffColorArgb;
            _settings.ImeOffOpacity  = _settingsForm.ImeOffOpacity;
            _settings.Save();
            ApplySettings();
            UpdateTrayIcon();
        }
        _settingsForm.Dispose();
        _settingsForm = null;
    }

    private void ExitApplication()
    {
        NativeMethods.UnregisterHotKey(Handle, ToggleVisibilityHotkeyId);
        _notifyIcon.Visible = false;
        Application.Exit();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            Hide();
            return;
        }
        base.OnFormClosing(e);
    }
}
