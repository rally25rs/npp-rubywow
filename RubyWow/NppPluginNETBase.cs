using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NppPluginNET
{
    public partial class PluginBase
    {
        #region " Fields "
        public string _pluginBaseName = null;
        public string _pluginModuleName = null;
        public NppData nppData;
        public FuncItems _funcItems = new FuncItems();
        #endregion

        #region " Notepad++ callbacks "
        public PluginBase(string pluginBaseName)
        {
            _pluginBaseName = pluginBaseName;
            _pluginModuleName = pluginBaseName + ".dll";
        }
        public bool __isUnicode()
        {
            return true;
        }
        public void __setInfo(NppData notpadPlusData)
        {
            nppData = notpadPlusData;
            CommandMenuInit();
        }
        public IntPtr __getFuncsArray(ref int nbF)
        {
            nbF = _funcItems.Items.Count;
            return _funcItems.NativePointer;
        }
        public uint __messageProc(uint Message, uint wParam, uint lParam)
        {
            return 1;
        }
        public string __getName()
        {
            return _pluginBaseName;
        }
        public void __beNotified(SCNotification notifyCode)
        {
            if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                PluginCleanUp();
            }
            else if (notifyCode.nmhdr.code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                SetToolBarIcon();
            }
            else if (notifyCode.nmhdr.code == (uint)SciMsg.SCN_CHARADDED)
            {
                CharAdded((char)notifyCode.ch);
            }
        }
        #endregion

        #region " Helper "
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
        }
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
        {
            SetCommand(index, commandName, functionPointer, shortcut, false);
        }
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }
        void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
        }

        public IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
        }
        #endregion
    }
}
