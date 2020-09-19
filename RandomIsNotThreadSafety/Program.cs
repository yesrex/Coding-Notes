using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RandomIsNotThreadSafety
{
    internal class Utility
    {
        internal static void DoSomething()
        {
            int i = 0;
            while (i < 500)
            {
                i++;
            }
        }
    }
    internal class RandomNumberGenerator
    {
        
        private static readonly Random staticGenerator = new Random();
        [ThreadStatic] private static Random threadStaticGenerator;
        private readonly Random instanceGenerator = new Random();

        internal static int GetRandomIntFromStaticGenerator(int max)
        {
            return staticGenerator.Next(max);
        }
        internal static int GetRandomIntFromNewGenerator(int max)
        {
            var generator = new Random();
            return generator.Next(max);
        }

        internal static int GetRandomIntFromThreadStaticGenerator(int max)
        {
            if (threadStaticGenerator == null) { threadStaticGenerator = new Random(); }
            return threadStaticGenerator.Next(max);
        }

        internal int GetRandomIntFromInstanceGenerator(int max)
        {
            return instanceGenerator.Next(max);
        }
    }



    internal static class Tester
    {
        internal static void TestResultFromStaticGenerator()
        {
            Task<long>[] tasks = new Task<long>[10];
            long aggregateNumberofZero;
            long aggregateNumberToRun;
            long numberToRun = 1000000;
            double resultBuffer;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for(int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    long numberOfZero = 0;
                    for (long i = 0; i < numberToRun; i++)
                    {
                        resultBuffer = RandomNumberGenerator.GetRandomIntFromStaticGenerator(5);
                        if (resultBuffer == 0)
                        {
                            numberOfZero++;
                        }
                        Utility.DoSomething();
                    }
                    return numberOfZero;
                });
            }
            Task.WaitAll(tasks);

            //aggregate result
            aggregateNumberToRun = 0;
            aggregateNumberofZero = 0;
            foreach(var task in tasks)
            {
                aggregateNumberofZero += task.Result;
                aggregateNumberToRun += numberToRun;
            }
            stopwatch.Stop();
            Console.WriteLine($"Takes {stopwatch.Elapsed}");
            Console.WriteLine($"Probability of 0 is {Convert.ToDouble(aggregateNumberofZero) / Convert.ToDouble(aggregateNumberToRun)}");
        }

        internal static void TestResultFromThreadStaticGenerator()
        {
            Task<long>[] tasks = new Task<long>[10];
            long aggregateNumberofZero;
            long aggregateNumberToRun;
            long numberToRun = 1000000;
            double resultBuffer;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    long numberOfZero = 0;
                    for (long i = 0; i < numberToRun; i++)
                    {
                        resultBuffer = RandomNumberGenerator.GetRandomIntFromThreadStaticGenerator(5);
                        if (resultBuffer == 0)
                        {
                            numberOfZero++;
                        }
                        Utility.DoSomething();
                    }
                    return numberOfZero;
                });
            }
            Task.WaitAll(tasks);

            //aggregate result
            aggregateNumberToRun = 0;
            aggregateNumberofZero = 0;
            foreach (var task in tasks)
            {
                aggregateNumberofZero += task.Result;
                aggregateNumberToRun += numberToRun;
            }
            stopwatch.Stop();
            Console.WriteLine($"Takes {stopwatch.Elapsed}");
            Console.WriteLine($"Probability of 0 is {Convert.ToDouble(aggregateNumberofZero) / Convert.ToDouble(aggregateNumberToRun)}");
        }


        internal static void TestResultFromManyGenerator()
        {
            Task<long>[] tasks = new Task<long>[10];
            long aggregateNumberofZero;
            long aggregateNumberToRun;
            long numberToRun = 1000000;
            double resultBuffer;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    long numberOfZero = 0;
                    for (long i = 0; i < numberToRun; i++)
                    {
                        resultBuffer = RandomNumberGenerator.GetRandomIntFromNewGenerator(5);
                        if (resultBuffer == 0)
                        {
                            numberOfZero++;
                        }
                        Utility.DoSomething();
                    }
                    return numberOfZero;
                });
            }
            Task.WaitAll(tasks);

            //aggregate result
            aggregateNumberToRun = 0;
            aggregateNumberofZero = 0;
            foreach (var task in tasks)
            {
                aggregateNumberofZero += task.Result;
                aggregateNumberToRun += numberToRun;
            }
            stopwatch.Stop();
            Console.WriteLine($"Takes {stopwatch.Elapsed}");
            Console.WriteLine($"Probability of 0 is {Convert.ToDouble(aggregateNumberofZero) / Convert.ToDouble(aggregateNumberToRun)}");
        }

        internal static void TestResultFromNonParallel()
        {
            long numberToRun = 10000000;
            double resultBuffer;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long numberOfZero = 0;
            for (long i = 0; i < numberToRun; i++)
            {
                resultBuffer = RandomNumberGenerator.GetRandomIntFromStaticGenerator(5);
                if (resultBuffer == 0)
                {
                    numberOfZero++;
                }
                Utility.DoSomething();
            }
            stopwatch.Stop();
            Console.WriteLine($"Takes {stopwatch.Elapsed}");
            Console.WriteLine($"Probability of 0 is {Convert.ToDouble(numberOfZero) / Convert.ToDouble(numberToRun)}");
        }

        internal static void TestResultFromInstanceGenerator()
        {
            Task<long>[] tasks = new Task<long>[10];
            long aggregateNumberofZero;
            long aggregateNumberToRun;
            long numberToRun = 1000000;
            double resultBuffer;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    long numberOfZero = 0;
                    RandomNumberGenerator instanceGenerator = new RandomNumberGenerator();
                    for (long i = 0; i < numberToRun; i++)
                    {
                        resultBuffer = instanceGenerator.GetRandomIntFromInstanceGenerator(5);
                        if (resultBuffer == 0)
                        {
                            numberOfZero++;
                        }
                        Utility.DoSomething();
                    }
                    return numberOfZero;
                });
            }
            Task.WaitAll(tasks);

            //aggregate result
            aggregateNumberToRun = 0;
            aggregateNumberofZero = 0;
            foreach (var task in tasks)
            {
                aggregateNumberofZero += task.Result;
                aggregateNumberToRun += numberToRun;
            }
            stopwatch.Stop();
            Console.WriteLine($"Takes {stopwatch.Elapsed}");
            Console.WriteLine($"Probability of 0 is {Convert.ToDouble(aggregateNumberofZero) / Convert.ToDouble(aggregateNumberToRun)}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Result from non-parallel program");
            Tester.TestResultFromNonParallel();
            Console.WriteLine();
            Console.WriteLine("Result from static generator");
            Tester.TestResultFromStaticGenerator();
            Console.WriteLine();
            Console.WriteLine("Result from creating many generator");
            Tester.TestResultFromManyGenerator();
            Console.WriteLine();
            Console.WriteLine("Result from thread static generator");
            Tester.TestResultFromThreadStaticGenerator();
            Console.WriteLine();
            Console.WriteLine("Result from instance generator");
            Tester.TestResultFromInstanceGenerator();

            Console.ReadKey();
        }
    }
}
