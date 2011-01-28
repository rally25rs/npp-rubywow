#include "NppIncludes\PluginInterface.h"
#include "WrapperUnmanaged.h"
#include "WrapperManaged.h"

void W_Init()
{
	NppPluginNET::WrapperManaged::Init();
	NppPluginNET::WrapperManaged::CreatePluginInstance();
}

void W_setInfo(NppData notepadPlusData)
{
	NppPluginNET::NppData _notepadPlusData;
	_notepadPlusData._nppHandle = (IntPtr)(void*)notepadPlusData._nppHandle;
	_notepadPlusData._scintillaMainHandle = (IntPtr)(void*)notepadPlusData._scintillaMainHandle;
	_notepadPlusData._scintillaSecondHandle = (IntPtr)(void*)notepadPlusData._scintillaSecondHandle;

	NppPluginNET::WrapperManaged::_pluginBase->__setInfo(_notepadPlusData);
}
FuncItem * W_getFuncsArray(int *nbF)
{
	return (FuncItem *)NppPluginNET::WrapperManaged::_pluginBase->__getFuncsArray(*nbF).ToPointer();
}
LRESULT W_messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	return NppPluginNET::WrapperManaged::_pluginBase->__messageProc(Message, wParam, lParam);
}
void W_beNotified(SCNotification *notifyCode)
{
	NppPluginNET::SCNotification _notifyCode;
    NppPluginNET::Sci_NotifyHeader _nmhdr;
    _nmhdr.hwndFrom = (IntPtr)notifyCode->nmhdr.hwndFrom;
    _nmhdr.idFrom = notifyCode->nmhdr.idFrom;
    _nmhdr.code = notifyCode->nmhdr.code;
    _notifyCode.nmhdr = _nmhdr;
	_notifyCode.position = notifyCode->position;
	_notifyCode.ch = notifyCode->ch;
	_notifyCode.modifiers = notifyCode->modifiers;
	_notifyCode.modificationType = notifyCode->modificationType;
	_notifyCode.text = System::Runtime::InteropServices::Marshal::PtrToStringAnsi((IntPtr)(void*)notifyCode->text);
	_notifyCode.length = notifyCode->length;
	_notifyCode.linesAdded = notifyCode->linesAdded;
	_notifyCode.message = notifyCode->message;
	_notifyCode.wParam = notifyCode->wParam;
	_notifyCode.lParam = notifyCode->lParam;
	_notifyCode.line = notifyCode->line;
	_notifyCode.foldLevelNow = notifyCode->foldLevelNow;
	_notifyCode.foldLevelPrev = notifyCode->foldLevelPrev;
	_notifyCode.margin = notifyCode->margin;
	_notifyCode.listType = notifyCode->listType;
	_notifyCode.x = notifyCode->x;
	_notifyCode.y = notifyCode->y;
	_notifyCode.token = notifyCode->token;
	_notifyCode.annotationLinesAdded = notifyCode->annotationLinesAdded;
	NppPluginNET::WrapperManaged::_pluginBase->__beNotified(_notifyCode);
}
