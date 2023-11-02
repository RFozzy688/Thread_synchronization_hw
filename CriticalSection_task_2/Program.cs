using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CriticalSection_task_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LockFile lf = new LockFile();
            Monitor.Enter(lf);

            ParameterizedThreadStart threadStart1 = new ParameterizedThreadStart(lf.CountWordsToFileThread);
            Thread thread1 = new Thread(threadStart1);
            thread1.Start("test.txt");

            ParameterizedThreadStart threadStart2 = new ParameterizedThreadStart(lf.ModifyFileThread);
            Thread thread2 = new Thread(threadStart2);
            thread2.Start("test.txt");

            Console.ReadLine();
        }
    }
    public class LockFile
    {
        object _lockObj = new object();

        public void CountWordsToFileThread(object obj) 
        {
            string path = obj as string;
            string str;

            lock (_lockObj)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        str = sr.ReadToEnd();
                    }
                }

                char[] chars = { ' ', ',', '.', '-', '!', '?' };
                int countWords = str.Split(chars).Length;

                Console.WriteLine("Количество слов в тексте: {0}", countWords);
            }
        }
        public void ModifyFileThread(object obj)
        {
            string path = obj as string;
            string str;

            lock (_lockObj)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        str = sr.ReadToEnd();
                    }
                }

                str = str.Replace('!', '#');

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(str);
                    }
                }

                Console.WriteLine("Модификация файла выполнена");
            }
        }
    }
}
