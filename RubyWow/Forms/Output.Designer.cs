namespace NppPluginNET.Forms
{
  partial class Output
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
      this.txtOutput = new System.Windows.Forms.TextBox();
      this.pnlButtons = new System.Windows.Forms.Panel();
      this.cmdKill = new System.Windows.Forms.Button();
      this.processRunner = new System.ComponentModel.BackgroundWorker();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      // 
      // txtOutput
      // 
      this.txtOutput.BackColor = System.Drawing.SystemColors.Window;
      this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtOutput.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtOutput.Location = new System.Drawing.Point(0, 30);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ReadOnly = true;
      this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtOutput.Size = new System.Drawing.Size(582, 232);
      this.txtOutput.TabIndex = 0;
      this.txtOutput.WordWrap = false;
      this.txtOutput.DoubleClick += new System.EventHandler(this.txtOutput_DoubleClick);
      // 
      // pnlButtons
      // 
      this.pnlButtons.Controls.Add(this.cmdKill);
      this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlButtons.Location = new System.Drawing.Point(0, 0);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new System.Drawing.Size(582, 30);
      this.pnlButtons.TabIndex = 1;
      // 
      // cmdKill
      // 
      this.cmdKill.Enabled = false;
      this.cmdKill.Location = new System.Drawing.Point(3, 4);
      this.cmdKill.Name = "cmdKill";
      this.cmdKill.Size = new System.Drawing.Size(75, 23);
      this.cmdKill.TabIndex = 0;
      this.cmdKill.Text = "Kill";
      this.cmdKill.UseVisualStyleBackColor = true;
      this.cmdKill.Click += new System.EventHandler(this.cmdKill_Click);
      // 
      // processRunner
      // 
      this.processRunner.WorkerSupportsCancellation = true;
      this.processRunner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.processRunner_DoWork);
      this.processRunner.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.processRunner_RunWorkerCompleted);
      // 
      // Output
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(582, 262);
      this.Controls.Add(this.txtOutput);
      this.Controls.Add(this.pnlButtons);
      this.Name = "Output";
      this.Text = "Output";
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtOutput;
    private System.Windows.Forms.Panel pnlButtons;
    private System.Windows.Forms.Button cmdKill;
    private System.ComponentModel.BackgroundWorker processRunner;
  }
}