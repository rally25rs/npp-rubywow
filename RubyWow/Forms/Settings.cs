using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NppPluginNET.Forms
{
  public partial class Settings : Form
  {
    PluginBase pluginBase;

    public Settings(PluginBase plgBase)
    {
      pluginBase = plgBase;
      InitializeComponent();
    }

    private void cmdRubyBrowse_Click(object sender, EventArgs e)
    {
        folderBrowserDialog1.SelectedPath = txtRuby.Text;
        if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            txtRuby.Text = folderBrowserDialog1.SelectedPath;
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void cmdSave_Click(object sender, EventArgs e)
    {
        pluginBase.settings.RubyPath = txtRuby.Text;
        pluginBase.SaveSettings();
        Close();
    }

    private void Settings_Load(object sender, EventArgs e)
    {
        txtRuby.Text = pluginBase.settings.RubyPath;
    }
  }
}
