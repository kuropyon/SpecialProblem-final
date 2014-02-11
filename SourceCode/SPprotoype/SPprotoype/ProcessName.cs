using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace SPprotoype
{
    class ProcessName
    {
        #region fields
        private string[] stringSeperator = new string[] {"\\"};
        private Process p;
        #endregion fields

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public string GetActiveProcessFilePath()
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                this.p = Process.GetProcessById((int)pid);
                return this.p.MainModule.FileName;
            }
            catch(Exception e)
            {
                return "exception";
            }

        }
        public void killProcess()
        {
            p.Kill();
        }

        public String getProcessName(String processPath)
        {
            String[] result;
            string pName;
            result = processPath.Split(stringSeperator, StringSplitOptions.RemoveEmptyEntries);

            pName = result[result.Length - 1];

            return pName;
        }

        public int getMode(string pName) //must be in standard mode
        {
            StringComparison[] comparisons = (StringComparison[])Enum.GetValues(typeof(StringComparison));

            if (string.Equals(pName,"Explorer.EXE",StringComparison.OrdinalIgnoreCase)) { return 1; }
            else if (string.Equals(pName, "chrome.exe",StringComparison.OrdinalIgnoreCase)) { return 1; }
            else if (string.Equals(pName, "prezi.exe",StringComparison.OrdinalIgnoreCase)) { return 2; }
            else if (string.Equals(pName, "AcroRd32.exe", StringComparison.OrdinalIgnoreCase)) { return 2; }
            else if (string.Equals(pName, "PicasaPhotoViewer.exe",StringComparison.OrdinalIgnoreCase)) { return 2; }
            else if (string.Equals(pName, "iTunes.exe",StringComparison.OrdinalIgnoreCase)) { return 2; }
            else if (string.Equals(pName, "SPprotoype.exe", StringComparison.OrdinalIgnoreCase) || string.Equals(pName, "SPprotoype.vhost.exe", StringComparison.OrdinalIgnoreCase)) { return 3; }
            else return -1;
                
        }
    }
}
