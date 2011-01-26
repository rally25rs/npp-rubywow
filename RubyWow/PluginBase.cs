using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace NppPluginNET
{
  public partial class PluginBase
  {
    private TextMonitor textMonitor = null;
    private ScriptRunner scriptRunner = null;
    internal Settings settings = null;
    internal Forms.Output frmOutput = null;

    #region " Fields "
    int idOutputDlg = -1;
    // Bitmap tbBmp = null;
    #endregion

    #region " StartUp/CleanUp "
    void CommandMenuInit()
    {
        if(textMonitor == null)
            textMonitor = new TextMonitor(this);
        if (scriptRunner == null)
            scriptRunner = new ScriptRunner(this);
        if (settings == null)
            settings = LoadSettings();

      SetCommand(0, "Run Ruby Script(This File)", runTestClass, new ShortcutKey(true, false, true, Keys.Y));
      SetCommand(1, "Run Test (Single Method)", runTestMethod, new ShortcutKey(true, false, true, Keys.T));
      SetCommand(2, "---", null);
      SetCommand(3, "Toggle Output Window", toggleOutputDialog, new ShortcutKey(true, false, true, Keys.R)); idOutputDlg = 1;
      SetCommand(4, "Toggle Auto-Insert 'end's", toggleInsertEnds, settings.InsertEnds); idOutputDlg = 1;
      SetCommand(5, "---", null);
      SetCommand(6, "Settings...", showSettings);
    }
    void SetToolBarIcon()
    {
      // toolbarIcons tbIcons = new toolbarIcons();
      // tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
      // IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
      // Marshal.StructureToPtr(tbIcons, pTbIcons, false);
      // Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[idMyDlg]._cmdID, pTbIcons);
      // Marshal.FreeHGlobal(pTbIcons);
    }
    void PluginCleanUp()
    {
        if (settings != null)
            SaveSettings();
    }
    void CharAdded(char c)
    {
        if (IsRubyFile())
        {
            textMonitor.CharAdded(c);
        }
    }
    #endregion

    #region " Menu functions "
    internal void showSettings()
    {
        var dlgSettings = new Forms.Settings(this);
        dlgSettings.ShowDialog();
    }
    void runTestMethod()
    {
      try
      {
        // make sure the file is saved first
        Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SAVECURRENTFILE, 0, 0);

        // now get the file path and the current line number
        StringBuilder path = new StringBuilder(Win32.MAX_PATH);
        Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, path);
        var lineNum = (int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0);

        if (!path.ToString().StartsWith("new") && !IsRubyFile())
        {
          MessageBox.Show("This is only available for Ruby files. Please save the file as .rb, or set the Language in Notepad++ to \"Ruby\".", "Ruby. Yay!");
          return;
        }

        var fi = new FileInfo(path.ToString());
        if (!fi.Exists)
        {
          //MessageBox.Show(string.Format("Unable to open the file: {0}", path));
          return;
        }

        // read all the lines up to the current one into a buffer
        var lines = new string[lineNum + 1];
        using(var reader = new StreamReader(fi.OpenRead()))
        {
          for (int i = lineNum; i >= 0 && !reader.EndOfStream; i--)
            lines[i] = reader.ReadLine();
          reader.Close();
        }

        // now start at the current line and go backwards until we find the method name for this test.
        var testMethodRegex = new Regex(@"^\s*def\s*(test_[A-Za-z0-9_\-\!\?]+)");
        var railsTestRegex = new Regex(@"^\s*test\s+[\""\'](*+)[\""\']\s+do");
        var shouldaTestRegex = new Regex(@"^\s*should\s+[\""\'](*+)[\""\']\s+do");
        var asRegex = false;
        string testMethodName = null;
        for (int i = 0; i < lines.Length; i++)
        {
          // TestUnit
          var match = testMethodRegex.Match(lines[i]);
          if (match.Success)
          {
            testMethodName = match.Groups[1].Value;
            break;
          }
          // Rails
          match = railsTestRegex.Match(lines[i]);
          if (match.Success)
          {
            testMethodName = match.Groups[1].Value.Replace(' ', '_');
            asRegex = true;
            break;
          }
          // Shoulda
          match = shouldaTestRegex.Match(lines[i]);
          if (match.Success)
          {
              testMethodName = "should " + match.Groups[1].Value;
              asRegex = true;
              break;
          }
        }

        if (testMethodName == null)
        {
          MessageBox.Show("Unable to determine test method name.");
          return;
        }

        // so far so good.. now run it.
        scriptRunner.RunMethod(fi, testMethodName, asRegex);
      }
      catch (Exception ex)
      {
        MessageBox.Show(null, "Error: " + ex.Message + ex.StackTrace, "Ruby. Boo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    void runTestClass()
    {
      try
      {
        // make sure the file is saved first
        Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SAVECURRENTFILE, 0, 0);

        // now get the file path
        StringBuilder path = new StringBuilder(Win32.MAX_PATH);
        Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, path);

        if (!path.ToString().StartsWith("new") && !IsRubyFile())
        {
          MessageBox.Show("This is only available for Ruby files. Please save the file as .rb, or set the Language in Notepad++ to \"Ruby\".", "Ruby. Wow!");
          return;
        }

        var fi = new FileInfo(path.ToString());
        if (!fi.Exists)
        {
          //MessageBox.Show(string.Format("Unable to open the file: {0}", path));
          return;
        }

        scriptRunner.RunScript(fi);
      }
      catch (Exception ex)
      {
        MessageBox.Show(null, "Error: " + ex.Message + ex.StackTrace, "Ruby. Boo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    internal void showOutputDialog()
    {
      if (frmOutput == null)
        toggleOutputDialog();
      else if(!frmOutput.Visible)
        Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, frmOutput.Handle);
    }

    void toggleInsertEnds()
    {
        settings.InsertEnds = !settings.InsertEnds;

        int i = Win32.CheckMenuItem(Win32.GetMenu(nppData._nppHandle), _funcItems.Items[4]._cmdID,
            Win32.MF_BYCOMMAND | (settings.InsertEnds ? Win32.MF_CHECKED : Win32.MF_UNCHECKED));
    }

     void toggleOutputDialog()
     {
       if (frmOutput == null)
       {
         frmOutput = new Forms.Output(this);

         var _nppTbData = new NppTbData();
         _nppTbData.hClient = frmOutput.Handle;
         _nppTbData.pszName = "Ruby. Wow! (Output)";
         _nppTbData.dlgID = idOutputDlg;
         _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_BOTTOM;
         _nppTbData.pszModuleName = _pluginModuleName;
         IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
         Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

         Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
       }
       else
       {
         if(frmOutput.Visible)
           Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, frmOutput.Handle);
         else
           Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, frmOutput.Handle);
       }
     }
    #endregion

    private bool IsRubyFile()
    {
      LangType docType = LangType.L_TXT;
      Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTLANGTYPE, 0, ref docType);
      return (docType == LangType.L_RUBY);
    }

    #region Scintilla messaging convenience methods.
    public int SendScintillaMessage(SciMsg msg)
    {
      return SendScintillaMessage(msg, 0);
    }

    public int SendScintillaMessage(SciMsg msg, int lparam)
    {
      return SendScintillaMessage(msg, lparam, 0);
    }

    public int SendScintillaMessage(SciMsg msg, int lparam, int rparam)
    {
      return (int)Win32.SendMessage(GetCurrentScintilla(), msg, lparam, rparam);
    }
    #endregion

    #region Load and Save Settings

    private Settings LoadSettings()
    {
        var settings = new Settings();
        try
        {
            var iniFilePath = GetSettingsFile();
            var returnString = new StringBuilder(Win32.MAX_PATH);

            Win32.GetPrivateProfileString("RubyWow", "RubyPath", "", returnString, Win32.MAX_PATH, iniFilePath);
            settings.RubyPath = returnString.ToString();

            Win32.GetPrivateProfileString("RubyWow", "InsertEnds", "1", returnString, Win32.MAX_PATH, iniFilePath);
            settings.InsertEnds = returnString.ToString() == "1";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading settings. " + ex.Message, "Ruby. Boo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return settings;
    }

    internal void SaveSettings()
    {
        try
        {
            var iniFilePath = GetSettingsFile();

            Win32.WritePrivateProfileString("RubyWow", "RubyPath", settings.RubyPath, iniFilePath);
            Win32.WritePrivateProfileString("RubyWow", "InsertEnds", settings.InsertEnds ? "1" : "0", iniFilePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error saving settings. " + ex.Message, "Ruby. Boo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string GetSettingsFile()
    {
        StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
        Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
        var iniFilePath = sbIniFilePath.ToString();
        if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
        iniFilePath = Path.Combine(iniFilePath, _pluginBaseName + ".ini");
        return iniFilePath;
    }

    #endregion
  }
}