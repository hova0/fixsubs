using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace fixsubs
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: fixsubs filename <dictionaryfile>");
                Console.WriteLine("Return Error Codes:");
                Console.WriteLine("1        : Input file does not exist");
                Console.WriteLine("2        : Word Dictionary file does not exist");
                Console.WriteLine("-1       : Abnormal termination");
                return 0;
            }
            if (args.Length > 0 && !File.Exists(args[0]))
            {
                Console.WriteLine("File does not exist!");
                return 1;
            }
            string defaultwordlist = "english-words.10";
            if (args.Length > 1 )
            {
                defaultwordlist = args[1];
            }
            if (!File.Exists(defaultwordlist))
            {
                Console.WriteLine(String.Format("Default word list file {0} not found!", defaultwordlist));
                return 2;
            }

            HashSet<string> wordlist = new HashSet<string>();



                return -1;
        }
    }
}
