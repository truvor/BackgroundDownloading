using DownloadProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestBackgroundDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> fileUrls4FirstCall = new List<string>()
                { "test/first/1", "test/first/2", "test/first/another_one" };

            List<string> fileUrls4SecondCall = new List<string>()
                { "test/second/1", "test/second/2", "test/second/another_one" };

            DownloadManager.DownloadFilesAsync(fileUrls4FirstCall);
            DownloadManager.DownloadFilesAsync(fileUrls4SecondCall);

            Console.ReadLine();
        }
    }
}
