using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlyTask
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            //TaskCreateRun();
            //TaskRunSynchronoushly();
            //OnlyThreadRun();
            //ContinueTask();
            ParentAndChildTask();
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //Console.WriteLine("ValueTask开始");
            //for (int i = 0; i < 100000; i++)
            //{
            //   var itemList= await GetStringDicAsync(); 
            //}
            //Console.WriteLine("ValueTask结束");
            //Console.WriteLine($"ValueTask耗时：{stopwatch.ElapsedMilliseconds}");

            //Console.WriteLine();
            //Console.WriteLine();

            //stopwatch.Restart();
            //Console.WriteLine("Task开始");
            //for (int i = 0; i < 100000; i++)
            //{
            //    var itemList = await GetStringList(); 
            //}
            //Console.WriteLine("Task结束");
            //Console.WriteLine($"Task耗时：{stopwatch.ElapsedMilliseconds}");
            //Console.ReadLine();
        }
        private static object _lock = new object();
        public static void TaskMethond(object item)
        {
            lock (_lock)
            {
                Console.WriteLine(item?.ToString());
                Console.WriteLine($"任务Id：{Task.CurrentId?.ToString() ?? "没有任务运行"}\t 线程Id：{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"是否是线程池中的线程：{Thread.CurrentThread.IsThreadPoolThread}");
                Console.WriteLine($"是否是后台线程：{Thread.CurrentThread.IsBackground}");
                Console.WriteLine();
            }
        }

        #region 任务创建
        public static void TaskCreateRun()
        {
            var taskFactory = new TaskFactory();
            var task1 = taskFactory.StartNew(TaskMethond, "使用实例化TaskFactory");
            var task2 = Task.Factory.StartNew(TaskMethond, "使用Task静态调用Factory");
            var task3 = new Task(TaskMethond, "使用Task构造函数实例化");
            task3.Start();
            var task4 = Task.Run(() => TaskMethond("使用Task.Run"));

        }
        #endregion


        #region 单独线程运行任务
        public static void OnlyThreadRun()
        {
            var task1 = new Task(TaskMethond, "单独线程运行任务", TaskCreationOptions.LongRunning);
            task1.Start();
        }
        #endregion


        #region 同步任务

        public static void TaskRunSynchronoushly()
        {
            TaskMethond("主线程调用");
            var task1 = new Task(TaskMethond, "任务同步调用");
            task1.RunSynchronously();
        }
        #endregion


        #region 连续任务
        public static void TaskOne()
        {
            Console.WriteLine($"任务{Task.CurrentId},方法名：{System.Reflection.MethodBase.GetCurrentMethod().Name }启动");
            Task.Delay(1000).Wait();
            Console.WriteLine($"任务{Task.CurrentId},方法名：{System.Reflection.MethodBase.GetCurrentMethod().Name }结束");
        }
        public static void TaskTwo(Task task)
        {
            Console.WriteLine($"任务{task.Id}以及结束了");
            Console.WriteLine($"现在开始的 任务是任务{Task.CurrentId},方法名称：{System.Reflection.MethodBase.GetCurrentMethod().Name  }  ");
            Console.WriteLine($"任务处理");
            Task.Delay(1000).Wait();
        }
        public static void TaskThree(Task task)
        {
            Console.WriteLine($"任务{task.Id}以及结束了");
            Console.WriteLine($"现在开始的 任务是任务{Task.CurrentId}.方法名称：{System.Reflection.MethodBase.GetCurrentMethod().Name } ");
            Console.WriteLine($"任务处理");
            Task.Delay(1000).Wait();
        }
        public static void ContinueTask()
        {
            Task task1 = new Task(TaskOne);
            Task task2 = task1.ContinueWith(TaskTwo, TaskContinuationOptions.OnlyOnRanToCompletion);//已完成情况下继续任务
            Task task3 = task1.ContinueWith(TaskThree, TaskContinuationOptions.OnlyOnFaulted);//出现未处理异常情况下继续任务
            task1.Start();
        }
        #endregion


        #region 任务的层次结构——父子层次结构
        public static void ChildTask()
        {
            Console.WriteLine("当前运行的子任务，开启");
            Task.Delay(5000).Wait();
            Console.WriteLine("子任务运行结束");
        }

        public static void ParentTask()
        {
            Console.WriteLine("父级任务开启");
            var child = new Task(ChildTask);
            child.Start();
            Task.Delay(1000).Wait();
            Console.WriteLine("父级任务结束");
        }

        public static void ParentAndChildTask()
        {
            var parent = new Task(ParentTask);
            parent.Start();
            Task.Delay(2000).Wait();
            Console.WriteLine($"父级任务的状态 :{parent.Status}");
            Task.Delay(4000).Wait();
            Console.WriteLine($"父级任务的状态 ：{parent.Status}");
        }
        #endregion

        #region 等待任务
        private static DateTime _time;
        private static List<string> _data;
        public static async ValueTask<List<string>> GetStringDicAsync()
        {
            if (_time >= DateTime.Now.AddSeconds(-5))
            {
                return await new ValueTask<List<string>>(_data);
            }

            else
            {
                (_data, _time) = await GetData();
                return _data; 
            }
        }
        public static Task<(List<string> data, DateTime time)> GetData() => 
            Task.FromResult(
                (Enumerable.Range(0, 10).Select(x => $"itemString{x}").ToList(), DateTime.Now));

        public static async Task<List<string>> GetStringList()
        {
            (_data, _time) = await GetData();
            return _data;
        }
        #endregion
    }
}
