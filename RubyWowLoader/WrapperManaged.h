#pragma warning( disable : 4947 ) // 'AppendPrivatePath' has been deprecated..

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Windows::Forms;

namespace NppPluginNET
{
	public ref class WrapperManaged
	{
	public:
		static PluginBase^ _pluginBase;

		static void Init()
		{
			try
			{
				//System::Diagnostics::Debugger::Launch();
				String^ pluginPath = Assembly::GetExecutingAssembly()->Location;
				String^ nppPluginsDir = Path::GetDirectoryName(pluginPath);
				AppDomain::CurrentDomain::get()->AppendPrivatePath(nppPluginsDir + "\\RubyWow");
			}
			catch (Exception^ ex) { MessageBox::Show(ex->Message); }
		}
		static void CreatePluginInstance()
		{
			try
			{
				_pluginBase = gcnew PluginBase();
			}
			catch (Exception^ ex) { MessageBox::Show(ex->Message); }
		}
	};
}
