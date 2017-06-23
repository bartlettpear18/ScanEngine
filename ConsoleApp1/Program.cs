using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreScanner;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scan1 = new Scanner("SE4710");
            scan1.runTests();
        }

    }
}
