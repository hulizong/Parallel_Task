using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_Task
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Start");
            //ForEx();
            //Console.WriteLine("for循环完成"); 
            //ParallelFor();
            //Console.WriteLine("End");

            //ParallelForEach();
            
            //BraekFor();
            //StopFor();

            //BraekForEach();
            //StopForEach();

            //五百条数据顺序完成
            //Stopwatch swFive = new Stopwatch();
            //swFive.Start();
            //Thread.Sleep(3000);
            //_tests = new List<Test>();
            //TaskFive_One();
            //TaskFive_Two();
            //TaskFive_Three();
            //TaskFive_Four();
            //swFive.Stop();
            //Console.WriteLine("500条数据  顺序编程所耗时间:" + swFive.ElapsedMilliseconds);


            ////五百条数据并行完成
            //Stopwatch swFiveTask = new Stopwatch();
            //swFiveTask.Start();
            //Thread.Sleep(3000);
            //_tests = new List<Test>(); 
            //Parallel.Invoke(()=> TaskFive_One(), () => TaskFive_Two(), () => TaskFive_Three(), () => TaskFive_Four());
            //swFiveTask.Stop();
            //Console.WriteLine("500条数据  并行编程所耗时间:" + swFiveTask.ElapsedMilliseconds);


            ////一千条数据顺序完成
            //Stopwatch swOnethousand = new Stopwatch();
            //swOnethousand.Start();
            //Thread.Sleep(3000);
            //_tests = new List<Test>();
            //TaskOnethousand_One();
            //TaskOnethousand_Two();
            //TaskOnethousand_Three();
            //TaskOnethousand_Four();
            //swOnethousand.Stop();
            //Console.WriteLine("1000条数据  顺序编程所耗时间:" + swOnethousand.ElapsedMilliseconds);


            ////一千条数据并行完成
            //Stopwatch swOnethousandTask = new Stopwatch();
            //swOnethousandTask.Start();
            //Thread.Sleep(3000);
            //_tests = new List<Test>();
            //Parallel.Invoke(() => TaskOnethousand_One(), () => TaskOnethousand_Two(), () => TaskOnethousand_Three(), () => TaskOnethousand_Four());
            //swOnethousandTask.Stop();
            //Console.WriteLine("1000条数据  并行编程所耗时间:" + swOnethousandTask.ElapsedMilliseconds);







            Stopwatch swTest = new Stopwatch();
            swTest.Start();
            Thread.Sleep(3000); 
            TaskFive_One();
            TaskFive_Two();
            TaskFive_Three();
            TaskFive_Four();
            swTest.Stop();
            Console.WriteLine("500条数据  顺序编程所耗时间:" + swTest.ElapsedMilliseconds);


            //五百条数据并行完成 
            swTest.Restart();
            Thread.Sleep(3000); 
            Parallel.Invoke(() => TaskFive_One(), () => TaskFive_Two(), () => TaskFive_Three(), () => TaskFive_Four());
            swTest.Stop();
            Console.WriteLine("500条数据  并行编程所耗时间:" + swTest.ElapsedMilliseconds);


            //一千条数据顺序完成 
            swTest.Restart();
            Thread.Sleep(3000); 
            TaskOnethousand_One();
            TaskOnethousand_Two();
            TaskOnethousand_Three();
            TaskOnethousand_Four();
            swTest.Stop();
            Console.WriteLine("1000条数据  顺序编程所耗时间:" + swTest.ElapsedMilliseconds);


            //一千条数据并行完成 
            swTest.Restart();
            Thread.Sleep(3000); 
            Parallel.Invoke(() => TaskOnethousand_One(), () => TaskOnethousand_Two(), () => TaskOnethousand_Three(), () => TaskOnethousand_Four());
            swTest.Stop();
            Console.WriteLine("1000条数据  并行编程所耗时间:" + swTest.ElapsedMilliseconds);
        }



        #region   For循环比较

        public static void ForEx()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Test> list = new List<Test>();
            for (int i = 0; i < 100; i++)
            {
                Test test = new Test();
                test.Name = "Name:" + i;
                test.Index = "Index:" + i;
                test.Time = DateTime.Now;
                list.Add(test);
                Task.Delay(10).Wait();
                Console.WriteLine("C#中的for循环：" + i);
            }
            stopwatch.Stop();
            Console.WriteLine("for  0-100 执行完成 耗时：{0}", stopwatch.ElapsedMilliseconds);

        }
        public static void ParallelFor()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Test> lists = new List<Test>();
            Parallel.For(1, 100, i =>
            {
                Test tests = new Test();
                tests.Name = "Name:" + i;
                tests.Index = "Index:" + i;
                tests.Time = DateTime.Now;
                lists.Add(tests);
                Task.Delay(10).Wait();
                Console.WriteLine("Parallel中的ParallelFor循环：" + i);
            });
            stopwatch.Stop();
            Console.WriteLine("ParallelFor  0-100 执行完成 耗时：{0}", stopwatch.ElapsedMilliseconds);
        }

        #endregion

        #region ForEach 语句比较
         
        public static void ParallelForEach() 
        {
            List<Test> result = new List<Test>();
            for (int i = 1; i < 100; i++)
            {
                Test model = new Test();
                model.Name = "Name" + i;
                model.Index = "Index" + i;
                model.Time = DateTime.Now;
                result.Add(model);
            }
            Parallel.ForEach<Test>(result,s=>{
                Console.WriteLine(s.Name);
            });
        }
        #endregion

        #region Stop 和Break
        public static List<Test> GetListTest()
        {
            List<Test> result = new List<Test>();
            for (int i = 1; i < 100; i++)
            {
                Test model = new Test();
                model.Name =  i.ToString();
                model.Index = "Index" + i;
                model.Time = DateTime.Now;
                result.Add(model);
            }
            return result;
        }
        public static void BraekFor()
        {
            var result = GetListTest();
            Parallel.For(0, result.Count, (int i, ParallelLoopState ls) =>
            {
                Task.Delay(10).Wait();
                if (i > 50)
                {
                    Console.WriteLine("Parallel.For使用Break停止当前迭代:" + i);
                    ls.Break();
                    return;
                }
                Console.WriteLine("测试Parallel.For的Break：" + i);
            });
         
        }

        public static void StopFor() 
        { 
            var result = GetListTest();
            Parallel.For(0, result.Count, (int i, ParallelLoopState ls) =>
            {
                Task.Delay(10).Wait();
                if (i > 50)
                {
                    Console.WriteLine("Parallel.For使用Stop停止 迭代:" + i);
                    ls.Stop();
                    return;
                }
                Console.WriteLine("测试Parallel.For的Stop：" + i);
            });
        }


        public static void BraekForEach()
        {
            var result = GetListTest();
            Parallel.ForEach<Test>( result,(Test s,ParallelLoopState ls) =>
            {
                Task.Delay(10).Wait();
                if (Convert.ToInt32(s.Name) > 50)
                {
                    Console.WriteLine("Parallel.ForEach使用Break停止当前迭代:" + s.Name);
                    ls.Break();
                    return;
                }
                Console.WriteLine("测试Parallel.ForEach的Break：" + s.Name);
            });

        }

        public static void StopForEach()
        {
            var result = GetListTest();
            Parallel.ForEach<Test>(result, (Test s, ParallelLoopState ls) =>
            {
                Task.Delay(10).Wait();
                if (Convert.ToInt32(s.Name) > 50)
                {
                    Console.WriteLine("Parallel.ForEach使用Stop停止 迭代:" + s.Name);
                    ls.Stop();
                    return;
                }
                Console.WriteLine("测试Parallel.ForEach的Stop：" + s.Name);
            });
        }
        #endregion

        #region Parallel.Invoke()使用共同资源

        //public static List<Test> _tests = null;
        //public static void TaskFive_One() 
        //{
        //    for (int i = 0; i < 500; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskFive_One 500条数据第一个方法 执行完成");
        //}
        //public static void TaskFive_Two()
        //{
        //    for (int i = 500; i < 1000; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskFive_Two 500条数据第二个方法 执行完成");
        //}
        //public static void TaskFive_Three()
        //{
        //    for (int i = 1000; i < 1500; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskFive_Three 500条数据第三个方法 执行完成");
        //}
        //public static void TaskFive_Four()
        //{
        //    for (int i = 1500; i < 2000; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskFive_Four 500条数据第四个方法 执行完成");
        //}



        //public static void TaskOnethousand_One()
        //{
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskOnethousand_One 1000条数据第一个方法 执行完成");
        //}
        //public static void TaskOnethousand_Two()
        //{
        //    for (int i = 1000; i < 2000; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskOnethousand_Two 1000条数据第二个方法 执行完成");
        //}
        //public static void TaskOnethousand_Three()
        //{
        //    for (int i = 2000; i < 3000; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskOnethousand_Three 1000条数据第三个方法 执行完成");
        //}
        //public static void TaskOnethousand_Four()
        //{
        //    for (int i = 3000; i < 4000; i++)
        //    {
        //        Test test = new Test();
        //        test.Name = i.ToString();
        //        test.Index = i.ToString();
        //        test.Time = DateTime.Now;
        //        _tests.Add(test);
        //    }
        //    Console.WriteLine("TaskOnethousand_Three 1000条数据第四个方法 执行完成");
        //}
        #endregion


        #region Parallel.Invoke()不使用共同资源
         
        public static void TaskFive_One()
        {
            List<Test> tests = new List<Test>();
            for (int i = 0; i < 500; i++)
            {
                Test test = new Test();
                test.Name = "Name"+ i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskFive_One 500条数据第一个方法 执行完成");
        }
        public static void TaskFive_Two()
        {
            List<Test> tests = new List<Test>();
            for (int i = 500; i < 1000; i++)
            {
                Test test = new Test();
                test.Name = "Name" + i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskFive_Two 500条数据第二个方法 执行完成");
        }
        public static void TaskFive_Three()
        {
            List<Test> tests = new List<Test>();
            for (int i = 1000; i < 1500; i++)
            {
                Test test = new Test();
                test.Name = "Name"+i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskFive_Three 500条数据第三个方法 执行完成");
        }
        public static void TaskFive_Four()
        {
            List<Test> tests = new List<Test>();
            for (int i = 1500; i < 2000; i++)
            {
                Test test = new Test();
                test.Name = "Name" + i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskFive_Four 500条数据第四个方法 执行完成");
        }



        public static void TaskOnethousand_One()
        {
            List<Test> tests = new List<Test>();
            for (int i = 0; i < 1000; i++)
            {
                Test test = new Test();
                test.Name = "Name" + i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskOnethousand_One 1000条数据第一个方法 执行完成");
        }
        public static void TaskOnethousand_Two()
        {
            List<Test> tests = new List<Test>();
            for (int i = 1000; i < 2000; i++)
            {
                Test test = new Test();
                test.Name = "Name" + i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskOnethousand_Two 1000条数据第二个方法 执行完成");
        }
        public static void TaskOnethousand_Three()
        {
            List<Test> tests = new List<Test>();
            for (int i = 2000; i < 3000; i++)
            {
                Test test = new Test();
                test.Name = "Name" + i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskOnethousand_Three 1000条数据第三个方法 执行完成");
        }
        public static void TaskOnethousand_Four()
        {
            List<Test> tests = new List<Test>();
            for (int i = 3000; i < 4000; i++)
            {
                Test test = new Test();
                test.Name = "Name" + i.ToString();
                test.Index = "Index" + i.ToString();
                test.Time = DateTime.Now;
                tests.Add(test);
            }
            Console.WriteLine("TaskOnethousand_Four 1000条数据第四个方法 执行完成");
        }
        #endregion
    }
    public class Test 
    {
        public string Name { get; set; }
        public string Index { get; set; }
        public DateTime Time { get; set; }
    }
}



