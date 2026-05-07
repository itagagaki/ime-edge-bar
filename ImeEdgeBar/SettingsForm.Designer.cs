namespace ImeEdgeBar;

partial class SettingsForm
{
    private System.ComponentModel.IContainer? components = null;

    // Position
    private GroupBox _grpPosition;
    private RadioButton _rdoTop, _rdoBottom, _rdoLeft, _rdoRight;

    // Bar size
    private GroupBox _grpSize;
    private Label _lblThickness;
    private NumericUpDown _numThickness;
    private Label _lblThicknessUnit;

    // IME ON
    private GroupBox _grpImeOn;
    private Label _lblImeOnColor;
    private Panel _pnlImeOnColor;
    private Button _btnImeOnColor;
    private Label _lblImeOnOpacity;
    private TrackBar _trkImeOnOpacity;
    private Label _lblImeOnOpacityValue;

    // IME OFF
    private GroupBox _grpImeOff;
    private Label _lblImeOffColor;
    private Panel _pnlImeOffColor;
    private Button _btnImeOffColor;
    private Label _lblImeOffOpacity;
    private TrackBar _trkImeOffOpacity;
    private Label _lblImeOffOpacityValue;

    // Mouse pointer indicator
    private CheckBox _chkShowMousePointerIndicator;

    // Buttons
    private Button _btnOk;
    private Button _btnCancel;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _grpPosition         = new GroupBox();
        _rdoTop              = new RadioButton();
        _rdoBottom           = new RadioButton();
        _rdoLeft             = new RadioButton();
        _rdoRight            = new RadioButton();
        _grpSize             = new GroupBox();
        _lblThickness        = new Label();
        _numThickness        = new NumericUpDown();
        _lblThicknessUnit    = new Label();
        _grpImeOn            = new GroupBox();
        _lblImeOnColor       = new Label();
        _pnlImeOnColor       = new Panel();
        _btnImeOnColor       = new Button();
        _lblImeOnOpacity     = new Label();
        _trkImeOnOpacity     = new TrackBar();
        _lblImeOnOpacityValue = new Label();
        _grpImeOff           = new GroupBox();
        _lblImeOffColor      = new Label();
        _pnlImeOffColor      = new Panel();
        _btnImeOffColor      = new Button();
        _lblImeOffOpacity    = new Label();
        _trkImeOffOpacity    = new TrackBar();
        _lblImeOffOpacityValue = new Label();
        _chkShowMousePointerIndicator = new CheckBox();
        _btnOk               = new Button();
        _btnCancel           = new Button();

