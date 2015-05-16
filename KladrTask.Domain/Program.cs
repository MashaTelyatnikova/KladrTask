using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KladrTask.Domain.Concrete;

namespace KladrTask.Domain
{
    class Program
    {
        static void Main(string[] args)
        {
            var kladr = new DbKladrRepository();

            foreach (var reg in kladr.Regions)
            {
                Console.WriteLine(reg.Code);
            }
        }
    }
}
