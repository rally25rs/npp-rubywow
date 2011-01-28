#pragma unmanaged
//#define UNICODE
#include "NppIncludes\PluginInterface.h"
#include "WrapperUnmanaged.h"

const TCHAR NPP_PLUGIN_NAME[] = TEXT("RubyWow");
bool initialized = false;

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  reasonForCall, 
                       LPVOID lpReserved )
{
    switch (reasonForCall)
    {
      case DLL_PROCESS_ATTACH:
        //W_Init();
        break;

      case DLL_PROCESS_DETACH:
        break;

      case DLL_THREAD_ATTACH:
        break;

      case DLL_THREAD_DETACH:
        break;
    }

    return TRUE;
}

extern "C" __declspec(dllexport) void Init()
{
	W_Init();
}

extern "C" __declspec(dllexport) void setInfo(NppData notpadPlusData)
{
	if(!initialized)
	{
		W_Init();
		initialized = true;
	}
	W_setInfo(notpadPlusData);
}

//extern "C" __declspec(dllexport) void Init()
//{
//	W_Init();
//}

extern "C" __declspec(dllexport) const TCHAR * getName()
{
	if(!initialized)
	{
		W_Init();
		initialized = true;
	}
	return NPP_PLUGIN_NAME;
}

extern "C" __declspec(dllexport) FuncItem * getFuncsArray(int *nbF)
{
	if(!initialized)
	{
		W_Init();
		initialized = true;
	}
	return W_getFuncsArray(nbF);
}

extern "C" __declspec(dllexport) void beNotified(SCNotification *notifyCode)
{
	if(!initialized)
	{
		W_Init();
		initialized = true;
	}
	W_beNotified(notifyCode);
}

extern "C" __declspec(dllexport) LRESULT messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	return (LRESULT)W_messageProc(Message, wParam, lParam);
}

#ifdef UNICODE
extern "C" __declspec(dllexport) BOOL isUnicode()
{
    return TRUE;
}
#endif //UNICODE
