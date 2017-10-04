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
                Console.WriteLine("File {0} does not exist!", args[0]);
                return 1;
            }
            string defaultwordlist = "english-words.10";
            if (args.Length > 1)
            {
                defaultwordlist = args[1];
            }
            if (!File.Exists(defaultwordlist))
            {
                Console.WriteLine(String.Format("Default word list file {0} not found!", defaultwordlist));
                return 2;
            }

            HashSet<string> wordlist = new HashSet<string>();
            wordlist = ReadWordFile(defaultwordlist);
            Console.WriteLine("Read in {0} words", wordlist.Count);

            System.IO.FileStream subsfile = new System.IO.FileStream(args[0], System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream newsubsfile = new System.IO.FileStream(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(args[0]), "output.srt"), System.IO.FileMode.Create, System.IO.FileAccess.Write);
            int subsfile_linenumber = 0;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(subsfile))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(newsubsfile))
                {
                    while (!sr.EndOfStream)
                    {
                        string subline = sr.ReadLine();
                        subsfile_linenumber++;
                        if (IgnoreLine(subline))
                        {
                            sw.WriteLine(subline);
                            continue;
                        }
                        //if(subsfile_linenumber == 90) 

                        string[] words = subline.ToLowerInvariant().Split( new char[] {' '} , System.StringSplitOptions.RemoveEmptyEntries).ToArray();

                        for (int i = 0; i < words.Length; i++)
                        {
                            bool wordsjoined = false;
                            if (words[i] == "e")
                                Console.WriteLine(words[i]);
                            for (int y = words.Length - 1; y >= i; y--)
                            {
                                string newword = String.Concat(words.Where((xz, yz) => yz >= i && yz <= y));
                                
                                if (wordlist.Contains(newword.TrimEnd(new char[] { '.', '?', ','})))
                                {
                                    sw.Write(newword);
                                    i += (y - i);
                                    wordsjoined = true;
                                }
                            }
                            if (!wordsjoined) {
                                if(words[i] == "l")
                                    words[i] = "I"; // OCR error seeing "I" as "L"
                                sw.Write(words[i]);
                            }
                            sw.Write(" ");
                            // word seperated by a space
                            //if (!wordlist.Contains(words[i]) && wordlist.Contains(words[i] + words[i+1]))
                            //{
                            //    Console.WriteLine("[ {0} ] : {1}", subsfile_linenumber, words[i] + words[i+1]);
                            //}
                        }
                        sw.WriteLine();
                    }
                }
            }


            return -1;
        }

        private static System.Text.RegularExpressions.Regex linenumber = new System.Text.RegularExpressions.Regex(@"^\d+$", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Singleline);
        // 00:01:22,949 --> 00:01:27,648
        private static System.Text.RegularExpressions.Regex srtTiming = new System.Text.RegularExpressions.Regex(@"^\d+:\d+:\d+,\d+", System.Text.RegularExpressions.RegexOptions.Compiled);


        public static bool IgnoreLine(string line)
        {
            if (line.Length == 0)
                return true;
            if (linenumber.IsMatch(line))
                return true;
            if (srtTiming.IsMatch(line))
                return true;
            return false;
        }



        public static HashSet<string> ReadWordFile(string filename)
        {
            HashSet<string> words = new HashSet<string>();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                string readword = "";
                while (!sr.EndOfStream)
                {
                    readword = sr.ReadLine();
                    if (!String.IsNullOrEmpty(readword) && !words.Contains(readword))
                    {
                        words.Add(readword);

                    }

                }

            }
            return words;
        }
    }
}
