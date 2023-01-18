using System;
using System.IO;
using System.Net;
using System.Management;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Web;

namespace ESM_API_CLIENT
{
    public class ESM_API_CLIENT
    {
        public static void Main()
        {
            Process MyProcess = new Process();
            MyProcess.OutputDataReceived += MyProcess_OutputDataReceived;
            MyProcess.StartInfo = new ProcessStartInfo("cmd.exe")
            {
                WindowStyle = ProcessWindowStyle.Maximized,
                Verb = "runas",
                Arguments = "/c powershell.exe -Command \"& {Get-EventLog -LogName security}\""
            };
            MyProcess.Start();
            
        }

        private static void MyProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data.ToString());
        }
    }
}