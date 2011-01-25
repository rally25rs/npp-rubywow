using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NppPluginNET.Forms
{
  public partial class Output : Form
  {
    PluginBase pluginBase;
    public delegate void AppendStringDelegate(string text);
    private Regex newlineRegex = new Regex(@"[^\r]\n");
    private Regex rubyFilePositionRegex = new Regex(@"\[(\w+\.rb)\:(\d+)\]");
    private string lastWorkingDir = "";

    public Output(PluginBase plgBase)
    {
      pluginBase = plgBase;
      InitializeComponent();
    }

    public bool Running
    {
      get { return processRunner.IsBusy; }
    }

    public void RunCommand(string workingDir, string cmd, string args)
    {
      try
      {
        if (Running)
        {
          MessageBox.Show("Another command is already running.");
          return;
        }

        lastWorkingDir = workingDir;
        cmdKill.Enabled = true;
        txtOutput.Text = string.Format("{0}> {1} {2}{3}", workingDir, cmd, args, Environment.NewLine);

        processRunner.RunWorkerAsync(new string[] { workingDir, cmd, args });
      }
      catch(Exception ex)
      {
        txtOutput.Text += string.Format("{2}*** Error: {0}{2}{1}", ex.Message, ex.StackTrace, Environment.NewLine);
        cmdKill.Enabled = false;
      }
    }

    void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      AppendText(e.Data + Environment.NewLine);
    }

    private void cmdKill_Click(object sender, EventArgs e)
    {
      if (Running && DialogResult.Yes == MessageBox.Show(this, string.Format("Killing a running process might lead to data corruption or other issues. Are you sure you want to kill it?"), "Kill this process?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
      {
        try
        {
          processRunner.CancelAsync();
        }
        catch
        {
          // ignore
        }
        finally
        {
        }
      }
      cmdKill.Enabled = false;
    }

    private void processRunner_DoWork(object sender, DoWorkEventArgs e)
    {
      var args = (string[])e.Argument;
      var ps = new ProcessStartInfo(args[1], args[2]);
      ps.WorkingDirectory = args[0];
      ps.UseShellExecute = false;
      ps.CreateNoWindow = true;
      ps.RedirectStandardOutput = true;
      ps.RedirectStandardError = true;

      var process = new Process();
      process.StartInfo = ps;
      process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
      process.ErrorDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
      //AppendText("...starting...");
      process.Start();
      //AppendText("...waiting...");
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();
      //string output = process.StandardOutput.ReadToEnd();
      //AppendText(output);
      //AppendText("...done...");
      process.WaitForExit();
      process.Close();
    }

    private void processRunner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
        AppendText(string.Format("{2}*** Error: {0}{2}{1}{2}", e.Error.Message, e.Error.StackTrace, Environment.NewLine));
      cmdKill.Enabled = false;
    }

    private void AppendText(string text)
    {
      if (text == null)
        text = "\r\n";
      else
        text = newlineRegex.Replace(text, "\r\n");
      if (txtOutput.InvokeRequired)
      {
        txtOutput.Invoke(new AppendStringDelegate(AppendText), text);
      }
      else
      {
        txtOutput.AppendText(text);
        txtOutput.SelectionStart = txtOutput.Text.Length;
        txtOutput.ScrollToCaret();
      }
    }

    private void txtOutput_DoubleClick(object sender, EventArgs e)
    {
      if (txtOutput.SelectionStart > 0 && txtOutput.Text.Length > 0)
      {
        // figure out the line of text the cursor is on.
        string line = null;
        using(var reader = new StringReader(txtOutput.Text))
        {
          var charsRead = 0;
          while ((line = reader.ReadLine()) != null)
          {
            charsRead += line.Length+2; // +2 accounts for "\r\n" at end of line.
            if (charsRead > txtOutput.SelectionStart)
              break;
          }
          reader.Close();
        }

        if (line == null)
          return;

        // parse line for any goodies
        var match = rubyFilePositionRegex.Match(line);
        if (match.Success)
          OpenFileToLine(match.Groups[1].Value, match.Groups[2].Value);
      }
    }

    private void OpenFileToLine(string fileName, string lineNumberStr)
    {
      // ruby reports a 1-based line number, and np++ uses 0-based
      int lineNumber;
      if (Int32.TryParse(lineNumberStr, out lineNumber))
        lineNumber--;
      else
        lineNumber = 0;

      var fi = new FileInfo(fileName);
      if (!fi.Exists)
      {
        fi = new FileInfo(lastWorkingDir + "\\" + fileName);
        if (!fi.Exists)
        {
          MessageBox.Show("Unable to open file " + fi.FullName, "Ruby. Boo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
      }

      StringBuilder path = new StringBuilder(fi.FullName);
      var success = (int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_DOOPEN, 0, path);
      if (success == 0)
      {
        MessageBox.Show("Notepad++ was unable to open file " + fi.FullName, "Ruby. Boo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      // move focus to the correct line
      IntPtr curScintilla = pluginBase.GetCurrentScintilla();
      Win32.SendMessage(curScintilla, SciMsg.SCI_ENSUREVISIBLE, lineNumber, 0);
      Win32.SendMessage(curScintilla, SciMsg.SCI_GOTOLINE, lineNumber, 0);
      Win32.SendMessage(curScintilla, SciMsg.SCI_GRABFOCUS, 0, 0);
    }
  }
}
