namespace NppPluginNET.Forms
{
  partial class Settings
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.label1 = new System.Windows.Forms.Label();
        this.txtRuby = new System.Windows.Forms.TextBox();
        this.cmdRubyBrowse = new System.Windows.Forms.Button();
        this.panel1 = new System.Windows.Forms.Panel();
        this.cmdCancel = new System.Windows.Forms.Button();
        this.cmdSave = new System.Windows.Forms.Button();
        this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 10);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(92, 13);
        this.label1.TabIndex = 0;
        this.label1.Text = "Path to Ruby.exe:";
        // 
        // txtRuby
        // 
        this.txtRuby.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.txtRuby.Location = new System.Drawing.Point(110, 8);
        this.txtRuby.Name = "txtRuby";
        this.txtRuby.Size = new System.Drawing.Size(210, 20);
        this.txtRuby.TabIndex = 1;
        // 
        // cmdRubyBrowse
        // 
        this.cmdRubyBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.cmdRubyBrowse.Location = new System.Drawing.Point(326, 7);
        this.cmdRubyBrowse.Name = "cmdRubyBrowse";
        this.cmdRubyBrowse.Size = new System.Drawing.Size(75, 23);
        this.cmdRubyBrowse.TabIndex = 2;
        this.cmdRubyBrowse.Text = "Browse...";
        this.cmdRubyBrowse.UseVisualStyleBackColor = true;
        this.cmdRubyBrowse.Click += new System.EventHandler(this.cmdRubyBrowse_Click);
        // 
        // panel1
        // 
        this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
        this.panel1.Controls.Add(this.cmdCancel);
        this.panel1.Controls.Add(this.cmdSave);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel1.Location = new System.Drawing.Point(0, 232);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(413, 30);
        this.panel1.TabIndex = 3;
        // 
        // cmdCancel
        // 
        this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.cmdCancel.Location = new System.Drawing.Point(244, 4);
        this.cmdCancel.Name = "cmdCancel";
        this.cmdCancel.Size = new System.Drawing.Size(75, 23);
        this.cmdCancel.TabIndex = 1;
        this.cmdCancel.Text = "Cancel";
        this.cmdCancel.UseVisualStyleBackColor = true;
        this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
        // 
        // cmdSave
        // 
        this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.cmdSave.Location = new System.Drawing.Point(326, 3);
        this.cmdSave.Name = "cmdSave";
        this.cmdSave.Size = new System.Drawing.Size(75, 23);
        this.cmdSave.TabIndex = 0;
        this.cmdSave.Text = "Save";
        this.cmdSave.UseVisualStyleBackColor = true;
        this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
        // 
        // folderBrowserDialog1
        // 
        this.folderBrowserDialog1.Description = "Path to Ruby.exe";
        // 
        // Settings
        // 
        this.AcceptButton = this.cmdSave;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.cmdCancel;
        this.ClientSize = new System.Drawing.Size(413, 262);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.cmdRubyBrowse);
        this.Controls.Add(this.txtRuby);
        this.Controls.Add(this.label1);
        this.Name = "Settings";
        this.Text = "Ruby. Wow! (Settings)";
        this.Load += new System.EventHandler(this.Settings_Load);
        this.panel1.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtRuby;
    private System.Windows.Forms.Button cmdRubyBrowse;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button cmdCancel;
    private System.Windows.Forms.Button cmdSave;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
  }
}