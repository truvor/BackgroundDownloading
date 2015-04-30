using DownloadProjects;
using System;

namespace TestBackgroundDownload
{
    class Program
    {
        static void TestDownloading(string testFileUrl)
        {
            DownloadManager.RunDownload(testFileUrl);
            Console.WriteLine("Файл, расположенный по адресу {0} начал скачиваться",
                testFileUrl);
        }

        static void Main(string[] args)
        {
            TestDownloading("test/location/1");
            TestDownloading("test/location/2");
            TestDownloading("test/location/another_one");
            Console.ReadLine();
        }
    }
}
