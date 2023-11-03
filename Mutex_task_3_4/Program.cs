using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mutex_task_3_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrimeNumber pn = new PrimeNumber();

            ThreadStart threadStart1 = new ThreadStart(pn.GenerateNumber);
            Thread generateNumberThread = new Thread(threadStart1);
            generateNumberThread.Start();

            ThreadStart threadStart2 = new ThreadStart(pn.SearchForPrimeNumbers);
            Thread searchForPrimeNumbersThread = new Thread(threadStart2);
            searchForPrimeNumbersThread.Start();

            Console.ReadLine();
        }
    }
    public class PrimeNumber
    {
        Mutex _mutex = new Mutex();
        List<int> _numbers = new List<int>();
        public void GenerateNumber()
        {
            _mutex.WaitOne();

            Console.WriteLine("Генерация и сохранение чисел в файл");

            Random random = new Random();

            for (int i = 0; i < 10000; i++)
            {
                _numbers.Add(random.Next());
            }

            SaveToFile("numbers.txt");

            _mutex.ReleaseMutex();
        }
        public void SearchForPrimeNumbers()
        {
            _mutex.WaitOne();

            Console.WriteLine("Поиск простых чисел");

            _numbers.Clear();

            using (FileStream fs = new FileStream("numbers.txt", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream) 
                    {
                        int num = Int32.Parse(sr.ReadLine());

                        if (IsPrimeNumber(num))
                        {
                            _numbers.Add(num);
                        }
                    }
                }
            }

            SaveToFile("primeNumbers.txt");

            _mutex.ReleaseMutex();
        }
        bool IsPrimeNumber(int x)
        {
            for (int d = 2; d <= Math.Sqrt(x); d++)
            {
                if (x % d == 0)
                {
                    return false;
                }
            }
            return true;
        }
        void SaveToFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < _numbers.Count; i++)
                    {
                        sw.WriteLine(_numbers[i]);
                    }
                }
            }
        }

    }
}
