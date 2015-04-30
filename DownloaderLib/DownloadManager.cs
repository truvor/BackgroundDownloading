using System;
using System.Threading;

namespace DownloadProjects
{
    public static class DownloadManager
    {
        private static Object thisLock = new Object();

        private static FileDownloader _fd = new FileDownloader();

        /// <summary>
        /// Имитирует загрузку
        /// </summary>
        private static void DownloadFile(string fileUrl)
        {
            lock (thisLock)
            {
                var result = _fd.Download(fileUrl);
                Console.WriteLine("Файл, расположенный по адресу {0} успешно скачан.",
                    fileUrl);
                Console.WriteLine("Сохранённые байты файла:");
                foreach (var item in result)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }

        /// <summary>
        /// Запускает загрузку файла в отдельном рабочем потоке
        /// </summary>
        /// <param name="fileUrl">Адрес скачиваемого файла</param>
        public static void RunDownload(string fileUrl)
        {
            Thread t = new Thread(() => DownloadFile(fileUrl));
            t.Start();
        }
    }
}
