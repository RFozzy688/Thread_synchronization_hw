using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public void GenerateNumber()
        {
            Random random = new Random();

            using (FileStream fs = new FileStream("numbers.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        sw.WriteLine(random.Next());
                    }
                }
            }
        }
    }
}
