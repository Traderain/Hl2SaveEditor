using System;
using System.Diagnostics;
using System.IO;

namespace Listsave
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "λ - Sourceruns listsave by Traderain - λ";
            var datapath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                datapath = args[0];
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException)
                    datapath = Environment.CurrentDirectory + "\\test.sav";
            }
            if (datapath == null) return;
            {
                if (Path.GetExtension(datapath) != ".sav") return;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("--PARSED SAVE--");
                var a = Listsave.ParseFile(datapath);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-----------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(
                    $@"
Filename     :    {Path.GetFileName(a.FileName)}
Save header  :    {a.Header
                        }
Save version :    {a.SaveVersion}
Size         :    {a.Size}
Token size   :    {
                        a.Tokensize}
TokenCount   :    {a.TokenCount}
Map          :    {a.Map
                        }
Chapter      :    {a.Chapter}
");
                Console.ForegroundColor = ConsoleColor.Red;
                /*string msg = ($"Tokentable consists of {a.Tokentable.Count} elements:");
                Console.WriteLine(msg);
                for (var i = 0; i < a.Tokentable.Count; i++)
                {
                    var token = a.Tokentable[i];
                    Console.WriteLine((i == (a.Tokentable.Count - 1))
                        ? ("└───" + token.Value).PadLeft(msg.Length + ("└───" + token.Value).Length - 1)
                        : ("├───" + token.Value).PadLeft(msg.Length + ("├───" + token.Value).Length - 1));
                }*/
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-----------------------------------------------------------------");
                Console.ReadKey();
            }
        }
    }
}