        _grpPosition.SuspendLayout();
        _grpSize.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_numThickness).BeginInit();
        _grpImeOn.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_trkImeOnOpacity).BeginInit();
        _grpImeOff.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_trkImeOffOpacity).BeginInit();
        SuspendLayout();

        // ── _grpPosition ────────────────────────────────────────────────
        _grpPosition.Controls.AddRange([_rdoTop, _rdoBottom, _rdoLeft, _rdoRight]);
        _grpPosition.Location = new Point(12, 8);
        _grpPosition.Size     = new Size(368, 60);
        _grpPosition.Text     = "表示位置";

        _rdoTop.Location    = new Point(12, 26);  _rdoTop.Size    = new Size(68, 20); _rdoTop.Text    = "上辺";
        _rdoBottom.Location = new Point(92, 26);  _rdoBottom.Size = new Size(68, 20); _rdoBottom.Text = "下辺";
        _rdoLeft.Location   = new Point(172, 26); _rdoLeft.Size   = new Size(68, 20); _rdoLeft.Text   = "左辺";
        _rdoRight.Location  = new Point(252, 26); _rdoRight.Size  = new Size(68, 20); _rdoRight.Text  = "右辺";

        // ── _grpSize ─────────────────────────────────────────────────────
        _grpSize.Controls.AddRange([_lblThickness, _numThickness, _lblThicknessUnit]);
        _grpSize.Location = new Point(12, 80);
        _grpSize.Size     = new Size(368, 52);
        _grpSize.Text     = "バーの幅";

        _lblThickness.Location  = new Point(12, 20); _lblThickness.Size  = new Size(34, 17); _lblThickness.Text = "幅：";
        _numThickness.Location  = new Point(50, 17); _numThickness.Size  = new Size(56, 23);
        _numThickness.Minimum   = 4; _numThickness.Maximum = 30;
        _lblThicknessUnit.Location = new Point(114, 20); _lblThicknessUnit.Size = new Size(160, 17);
        _lblThicknessUnit.Text  = "ピクセル　(4〜30)";

        // ── _grpImeOn ────────────────────────────────────────────────────
        _grpImeOn.Controls.AddRange([
            _lblImeOnColor, _pnlImeOnColor, _btnImeOnColor,
            _lblImeOnOpacity, _trkImeOnOpacity, _lblImeOnOpacityValue]);
        _grpImeOn.Location = new Point(12, 144);
        _grpImeOn.Size     = new Size(368, 90);
        _grpImeOn.Text     = "IME ON のとき";

        _lblImeOnColor.Location  = new Point(12, 28); _lblImeOnColor.Size  = new Size(34, 17); _lblImeOnColor.Text  = "色：";
        _pnlImeOnColor.Location  = new Point(50, 24); _pnlImeOnColor.Size  = new Size(42, 22);
        _pnlImeOnColor.BorderStyle = BorderStyle.FixedSingle;
        _btnImeOnColor.Location  = new Point(100, 22); _btnImeOnColor.Size  = new Size(72, 26); _btnImeOnColor.Text  = "選択...";

        _lblImeOnOpacity.Location = new Point(12, 62); _lblImeOnOpacity.Size = new Size(72, 17); _lblImeOnOpacity.Text = "不透明度：";
        _trkImeOnOpacity.Location = new Point(88, 52); _trkImeOnOpacity.Size = new Size(236, 30);
        _trkImeOnOpacity.Minimum  = 0; _trkImeOnOpacity.Maximum = 100; _trkImeOnOpacity.TickFrequency = 10;
        _lblImeOnOpacityValue.Location  = new Point(330, 62); _lblImeOnOpacityValue.Size  = new Size(34, 17);
        _lblImeOnOpacityValue.TextAlign = ContentAlignment.MiddleRight;

        // ── _grpImeOff ───────────────────────────────────────────────────
        _grpImeOff.Controls.AddRange([
            _lblImeOffColor, _pnlImeOffColor, _btnImeOffColor,
            _lblImeOffOpacity, _trkImeOffOpacity, _lblImeOffOpacityValue]);
        _grpImeOff.Location = new Point(12, 246);
        _grpImeOff.Size     = new Size(368, 90);
        _grpImeOff.Text     = "IME OFF のとき";

        _lblImeOffColor.Location  = new Point(12, 28); _lblImeOffColor.Size  = new Size(34, 17); _lblImeOffColor.Text  = "色：";
        _pnlImeOffColor.Location  = new Point(50, 24); _pnlImeOffColor.Size  = new Size(42, 22);
        _pnlImeOffColor.BorderStyle = BorderStyle.FixedSingle;
        _btnImeOffColor.Location  = new Point(100, 22); _btnImeOffColor.Size  = new Size(72, 26); _btnImeOffColor.Text  = "選択...";

        _lblImeOffOpacity.Location = new Point(12, 62); _lblImeOffOpacity.Size = new Size(72, 17); _lblImeOffOpacity.Text = "不透明度：";
        _trkImeOffOpacity.Location = new Point(88, 52); _trkImeOffOpacity.Size = new Size(236, 30);
        _trkImeOffOpacity.Minimum  = 0; _trkImeOffOpacity.Maximum = 100; _trkImeOffOpacity.TickFrequency = 10;
        _lblImeOffOpacityValue.Location  = new Point(330, 62); _lblImeOffOpacityValue.Size  = new Size(34, 17);
        _lblImeOffOpacityValue.TextAlign = ContentAlignment.MiddleRight;

        // ── Mouse pointer indicator ───────────────────────────────────────
        _chkShowMousePointerIndicator.Location = new Point(20, 352);
        _chkShowMousePointerIndicator.Size     = new Size(240, 20);
        _chkShowMousePointerIndicator.Text     = "マウスポインター位置を表示する";

        // ── Buttons ──────────────────────────────────────────────────────
        _btnOk.Location     = new Point(222, 382); _btnOk.Size     = new Size(72, 26);
        _btnOk.Text         = "OK";               _btnOk.DialogResult = DialogResult.OK;
        _btnCancel.Location = new Point(306, 382); _btnCancel.Size = new Size(72, 26);
        _btnCancel.Text     = "キャンセル";        _btnCancel.DialogResult = DialogResult.Cancel;

        // ── Form ─────────────────────────────────────────────────────────
        AcceptButton = _btnOk;
        CancelButton = _btnCancel;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize  = new Size(392, 422);
        Controls.AddRange([_grpPosition, _grpSize, _grpImeOn, _grpImeOff, _chkShowMousePointerIndicator, _btnOk, _btnCancel]);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        ShowInTaskbar   = false;
        StartPosition   = FormStartPosition.Manual;
        Text            = "IME Edge Bar — 設定";

        _grpPosition.ResumeLayout(false);
        _grpSize.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_numThickness).EndInit();
        _grpImeOn.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_trkImeOnOpacity).EndInit();
        _grpImeOff.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_trkImeOffOpacity).EndInit();
        ResumeLayout(false);
    }
}
