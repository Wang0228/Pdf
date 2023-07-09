using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pdf
{
    internal class PdfService
    {
        //public class LastData : EventArgs
        //{
        //    public FileInfo File { get; set; }
        //}
        //public event EventHandler<LastData> ReadAddedFile;
        public string _path;
        public PdfService(string path)
        {
            //ReadAddedFile += Process;
            _path = path;
            Run();
        }

        private async Task Run()
        {
            Console.WriteLine("偵測中! OuOb");
            List<string> trackedFiled =Directory.GetFiles(_path).ToList();
            while (true)
            {
                string[] nowFiles=Directory.GetFiles(_path);
                if(nowFiles.Length > trackedFiled.Count)
                {
                    IEnumerable<string> files = nowFiles.Except(trackedFiled);
                    foreach (string file in files)
                    {
                        trackedFiled.Add(file);
                        FileInfo lastFile=new FileInfo(file);
                        Console.WriteLine($"處理{lastFile.Name}");
                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7168/api/pdf");
                        var content = new StringContent("{\"AifileName\":\"" + lastFile.Name + "\"}", null, "application/json");
                        request.Content = content;
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        Console.WriteLine(await response.Content.ReadAsStringAsync());

                        //ReadAddedFile.Invoke(this, new LastData { File=lastFile});
                    }

                }
            }
        }
        //public void Process(object sender,LastData e)
        //{
        //    Console.WriteLine(e.File.Name);
        //}
    }
}
