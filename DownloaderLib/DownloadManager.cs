using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadProjects
{
    public static class DownloadManager
    {
        private static object _thisLock = new object();
        private static FileDownloader _fd = new FileDownloader();

        /// <summary>
        /// Возвращает задачи в порядке их выполнения
        /// </summary>
        private static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
        {
            var inputTasks = tasks.ToList();

            var buckets = new TaskCompletionSource<Task<T>>[inputTasks.Count];
            var results = new Task<Task<T>>[buckets.Length];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new TaskCompletionSource<Task<T>>();
                results[i] = buckets[i].Task;
            }

            int nextTaskIndex = -1;
            Action<Task<T>> continuation = completed =>
            {
                var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                bucket.TrySetResult(completed);
            };

            foreach (var inputTask in inputTasks)
                inputTask.ContinueWith(continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return results;
        }

        /// <summary>
        /// Имитирует загрузку списка файлов асинхронно
        /// </summary>
        public async static Task DownloadFilesAsync(List<string> fileUrls)
        {
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();

            foreach (var url in fileUrls)
            {
                tasks.Add(Task.Factory.StartNew(() => RunDownload(url)));
                Console.WriteLine("Файл, расположенный по адресу {0} начал скачиваться",
                    url);
            }

            foreach (var bucket in Interleaved(tasks))
            {
                var t = await bucket;
                byte[] result = await t;

                Console.WriteLine("Сохранённые байты файла:");
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
            }
        }

        /// <summary>
        /// Запускает загрузку файла
        /// </summary>
        /// <param name="fileUrl">Адрес скачиваемого файла</param>
        private static byte[] RunDownload(string fileUrl)
        {
            lock(_thisLock)
            {
                var bytes = _fd.Download(fileUrl);
                Console.WriteLine("Файл, расположенный по адресу {0} успешно скачан.",
                    fileUrl);
                return bytes;
            }
        }
    }
}
