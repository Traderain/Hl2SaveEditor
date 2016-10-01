using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using static System.Text.Encoding;
using static System.Math;
using static System.BitConverter;
using static System.Globalization.CultureInfo;
using System.Text.RegularExpressions;

namespace ListSave
{
    public class Flag
    {
        public string Ticks { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }

        public Flag(int t, float s, string type)
        {
            Ticks = t.ToString();
            Time = s.ToString(InvariantCulture) + "s";
            Type = type;
        }
    }

    public class Listsave
    {
        public static string Chaptername(int chapter)
        {
            string name = "Point Insertion";
            #region MapSwitch
            switch (chapter)
            {
                case 1:
                    {
                        name = "A Red Letter Day";
                        break;
                    }
                case 2:
                    {
                        name = "Route Kanal";
                        break;
                    }
                case 3:
                    {
                        name = "Water Hazard";
                        break;
                    }
                case 4:
                    {
                        name = "Black Mesa East";
                        break;
                    }
                case 5:
                    {
                        name = "We don't go to Ravenholm";
                        break;
                    }
                case 6:
                    {
                        name = "Highway 17";
                        break;
                    }
                case 7:
                    {
                        name = "Sandtraps";
                        break;
                    }
                case 8:
                    {
                        name = "Nova Prospekt";
                        break;
                    }
                case 9:
                    {
                        name = "Entanglement";
                        break;
                    }
                case 10:
                    {
                        name = "Anticitizen One";
                        break;
                    }
                case 11:
                    {
                        name = "Follow Freeman!";
                        break;
                    }
                case 12:
                    {
                        name = "Our Benefactors";
                        break;
                    }
                case 13:
                    {
                        name = "Dark Energy";
                        break;
                    }
                default:
                    {
                        name = "Mod/Unknown";
                        break;
                    }
            }
            #endregion
            return name;
        }
        public static SaveFile ParseFile(string file)
        {
            string[] cheats =
            {
                "host_timescale", "god", "sv_cheats", "buddha", "host_framerate", "sv_accelerate",
                "sv_airaccelerate", "noclip", "ent_fire"
            };
            var result = new SaveFile();
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                var magicnumber = 0x54541234;
                var versionNumber = 2;
                result.Tokentable = new List<Token>();
                var identifier = ASCII.GetString(br.ReadBytes(4)).TrimEnd('\0');
                if (identifier != "JSAV")
                    throw new Exception("Not a demo");
                result.FileName = file;
                result.Header = identifier;
                result.SaveVersion = (ToInt32(br.ReadBytes(4), 0));
                result.Size = (ToInt32(br.ReadBytes(4), 0));
                result.TokenCount = (ToInt32(br.ReadBytes(4), 0));
                result.Tokensize = (ToInt32(br.ReadBytes(4), 0));
                int bytesread = 0;
                
                string token_uns = ASCII.GetString(br.ReadBytes(result.Tokensize)).TrimEnd('\0');
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[\0]{2,}", options);
                var tempo = regex.Replace(token_uns, " ");
                List<Token> tempTokens = tempo.Split(' ').ToList().Where(x => x.Length > 0).Select(x => new Token(x, x)).ToList();
                result.Tokentable = tempTokens;
                Token commenToken = new Token("","");
                commenToken.Name = "Comment";
                br.ReadBytes(20);//Skip the /0's//Skip the actual comment its always empty so it has no point gg volvo
                /*
            ___________
    ---    //   |||   \\         
 -      __//____|||____\\____   
  ---  | _|      | VOLVO _  ||
 ---   |/ \______|______/ \_|| 
 -     _\_/_____________\_/____
                */
                var map = ASCII.GetString(br.ReadBytes(31)).TrimEnd('\0');//34 = P-letter
                var p = ASCII.GetString(br.ReadBytes(4)).Replace("0","");
                var hesteg = ASCII.GetString(br.ReadBytes(1)).TrimEnd('\0');
                var chapter = ASCII.GetString(br.ReadBytes(63)).Trim('\0');
                commenToken.Value = " ";
                result.Map = map;
                result.Chapter = Chaptername(int.Parse(Regex.Match(chapter.Split('_')[1], @"\d+").Value));

                return result;
            }
        }
    }

    [Serializable]
    public class SaveFile
    {
       public string FileName { get; set; }
       public string Header { get; set; }
       public int SaveVersion { get; set; }
       public int Size { get; set; }
       public int Tokensize { get; set; }
       public int TokenCount { get; set; }
       public List<Token> Tokentable { get; set;}
       public string Chapter { get; set; }
       public string Map { get; set; }
        
    }

    public class Token
    {
        public string Name;
        public string Value;

        public Token(string n, string v)
        {
            this.Name = n;
            this.Value = v;
        }
    }
}
