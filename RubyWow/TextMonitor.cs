using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace NppPluginNET
{
    /// <summary>
    /// Monitors the text being typed in to the editor.
    /// </summary>
    internal class TextMonitor
    {
        private PluginBase pluginBase;
        private Regex endsRegex;
        private string indent;
        private string eol;
        private SciMsg eolMode;
        private int lastAutoInsertLine = -1;

        internal TextMonitor(PluginBase pluginBase)
        {
            this.pluginBase = pluginBase;
            GetEOL();
            GetTabs();
            endsRegex = new Regex(@"^(\s*)(if[ \(]|unless[ \(]|class |begin|def )");
        }

        /// <summary>
        /// Determines the EOL mode currently in use, and returns a string containing the EOL sequence.
        /// This would be "\r\n" for windows or "\n" for unix.
        /// </summary>
        private void GetEOL()
        {
          eolMode = (SciMsg)pluginBase.SendScintillaMessage(SciMsg.SCI_GETEOLMODE);
          if (eolMode == SciMsg.SC_EOL_CRLF)
            this.eol = "\r\n";
          else if (eolMode == SciMsg.SC_EOL_CR)
            this.eol = "\n";
          else if (eolMode == SciMsg.SC_EOL_LF)
            this.eol = "\r";
        }

        /// <summary>
        /// Gets tab width data.
        /// </summary>
        private void GetTabs()
        {
          var useTabs = pluginBase.SendScintillaMessage(SciMsg.SCI_GETUSETABS) == 1;
          var tabWidth = pluginBase.SendScintillaMessage(SciMsg.SCI_GETTABWIDTH);
          //if (useTabs)
          //  indent = "\t";
          //else
            indent = "".PadRight(tabWidth, ' ');
        }

        /// <summary>
        /// returns true if there are more instance of 'a' than of 'b' in the string 'line'.
        /// i.e.: return (a.count > b.count)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool AreMoreOf(char a, char b, string line)
        {
          if (line == null)
            return false;

          int open = 0, close = 0;
          foreach (var lc in line.ToCharArray())
          {
            if (lc == a)
              open++;
            else if (lc == b)
              close++;
          }
          return (open > close);
        }

        internal void CharAdded(char c)
        {
          try
          {
            // Add 'end' after certain statements
            if (((eolMode == SciMsg.SC_EOL_CR || eolMode == SciMsg.SC_EOL_CRLF) && c == '\n') || (eolMode == SciMsg.SC_EOL_LF && c == '\r'))
            {
              var curLineBuilder = new StringBuilder(512); //TODO: get line length from Scintilla
              var lineNum = (int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0);
              Win32.SendMessage(pluginBase.GetCurrentScintilla(), SciMsg.SCI_GETLINE, lineNum - 1, curLineBuilder);
              var curLine = curLineBuilder.ToString();

              var match = endsRegex.Match(curLine);
              if (match.Success)
              {
                lastAutoInsertLine = -1;
                Win32.SendMessage(pluginBase.GetCurrentScintilla(), SciMsg.SCI_INSERTTEXT, -1, string.Format("{0}{1}end", eol, match.Groups[1].Value));
                pluginBase.SendScintillaMessage(SciMsg.SCI_TAB);
              }
            }
            // add closing array bracket
            else if (c == '[')
            {
              var curLineBuilder = new StringBuilder(512); //TODO: get line length from Scintilla
              var lineNum = (int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0);
              var linePos = pluginBase.SendScintillaMessage(SciMsg.SCI_POSITIONFROMLINE, lineNum);
              Win32.SendMessage(pluginBase.GetCurrentScintilla(), SciMsg.SCI_GETLINE, lineNum, curLineBuilder);
              var curLine = curLineBuilder.ToString();
              var curPos = pluginBase.SendScintillaMessage(SciMsg.SCI_GETCURRENTPOS);

              if (AreMoreOf('[', ']', curLine)) // if there are more opening brackets than closing ones
              {
                var posInLine = curPos - linePos;
                if (posInLine == curLine.Length - eol.Length || (posInLine < curLine.Length && curLine[posInLine] == ' ')) // only add if caret is left of whitespace, or at EOL
                {
                  lastAutoInsertLine = lineNum;
                  Win32.SendMessage(pluginBase.GetCurrentScintilla(), SciMsg.SCI_INSERTTEXT, -1, "]");
                }
              }
            }
            // ignore closing array bracket if we already auto-inserted it
            else if (c == ']')
            {
              var curLineBuilder = new StringBuilder(512); //TODO: get line length from Scintilla
              var lineNum = (int)Win32.SendMessage(pluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0);
              if (lastAutoInsertLine != lineNum)
                return;
              var linePos = pluginBase.SendScintillaMessage(SciMsg.SCI_POSITIONFROMLINE, lineNum);
              Win32.SendMessage(pluginBase.GetCurrentScintilla(), SciMsg.SCI_GETLINE, lineNum, curLineBuilder);
              var curLine = curLineBuilder.ToString();
              var curPos = pluginBase.SendScintillaMessage(SciMsg.SCI_GETCURRENTPOS);

              if (AreMoreOf(']', '[', curLine)) // if there are more closing brackets than opening ones
              {
                var posInLine = curPos - linePos;
                if (posInLine < curLine.Length && curLine[posInLine] == ']') // only add if caret is left of whitespace, or at EOL
                {
                  pluginBase.SendScintillaMessage(SciMsg.SCI_CHARRIGHT);
                  pluginBase.SendScintillaMessage(SciMsg.SCI_DELETEBACK);
                }
              }
            }
          }
          catch (Exception e)
          {
            MessageBox.Show("Error while handling character key press: " + e.Message + "\r\n" + e.StackTrace, "Ruby. Boo!");
          }
        }
    }
}
