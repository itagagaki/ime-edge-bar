namespace ImeEdgeBar;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _pollTimer.Dispose();
            _notifyIcon.Dispose();
            if (_currentTrayIconHandle != IntPtr.Zero)
                NativeMethods.DestroyIcon(_currentTrayIconHandle);
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        SuspendLayout();

        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(200, 8);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.None;
        Name = "MainForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        Text = "IME Edge Bar";
        TopMost = true;

        ResumeLayout(false);
    }
}
