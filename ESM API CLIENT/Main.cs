using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ESM_API_CLIENT
{
    public class ESM_API_CLIENT
    {
        private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ESM");
        private static readonly string FilePath = Path.Combine(FolderPath, "VersionsExecuted.json");
        private static List<int> VersionsExecuted = new List<int>();

        public static void Main()
        {
            try
            {
                VerifyFiles();
                string JSON_Response = new WebClient().DownloadString("http://192.168.35.130");
                var Response = JsonConvert.DeserializeObject<VersionsInformation[]>(JSON_Response);
                
                foreach (var data in Response)
                {
                    if (!VersionsExecuted.Contains(data.Version))
                    {
                        Process MyProcess = new Process();
                        MyProcess.OutputDataReceived += MyProcess_OutputDataReceived;
                        MyProcess.StartInfo = new ProcessStartInfo("cmd.exe")
                        {
                            WindowStyle = ProcessWindowStyle.Maximized,
                            Verb = "runas",
                            Arguments = $"/c powershell.exe -Command \"& {{{data.CodePowershell}}}\""
                        };
                        MyProcess.Start();
                        VersionsExecuted.Add(data.Version);
                    }
                    UpdateFiles();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private static void UpdateFiles()
        {
            StreamWriter sw = new StreamWriter(FilePath);
            sw.Write(JsonConvert.SerializeObject(VersionsExecuted));
            sw.Close();
        }

        private static void VerifyFiles()
        {
            try
            {
                Directory.CreateDirectory(FolderPath);
                if (!File.Exists(FilePath))
                {
                    StreamWriter sw = new StreamWriter(FilePath);
                    sw.Write("[]");
                    sw.Close();
                }
                StreamReader FileReader = new StreamReader(FilePath);
                VersionsExecuted = JsonConvert.DeserializeObject<List<int>>(FileReader.ReadToEnd());
                FileReader.Close();
            } 
            catch (JsonException)
            {
                StreamWriter sw = new StreamWriter(FilePath);
                sw.Write("[]");
                sw.Close();
                StreamReader FileReader = new StreamReader(FilePath);
                VersionsExecuted = JsonConvert.DeserializeObject<List<int>>(FileReader.ReadToEnd());
                FileReader.Close();
            } 
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n El programa se va a cerrar, Por favor contacte a soporte!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(e.GetHashCode());
            }

        }

        private static void MyProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            MessageBox.Show(e.Data.ToString(), "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}