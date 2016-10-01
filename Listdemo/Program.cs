using System;
using System.Diagnostics;
using System.IO;
using static System.Console;
using static System.Convert;
using ListSave;
using System.IO.Compression;

namespace LSave
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Title = "λ - Sourceruns listsave by Traderain - λ";
            var datapath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                datapath = args[0];
            }
            catch (Exception e)
            {

                if (e is IndexOutOfRangeException)
                {
                    datapath = Environment.CurrentDirectory + "\\test.sav";
                }
            }
            if (datapath == null) return;
            {
                if (Path.GetExtension(datapath) == ".sav")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("--PARSED SAVE--");
                    var a = Listsave.ParseFile(datapath);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($@"
Filename     :    {Path.GetFileName(a.FileName)}
Save header  :    {a.Header}
Save version :    {a.SaveVersion}
Size         :    {a.Size}
Token size   :    {a.Tokensize}
TokenCount   :    {a.TokenCount}
Map          :    {a.Map}
Chapter      :    {a.Chapter}
");
                    Console.ForegroundColor = ConsoleColor.Red;
                    string msg = ($"Tokentable consists of {a.Tokentable.Count} elements:");
                    Console.WriteLine(msg);
                    for (int i = 0; i < a.Tokentable.Count; i++)
                    {
                        var token = a.Tokentable[i];
                        Console.WriteLine((i == (a.Tokentable.Count - 1))
                            ? ("└───" + token.Value).PadLeft(msg.Length + ("└───" + token.Value).Length-1)
                            : ("├───" + token.Value).PadLeft(msg.Length + ("├───" + token.Value).Length-1));
                    }
                    //Console.WriteLine("└");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("-----------------------------------------------------------------");
                    Console.ReadKey();
                }
                else
                {
                    #region nofile
                    do
                    {
                        Clear();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("(Incorrect demo file/You didn't drag&drop!)");
                        WriteLine(@"
 ┌────────────────────────┐
 │Menu:                   │
 │1 :  Meaning of life.   │
 │2 :  Black mesa logo    │
 │3 :  Sourceruns.org     │
 │4 :  Credits            │
 │                        │
 └────────────────────────┘
");
                        ForegroundColor = ConsoleColor.Red;
                        var k = 0;
                        do
                        {
                            try
                            {
                                k = ToInt32(ReadLine());
                            }
                            catch (Exception e)
                            {

                                if (e is FormatException)
                                {
                                    Write("Please use the given numbers");
                                }

                            }

                        } while (k < 0 || k > 4);
                        switch (k)
                        {
                            case 1:
                            {
                                Clear();
                                WriteLine("42");
                                break;
                            }
                            case 2:
                            {
                                Clear();
                                #region bms
                                WriteLine(@"
                    .-;+$XHHHHHHX$+;-.
	        ,;X@@X%/;=----=: /%X@@X/
	      =$@@%=.              .=+H@X: 
	    -XMX:                       =XMX=
	   /@@:                           =H@+
	  %@X,                            .$@$
	 +@X.                               $@%
	-@@,                                .@@=
	%@%                                  +@$
	H@:                                   : @H
	H@:          : HHHHHHHHHHHHHHHHHHX,    =@H
	%@%         ;@M@@@@@@@@@@@@@@@@@H-   +@$
	=@@,        : @@@@@@@@@@@@@@@@@@@@@= .@@: 
	 +@X        : @@@@@@@@@@@@@@@M@@@@@@: %@%
	  $@$,      ;@@@@@@@@@@@@@@@@@M@@@@@@$.
	   +@@HHHHHHH@@@@@@@@@@@@@@@@@@@@@@@+
	    =X@@@@@@@@@@@@@@@@@@@@@@@@@@@@X=
	      : $@@@@@@@@@@@@@@@@@@@M@@@@$: 
	        ,;$@@@@@@@@@@@@@@@@@@X/-
	           .-;+$XXHHHHHX$+;-.");
                                break;
                                #endregion
                                ReadKey();
                            }
                            case 3:
                            {
                                Process.Start("http://www.sourceruns.org");
                                break;
                                Clear();
                            }
                            case 4:
                            {
                                WriteLine(@"
CBenni for the demoparser.
YaLTeR for the c++ version(idea).
Centaur1um for this awesome community.");
                                break;
                            }
                            default:
                                break;
                        }
                    } while (ReadKey(true).Key != ConsoleKey.Escape);
                    #endregion
                }
            }
        }
    }
}