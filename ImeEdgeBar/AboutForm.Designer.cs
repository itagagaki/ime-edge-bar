namespace ImeEdgeBar;

partial class AboutForm
{
    private System.ComponentModel.IContainer? components = null;
    private System.Windows.Forms.Label _lblAppName;
    private System.Windows.Forms.Label _lblVersion;
    private System.Windows.Forms.LinkLabel _lnkGitHub;
    private System.Windows.Forms.Button _btnKoFi;
    private System.Windows.Forms.Button _btnOk;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _lblAppName = new System.Windows.Forms.Label();
        _lblVersion = new System.Windows.Forms.Label();
        _lnkGitHub  = new System.Windows.Forms.LinkLabel();
        _btnKoFi    = new System.Windows.Forms.Button();
        _btnOk      = new System.Windows.Forms.Button();
        SuspendLayout();

        // _lblAppName
        _lblAppName.Font      = new System.Drawing.Font(Font.FontFamily, 11f, System.Drawing.FontStyle.Bold);
        _lblAppName.Location  = new System.Drawing.Point(12, 24);
        _lblAppName.Name      = "_lblAppName";
        _lblAppName.Size      = new System.Drawing.Size(436, 28);
        _lblAppName.Text      = "IME Edge Bar";
        _lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // _lblVersion
        _lblVersion.Location  = new System.Drawing.Point(12, 60);
        _lblVersion.Name      = "_lblVersion";
        _lblVersion.Size      = new System.Drawing.Size(436, 20);
        _lblVersion.Text      = "Version";
        _lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // _lnkGitHub
        _lnkGitHub.Location  = new System.Drawing.Point(12, 88);
        _lnkGitHub.Name      = "_lnkGitHub";
        _lnkGitHub.Size      = new System.Drawing.Size(436, 24);
        _lnkGitHub.Text      = "https://github.com/itagagaki/ime-edge-bar";
        _lnkGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        _lnkGitHub.LinkClicked += _lnkGitHub_LinkClicked;

        // _btnKoFi
        _btnKoFi.Location = new System.Drawing.Point(102, 132);
        _btnKoFi.Name     = "_btnKoFi";
        _btnKoFi.Size     = new System.Drawing.Size(160, 30);
        _btnKoFi.TabIndex = 1;
        _btnKoFi.Text     = "\u2615 Buy me a coffee";
        _btnKoFi.Click   += _btnKoFi_Click;

        // _btnOk
        _btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        _btnOk.Location     = new System.Drawing.Point(278, 132);
        _btnOk.Name         = "_btnOk";
        _btnOk.Size         = new System.Drawing.Size(80, 30);
        _btnOk.TabIndex     = 0;
        _btnOk.Text         = "OK";

        // AboutForm
        AcceptButton          = _btnOk;
        AutoScaleDimensions   = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode         = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize            = new System.Drawing.Size(460, 180);
        Controls.AddRange([_lblAppName, _lblVersion, _lnkGitHub, _btnKoFi, _btnOk]);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Name            = "AboutForm";
        ShowInTaskbar   = false;
        StartPosition   = System.Windows.Forms.FormStartPosition.Manual;
        Text            = "About IME Edge Bar";
        ResumeLayout(false);
    }
}
