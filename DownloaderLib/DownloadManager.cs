using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadProjects
{
    public static class DownloadManager
    {
        //List<Task<byte[]>> _tasks = new List<Task<byte[]>>();

        /*public static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
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
        }*/

        /// <summary>
        /// Имитирует загрузку
        /// </summary>
        /*private async static Task<byte[]> DownloadFiles(List<string> fileUrls)
        {
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();

            foreach(var fileUrl in fileUrls)
            {
                tasks.Add(new Task<byte[]>(_fd.Download(fileUrl)));
            }

            foreach (var bucket in Interleaved(tasks))
            {
                var t = await bucket;
                try { Process(await t); }
                catch (OperationCanceledException) { }
            }
        }*/

        private static FileDownloader _fd = new FileDownloader();


        /// <summary>
        /// Запускает загрузку файла в отдельном рабочем потоке
        /// </summary>
        /// <param name="fileUrl">Адрес скачиваемого файла</param>
        /*public static void RunDownload(string fileUrl)
        {
            Thread t = new Thread(() => DownloadFile(fileUrl));
            t.Start();
        }*/

        async public static Task<byte[]> DownloadFile(string fileUrl)
        {
            return await Task.Run(() => _fd.Download(fileUrl)).ConfigureAwait(continueOnCapturedContext: false);
            
            /*Console.WriteLine("Файл, расположенный по адресу {0} успешно скачан.",
                fileUrl);
            Console.WriteLine("Сохранённые байты файла:");
            foreach (var item in result)
            {
                Console.WriteLine(item.ToString());
            }*/
        }
    }
}
