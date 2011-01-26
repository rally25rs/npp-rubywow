using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NppPluginNET
{
    /// <summary>
    /// Determines how to run Ruby scripts.
    /// </summary>
    internal class ScriptRunner
    {
        private PluginBase pluginBase;

        internal ScriptRunner(PluginBase plgBase)
        {
            pluginBase = plgBase;
        }

        /// <summary>
        /// Checks that the ruby path is set in the settings, or shows an alert to tell the user to set it.
        /// </summary>
        /// <returns>true if the path is set.</returns>
        private bool CheckRubyPath()
        {
            if (string.IsNullOrEmpty(pluginBase.settings.RubyPath))
            {
                var ret = MessageBox.Show("The path to the Ruby executable needs to be set first. Press 'OK' to open the settings window, or 'Cancel' to abort this action.", "Ruby. Wow!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (ret == DialogResult.OK)
                {
                    pluginBase.showSettings();
                    return CheckRubyPath();
                }
                else
                    return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Find the base path to run Ruby from.
        /// </summary>
        /// <remarks>
        /// The start path is determined by recursively checkng up the directory tree, looking for either:
        /// 1) a file named 'rubywow.proj'
        /// 2) a file named '.rubywow.proj'
        /// 3) a file named 'Rakefile'
        /// 
        /// The 'rubywow.proj' file (with or without a leading period) acts as a marker file, indicating the
        /// 'base' directory for a ruby project.  The file may also contain special setting for this project.
        /// 
        /// Also, once one of the above is found, it will also check for a file: 'test\test_helper.rb'
        /// If it exists, then the start directory will be moved into the 'test' directory.
        /// (this is for rails support)
        /// </remarks>
        /// <param name="fullPath">The full path to the file being run.</param>
        /// <param name="startPath">The path where ruby should be started from.</param>
        /// <param name="remainingPath">The remainder of the fullPath after the startPath is removed.</param>
        private void FindStartPath(DirectoryInfo fullPath, out string startPath, out string remainingPath)
        {
            DirectoryInfo di = fullPath;
            while (di != null)
            {
                foreach (var f in di.GetFiles())
                {
                    if (f.Name.Equals(".rubywow.proj", StringComparison.CurrentCultureIgnoreCase)
                        || f.Name.Equals("rubywow.proj", StringComparison.CurrentCultureIgnoreCase)
                        || f.Name.Equals("rakefile", StringComparison.CurrentCultureIgnoreCase))
                    {
                        startPath = di.FullName;
                        
                        // rails suport
                        foreach (var d in di.GetDirectories())
                        {
                            if (d.Name.Equals("test", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if(d.GetFiles("test_helper.rb").Length > 0)
                                    startPath = d.FullName;
                            }
                        }

                        remainingPath = fullPath.FullName.Substring(startPath.Length);
                        if (!string.IsNullOrEmpty(remainingPath) && remainingPath[0] == Path.DirectorySeparatorChar)
                            remainingPath = remainingPath.Substring(1);
                        if (!string.IsNullOrEmpty(remainingPath) && remainingPath[remainingPath.Length - 1] != Path.DirectorySeparatorChar)
                            remainingPath += Path.DirectorySeparatorChar;
                        return;
                    }
                }
                di = di.Parent;
            }

            // use the original path
            startPath = fullPath.FullName;
            remainingPath = "";
        }

        internal void RunScript(FileInfo fi)
        {
            if(!CheckRubyPath())
                return;

            string startPath, remainingPath;
            FindStartPath(fi.Directory, out startPath, out remainingPath);
            pluginBase.showOutputDialog();
            pluginBase.frmOutput.RunCommand(startPath, pluginBase.settings.RubyPath + "\\ruby", string.Format("{0}{1}", remainingPath, fi.Name));
        }

        internal void RunMethod(FileInfo fi, string methodName, bool asRegex)
        {
            if (!CheckRubyPath())
                return;

            string startPath, remainingPath;
            FindStartPath(fi.Directory, out startPath, out remainingPath);
            pluginBase.showOutputDialog();
            pluginBase.frmOutput.RunCommand(fi.DirectoryName, pluginBase.settings.RubyPath + "\\ruby", string.Format("{0}{1} -n \"{2}\"", fi.Name, (asRegex ? "\"/" + methodName + "/\"" : methodName)));
        }
    }
}
