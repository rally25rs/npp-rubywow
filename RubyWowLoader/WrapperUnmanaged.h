
void W_Init();

void W_setInfo(NppData notepadPlusData);
FuncItem * W_getFuncsArray(int *nbF);
LRESULT W_messageProc(UINT Message, WPARAM wParam, LPARAM lParam);
void W_beNotified(SCNotification *notifyCode);
