﻿using System.Threading;

namespace DownloadProjects
{
    class FileDownloader
    {
        public byte[] Download(string url)
        {
            //    Имитируем бурную деятельность
            Thread.Sleep(4000);
            return new byte[] { 1, 2, 3, 4 };
        }
    }
}
