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
        /*static void TestDownloading(string testFileUrl)
        {
            DownloadManager.RunDownload(testFileUrl);
            Console.WriteLine("Файл, расположенный по адресу {0} начал скачиваться",
                testFileUrl);
        }*/

        public static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
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

        async static void RunDownloading(List<string> fileUrls)
        {
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();
            foreach (var fileUrl in fileUrls)
            {
                tasks.Add(Task.Run(() => (DownloadManager.DownloadFile(fileUrl))));
                Console.WriteLine("Файл, расположенный по адресу {0} начал скачиваться",
                    fileUrl);
            }

            foreach (var t in tasks)
            {
                byte[] result = await t;
                Console.WriteLine("Сохранённые байты файла:");
                foreach (var item in result)
                {
                    Console.WriteLine(item);
                }
            }
        }

        static void Main(string[] args)
        {
            /*DownloadManager.RunDownload(new List<string>(){ "test/location/1"
            ,"test/location/2"
            ,"test/location/another_one"});*/

            /*TestDownloading("test/location/1");
            TestDownloading("test/location/2");
            TestDownloading("test/location/another_one");*/

            List<string> fileUrls = new List<string>() { "test/location/1", "test/location/2", "test/location/another_one" };

            RunDownloading(fileUrls);

            Console.ReadLine();
        }
    }
}
