using System;

namespace NppPluginNET
{
    /// <summary>
    /// Holds settings for the plugin.
    /// </summary>
    internal class Settings
    {
        /// <summary>
        /// The directory that contains the ruby executables (namely Ruby.exe)
        /// </summary>
        public string RubyPath = "";

        /// <summary>
        /// Whether or not to enable the auto-insert of 'end' keywords.
        /// </summary>
        public bool InsertEnds = true;
    }
}
