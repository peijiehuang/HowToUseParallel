namespace HowToUseParallel
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // 创建一个取消令牌源
            var cts = new CancellationTokenSource();

            // 设置取消时间为10秒后（避免一直等待，导致程序卡死）
            cts.CancelAfter(10000);

            // 配置 ParallelOptions
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 3, // 限制最多并发3个任务
                CancellationToken = cts.Token // 设置取消令牌
            };

            //方法一Parallel.For
            ParallelFor(options);

            //方法二Parallel.ForAsync
            await ParallelForAsync(options);

            //方法三Parallel.ForEach
            ParallelForEach(options);

            //方法四Parallel.ForEachAsync
            await ParallelForEachAsync(options);

            //方法五Parallel.Invoke
            ParallelInvoke(options);

        }

        private static void ParallelFor(ParallelOptions options)
        {
            Console.WriteLine("Parallel.For-------------------");
            //并发开始
            Parallel.For(1, 6, options, i =>
            {
                var startTime = DateTime.Now;
                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 开始");
                Thread.Sleep((5 - i) * 1000); // 模拟不同任务耗时
                var endTime = DateTime.Now;
                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 完成 (耗时 {endTime - startTime} 毫秒)");
            });

            //并发未完成前，会堵塞线程
            Console.WriteLine("所有任务完成");
            Console.WriteLine("Parallel.For-------------------\n");
        }

        private static async Task ParallelForAsync(ParallelOptions options)
        {
            Console.WriteLine("Parallel.ForAsync-------------------");
            //并发开始
            var forAsync = Parallel.ForAsync(1, 6, options, async (i, cancellationToken) =>
            {
                var startTime = DateTime.Now;
                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 开始");
                await Task.Delay((5 - i) * 1000, cancellationToken); // 模拟不同任务耗时
                var endTime = DateTime.Now;
                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 完成 (耗时 {endTime - startTime} 毫秒)");
            });
            //并发未完成前，会等待线程，可以运行其他方法
            Console.WriteLine("Parallel.ForAsync异步不堵塞线程");
            await forAsync;
            Console.WriteLine("所有任务完成");
            Console.WriteLine("Parallel.ForAsync-------------------\n");
        }

        private static void ParallelForEach(ParallelOptions options)
        {
            Console.WriteLine("Parallel.ForEach-------------------");
            //并发开始
            Parallel.ForEach(Enumerable.Range(1, 5), options, i =>
            {
                var startTime = DateTime.Now;
                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 开始");
                Thread.Sleep((5 - i) * 1000); // 模拟不同任务耗时
                var endTime = DateTime.Now;
                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 完成 (耗时 {endTime - startTime} 毫秒)");
            });
            //并发未完成前，会堵塞线程
            Console.WriteLine("所有任务完成");
            Console.WriteLine("Parallel.ForEach-------------------\n");
        }

        private static async Task ParallelForEachAsync(ParallelOptions options)
        {
            Console.WriteLine("Parallel.ForEachAsync-------------------");
            //并发开始
            var forEachAsync = Parallel.ForEachAsync(Enumerable.Range(1, 5), options, async (i, cancellationToken) =>
            {
                var startTime = DateTime.Now;
                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 开始");
                await Task.Delay((5 - i) * 1000); // 模拟不同任务耗时
                var endTime = DateTime.Now;
                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 {i} 完成 (耗时 {endTime - startTime} 毫秒)");
            });
            //并发未完成前，会等待线程，可以运行其他方法
            Console.WriteLine("Parallel.ForEachAsync异步不堵塞线程");
            await forEachAsync;
            Console.WriteLine("所有任务完成");
            Console.WriteLine("Parallel.ForEachAsync-------------------\n");
        }

        private static void ParallelInvoke(ParallelOptions options)
        {
            Console.WriteLine("Parallel.Invoke-------------------");
            //并发开始
            Parallel.Invoke(options,
                            () =>
                            {
                                var startTime = DateTime.Now;
                                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 1 开始");
                                Thread.Sleep(5000); // 模拟任务耗时
                                var endTime = DateTime.Now;
                                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 1 完成 (耗时 {endTime - startTime} 毫秒)");
                            },
                            () =>
                            {
                                var startTime = DateTime.Now;
                                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 2 开始");
                                Thread.Sleep(4000); // 模拟任务耗时
                                var endTime = DateTime.Now;
                                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 2 完成 (耗时 {endTime - startTime} 毫秒)");
                            },
                            () =>
                            {
                                var startTime = DateTime.Now;
                                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 3 开始");
                                Thread.Sleep(3000); // 模拟任务耗时
                                var endTime = DateTime.Now;
                                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 3 完成 (耗时 {endTime - startTime} 毫秒)");
                            },
                            () =>
                            {
                                var startTime = DateTime.Now;
                                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 4 开始");
                                Thread.Sleep(2000); // 模拟任务耗时
                                var endTime = DateTime.Now;
                                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 4 完成 (耗时 {endTime - startTime} 毫秒)");
                            },
                            () =>
                            {
                                var startTime = DateTime.Now;
                                Console.WriteLine($"[{startTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 5 开始");
                                Thread.Sleep(1000); // 模拟任务耗时
                                var endTime = DateTime.Now;
                                Console.WriteLine($"[{endTime}] 线程 {Thread.CurrentThread.ManagedThreadId}: 任务 5 完成 (耗时 {endTime - startTime} 毫秒)");
                            }
                        );
            //堵塞主线程
            Console.WriteLine("所有任务完成");
            Console.WriteLine("Parallel.Invoke-------------------\n");
        }
    }
}
