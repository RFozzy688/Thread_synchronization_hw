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
            pn.GenerateNumber();
        }
    }
    public class PrimeNumber
    {
        Mutex _mutex = new Mutex();
        List<int> _numbers = new List<int>();
        public void GenerateNumber()
        {
            Console.WriteLine("Генерация и сохранение чисел в файл");

            Random random = new Random();

            _mutex.WaitOne();

            for (int i = 0; i < 10000; i++)
            {
                _numbers.Add(random.Next());
            }

            SaveToFile("numbers.txt");

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
