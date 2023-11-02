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
        int[,] _arrNumbers = new int[5, 2];
        AutoResetEvent _autoReset = new AutoResetEvent(true);
        static void Main(string[] args)
        {
            Program pr = new Program();

            ThreadStart threadStart = new ThreadStart(pr.GenerateSaveNumbersThread);
            Thread generateNumbersThread = new Thread(threadStart);
            generateNumbersThread.Start();

            ParameterizedThreadStart parameterThread1 = new ParameterizedThreadStart(pr.SumNumbersThread);
            Thread sumNumbersThread = new Thread(parameterThread1);
            sumNumbersThread.Start("sumNumbers.txt");

            ParameterizedThreadStart parameterThread2 = new ParameterizedThreadStart(pr.MultiplyNumbersThread);
            Thread multiplyNumbersThread = new Thread(parameterThread2);
            multiplyNumbersThread.Start("multiplyNumbers.txt");

            Console.ReadLine();
        }
        void GenerateSaveNumbersThread()
        {
            _autoReset.WaitOne();

            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    _arrNumbers[i, j] = rnd.Next(0, 100);
                }
            }

            using (FileStream fs = new FileStream("numbers.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < _arrNumbers.GetLength(0); i++)
                    {
                        sw.WriteLine(_arrNumbers[i, 0] + " " + _arrNumbers[i, 1]);
                    }
                }
            }

            _autoReset.Reset();
            _autoReset.Set();
        }
        void SumNumbersThread(object obj)
        {
            _autoReset.WaitOne();

            string path = obj as string;

            Array.Clear(_arrNumbers, 0, 10);
            LoadNumbers();

            int[] arrSum = new int[5];

            for (int i = 0; i < _arrNumbers.GetLength(0); i++)
            {
                arrSum[i] = _arrNumbers[i, 0] + _arrNumbers[i, 1];
            }

            SaveNumbers(path, arrSum);

            _autoReset.Reset();
            _autoReset.Set();
        }
        void MultiplyNumbersThread(object obj)
        {
            _autoReset.WaitOne();

            string path = obj as string;

            Array.Clear(_arrNumbers, 0, 10);
            LoadNumbers();

            int[] arrSum = new int[5];

            for (int i = 0; i < _arrNumbers.GetLength(0); i++)
            {
                arrSum[i] = _arrNumbers[i, 0] * _arrNumbers[i, 1];
            }

            SaveNumbers(path, arrSum);

            _autoReset.Reset();
            _autoReset.Set();
        }
        void LoadNumbers()
        {
            int row = 0;

            using (FileStream fs = new FileStream("numbers.txt", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string str = sr.ReadLine();
                        string[] numbers = str.Split(' ');

                        _arrNumbers[row, 0] = Int32.Parse(numbers[0]);
                        _arrNumbers[row, 1] = Int32.Parse(numbers[1]);

                        row++;
                    }
                }
            }
        }
        void SaveNumbers(string path, int[] arr)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        sw.WriteLine(arr[i]);
                    }
                }
            }
        }
    }
}
