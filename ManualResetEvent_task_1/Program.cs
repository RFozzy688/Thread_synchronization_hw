using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEvent_task_1
{
    internal class Program
    {
        string _pathNumbers = "numbers.txt";
        string _pathSumNumbers = "sumNumbers.txt";
        string _pathMultiplyNumbers = "multiplyNumbers.txt";
        int[,] _arrNumbers = new int[5, 2];
        Thread _generateNumbersThread;
        Thread _sumNumbersThread;
        Thread _multiplyNumbersThread;
        ManualResetEvent _manualReset = new ManualResetEvent(true);
        static void Main(string[] args)
        {
            Program pr = new Program();

            ThreadStart threadStart1 = new ThreadStart(pr.GenerateSaveNumbersThread);
            pr._generateNumbersThread = new Thread(threadStart1);
            pr._generateNumbersThread.Start();

            Console.ReadLine();
        }
        void GenerateSaveNumbersThread()
        {
            _manualReset.WaitOne();

            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    _arrNumbers[i, j] = rnd.Next(0, 100);
                }
            }

            using (FileStream fs = new FileStream(_pathNumbers, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < _arrNumbers.GetLength(0); i++)
                    {
                        sw.WriteLine(_arrNumbers[i, 0].ToString() + " " + _arrNumbers[i, 1].ToString());
                    }
                }
            }

            _manualReset.Reset();
        }
    }
}
