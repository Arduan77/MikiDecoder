using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
//using System.Collections.Generic;
//using System.Linq;
using Miki;
//using System.Numerics;
using HPCsharp.ParallelAlgorithms;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.ComponentModel.Design;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO.Compression;
using System.IO;
//using System.ComponentModel.Design;
//using System.ComponentModel;
//using System.Collections.Generic;
//using CommunityToolkit.HighPerformance;
//using CommunityToolkit.Common;
//using LehmerGen;
//using System.ComponentModel;
//using System.Collections.Immutable;
//using System.Collections.Specialized;
//using System.IO.Compression;
//using Miki;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.ComponentModel.Design;
//using System.Collections;
//using static System.Runtime.InteropServices.JavaScript.JSType;
//using static System.Net.WebRequestMethods;

//format code ctrl-k + ctrl-f

namespace RandomPassword
{

    class RandomPassword
    {

        public static int DecoderMode = 100;
        public static int SysEx = 0;
        public static int OtherCode = 0;
        public static int FalsePosCount = 0;

        public static int DisplayMode = 3;
        public static int ActTaskCount = 0;
        public static int GlobalTaskCount = 0;
        

        public static int GlobalTaskCountAdjusted = 0;

        public static int A7zProcCount = 0;
        public static string A7zParam = "";
        public static int A7zSpeedLoop;
        public static int A7zAutoAdjust = 0;

        public static string H7zParam = "";

        public static string SessionPassCountDoneS = "0";
        //public static List<long> SessionPassCountDoneL = [0];
        public static string Done7zCountS = "0";
        //public static List<long> Done7zCountL = [0];

        private static readonly AsyncLocal<string> ExtrMyPass = new();
        //private static readonly AsyncLocal<ProcessStartInfo> Process7z = new();
        private static ProcessStartInfo Process7zN = new();
        private static readonly AsyncLocal<string> SesPassCountGenSAsync = new();
        //private static readonly AsyncLocal<List<long>> SesPassCountGenLAsync = new();

        //public static long MySpeedMin = 0;
        //public static long MySpeedTempMin = 0;
        //public static long MySpeedSec = 0;
        //public static long MySpeedTempSec = 0;

        public static string MySpeedMinStr = "0";
        public static string MySpeedTempMinStr = "0";
        public static string MySpeedSecStr = "0";
        public static string MySpeedTempSecStr = "0";

        public static DateTime StartTime = DateTime.Now;
        public static DateTime EndTime = DateTime.Now;
        //public static Stopwatch? Stopwatch7zLap;
        public static Stopwatch? Stopwatch7zFull;
        public static double LastLapTimer = 0;
        //public static double StableLapTime = 0;
        public static int LapCount7z = 0;
        

        public static string MyExit = "n";
        public static string CurrentOS = "";
        public static string ProgramDir = "";
        //public static string Add7z = "";
        //public static Stopwatch? StopwatchJob;
        //public static double ElapsNsJob = 0;
        public static string DictExitReason = "";

        public static string HashcatParam = "";
        public static string JTRParam = "";
        
        public static int Task7zInProgress = 0;
        public static int Task7zWaiting = 0;
        public static int Start7z = 1;
        public static int ActActive7z = 0;
        public static int MinActive7z = 0;
        public static int MaxActive7z = 0;
        public static int MaxActive7zTasksLoop = 0;
        public static int MinActive7zTasksLoop = 0;
        //public static double MinProcTime = double.MaxValue;
        //public static double MaxProcTime = 0;
        //public static int Max7zActive = 0;
        //public static int MinActive7zLoop = 0;
        //public static int MaxActive7zLoop = 0;
        public static List<string> PassRunList = new();
        public static object locking_List = new object();
        public static object locking_Progress = new object();
        public static object locking_Waiting = new object();
        public static object locking_Active7z = new object();
        public static object locking_Loop7z = new object();
        public static int Loop7zDone = 0;

        public static string SessionDictGenPasswords = "0";
        public static string DictionaryFirstIteration = "0";

        
        static void Main(string[] args)
        {
            Console.WriteLine(CheckOS());
            Console.OutputEncoding = Encoding.GetEncoding("ISO-8859-1");
            Console.WriteLine("Miki Calculator and Pseudo-Random Password Generator for decoding 7z (Qlocker)...");
            Console.WriteLine("");
            //TestCalculation();
            //ReadFiles();
            //GetPrimes();
            //MyPrimes.MyPrimes.GetPrimeFactorsPar();
            //MyPrimes.MyPrimes.GeneratePrimesPar();

            //GCDandLCG();
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("1.RndTo7z,   2.RndToHash7z,    3.RndToHashcat,    4.RndToJTR");
                Console.WriteLine("5.Mono/Polynomial to Decimal Converter,    6.Decimal to Mono/Polynomial to Converter");
                Console.WriteLine("7.Mono/Polynomial Calculator");
                Console.WriteLine("8.Decimal Calculator,    9.Decimal Calculator Radom");
                Console.WriteLine("0.Test Generator");
                DecoderMode = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

                if (DecoderMode != 0 & DecoderMode != 1 & DecoderMode != 2 & DecoderMode != 3 & DecoderMode != 4 & DecoderMode != 5 & DecoderMode != 6 & DecoderMode != 7 & DecoderMode != 8 & DecoderMode != 9)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Wrong option, try again...");
                }
                else { Console.WriteLine(""); break; }
            }

            if (DecoderMode == 8)
            {
                Console.WriteLine("Calculator");
                Miki.Miki.Calculator();
                System.Environment.Exit(0);

            }
            if (DecoderMode == 9)
            {
                Console.WriteLine("Calculator Random Test");
                Miki.Miki.TestRandom();
                System.Environment.Exit(0);
            }

            if (DecoderMode == 5)
            {
                Console.WriteLine("Mono/Polynomial to Decimal Converter");
                Polynomials.Polynomials.ReadConfig();
                Polynomials.Polynomials.MakeConvTable();
                Polynomials.Polynomials.WordToDig();
                System.Environment.Exit(0);
            }

            if (DecoderMode == 6)
            {
                Console.WriteLine("Decimal to Mono/Polynomial Converter");
                Polynomials.Polynomials.ReadConfig();
                Polynomials.Polynomials.MakeConvTable();
                Polynomials.Polynomials.DigToWord();
                System.Environment.Exit(0);
            }
            if (DecoderMode == 7)
            {
                Console.WriteLine("Mono/Polynomial Calculator");
                Polynomials.Polynomials.ReadConfig();
                Polynomials.Polynomials.MakeConvTable();
                Polynomials.Polynomials.CalculatorPoly();
                System.Environment.Exit(0);
            }

            ProgramDir = Environment.CurrentDirectory;
            ReadConfig();

            //ReadFiles();

            string ProgressFile = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.txt");

            List<string> ProgressStringList = new List<string>(File.ReadAllLines(ProgressFile));
            //to do
            //max pool
            //TempRemoved....

            LehmerGen.Passwords.ProgressIterToDoString = Miki.CalcStringExt.DeSplitString(ProgressStringList[0]); //to do
            LehmerGen.Passwords.ProgressPoolMaxString = Miki.CalcStringExt.DeSplitString(ProgressStringList[1]); //max pool

            for (int i = 2; i < ProgressStringList.Count; i++) //restore TempRemoved passwords from SessionProgress.txt
            { LehmerGen.Passwords.TempRemoved.Add(ProgressStringList[i]); }

            //Miki.CalcConvert.StringToLongList(in LehmerGen.Passwords.ProgressIterDoneString, 18, out LehmerGen.Passwords.ProgressIterDoneList18);
            Miki.CalcConvert.StringToLongList(in LehmerGen.Passwords.ProgressPoolMaxString, 18, out LehmerGen.Passwords.ProgressPoolMaxList);

            //LehmerGen.Passwords.ProgressIterToDoString = Miki.CalcStrings.Add(LehmerGen.Passwords.ProgressIterDoneString, "1")[0];
            Miki.CalcConvert.StringToLongList(in LehmerGen.Passwords.ProgressIterToDoString, 18, out LehmerGen.Passwords.ProgressIterToDoList18);
            LehmerGen.Passwords.ProgressIterStartString = LehmerGen.Passwords.ProgressIterToDoString;
            DictionaryFirstIteration = LehmerGen.Passwords.ProgressIterToDoString;

            /*
             
             LehmerGen.Passwords.ProgressIterDoneString = Miki.CalcStringExt.DeSplitString(ProgressStringList[0]);
            LehmerGen.Passwords.ProgressPoolMaxString = Miki.CalcStringExt.DeSplitString(ProgressStringList[1]);

            for (int i = 2; i < ProgressStringList.Count; i++) //restore TempRemoved passwords from SessionProgress.txt
            { LehmerGen.Passwords.TempRemoved.Add(ProgressStringList[i]); }

            Miki.CalcConvert.StringToLongList(in LehmerGen.Passwords.ProgressIterDoneString, 18, out LehmerGen.Passwords.ProgressIterDoneList18);
            Miki.CalcConvert.StringToLongList(in LehmerGen.Passwords.ProgressPoolMaxString, 18, out LehmerGen.Passwords.ProgressPoolMaxList);

            LehmerGen.Passwords.ProgressIterToDoString = Miki.CalcStrings.Add(LehmerGen.Passwords.ProgressIterDoneString, "1")[0];
            Miki.CalcConvert.StringToLongList(in LehmerGen.Passwords.ProgressIterToDoString, 18, out LehmerGen.Passwords.ProgressIterToDoList18);
            LehmerGen.Passwords.ProgressIterStartString = LehmerGen.Passwords.ProgressIterToDoString;
            DictionaryFirstIteration = LehmerGen.Passwords.ProgressIterToDoString;
             
             */



            LehmerGen.Passwords.InitializeGenerator();

            Console.WriteLine("Session restored...");
            Console.WriteLine(string.Format("Iteration to do:   {0}", Miki.CalcStringExt.SplitString(LehmerGen.Passwords.ProgressIterToDoString)));

            Console.WriteLine("");
            Console.WriteLine("Now You can set:");
            Console.WriteLine("If 7z - Task Count,");
            Console.WriteLine("If HC / JTR - Dictionary size,");
            Console.WriteLine("If Test - Password checking and Loops count for display,");
            Console.WriteLine("OR..........");
            Console.WriteLine("Use Config settings??? (y/n) ????");
            string UseConfig = Console.ReadKey().KeyChar.ToString();
            Console.WriteLine("");

            if (UseConfig == "y")
            {
                if (DecoderMode == 1)
                {
                    Task.Run(() => PutRandomPassTo7z(RandomPassword.GlobalTaskCount)).ConfigureAwait(false);
                }
                if (DecoderMode == 2)
                {
                    Task.Run(() => RunHash7zProgram()).ConfigureAwait(false);
                }
                if (DecoderMode == 3)
                {
                    Task.Run(() => RunHashcatProgram()).ConfigureAwait(false);
                }
                if (DecoderMode == 4)
                {
                    Task.Run(() => RunJTRProgram()).ConfigureAwait(false);
                }
                if (DecoderMode == 0)
                {
                    LehmerGen.Passwords.TestGenerator(LehmerGen.Passwords.TestPassCheck, LehmerGen.Passwords.TestPassLoops);
                }
            }
            else
            {
                if (DecoderMode == 1)
                {
                    Console.WriteLine("How much 7z processes - confirm ENTER:");
                    RandomPassword.GlobalTaskCount = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    Task.Run(() => PutRandomPassTo7z(RandomPassword.GlobalTaskCount)).ConfigureAwait(false);
                }

                if (DecoderMode == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much passwords in Dictionary file???");
                    LehmerGen.Passwords.HCDictSize = Convert.ToInt32(Miki.CalcStringExt.DeSplitString(Console.ReadLine()));
                    Console.WriteLine();
                    Task.Run(() => RunHashcatProgram()).ConfigureAwait(false);
                }

                if (DecoderMode == 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much passwords in Dictionary file???");
                    LehmerGen.Passwords.HCDictSize = Convert.ToInt32(Miki.CalcStringExt.DeSplitString(Console.ReadLine()));
                    Console.WriteLine();
                    Task.Run(() => RunJTRProgram()).ConfigureAwait(false);
                }
                if (DecoderMode == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Use password checking??? (y/n)   :");
                    string TestOpt = Console.ReadKey().KeyChar.ToString();
                    if (TestOpt == "y") { LehmerGen.Passwords.TestPassCheck = 1; } else { LehmerGen.Passwords.TestPassCheck = 0; }
                    Console.WriteLine();
                    Console.WriteLine("Loops count for display result:  ");
                    LehmerGen.Passwords.TestPassLoops = Convert.ToInt32(Miki.CalcStringExt.DeSplitString(Miki.CalcStringExt.DeSplitString(Console.ReadLine())));
                    LehmerGen.Passwords.TestGenerator(LehmerGen.Passwords.TestPassCheck, LehmerGen.Passwords.TestPassLoops);
                }
            }

            //run KeyPress monitor
            int RefreshKeyPress = MonitorKeypress(); //Ctrl+P for reload monitor
            while (RefreshKeyPress == 0)
            {
                RefreshKeyPress = MonitorKeypress();
            }

        }

        
        public static void TestCalculation()
        {
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            Stopwatch swTest = new Stopwatch();
            

            string str1 = "999999999999999999999999999999999999999999999999999999000000000999999999";
            string str2 = "9999999999999999999999999999000000000999999999";
            Miki.CalcConvert.StringToLongList(str1, 18, out List<long> Li1);
            Miki.CalcConvert.StringToLongList(str2, 18, out List<long> Li2);
            var Test1 = Miki.CalcLists.Mul(Li2, Li1);
            Miki.CalcConvert.LongListToString(Test1, "000000000000000000", out string Test1S);
            Console.WriteLine(Test1S);

            Miki.CalcConvert.StringToLongList(str1, 9, out List<long> Li3);
            Miki.CalcConvert.StringToLongList(str2, 9, out List<long> Li4);
            var Test2 = Miki.CalcLists.MulGood(Li3, 9, Li4, 9, 18);
            Miki.CalcConvert.LongListToString(Test2, "000000000000000000", out string Test2S);
            Console.WriteLine(Test2S);
            swTest.Start();
            DateTime MyDateTime = DateTime.Now;
            double MyElaps1;
            double MyElaps2;
            sw1.Start();
            for (int i = 0; i <= 1000000; i++)
            {
                swTest.Stop();
                MyElaps1 = swTest.Elapsed.TotalMilliseconds;
                swTest.Start();
            }
;           sw1.Stop();

            sw2.Start();

            for (int i = 0; i <= 1000000; i++)
            {
                //EndDateTime = DateTime.Now;
                TimeSpan aaa = DateTime.Now.Subtract(MyDateTime);
                MyElaps2 = aaa.TotalMilliseconds;
                
            }
            sw2.Stop();

            Console.WriteLine(sw1.Elapsed.TotalMilliseconds);
            Console.WriteLine(sw2.Elapsed.TotalMilliseconds);
        }

        static void ReadFiles()
        {
            string OutputsDir = System.IO.Path.Combine(ProgramDir, "Outputy");
            string OutputsFile = System.IO.Path.Combine(ProgramDir, "Outputy", "Outputs.txt");
            string[] files = Directory.GetFiles(OutputsDir);
            List<string> FilesCont = [];
            foreach (string file in files)
            {
                Console.WriteLine(file);
                List<string> FileCont = new List<string>(File.ReadAllLines(file));
                FilesCont.AddRange(FileCont);
            }
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");
            string[] ProgressArray = FilesCont.ToArray();

            File.WriteAllLines(OutputsFile, FilesCont, isoLatin1Encoding);

        }

        public static void ReadConfig()
        {
            string ReadString;
            List<string> ConfigStringList = new();
            List<List<char>> MaskTableTemp = new();
            List<char> CharactersListLocal = new();
            string ConfigFile = System.IO.Path.Combine(ProgramDir, "Config", "Config.txt");

            using (System.IO.StreamReader Cfile = new System.IO.StreamReader(ConfigFile))
            {
                while ((ReadString = Cfile.ReadLine()) != null)
                {
                    if (ReadString != "" && ReadString[..1] != "#")
                    {
                        if (ReadString.Contains("PassCharacters="))
                        {
                            string TempString = ReadString.Replace("PassCharacters=", "").Trim();
                            char[] characters = TempString.ToCharArray();
                            Array.Sort(characters);
                            CharactersListLocal = new List<char>(characters);
                        }
                        if (ReadString.Contains("PassMinLength="))
                        {
                            LehmerGen.Passwords.PassMinLength = Convert.ToInt32(ReadString.Replace("PassMinLength=", "").Trim());
                        }
                        if (ReadString.Contains("PassMaxLength="))
                        {
                            LehmerGen.Passwords.PassMaxLength = Convert.ToInt32(ReadString.Replace("PassMaxLength=", "").Trim());
                            for (int c = 0; c < LehmerGen.Passwords.PassMaxLength; c++)
                            { LehmerGen.Passwords.MaskTable.Add(CharactersListLocal); }
                        }
                        if (ReadString.Contains("FillToLength="))
                        {
                            LehmerGen.Passwords.FillToLength = Convert.ToInt32(ReadString.Replace("FillToLength=", "").Trim());
                        }
                        if (ReadString.Contains("IncrementBase="))
                        {
                            LehmerGen.Passwords.IncrementBase = ReadString.Replace("IncrementBase=", "").Trim();
                        }
                        if (ReadString.Contains("IncrementPower="))
                        {
                            LehmerGen.Passwords.IncrementPower = ReadString.Replace("IncrementPower=", "").Trim();
                        }
                        if (ReadString.Contains("CounterDisplay="))
                        {
                            LehmerGen.Passwords.CounterDisplay = Convert.ToInt32(ReadString.Replace("CounterDisplay=", "").Trim());
                        }
                        //////////7z//////////
                        if (ReadString.Contains("A7zParam="))
                        {
                            A7zParam = ReadString.Replace("A7zParam=", "").Trim();
                        }
                        if (ReadString.Contains("A7zSpeedLoop="))
                        {
                            A7zSpeedLoop = Convert.ToInt32(ReadString.Replace("A7zSpeedLoop=", "").Trim());
                        }
                        if (ReadString.Contains("A7zProcCount="))
                        {
                            A7zProcCount = Convert.ToInt32(ReadString.Replace("A7zProcCount=", "").Trim());
                            GlobalTaskCount = A7zProcCount;
                        }
                        if (ReadString.Contains("A7zAutoAdjust="))
                        {
                            A7zAutoAdjust = Convert.ToInt32(ReadString.Replace("A7zAutoAdjust=", "").Trim());
                        }
                        if (ReadString.Contains("A7zDictSize="))
                        {
                            LehmerGen.Passwords.A7zDictSize = Convert.ToInt32(ReadString.Replace("A7zDictSize=", "").Trim());
                        }
                        /////////////Hash7z///////////////
                        if (ReadString.Contains("H7zParam="))
                        {
                            H7zParam = ReadString.Replace("H7zParam=", "").Trim();
                        }
                        if (ReadString.Contains("H7zDictSize="))
                        {
                            LehmerGen.Passwords.H7zDictSize = Convert.ToInt32(ReadString.Replace("H7zDictSize=", "").Trim());
                        }



                        if (ReadString.Contains("HCParam="))
                        {
                            HashcatParam = ReadString.Replace("HCParam=", "").Trim();
                        }
                        if (ReadString.Contains("JTRParam="))
                        {
                            JTRParam = ReadString.Replace("JTRParam=", "").Trim();
                        }
                        if (ReadString.Contains("HCDictSize="))
                        {
                            LehmerGen.Passwords.HCDictSize = Convert.ToInt32(ReadString.Replace("HCDictSize=", "").Trim());
                        }
                        if (ReadString.Contains("JTRDictSize="))
                        {
                            LehmerGen.Passwords.JTRDictSize = Convert.ToInt32(ReadString.Replace("JTRDictSize=", "").Trim());
                        }
                        if (ReadString.Contains("PassCheck="))
                        {
                            LehmerGen.Passwords.TestPassCheck = Convert.ToInt32(ReadString.Replace("PassCheck=", "").Trim());
                        }
                        if (ReadString.Contains("PassLoops="))
                        {
                            LehmerGen.Passwords.TestPassLoops = Convert.ToInt32(ReadString.Replace("PassLoops=", "").Trim());
                        }

                        if (ReadString[..1] == "?")
                        {
                            int IndexOfEq = ReadString.IndexOf('=');
                            int MaskNumber = Convert.ToInt32(ReadString[1..IndexOfEq].Trim());
                            char[] ThisMask = ReadString[(ReadString.IndexOf('=') + 1)..].ToArray();
                            Array.Sort(ThisMask); //must be, first char is 0, next 1... aso...
                                                  //MaskTableTemp.Add(ThisMask.ToList());
                            LehmerGen.Passwords.MaskTable[MaskNumber - 1] = ThisMask.ToList();
                        }

                    }
                }
                Cfile.Dispose();
                Cfile.Close();
            }

            Console.WriteLine("Config loaded...");
        }
        public static int MonitorKeypress()
        {
            string InfoString1 = "1. Visual Mode, 2. Show status, 3. Silent Mode";
            string InfoString2 = "";
            string InfoString3 = "";
            string InfoString4 = "";
            string InfoString5 = "End program if cracked...";
            string InfoString6 = "";
            string InfoString = "";
            if (A7zAutoAdjust == 0) { InfoString6 = "Auto adjust function is OFF";  } else { InfoString6 = "Auto adjust function is ON"; }
            if (DecoderMode == 1) { InfoString2 = "+. 7z Task++, -. 7z Task--, /. Task=2, *. Task=From Config"; }
            InfoString3 = "n. End if cracked, e. Safe End, Ctrl+s. Reload Config, Ctrl+t. Toggle AutoAdjust";
            if (DecoderMode == 1) { InfoString4 = string.Format("Max number of active tasks is set to: {0}", RandomPassword.GlobalTaskCount); }

            InfoString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", InfoString1, Environment.NewLine, InfoString2, Environment.NewLine, InfoString3, Environment.NewLine, InfoString4, Environment.NewLine, InfoString6, Environment.NewLine, InfoString5);

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(InfoString);
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");

            int PrevGlobalTaskCountAdjusted = 0;
            while (true)
            {
                // true hides the pressed character from the console
                ConsoleKeyInfo cki;
                bool DisplayInfo = false;
                bool Modifiers = false;

                cki = Console.ReadKey(true);
                string ckiString = cki.Key.ToString();
                
                if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if (cki.Key !=0 && ckiString == "S")//((cki.Key & ConsoleKey.S) != 0)
                    {
                        int TempAutoAdjust = A7zAutoAdjust;
                        ReadConfig();
                        A7zAutoAdjust = TempAutoAdjust;
                        DisplayInfo = true;
                        Modifiers = true;
                        //break;
                    }
                    if (cki.Key != 0 && ckiString == "T")
                    {
                        if (A7zAutoAdjust == 1)
                        {
                            A7zAutoAdjust--;
                            DisplayInfo = true;
                            Modifiers = true;
                        }
                        else
                        {
                            A7zAutoAdjust++;
                            DisplayInfo = true;
                            Modifiers = true;
                            Console.WriteLine("Auto adjust function is ON");
                        }
                        //break;
                    }

                }

                if (Modifiers == false)
                {
                    char SwitchKey = cki.KeyChar;

                    switch (SwitchKey)
                    {
                        case '1':
                            DisplayMode = 1;
                            DisplayInfo = true;
                            break;
                        case '2':
                            DisplayMode = 2;
                            DisplayInfo = true;
                            break;
                        case '3':
                            DisplayMode = 3;
                            DisplayInfo = true;
                            break;
                        case '+':
                            RandomPassword.GlobalTaskCount++;
                            DisplayInfo = true;
                            break;
                        case '*':
                            RandomPassword.GlobalTaskCount = RandomPassword.A7zProcCount;
                            DisplayInfo = true;
                            GlobalTaskCountAdjusted = PrevGlobalTaskCountAdjusted;
                            break;
                        case '/':
                            RandomPassword.GlobalTaskCount = 2;
                            DisplayInfo = true;
                            PrevGlobalTaskCountAdjusted = GlobalTaskCountAdjusted;
                            break;
                        case '-':
                            if (RandomPassword.GlobalTaskCount > 1)
                            {
                                RandomPassword.GlobalTaskCount--;
                            }
                            DisplayInfo = true;
                            break;
                        case 'e':
                            RandomPassword.MyExit = "e";
                            DisplayInfo = true;
                            break;
                        case 'n':
                            RandomPassword.MyExit = "n";
                            DisplayInfo = true;
                            break;
                        case 'c':
                            RandomPassword.MyExit = "c";
                            DisplayInfo = true;
                            break;
                        default:
                            DisplayInfo = false;
                            Stopwatch WaitWatch = Stopwatch.StartNew();
                            Console.WriteLine("Waiting 5sec for key...");
                            bool KeyPressed = false;
                            while (WaitWatch.ElapsedMilliseconds <= 5000)
                            {
                                Thread.Sleep(1000);
                                if (Console.KeyAvailable)
                                {
                                    KeyPressed = true;
                                    break;
                                }
                            }
                            WaitWatch.Stop();

                            if (KeyPressed == true)
                            {
                                Thread.Sleep(100);
                            }
                            else
                            {
                                Console.WriteLine("End waiting for key...");
                            }
                            break;

                    }
                }
                Modifiers = false;
                if (DecoderMode == 1) { InfoString4 = string.Format("Number of active tasks is set to: {0}", RandomPassword.GlobalTaskCount); }

                if (MyExit == "n")
                { InfoString5 = "End program if cracked..."; }
                if (MyExit == "e")
                { InfoString5 = "Sheduled Safe End... Waiting for end of dictionary..."; }
                if (MyExit == "c")
                { InfoString5 = "Fast End..."; }
                if (A7zAutoAdjust == 0) { InfoString6 = "Auto adjust function is OFF"; } else { InfoString6 = "Auto adjust function is ON"; }


                if (DisplayInfo == true)
                {
                    Console.WriteLine("");
                    InfoString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", InfoString1, Environment.NewLine, InfoString2, Environment.NewLine, InfoString3, Environment.NewLine, InfoString4, Environment.NewLine, InfoString6, Environment.NewLine, InfoString5);
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine(InfoString);
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine("");
                }

            }

            return 0;
        }


        public static async Task CheckForDictionary(int DictionarySize, string DictFile, string DictFileNext, string JobKind, string SesPassCountDoneS)
        {
            if (File.Exists(DictFile) && File.Exists(DictFileNext))
            {
                Console.WriteLine("Both dictionaries exists...");
                DictExitReason = " - OK.";
                return;
            }

            if (!File.Exists(DictFile) && !File.Exists(DictFileNext))
            {
                Console.WriteLine("Both dictionaries do not exist...");
                Task<string> GenDict = GenerateDictionary(DictFile, "Dictionary.txt", DictionarySize, JobKind, SesPassCountDoneS);
                //Task.WaitAny(GenDict);
                await Task.WhenAny(GenDict);
                DictExitReason = GenDict.Result;
                return;

            }

            if (!File.Exists(DictFile) && File.Exists(DictFileNext))
            {
                Console.WriteLine("No Dictionary.txt, rename DictionaryNext.txt to Dictionary.txt...");
                //File.Move may be not safe becouse of buffering
                Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");
                List<string> TempDictFileNextPass = new List<string>(File.ReadAllLines(DictFileNext)); //read old DictionaryNext.txt

                using (FileStream fs = new FileStream(DictFile, FileMode.CreateNew)) //create new Dictionary.txt
                {
                    using (StreamWriter writer = new StreamWriter(fs, isoLatin1Encoding))
                    {
                        for (int i = 0; i < TempDictFileNextPass.Count; i++)
                        {
                            writer.WriteLine(TempDictFileNextPass[i]); //write all lines from DictionaryNext.txt
                        }
                        writer.Flush();
                        fs.Flush(true);
                    }
                }
                File.Delete(DictFileNext); //delete DictionaryNext.txt

                //System.IO.File.Move(DictFileNext, DictFile);
                DictExitReason = " - OK.";
                return;
            }

            if (File.Exists(DictFile) && !File.Exists(DictFileNext))
            {
                Console.WriteLine("No DictionaryNext.txt, Generating in progress...");
                Task<string> GenDict = GenerateDictionary(DictFileNext, "DictionaryNext.txt", DictionarySize, JobKind, SesPassCountDoneS);
                //Task.WaitAll(GenDict);
                await Task.WhenAny(GenDict);
                DictExitReason = GenDict.Result;
                return;
            }
            DictExitReason = " - OK.";
            return;
        }

        public static async Task<string> GenerateDictionary(string DictFile, string DictionaryName, int DictionarySize, string JobKind, string SesPassCountDoneS)
        {

            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            string ProgressIterDoneStringLocal = "";

            Console.WriteLine("");
            Console.WriteLine("New " + DictionaryName + " file is generated: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            List<string> PassList;
            if (LehmerGen.Passwords.IsUnary != 1)
            { PassList = LehmerGen.Passwords.PassListFromDigByTabN(DictionarySize); } //build List of Passwords
            else
            { PassList = LehmerGen.Passwords.PassListFromDigByTabU(DictionarySize); }


            //File.WriteAllLines(DictFile, [.. PassList], isoLatin1Encoding); //no safe, no flushing
            
            //Console.WriteLine("Save start");
            using (FileStream fs = new FileStream(DictFile, FileMode.CreateNew))
            {
                using (StreamWriter writer = new StreamWriter(fs, isoLatin1Encoding))
                {
                    for (int i = 0; i < DictionarySize; i++)
                    {
                        writer.WriteLine(PassList[i]);
                    }
                    writer.Flush();
                    fs.Flush(true);
                }
            }
            //Console.WriteLine("Save stop");
            


            PassList.Clear();

            ProgressIterDoneStringLocal = LehmerGen.Passwords.ProgressIterDoneString; //last done iteration
            //for display info, how much we done in this session
            string IterDoneLocal = Miki.CalcStrings.Sub(ProgressIterDoneStringLocal, LehmerGen.Passwords.ProgressIterStartString)[0];
            IterDoneLocal = Miki.CalcStrings.Add(IterDoneLocal, "1")[0];
            //

            Console.WriteLine("");
            Console.WriteLine("New " + DictionaryName + " file is done:      " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("");
            
            
            string String1 = Miki.CalcStringExt.SplitString(LehmerGen.Passwords.ProgressPoolMaxString);
            int String1Len = String1.Length;
            string String2 = Miki.CalcStringExt.SplitString(LehmerGen.Passwords.ProgressIterDoneString).PadLeft(String1Len);
            string String3 = Miki.CalcStringExt.SplitString(LehmerGen.Passwords.ProgressIterToDoString).PadLeft(String1Len);
            string String4 = Miki.CalcStringExt.SplitString(ProgressIterDoneStringLocal).PadLeft(String1Len);
            string String5 = Miki.CalcStringExt.SplitString(IterDoneLocal).PadLeft(String1Len);
            string String6 = Miki.CalcStringExt.SplitString(Convert.ToString(DictionarySize)).PadLeft(String1Len);
            string String7 = Miki.CalcStringExt.SplitString(SesPassCountDoneS).PadLeft(String1Len);
            SessionDictGenPasswords = Miki.CalcStrings.Add(SessionDictGenPasswords, Convert.ToString(DictionarySize))[0];
            string String8 = Miki.CalcStringExt.SplitString(SessionDictGenPasswords).PadLeft(String1Len);
            string String9 = Miki.CalcStringExt.SplitString(DictionaryFirstIteration).PadLeft(String1Len);

            Console.WriteLine("DICTIONARY " + DictionaryName + " AND SESSION INFO ->");
            Console.WriteLine("Dictionary size:                    " + String6);
            Console.WriteLine("First iteration in current dict.:   " + String9);
            Console.WriteLine("Last iteration in current dict.:    " + String2);
            Console.WriteLine("First iteration in next dict.:      " + String3);
            Console.WriteLine("Session done iterations:            " + String5);
            Console.WriteLine("Saved, Session Progress iteration:  " + String4);
            Console.WriteLine("Max Session Progress iteration:     " + String1);
            Console.WriteLine("Session dict. generated passwords:  " + String8);
            Console.WriteLine("Session checked passwords:          " + String7);
            Console.WriteLine("");
            //in progress we save first iteration to do, so it must be ProgressIterDoneStringLocal + 1
            string ProgressToSave = Miki.CalcStrings.Add(ProgressIterDoneStringLocal, "1")[0];
            SaveProgress(ProgressToSave, LehmerGen.Passwords.ProgressPoolMaxString, JobKind);

            DictionaryFirstIteration = LehmerGen.Passwords.ProgressIterToDoString;
            LehmerGen.Passwords.ProgressIterDoneString = ProgressIterDoneStringLocal;
            LehmerGen.Passwords.ProgressIterToDoString = Miki.CalcStrings.Add(LehmerGen.Passwords.ProgressIterDoneString, "1")[0];

            Miki.CalcCompare.StringBigger(in ProgressIterDoneStringLocal, in LehmerGen.Passwords.ProgressPoolMaxString, out int StringBigger);
            if (StringBigger < 2)
            {
                RandomPassword.MyExit = "ex";
                return " - Exhausted Pool.";
            }
            return " - OK.";
        }

        private static async Task PutRandomPassTo7z(int StartTaskCount)
        {
            string ActOS = CheckOS();
            string ZipFile = "";
            string DictFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
            string DictFileNext = System.IO.Path.Combine(ProgramDir, "Dictionary", "DictionaryNext.txt");
            int l = 0;

            if (ActOS == "Windows")
            {
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za.exe");
            }
            else
            {
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za");
            }

            string CodedFile = System.IO.Path.Combine(ProgramDir, "Coded", "Coded.7z");

            MakeProcess7z(ZipFile);

            int Task7zCount = RandomPassword.GlobalTaskCount;

            Console.WriteLine("Cracking with 7z started...");

            SesPassCountGenSAsync.Value = "0";

            //string ExitReason1 = "Safe exit";

            await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.A7zDictSize, DictFile, DictFileNext, "7z", SessionPassCountDoneS)).ConfigureAwait(true);
            string ExitReason2 = DictExitReason;
            List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

            //StartTime = DateTime.Now;
            //Stopwatch7zLap = Stopwatch.StartNew();
            //LapCount7z = 0;
            
            MinActive7z = GlobalTaskCount;
            Stopwatch7zFull = Stopwatch.StartNew();
            while (true)
            {

                await Task.Delay(1);
                if (A7zAutoAdjust == 1)
                {
                    if (Start7z == 1)
                    {
                        Task7zCount = GlobalTaskCount;
                        GlobalTaskCountAdjusted = GlobalTaskCount;
                    } //only once
                    else
                    {
                        if (Task7zCount == 0)
                        { 
                            Task7zCount = GlobalTaskCount; 
                            GlobalTaskCountAdjusted = GlobalTaskCount; 
                        }
                        else //Count TaskCount and limit it to GlobalTaskCount
                        {
                            Task7zCount = GlobalTaskCountAdjusted;
                            if (Task7zCount > GlobalTaskCount) //limit to GlobalTaskCount
                            { Task7zCount = GlobalTaskCount; }
                        }
                    }
                }
                else
                {
                    Task7zCount = GlobalTaskCount;
                    GlobalTaskCountAdjusted = GlobalTaskCount;
                }

                if (RandomPassword.MyExit == "c") //End now, don't make new dictionary, but wait for end of all tasks
                {
                    while (PassRunList.Count != 0)
                    {
                        await Task.Delay(5);
                    }
                    Console.WriteLine("Fast safe exit...");
                    return;
                }

                while (PassRunList.Count < Task7zCount) //PassRunList.Count //Task7zInProgress
                {
                    if (l == 0) //there is a time to generate DictionaryNext.txt, not waiting...
                    {
                        _ = Task.Run(async () => await CheckForDictionary(LehmerGen.Passwords.A7zDictSize, DictFile, DictFileNext, "7z", SesPassCountGenSAsync.Value)).ConfigureAwait(false);
                    }

                    ExtrMyPass.Value = TempDictList[l];
                    SesPassCountGenSAsync.Value = Miki.CalcStrings.Add("1", SesPassCountGenSAsync.Value)[0];
                    lock (locking_List)
                    {
                        PassRunList.Add(ExtrMyPass.Value);
                    }
                    Task.Run(async () => await ExtractFile(CodedFile, ExtrMyPass.Value, SesPassCountGenSAsync.Value).ConfigureAwait(false));
                    
                    SessionPassCountDoneS = SesPassCountGenSAsync.Value;
                    l++;

                    if (l == TempDictList.Count) //here we must have Dictionary.txt, so rename DictionaryNext.txt or generate it
                    {
                        File.Delete(DictFile);
                        Console.WriteLine();
                        Console.WriteLine("Dictionary.txt checked... deleted...");
                        if (RandomPassword.MyExit == "e") //Scheduled end, don't make new dictionary, but wait for end of all tasks
                        {
                            while (PassRunList.Count != 0)
                            {
                                await Task.Delay(5);
                            }
                            Console.WriteLine("Scheduled safe exit...");
                            return;
                        }
                        if (RandomPassword.MyExit == "ex") //Scheduled end, don't make new dictionary, but wait for end of all tasks
                        {
                            while (PassRunList.Count != 0)
                            {
                                await Task.Delay(5);
                            }
                            Console.WriteLine("Exhausted pool...");
                            return;
                        }
                        await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.A7zDictSize, DictFile, DictFileNext, "7z", SesPassCountGenSAsync.Value)).ConfigureAwait(true);
                        TempDictList = new List<string>(File.ReadAllLines(DictFile));
                        l = 0;
                        break;
                    }
                }
            }
        }


        private static async void RunJTRProgram()
        {
            StartTime = DateTime.Now;
            string JTRFile;

            if (RandomPassword.CurrentOS == "Windows")
            {
                JTRFile = System.IO.Path.Combine(ProgramDir, "JTR", "run", "john.exe");
            }
            else
            {
                JTRFile = System.IO.Path.Combine(ProgramDir, "JTR", "run", "john");
            }

            string DictFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
            string DictFileNext = System.IO.Path.Combine(ProgramDir, "Dictionary", "DictionaryNext.txt");
            string JTRDir = System.IO.Path.Combine(ProgramDir, "JTR", "run");
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            string ExitReason1 = "Safe exit";
            string ExitReason2 = "";

            //ProgressString is a real password number, eg. 1 is for 000000000...0, and it is last chcecked password
            //but password 000000...0 cames from iteration 0, so if we want to check iteration 0, ProgressString should be -1

            //StopwatchJob = Stopwatch.StartNew();
            try
            {
                while (true)
                {

                    await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.JTRDictSize, DictFile, DictFileNext, "JTR", SessionPassCountDoneS)).ConfigureAwait(true);
                    ExitReason2 = DictExitReason;

                    var swHC = Stopwatch.StartNew();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Start JTR: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");
                    Directory.SetCurrentDirectory(JTRDir);

                    ProcessStartInfo pro = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        CreateNoWindow = false,
                        FileName = JTRFile
                    };
                    string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
                    string MyHash = System.IO.Path.Combine(ProgramDir, "Coded", "Hash.txt");
                    string MyResult = System.IO.Path.Combine(ProgramDir, "JTR", "run", "john.pot");
                    pro.Arguments = string.Format("{0} --wordlist={1} {2}", JTRParam, DictFile, MyHash);

                    Process x = Process.Start(pro);

                    Thread.Sleep(10000);
                    if (RandomPassword.MyExit == "n")
                    {
                        await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.JTRDictSize, DictFile, DictFileNext, "JTR", SessionPassCountDoneS)).ConfigureAwait(true);
                        ExitReason2 = DictExitReason;
                    }

                    x.WaitForExit();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Stop JTR: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");

                    swHC.Stop();
                    RandomPassword.EndTime = DateTime.Now;
                    TimeSpan span = EndTime.Subtract(RandomPassword.StartTime);
                    SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.JTRDictSize), SessionPassCountDoneS)[0];
                    CalculateSpeed(SessionPassCountDoneS, span.TotalNanoseconds, LehmerGen.Passwords.JTRDictSize, swHC.Elapsed.TotalNanoseconds);
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " JTR Speed Act/Full: " + MySpeedTempMinStr + " / " + MySpeedMinStr + " /m  " + MySpeedTempSecStr + " / " + MySpeedSecStr + " /s" + "   FalsePos: " + FalsePosCount);

                    bool DeleteDictFile = true;

                    if (x.ExitCode == 0)
                    {
                        //check if false positive password
                        string MyPasss;
                        System.IO.StreamReader Passfile = new System.IO.StreamReader(MyResult);
                        while ((MyPasss = Passfile.ReadLine()) != null)
                        { break; }
                        Passfile.Close();
                        if (MyPasss != null)
                        {
                            int InString = MyPasss.IndexOf(":");
                            MyPasss = MyPasss[(InString + 1)..];

                            int ExCodeCheck = CheckPassword(MyPasss);

                            if (ExCodeCheck == 0) // password is true positive
                            {
                                Console.WriteLine("");
                                Console.WriteLine(string.Format("Password is: {0}", MyPasss));
                                SavePassword(MyPasss);
                                DeleteDictFile = false;
                                Console.WriteLine("");
                                Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                                return;

                            }
                            if (ExCodeCheck == 2)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("FALSE POSITIVE - password, cleaning password list....");
                                Console.WriteLine("");
                                List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

                                TempDictList.Remove(MyPasss);

                                File.Delete(DictFile);
                                File.WriteAllLines(DictFile, TempDictList.ToArray(), isoLatin1Encoding);
                                TempDictList.Clear();
                                string MyPasswordFalseFilename = "FP-" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
                                string MyPasswordFalse = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);

                                Console.WriteLine("Move john.pot file to FalsePassword....");
                                System.IO.File.Move(MyResult, MyPasswordFalse);

                                Console.WriteLine("");
                                Console.WriteLine("Return to check rest of the list....");
                                Console.WriteLine("");
                                DeleteDictFile = false;
                                FalsePosCount++;
                            }
                        }

                    }

                    if (DeleteDictFile == true) //don't delete file if after false positive
                    {
                        File.Delete(DictFile);
                        Console.WriteLine();
                        //SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.JTRDictSize), SessionPassCountDoneS)[0];
                    }

                    if (RandomPassword.MyExit == "e" || RandomPassword.MyExit == "c")
                    {
                        Console.WriteLine("");
                        Console.WriteLine(ExitReason1 + ExitReason2);
                        Console.ReadKey();
                        return;
                    }
                    if (RandomPassword.MyExit == "n")
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Dictionary checked" + ExitReason2);
                    }

                    if (RandomPassword.MyExit == "ex")
                    {
                        RandomPassword.MyExit = "e";
                    }
                    x.Dispose();
                    x.Close();

                    //RandomPassword.ElapsNsJob = RandomPassword.StopwatchJob.Elapsed.TotalNanoseconds;
                    //RandomPassword.StopwatchJob = Stopwatch.StartNew();
                    //RandomPassword.EndTime = DateTime.Now;
                }

            }

            catch (System.Exception Ex)
            {
                SysEx++;
                Console.WriteLine(Ex.Message);
            }
        }




        private static async void RunHashcatProgram()
        {
            StartTime = DateTime.Now;
            string HascatFile;

            if (RandomPassword.CurrentOS == "Windows")
            {
                HascatFile = System.IO.Path.Combine(ProgramDir, "Hashcat", "hashcat.exe");
            }
            else
            {
                HascatFile = System.IO.Path.Combine(ProgramDir, "Hashcat", "hashcat");
            }

            string DictFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
            string DictFileNext = System.IO.Path.Combine(ProgramDir, "Dictionary", "DictionaryNext.txt");
            string HashcatDir = System.IO.Path.Combine(ProgramDir, "Hashcat");
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            string ExitReason1 = "Safe exit";
            string ExitReason2 = "";

            //ProgressString is a real password number, eg. 1 is for 000000000...0, and it is last chcecked password
            //but password 000000...0 cames from iteration 0, so if we want to check iteration 0, ProgressString should be -1

            await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.HCDictSize, DictFile, DictFileNext, "HC", SessionPassCountDoneS)).ConfigureAwait(true);
            ExitReason2 = DictExitReason;

            //StopwatchJob = Stopwatch.StartNew();
            try
            {
                while (true)
                {
                    // save the list to the file and save configs too....
                    if (!File.Exists(DictFile))
                    {
                        System.IO.File.Move(DictFileNext, DictFile);
                    }
                    var swHC = Stopwatch.StartNew();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Start Hashcat: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");
                    Directory.SetCurrentDirectory(HashcatDir);

                    string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
                    string MyHash = System.IO.Path.Combine(ProgramDir, "Coded", "Hash.txt");
                    string MyResult = System.IO.Path.Combine(ProgramDir, "Output", "MyPasswordHashcat.txt");

                    ProcessStartInfo pro = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        FileName = HascatFile,
                        Arguments = string.Format("{0} -o {1} {2} {3}", HashcatParam, MyResult, MyHash, DictFile)
                    };
                    Process x = Process.Start(pro);

                    Thread.Sleep(10000);

                    if (RandomPassword.MyExit == "n")
                    {
                        //start generate new list, wait for it... then wait for x end....
                        await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.HCDictSize, DictFile, DictFileNext, "HC", SessionPassCountDoneS)).ConfigureAwait(true);
                        ExitReason2 = DictExitReason;
                    }

                    x.WaitForExit();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Stop Hashcat: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");

                    swHC.Stop();
                    RandomPassword.EndTime = DateTime.Now;
                    TimeSpan span = EndTime.Subtract(RandomPassword.StartTime);
                    SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.HCDictSize), SessionPassCountDoneS)[0]; ////
                    CalculateSpeed(SessionPassCountDoneS, span.TotalNanoseconds, LehmerGen.Passwords.HCDictSize, swHC.Elapsed.TotalNanoseconds);
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " HC Speed Act/Full: " + MySpeedTempMinStr + " / " + MySpeedMinStr + " /m  " + MySpeedTempSecStr + " / " + MySpeedSecStr + " /s" + "   FalsePos: " + FalsePosCount);

                    /*-2 = gpu - watchdog alarm
                     - 1 = error
                     0 = OK / cracked
                     1 = exhausted
                     2 = aborted
                     3 = aborted by checkpoint
                     4 = aborted by runtime*/

                    if (x.ExitCode == 0)
                    {
                        //check if false positive password
                        string MyPasss;
                        System.IO.StreamReader Passfile = new System.IO.StreamReader(MyResult);
                        while ((MyPasss = Passfile.ReadLine()) != null)
                        { break; }
                        Passfile.Dispose();
                        Passfile.Close();
                        int InString = MyPasss.IndexOf(":");
                        MyPasss = MyPasss[(InString + 1)..];

                        int ExCodeCheck = CheckPassword(MyPasss);

                        if (ExCodeCheck == 0) // password is true positive
                        {
                            Console.WriteLine("");
                            Console.WriteLine(string.Format("Password is: {0}", MyPasss));
                            SavePassword(MyPasss);

                            Console.WriteLine("");
                            Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                            return;
                        }
                        if (ExCodeCheck == 2)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("FALSE POSITIVE - password, cleaning password list....");
                            Console.WriteLine("");
                            List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

                            TempDictList.Remove(MyPasss);

                            File.Delete(DictFile);
                            File.WriteAllLines(DictFile, TempDictList.ToArray(), isoLatin1Encoding);
                            TempDictList.Clear();
                            string MyPasswordFalseFilename = "FP-" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
                            string MyPasswordFalse = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);
                            System.IO.File.Move(MyResult, MyPasswordFalse);
                            Console.WriteLine("");
                            Console.WriteLine("Return to check rest of the list....");
                            Console.WriteLine("");
                            FalsePosCount++;

                        }
                    }

                    if (x.ExitCode == 1)

                    {
                        File.Delete(DictFile);
                        Console.WriteLine();
                        //SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.HCDictSize), SessionPassCountDoneS)[0];
                    }


                    if (RandomPassword.MyExit == "e" || RandomPassword.MyExit == "c")
                    {
                        Console.WriteLine("");
                        Console.WriteLine(ExitReason1 + ExitReason2);
                        Console.ReadKey();
                        return;
                    }
                    if (RandomPassword.MyExit == "n")
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Dictionary checked" + ExitReason2);
                    }

                    if (RandomPassword.MyExit == "ex")
                    {
                        RandomPassword.MyExit = "e";
                    }

                    x.Dispose();
                    x.Close();

                    //RandomPassword.ElapsNsJob = RandomPassword.StopwatchJob.Elapsed.TotalNanoseconds;
                    //RandomPassword.StopwatchJob = Stopwatch.StartNew();
                    //RandomPassword.EndTime = DateTime.Now;
                }

            }

            catch (System.Exception Ex)
            {
                SysEx++;
                Console.WriteLine(Ex.Message);
            }
        }

        private static async void RunHash7zProgram()
        {
            StartTime = DateTime.Now;
            string Hash7zFile;

            if (RandomPassword.CurrentOS == "Windows")
            {
                Hash7zFile = System.IO.Path.Combine(ProgramDir, "Hash7z", "Hash7z.exe");
            }
            else
            {
                Hash7zFile = System.IO.Path.Combine(ProgramDir, "Hash7z", "Hash7z");
            }

            string DictFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
            string DictFileNext = System.IO.Path.Combine(ProgramDir, "Dictionary", "DictionaryNext.txt");
            string Hash7zDir = System.IO.Path.Combine(ProgramDir, "Hash7z");
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            string ExitReason1 = "Safe exit";
            string ExitReason2 = "";

            //ProgressString is a real password number, eg. 1 is for 000000000...0, and it is last chcecked password
            //but password 000000...0 cames from iteration 0, so if we want to check iteration 0, ProgressString should be -1

            await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.H7zDictSize, DictFile, DictFileNext, "H7z", SessionPassCountDoneS)).ConfigureAwait(true);
            ExitReason2 = DictExitReason;

            //StopwatchJob = Stopwatch.StartNew();
            try
            {
                while (true)
                {
                    // save the list to the file and save configs too....
                    if (!File.Exists(DictFile))
                    {
                        System.IO.File.Move(DictFileNext, DictFile);
                    }
                    var swHC = Stopwatch.StartNew();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Start Hash7z: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");
                    Directory.SetCurrentDirectory(Hash7zDir);

                    string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
                    string MyHash = System.IO.Path.Combine(ProgramDir, "Coded", "Hash.txt");
                    string MyResult = System.IO.Path.Combine(ProgramDir, "Output", "MyPasswordHash7z.txt");

                    ProcessStartInfo pro = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        FileName = Hash7zFile,
                        Arguments = string.Format("{0} -o {1} -H {2} -d {3}", H7zParam, MyResult, MyHash, DictFile)
                    };
                    Process x = Process.Start(pro);

                    //Thread.Sleep(10000);

                    if (RandomPassword.MyExit == "n")
                    {
                        //start generate new list, wait for it... then wait for x end....
                        await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.H7zDictSize, DictFile, DictFileNext, "H7z", SessionPassCountDoneS)).ConfigureAwait(true);
                        ExitReason2 = DictExitReason;
                    }

                    x.WaitForExit();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Stop Hash7z: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");

                    swHC.Stop();
                    RandomPassword.EndTime = DateTime.Now;
                    TimeSpan span = EndTime.Subtract(RandomPassword.StartTime);
                    SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.H7zDictSize), SessionPassCountDoneS)[0]; ////
                    CalculateSpeed(SessionPassCountDoneS, span.TotalNanoseconds, LehmerGen.Passwords.H7zDictSize, swHC.Elapsed.TotalNanoseconds);
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Hash7z Speed Act/Full: " + MySpeedTempMinStr + " / " + MySpeedMinStr + " /m  " + MySpeedTempSecStr + " / " + MySpeedSecStr + " /s" + "   FalsePos: " + FalsePosCount);

                    /*
                     0 - good password (or passwords)
                     2 - bad password (or passwords)
                     3 any error
                    */
                    
                    if (x.ExitCode == 0)
                    {
                        List<string> AllHashPass = new List<string>(File.ReadAllLines(MyResult));

                        for (int p = 0; p < AllHashPass.Count; p++)
                        {
                            string HashPass = AllHashPass[p];
                            int InString = HashPass.IndexOf(":");
                            string MyPasss = HashPass[(InString + 1)..];

                            int ExCodeCheck = CheckPassword(MyPasss);

                            if (ExCodeCheck == 0) // password is true positive
                            {
                                Console.WriteLine("");
                                Console.WriteLine(string.Format("Password is: {0}", MyPasss));
                                SavePassword(MyPasss);

                                Console.WriteLine("");
                                Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                                return;
                            }
                            if (ExCodeCheck == 2)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("FALSE POSITIVE - password. Saving it in new file...");
                                Console.WriteLine("");
                                Thread.Sleep(1000); //waiting to change time in filename
                                string MyPasswordFalseFilename = "FP-" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
                                string MyPasswordFalseFile = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);
                                
                                if (!File.Exists(MyPasswordFalseFile))
                                {
                                    using (Stream fs = new FileStream(MyPasswordFalseFile, FileMode.Create, FileAccess.Write, FileShare.None, 0x1000, FileOptions.WriteThrough))
                                    using (StreamWriter shellConfigWriter = new StreamWriter(fs))
                                    {
                                        shellConfigWriter.WriteLine(HashPass);
                                        shellConfigWriter.Flush();
                                        shellConfigWriter.Dispose();
                                        shellConfigWriter.Close();
                                    }
                                }
                                FalsePosCount++;
                            }
                        }
                        File.Delete(MyResult);
                        File.Delete(DictFile);
                        Console.WriteLine();
                    }

                    if (x.ExitCode == 2)
                    {
                        File.Delete(DictFile);
                        Console.WriteLine();
                        //SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.H7zDictSize), SessionPassCountDoneS)[0];
                    }

                    if (x.ExitCode == 3)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Something is wrong... check it.... End...");
                        System.Environment.Exit(3);
                    }


                    if (RandomPassword.MyExit == "e" || RandomPassword.MyExit == "c")
                    {
                        Console.WriteLine("");
                        Console.WriteLine(ExitReason1 + ExitReason2);
                        Console.ReadKey();
                        return;
                    }
                    if (RandomPassword.MyExit == "n")
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Dictionary checked" + ExitReason2);
                    }

                    if (RandomPassword.MyExit == "ex")
                    {
                        RandomPassword.MyExit = "e";
                    }

                    x.Dispose();
                    x.Close();

                    //RandomPassword.ElapsNsJob = RandomPassword.StopwatchJob.Elapsed.TotalNanoseconds;
                    //RandomPassword.StopwatchJob = Stopwatch.StartNew();
                    //RandomPassword.EndTime = DateTime.Now;
                }

            }

            catch (System.Exception Ex)
            {
                SysEx++;
                Console.WriteLine(Ex.Message);
            }
        }

        private static void MakeProcess7z(string ZipDir)
        {
            ProcessStartInfo pro = new()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                FileName = ZipDir,
                Arguments = "",
            };
            //Process7z.Value = pro;
            Process7zN = pro;
        }

        private static async Task ExtractFile(string CodedDir, string MyPass, string SesPassCountS)
        {
            lock (locking_Progress)
            {
                Task7zInProgress++;
            }
            int Loop7zDoneLocal = 0;
            string SesPassCountLocalS = SesPassCountS;
            //int MyExit = 100;
            string LocalPass = MyPass;
            int LoopCount = 0;
            long WaitTime = 0;
            int MinActive7zLoop=0;
            int MaxActive7zLoop = 0;
            int MaxActive7zTasksLoopLocal = 0;
            int MinActive7zTasksLoopLocal = 0;
            //long MeanProcessCountLoopLocal = 0;
            double FullTimer = 0;
            double LapTime = 0;

            int LapCount7zLocal = 0;
            string ArgumentString = string.Format("{0} \"{1}\" -p\"{2}\" -y", A7zParam, CodedDir, LocalPass);
            
            lock (locking_Active7z)
            { 
                ActActive7z++;
                LapCount7z++;
                
                if (ActActive7z < MinActive7z) { MinActive7z = ActActive7z; }
                if (ActActive7z > MaxActive7z) { MaxActive7z = ActActive7z; }

                if (Task7zInProgress < MinActive7zTasksLoop) { MinActive7zTasksLoop = Task7zInProgress; }
                if (Task7zInProgress > MaxActive7zTasksLoop) { MaxActive7zTasksLoop = Task7zInProgress; }
            }
            
            Run7z(in ArgumentString, out int ExCode);

            lock (locking_Active7z)
            {
                ActActive7z--; Loop7zDone++;
                Loop7zDoneLocal = Loop7zDone;
                if (Loop7zDoneLocal == A7zSpeedLoop)
                {
                    FullTimer = Stopwatch7zFull.Elapsed.TotalNanoseconds;
                    LapTime = FullTimer - LastLapTimer;
                    LapCount7zLocal = LapCount7z; 
                    LapCount7z -= Loop7zDoneLocal;
                    Loop7zDone -= Loop7zDoneLocal;
                    Start7z = 0;
                    MinActive7zLoop = MinActive7z;
                    MaxActive7zLoop = MaxActive7z; //znowu jest
                    MaxActive7zTasksLoopLocal = MaxActive7zTasksLoop; //głupie
                    MinActive7zTasksLoopLocal = MinActive7zTasksLoop; //głupie
                    MinActive7zTasksLoop = GlobalTaskCount;
                    MaxActive7zTasksLoop = 0;
                    MinActive7z = GlobalTaskCount;
                    MaxActive7z = 0;
                }
            }

            

            if (Loop7zDoneLocal == A7zSpeedLoop)
            {
                LastLapTimer = FullTimer;
                Done7zCountS = Miki.CalcStrings.Add(Done7zCountS, Convert.ToString(A7zSpeedLoop))[1];
                CalculateSpeed(Done7zCountS, FullTimer, A7zSpeedLoop, LapTime);
                //Console.WriteLine(((double)MySpeedMin / (double)MySpeedTempMin));

                if (A7zAutoAdjust == 1)
                {
                    if (GlobalTaskCountAdjusted == MaxActive7zLoop) //Try if can be bigger
                    {
                        GlobalTaskCountAdjusted += 2;
                        //Console.WriteLine("TryUp2");
                    }
                    else if (GlobalTaskCountAdjusted == MaxActive7zLoop + 1) //Try if can be bigger, maintain +2
                    {
                        GlobalTaskCountAdjusted++; // = MaxActive7zLoop + 1;
                                                   //Console.WriteLine("TryUp1");
                    }
                    else if (GlobalTaskCountAdjusted > MaxActive7zLoop + 2) //Decrease if we set bigger TaskCountAdjusted, but leave try option
                    {
                        GlobalTaskCountAdjusted = MaxActive7zLoop + 2; //+1?????
                                                                       //Console.WriteLine("Dec");
                    }
                    else if (GlobalTaskCountAdjusted < MaxActive7zLoop) //, not possible???
                    {
                        GlobalTaskCountAdjusted = MaxActive7zLoop + 2;
                        //Console.WriteLine("Inc");
                    }
                }
                if (GlobalTaskCountAdjusted > GlobalTaskCount)
                { GlobalTaskCountAdjusted = GlobalTaskCount; }
            }


            if (ExCode == 0)
            {
                int ExCodeCheck = CheckPassword(LocalPass);

                if (ExCodeCheck == 0) // password is true positive
                {
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("Password is: {0}", LocalPass));
                    Console.WriteLine("");
                    Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                    System.Environment.Exit(0);
                    Console.ReadKey();
                    return;
                }
                if (ExCodeCheck == 2)
                {
                    Console.WriteLine("");
                    Console.WriteLine("FALSE POSITIVE");
                    Console.WriteLine("");
                    string MyPasswordFalseFilename = "FP-" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
                    string MyPasswordFalse = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);
                    string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Output", "MyPassword7z.txt");
                    System.IO.File.Move(MyPasswordFile, MyPasswordFalse);
                    Console.WriteLine("");
                    Console.WriteLine("Return to check rest of the list....");
                    Console.WriteLine("");
                    FalsePosCount++;
                }
            }

            if (ExCode == 2)
            {
                switch (RandomPassword.DisplayMode)
                {
                    //FIFO like
                    case 1:
                        Stopwatch WaitWatch = Stopwatch.StartNew();
                        lock (locking_Waiting)
                        {
                            Task7zWaiting++;
                        }
                        while (LocalPass != PassRunList[0])
                        {
                            await Task.Delay(1);
                            LoopCount++;
                        }
                        LoopCount++;
                        WaitWatch.Stop();
                        WaitTime = WaitWatch.ElapsedMilliseconds;
                        Console.WriteLine(string.Format("{0}  {1}  A/F: {2} / {3} /m  {4} / {5} /s  ExC:{6} ExCx:{7}  P:{8}/{9}-{10}/{11}/{12} / 7z:{13}-{14} P:{15} / W:{16}  Wait L/T: {17} / {18}", Miki.CalcStringExt.SplitString(SesPassCountLocalS), LocalPass, MySpeedTempMinStr, MySpeedMinStr, MySpeedTempSecStr, MySpeedSecStr, ExCode, OtherCode, PassRunList.Count, MinActive7zTasksLoop, MaxActive7zTasksLoop, GlobalTaskCountAdjusted, GlobalTaskCount, MinActive7z, MaxActive7z, Task7zInProgress, Task7zWaiting, LoopCount, WaitTime));
                        lock (locking_Waiting)
                        {
                            Task7zWaiting--;
                        }
                        break;
                    case 2:
                        Console.WriteLine(string.Format("{0}  {1}  A/F: {2} / {3} /m  {4} / {5} /s  ExC:{6} ExCx:{7}  P:{8}/{9}-{10}/{11}/{12} / 7z:{13}-{14} P:{15} / W:{16}  Wait L/T: {17} / {18}", Miki.CalcStringExt.SplitString(SesPassCountLocalS), LocalPass, MySpeedTempMinStr, MySpeedMinStr, MySpeedTempSecStr, MySpeedSecStr, ExCode, OtherCode, PassRunList.Count, MinActive7zTasksLoop, MaxActive7zTasksLoop, GlobalTaskCountAdjusted, GlobalTaskCount, MinActive7z, MaxActive7z, Task7zInProgress, Task7zWaiting, LoopCount, WaitTime));
                        RandomPassword.DisplayMode = 3;
                        break;
                    case 3:
                        if (Loop7zDoneLocal == A7zSpeedLoop)
                        {
                            Console.WriteLine(string.Format("{0}  A/F: {1} / {2} /m  {3} / {4} /s  P:{5}/{6}-{7}/{8}/{9} / 7z:{10}-{11} FP:{12} FiLo:{13}", DateTime.Now.ToString("HH:mm:ss"), MySpeedTempMinStr, MySpeedMinStr, MySpeedTempSecStr, MySpeedSecStr, PassRunList.Count, MinActive7zTasksLoopLocal, MaxActive7zTasksLoopLocal, GlobalTaskCountAdjusted, GlobalTaskCount, MinActive7zLoop, MaxActive7zLoop, FalsePosCount, LapCount7zLocal));
                        }
                            break;
                }
            }
            else
            {
                OtherCode++;
                Console.WriteLine(string.Format("{0}  {1}  A/F: {2} / {3} /m  {4} / {5} /s  ExC:{6} ExCx:{7}  P:{8}/{9}-{10}/{11}/{12} / 7z:{13}-{14} P:{15} / W:{16}  Wait L/T: {17} / {18}", Miki.CalcStringExt.SplitString(SesPassCountLocalS), LocalPass, MySpeedTempMinStr, MySpeedMinStr, MySpeedTempSecStr, MySpeedSecStr, ExCode, OtherCode, PassRunList.Count, MinActive7zTasksLoop, MaxActive7zTasksLoop, GlobalTaskCountAdjusted, GlobalTaskCount, MinActive7z, MaxActive7z, Task7zInProgress, Task7zWaiting, LoopCount, WaitTime));
            }

            lock (locking_List) //FIFO like
            {
                PassRunList.Remove(LocalPass);
            }


            lock (locking_Progress)
            {
                Task7zInProgress--;
            }

            return;

        }


        public static void Run7z(in string ArgumentString, out int ExCode)
        {
            ProcessStartInfo? Process7zLocal = Process7zN;
            Process7zLocal.Arguments = ArgumentString;

            using (Process process = Process.Start(Process7zLocal))
                {
                    process.WaitForExit();
                    ExCode = process.ExitCode;
                }
        }


       


        public static string CheckOS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "Linux";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "Windows";
            }
            return "Unknown";
            
        }


        public static void CalculateSpeed(string FullCount, double FullNs, long LapCount, double LapNs)
        {
            string FullSpeedInterval = Convert.ToString(FullNs);
            try
            {
                string LapSpeedInterval = Convert.ToString(LapNs);
                if (LapSpeedInterval != "0")
                {
                    long InSec = LapCount * 1000000000;
                    long InMin = LapCount * 60000000000;
                    string SLapSpeedS = Miki.CalcStrings.Div(Convert.ToString(InSec), LapSpeedInterval)[0];
                    string SLapSpeedSR = Miki.CalcStrings.Div(Convert.ToString(InSec), LapSpeedInterval)[1];
                    string SLapSpeed = Miki.CalcStrings.Div(Convert.ToString(InMin), LapSpeedInterval)[0];
                    string SLapSpeedR = Miki.CalcStrings.Div(Convert.ToString(InMin), LapSpeedInterval)[1];
                    int SLapSpeedSRLength = SLapSpeedSR.Length;
                    int SLapSpeedRLength = SLapSpeedR.Length;
                    if (SLapSpeedSRLength > 2) { SLapSpeedSRLength = 2; }
                    if (SLapSpeedRLength > 2) { SLapSpeedRLength = 2; }
                    //MySpeedTempMin = Convert.ToInt64(SLapSpeed);
                    //MySpeedTempSec = Convert.ToInt64(SLapSpeedS);
                    MySpeedTempMinStr = SLapSpeed + "," + SLapSpeedR.Substring(0, SLapSpeedRLength);
                    MySpeedTempSecStr = SLapSpeedS + "," + SLapSpeedSR.Substring(0, SLapSpeedSRLength);

                }

                if (FullSpeedInterval != "0")
                {
                    string SFullSpeedS = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(FullCount, "1000000000")[0], FullSpeedInterval)[0];
                    string SFullSpeedSR = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(FullCount, "1000000000")[0], FullSpeedInterval)[1];
                    string SFullSpeed = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(FullCount, "60000000000")[0], FullSpeedInterval)[0];
                    string SFullSpeedR = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(FullCount, "60000000000")[0], FullSpeedInterval)[1];
                    //MySpeedMin = Convert.ToInt64(SFullSpeed);
                    //MySpeedSec = Convert.ToInt64(SFullSpeedS);
                    int SFullSpeedSRLength = SFullSpeedSR.Length;
                    int SFullSpeedRLength = SFullSpeedR.Length;
                    if (SFullSpeedSRLength > 2) { SFullSpeedSRLength = 2; }
                    if (SFullSpeedRLength > 2) { SFullSpeedRLength = 2; }
                    MySpeedMinStr = SFullSpeed + "," + SFullSpeedR.Substring(0, SFullSpeedRLength);
                    MySpeedSecStr = SFullSpeedS + "," + SFullSpeedSR.Substring(0, SFullSpeedSRLength);
                }
                else 
                {
                    //MySpeedMin = MySpeedTempMin;
                    //MySpeedSec = MySpeedTempSec;
                    MySpeedMinStr = MySpeedTempMinStr;
                    MySpeedSecStr = MySpeedTempSecStr;
                }
            }
            catch
            {
                //MySpeedMin = -1;
                //MySpeedSec = -1;
                //MySpeedTempMin = -1;
                //MySpeedTempSec = -1;

                MySpeedMinStr = "-1";
                MySpeedSecStr = "-1";
                MySpeedTempMinStr = "-1";
                MySpeedTempSecStr = "-1";
            }

        }
        public static void SaveProgress(string DoneIter, string MaxIter, string JobKind)
        {
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            //string IterAll = "";

            string ProgressFile = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.txt");
            string ProgressFileBak = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.bak.txt");


            if (JobKind == "7z")
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Session progress saved...");
            }
            if (JobKind == "HC")
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Session progress saved...");
            }
            if (JobKind == "JTR")
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Session progress saved...");
            }
            if (JobKind == "H7z")
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Session progress saved...");
            }

            //File.Move may be not safe becouse of buffering, we must use flush
            List<string> OldProgresList = new List<string>(File.ReadAllLines(ProgressFile)); //read old SessionProgress.txt
            
            if (File.Exists(ProgressFile))
            {
                if (File.Exists(ProgressFileBak)) { File.Delete(ProgressFileBak); }// delete SessionProgress.bak.txt

                using (FileStream fs = new FileStream(ProgressFileBak, FileMode.CreateNew)) //create new SessionProgress.bak.txt
                {
                    using (StreamWriter writer = new StreamWriter(fs, isoLatin1Encoding))
                    {
                        for (int i = 0; i < OldProgresList.Count; i++)
                        {
                            writer.WriteLine(OldProgresList[i]); //write all lines from old SessionProgress.txt
                        }
                        writer.Flush();
                        fs.Flush(true);
                    }
                }
                File.Delete(ProgressFile); //delete old SessionProgress.txt
                //if (File.Exists(ProgressFileBak)) // delete prev backup
                //    File.Delete(ProgressFileBak);

                //File.Move(ProgressFile, ProgressFileBak);
            }
            ////////////////////////////////////////////////???????????????????????????????????????????????? blackout
            /*
            List<string> ProgressList = new();
            ProgressList.Add(Miki.CalcStringExt.SplitString(DoneIter));
            ProgressList.Add(Miki.CalcStringExt.SplitString(MaxIter));
            for (int i = 0; i < LehmerGen.Passwords.TempRemoved.Count; i++)
            {
                ProgressList.Add(LehmerGen.Passwords.TempRemoved[i]);
            }

            string[] ProgressArray = ProgressList.ToArray();

            File.WriteAllLines(ProgressFile, ProgressArray, isoLatin1Encoding);
            */
            using (FileStream fs = new FileStream(ProgressFile, FileMode.CreateNew)) //create new SessionProgress.txt
            {
                using (StreamWriter writer = new StreamWriter(fs, isoLatin1Encoding))
                {
                    writer.WriteLine(Miki.CalcStringExt.SplitString(DoneIter)); //write progress
                    writer.WriteLine(Miki.CalcStringExt.SplitString(MaxIter));  //write max pool iter
                    for (int i = 0; i < LehmerGen.Passwords.TempRemoved.Count; i++)
                    {
                        writer.WriteLine(LehmerGen.Passwords.TempRemoved[i]); //write temporary removed pass
                    }
                    writer.Flush();
                    fs.Flush(true);
                }
            }


            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();

        }
        public static void SavePassword(string CrackedPass = "")
        {
            if (CrackedPass != "")
            {
                string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Output", "TruePassword.txt");
                Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");
                if (!File.Exists(MyPasswordFile))
                {

                    using (FileStream fs = new FileStream(MyPasswordFile, FileMode.CreateNew))
                    {
                        using (StreamWriter writer = new StreamWriter(fs, isoLatin1Encoding))
                        {
                            
                            writer.WriteLine("Password is:");
                            writer.WriteLine(CrackedPass);
                            writer.Flush();
                            fs.Flush(true);
                        }
                    }

                    /*
                    using (Stream fs = new FileStream(MyPasswordFile, FileMode.Create, FileAccess.Write, FileShare.None, 0x1000, FileOptions.WriteThrough))
                    using (StreamWriter shellConfigWriter = new StreamWriter(fs))
                    {
                        shellConfigWriter.WriteLine("Password is:");
                        shellConfigWriter.Write(string.Join(string.Empty, CrackedPass));

                        shellConfigWriter.Flush();
                        shellConfigWriter.Dispose();
                        shellConfigWriter.Close();
                    }
                    */
                }
            }
        }

        public static int CheckPassword(string Password)
        {
            string ZipFile;

            if (RandomPassword.CurrentOS == "Windows")
            {
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za.exe");
            }
            else
            {
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za");
            }

            string CodedFile = System.IO.Path.Combine(ProgramDir, "Coded", "CodedCheck.7z");
            ProcessStartInfo pro7z = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                FileName = ZipFile,
                Arguments = string.Format("t \"{0}\" -p\"{1}\" -y", CodedFile, Password) //x or t, t is much faster, and password is saved to file (it is not necessary to decrypt file)
            };

            Process x7z = Process.Start(pro7z);

            x7z.WaitForExit();
            string errorOutput = x7z.StandardError.ReadToEnd();
            string standardOutput = x7z.StandardOutput.ReadToEnd();
            int MyExitCode = x7z.ExitCode;
            x7z.Close();
            x7z.Dispose();
            return MyExitCode;
        }
    }

}

namespace MyPrimes
{
    class MyPrimes
    {
        private static List<List<long>> MyAllPrimes = new();
        private static List<long> PrimesChecked = new();
        private static readonly AsyncLocal<List<long>> PrimesToCheckAsync = new();
        private static readonly AsyncLocal<List<long>> FactorListAsync = new();
        private static List<long> FactorListGlobal = new();
        private static List<long> DigListGlobal = new();
        private static readonly AsyncLocal<List<long>> DigListAsync = new();
        //private static readonly AsyncLocal<long> StepAsync = new();
        public static object locking_Checked = new object();
        public static int ResetPrimes = 0;
        public static int EndPrimes = 0;
        private static List<long> PrimesAfterReset = new();
        private static List<long> DigListAfterReset = new();
        private static List<long> FactorAfterReset = new();
        //private static long StepAfterReset = 0;

        public static void GCDandLCG()
        {
            string Dig1 = "2036374380265640862475460607138083929396896285104227682237"; //bigger
            List<string> Dig1CList = ["3755694380586030003065247222678916480543336423203954689",
                                        "7676209091027857382431822959614237261894994766095110237",
                                        "12146883316695048308686366124668976422945126406526262299",
                                        "31336254119496703067212933456598557079850290995385671156",
                                        "31543890397977681684285524470900170333639395210295357354",
                                        "40176410445985822259750586041224811189531068371882449994",
                                        "50978046730341339808150358928996236180129663186933235827",
                                        "83711421875307233831737043217091611084767503960064348120",
                                        "187822621286125449890796126105002991209728230359035860429",
                                        "296617098518128619118015098687874488783534069632643260146",
                                        "314559722942811783336072959204254651435362353861280775342",
                                        "390068533064308455670106477191047034571680133907361801167",
                                        "407898618008960012332824634247222674413785016005161521945",
                                        "410208303256140177411418173882702218858474481623710975733",
                                        "419048353912227782148140725044918613580527694076161868937",
                                        "491503738613355640966345010936815489421512910402424676339",
                                        "629674712355902757840307263136622988282325681191803436417",
                                        "636683821489853434437266388034219037458040951041904847822",
                                        "643607007403468735260381089577909635540510968700107688324",
                                        "655841516550994549168022914344749316518746680339437310058",
                                        "685868357419476753097223413192423777223126388231395027388",
                                        "775036838508491695633095200677503008777870004447862036557",
                                        "775474784289619438457444226490432340170844772643169136033",
                                        "796085787420620166931166284117432981753467218817315064713",
                                        "874274491584196723965583903696811074544610778819604822428",
                                        "876621944489025963160629460025228159509066427600170606643",
                                        "891163104988096303173424745232024161391996781947379032768",
                                        "898045695353732043759270050862443655173066405488630486349",
                                        "922620055883114221110001317750105290817911809028564755788",
                                        "922780027907761012185411452577247908207502401720708535800",
                                        "922853752790397909032908976261046300533459503673148290165",
                                        "1062895327319019605793161545285324195013275302076270514765",
                                        "1094960579722686818621345822455072552407890668395593470853",
                                        "1102258128966730295326432013765751064565425778465991305555",
                                        "1117062230907632289757802177508423181119134826079962006747",
                                        "1254962676368317775376127311225905997675181818039857275132",
                                        "1261820117816710650306955568003330786644645214706840417748",
                                        "1301641024045630935356852558944559970320371380927978795860",
                                        "1330101489425281079569018322308622831326015523603171160104",
                                        "1366301166587099315228721366725593974753770164334162249869",
                                        "1372918388872706941160559241980154002504318744974422494975",
                                        "1401408086712247331012180581181660044347286996126975987558",
                                        "1407925357676487152360849030957049098515927049128604864174",
                                        "1463300997260091443257659266027906901738201071930738374929",
                                        "1487146029571530628159738370348518212113898099973790158043",
                                        "1514951705355795861085363657229149848584755106147143485523",
                                        "1622078257450675011966352183142462161523527751584232862952",
                                        "1626250850669832548640852391829559593001774431420951826038",
                                        "1678490180764214410273799964200637976444172207444068587378",
                                        "1725399349246400710421603061426600525978202236683385386605",
                                        "1736654304815936727826044763731903213463995769055152326182",
                                        "1736840528255463435340702528815998295847265742133174241559",
                                        "1803391566269156522822536393064996014420024111036775260470",
                                        "1831332447876155007610552149204112246751376465119837072303",
                                        "1851330191154504713731466793261072506629999712132377988686",
                                        "1882046176811738420854192716380729745535498420952886135643",
                                        "1917803495187956289572254676781886359493227790350851441572",
                                        "1923385914014385553837876836071604196900189410551275371129",
                                        "1993385732686105974932502360964647068321118483752914663721",
                                        "2005015882453425138439481115793726158884358712518526836254",
                                        "2036374380265640862475460607138083929396896285104227682237",
                                        "2129264429410063174477837203064782725150596922790803073854"]; //Pass Decimal

            //string Dig2 = "2261454906"; //CRC
            string Dig2 = "181"; //CRC prime
            //string Dig2 = "8603857629747393996303217036759126466607012602916212246272175590155663224029857622122168265451151956207747188485396190564225066741842038539230784042191777"; //Hash decimal
            //string Dig1 = "15"; //bigger
            //string Dig2 = "20";

            CalcConvert.StringToLongList(Dig2, 18, out List<long> Dig2List);
            for (int i = 0; i < Dig1CList.Count; i++)
            {
                CalcConvert.StringToLongList(Dig1CList[i], 18, out List<long> Dig1List);
                //int a = 15, b = 20;
                //Console.WriteLine("LCM of " + a +
                // " and " + b + " is " + lcm(a, b));
                List<long> GCD = gcd(Dig1List, Dig2List);
                List<long> LCM = lcm2(Dig1List, Dig2List, GCD);
                List<long> DivRest = CalcLists.Div(Dig1List, Dig2List)[1];
                //Console.WriteLine("D: " + Dig1CList[i]);
                CalcConvert.LongListToString(GCD, "000000000000000000", out string ToWriteG);
                Console.WriteLine("G: " + ToWriteG);
                CalcConvert.LongListToString(LCM, "000000000000000000", out string ToWriteL);
                //Console.WriteLine("L: " + ToWriteL);
                CalcConvert.LongListToString(DivRest, "000000000000000000", out string ToWriteR);
                //Console.WriteLine("R: " + ToWriteR);
                Console.WriteLine();
            }
        }

        static List<long> gcd(List<long> a, List<long> b) //Greatest Common Divisor
        {
            if (a.Count == 1 && a[0] == 0)
            { return b; }
            //return gcd(b % a, a);
            //List<long> res = gcd(CalcLists.Div(b, a)[1], a);
            return gcd(CalcLists.Div(b, a)[1], a);

        }

        // method to return  
        // LCM of two numbers 
        static List<long> lcm(List<long> a, List<long> b) //Least Common Multiple
        {
            //List<long> res = CalcLists.Mul(CalcLists.Div(a, gcd(a, b))[0], b);
            return CalcLists.Mul(CalcLists.Div(a, gcd(a, b))[0], b);
        }
        static List<long> lcm2(List<long> a, List<long> b, List<long> InGCD)
        {
            //List<long> res = CalcLists.Mul(CalcLists.Div(a, gcd(a, b))[0], b);
            return CalcLists.Mul(CalcLists.Div(a, InGCD)[0], b);
        }

        public static void GetPrimeFactors()
        {
            //string n = "28441320419090285828205154019987657933377";
            string n = "410208303256140177411418173882702218858474481623710975733";
            //string n = "273874657892";
            //string n = "21";
            CalcConvert.StringToLongList(in n, 18, out List<long> DigList);
            string MyString4 = n;

            List<List<long>> factors = [];
            List<long> Num1 = [1];
            List<long> Num2 = [2];
            //List<long> Num6 = [6];
            List<long> FactorFactor = [];

            int l = 0;
            int lDisplay = 1000000;
            List<long> lLong = [0];
            List<long> lLongAdd = [lDisplay];
            // factor=167636999999
            // step=2
            if (DigList.Count == 1 && DigList[0] == 1)
            { factors.Add(Num1); l++; }
            else
            {

                //List<long> factor = [292625999999]; //[2]
                List<long> factor = [2]; //[2]
                long step = 2;
                FactorFactor = CalcLists.Mul(in factor, in factor);
                CalcCompare.ListBigger(DigList, DigList.Count, FactorFactor, FactorFactor.Count, out int FactorBigger);

                //for small digs to eliminate next else if
                while (FactorBigger != 2 && factor.Count == 1 && factor[0] < 5) //factor * factor <= DigList
                {
                    l++;
                    List<List<long>> DigModFactor = CalcLists.Div(DigList, factor);
                    if (DigModFactor[1].Count == 1 && DigModFactor[1][0] == 0) //n % factor == 0
                    {
                        factors.Add(factor);
                        DigList = DigModFactor[0];
                        CalcConvert.LongListToString(in DigList, "000000000000000000", out MyString4);
                    }
                    else if (factor.Count == 1 && factor[0] < 3)
                    {
                        factor = CalcLists.Add(factor, Num1);
                    }
                    else if (factor.Count == 1 && factor[0] < 5)
                    {
                        factor = CalcLists.Add(factor, Num2);
                    }
                    else
                    {
                        factor = CalcLists.Add(factor, [step]);
                        step = 6 - step;
                    }


                    if (l == lDisplay)
                    {
                        FactorFactor = CalcLists.Mul(in factor, in factor);
                        CalcCompare.ListBigger(DigList, DigList.Count, FactorFactor, FactorFactor.Count, out FactorBigger);
                        lLong = CalcLists.Add(lLong, lLongAdd);
                        CalcConvert.LongListToString(in lLong, "000000000000000000", out string MyString2);
                        //Console.WriteLine(MyString2);
                        CalcConvert.LongListToString(in factor, "000000000000000000", out string MyString3);
                        Console.WriteLine("Count: " + MyString2 + "   Factor: " + MyString3);
                        l = 0;
                        if (factors.Count > 0)
                        {
                            for (int i = 0; i < factors.Count; i++)
                            {
                                CalcConvert.LongListToString(factors[i], "000000000000000000", out string MyString);
                                Console.WriteLine("Factors: " + MyString);
                            }
                        }
                    }
                }

                //for bigger digs, now without else if checking
                while (FactorBigger != 2) //factor * factor <= DigList
                {
                    l++;
                    List<List<long>> DigModFactor = CalcLists.Div(DigList, factor);
                    if (DigModFactor[1].Count == 1 && DigModFactor[1][0] == 0) //n % factor == 0
                    {
                        factors.Add(factor);
                        DigList = DigModFactor[0];
                        CalcConvert.LongListToString(in DigList, "000000000000000000", out MyString4);
                    }
                    else
                    {
                        factor = CalcLists.Add(factor, [step]);
                        step = 6 - step;
                    }


                    if (l == lDisplay)
                    {
                        FactorFactor = CalcLists.Mul(in factor, in factor);
                        CalcCompare.ListBigger(DigList, DigList.Count, FactorFactor, FactorFactor.Count, out FactorBigger);
                        lLong = CalcLists.Add(lLong, lLongAdd);
                        CalcConvert.LongListToString(in lLong, "000000000000000000", out string MyString2);
                        CalcConvert.LongListToString(in factor, "000000000000000000", out string MyString3);
                        Console.WriteLine("Count: " + MyString2 + "   Dig: " + MyString4 + "   Factor: " + MyString3 + "   Step: " + step);
                        l = 0;
                        if (factors.Count > 0)
                        {
                            for (int i = 0; i < factors.Count; i++)
                            {
                                CalcConvert.LongListToString(factors[i], "000000000000000000", out string MyString);
                                Console.WriteLine("Factors: " + MyString);
                            }
                        }

                    }
                }

                factors.Add(DigList);

            }

            for (int i = 0; i < factors.Count; i++)
            {
                CalcConvert.LongListToString(factors[i], "000000000000000000", out string MyString);
                Console.WriteLine("Factors: " + MyString);
            }

        }

        public static void GetPrimeFactorsPar()
        {
            string n = "410208303256140177411418173882702218858474481623710975733";
            //string n = "273874657892";
            //string n = "13436790520670073527238076749162231404431";
            //string n = "21";
            CalcConvert.StringToLongList(in n, 18, out List<long> DigList);
            
            string MyString4 = n;

            List<List<long>> factors = [];
            List<long> Num1 = [1];
            List<long> Num2 = [2];
            //List<long> Num6 = [6];
            List<long> FactorFactor = [];

            int l = 0;
            
            //List<long> lLongAdd = [PrimesToCheckPool];
            // factor=167636999999
            // step=2
            if (DigList.Count == 1 && DigList[0] == 1)
            { factors.Add(Num1); l++; }
            else
            {

                //List<long> factor = [292625999999]; //[2]
                List<long> factor = [2]; //[2]
                long step = 2;
                FactorFactor = CalcLists.Mul(in factor, in factor);
                CalcCompare.ListBigger(DigList, DigList.Count, FactorFactor, FactorFactor.Count, out int FactorBigger);

                //for small digs (factor < 5) to eliminate next else if
                while (FactorBigger != 2 && factor.Count == 1 && factor[0] < 5) //factor * factor <= DigList
                {
                    l++;
                    List<List<long>> DigModFactor = CalcLists.Div(DigList, factor);
                    if (DigModFactor[1].Count == 1 && DigModFactor[1][0] == 0) //n % factor == 0
                    {
                        MyAllPrimes.Add(factor);
                        DigList = DigModFactor[0];
                        DigListAsync.Value = DigList;
                        CalcConvert.LongListToString(in DigList, "000000000000000000", out MyString4);
                    }
                    else if (factor.Count == 1 && factor[0] < 3)
                    {
                        factor = CalcLists.Add(factor, Num1);
                    }
                    else if (factor.Count == 1 && factor[0] < 5)
                    {
                        factor = CalcLists.Add(factor, Num2);
                    }
                    else
                    {
                        factor = CalcLists.Add(factor, [step]);
                        step = 6 - step;
                    }
                } //from now we can try to do it in parallel manner

                
                List<long> PrimesCheckedLocal = [0];
                PrimesChecked = PrimesCheckedLocal;
                PrimesToCheckAsync.Value = PrimesCheckedLocal;
                DigListAsync.Value = DigList;
                FactorListAsync.Value = factor;
                FactorListGlobal = factor;
                DigListGlobal = DigList;
                //StepAsync.Value = step;
                List<long> FactorCount = [1]; //any prime number greater than 3 can be written as (6n + 1) or (6n - 1), so start from n==1
                //for bigger digs, now without else if checking

                
                int TaskCount = 6;
                int PrimesToCheckPool = 1000000;
                List<Task> TaskList = new List<Task>(TaskCount);
                while (true)//(FactorBigger != 2) //factor * factor <= DigList
                {

                    while (TaskList.Count < TaskCount && ResetPrimes == 0)
                    {
                        FactorListAsync.Value = FactorCount;
                        TaskList.Add(Task.Run(async () => await PrimesParallel(PrimesToCheckAsync.Value, PrimesToCheckPool, DigListAsync.Value, FactorListAsync.Value).ConfigureAwait(false)));
                        PrimesToCheckAsync.Value = CalcLists.Add(PrimesToCheckAsync.Value, [PrimesToCheckPool]);
                        //FactorListAsync.Value = CalcLists.Add(FactorListAsync.Value, CalcLists.Mul([3], [PrimesToCheckPool]));
                        FactorCount = CalcLists.Add(FactorCount, [PrimesToCheckPool/2]); //make the factor for next task
                        //Thread.Sleep(200);
                    }

                    if (ResetPrimes == 1)
                    {
                        Task.WhenAll(TaskList);
                        TaskList.Clear();
                        PrimesToCheckAsync.Value = PrimesAfterReset;
                        DigListAsync.Value = DigListAfterReset;
                        //FactorCount = CalcLists.Add(FactorAfterReset, [PrimesToCheckPool]); // FactorAfterReset;
                        FactorCount = FactorAfterReset;
                        //StepAsync.Value = StepAfterReset;
                        ResetPrimes = 0;
                    }
                    else
                    {
                        while (TaskList.Count >= TaskCount)
                        {
                            Thread.Sleep(5);
                            //Task.WhenAny(TaskList);
                            TaskList.RemoveAll(x => x.IsCompleted);
                        }
                    }

                    FactorFactor = CalcLists.Mul(in FactorListGlobal, in FactorListGlobal);
                    CalcCompare.ListBigger(DigListGlobal, DigListGlobal.Count, FactorFactor, FactorFactor.Count, out FactorBigger);
                    if (FactorBigger == 2 || FactorBigger == 0)
                    { EndPrimes = 1; }

                    if (EndPrimes == 1)
                    {
                        Task.WhenAll(TaskList);
                        TaskList.Clear();
                        MyAllPrimes.Add(DigListGlobal);
                        break;

                    }
                }
            }
            Console.WriteLine("Dig: " + n + " primes:");
            for (int i = 0; i < MyAllPrimes.Count; i++)
            {
                CalcConvert.LongListToString(MyAllPrimes[i], "000000000000000000", out string MyString);
                Console.WriteLine(MyString);
            }

        }

        public static async Task PrimesParallel(List<long> MyCount, int Pool, List<long> DigList, List<long> FactorList)
        {
            if (ResetPrimes == 1) 
            { return; } //break if any other parallel task found any prime
            List<long> ThisChecked = CalcLists.Add(MyCount, [Pool]); //PrimesToCheckAsync
            
            List<List<long>> TempFactors = new();
            List<long> FactorListLocal = CalcLists.Sub(CalcLists.Mul(FactorList, [6]), [1]);
            List<List<long>> TestFactors = new();
            //FactorListLocal = CalcLists.Sub(FactorList, [1]);
            List<long> DigListLocal = new(DigList);
            long StepLocal = 2;
            CalcConvert.LongListToString(in DigListLocal, "000000000000000000", out string MyString4);
            int PoolDone = 0;
            while (true) //for (int l = 1; l <= Pool; l++)
            {
                if (ResetPrimes == 1) 
                { return; } //break if any other parallel task found any prime -ok
                
                
                //CalcConvert.LongListToString(in FactorListLocal, "000000000000000000", out string MyString9);
                //Console.WriteLine(MyString9);
                List<List<long>> DigModFactor = CalcLists.Div(DigListLocal, FactorListLocal);
                if (DigModFactor[1].Count == 1 && DigModFactor[1][0] == 0) //n % factor == 0
                {
                    TempFactors.Add(FactorListLocal);
                    DigListLocal = DigModFactor[0];
                    CalcConvert.LongListToString(in DigListLocal, "000000000000000000", out MyString4);
                }
                else
                {
                    PoolDone++;
                    if (PoolDone == Pool) { break; }
                    FactorListLocal = CalcLists.Add(FactorListLocal, [StepLocal]);
                    StepLocal = 6 - StepLocal;
                }
                
            }
            
            while (true) //trap for tasks
            {
                if (ResetPrimes == 1) 
                { return; } //break if any other parallel task found any prime
                lock (locking_Checked)
                {
                    List<long> AllChecked = CalcLists.Add(PrimesChecked, [Pool]); //FIFO, check if previous task ended
                    CalcCompare.ListBigger(ThisChecked, ThisChecked.Count, AllChecked, AllChecked.Count, out int BiggerList);
                    if (BiggerList == 0)
                    {
                        if (ResetPrimes == 1) 
                        { return; } //break if any other parallel task found any prime
                        break;
                    }
                }

                //await Task.Delay(1);
                Thread.Sleep(2);
            }
            if (ResetPrimes == 1) 
            { return; } //break if any other parallel task found any prime

            

            if (TempFactors.Count > 0 && ResetPrimes == 0) //if released from trap and found factors, add to global list
            {
                ResetPrimes = 1;
                MyAllPrimes.AddRange(TempFactors); //Add factors to list
                //now we must abandon others parallel tasks, update DigList, Factor and Step.
                PrimesAfterReset = ThisChecked;
                DigListAfterReset = DigListLocal;
                FactorAfterReset = CalcLists.Div(CalcLists.Add(FactorListLocal, [StepLocal + 1]),[6])[0] ;
            }


            //Display Checked pool info
            CalcConvert.LongListToString(ThisChecked, "000000000000000000", out string MyString2);
            CalcConvert.LongListToString(in FactorListLocal, "000000000000000000", out string MyString3);
            CalcConvert.LongListToString(in FactorList, "000000000000000000", out string MyString5);
            MyString2 = CalcStringExt.SplitString(MyString2);
            MyString3 = CalcStringExt.SplitString(MyString3);
            MyString5 = CalcStringExt.SplitString(MyString5);
            Console.WriteLine("Count: " + MyString2 + "   Dig: " + MyString4 + "   Factor: " + MyString3 + "   FactorN: " + MyString5);
            //Display All calculated primes
            if (MyAllPrimes.Count > 0) 
            {
                for (int i = 0; i < MyAllPrimes.Count; i++)
                {
                    CalcConvert.LongListToString(MyAllPrimes[i], "000000000000000000", out string MyString);
                    Console.WriteLine("Factors: " + MyString);
                }
            }

            
            lock (locking_Checked) //release trap
            {
                
                FactorListGlobal = FactorListLocal;
                DigListGlobal = DigListLocal;
                PrimesChecked = ThisChecked;
                //CalcConvert.LongListToString(in ThisChecked, "000000000000000000", out string MyString0);
                //Console.WriteLine("Checked: " + MyString0);
                
            }
            //var a = TestFactors[999999];


        }

        public static void GeneratePrimesPar()
        {
            List<long> StartPrimes = [1];
            long step = 2;
            //First two primes
            Console.WriteLine(1);
            Console.WriteLine(3);
            //Next primes
            List<long> PrimeExpected = CalcLists.Sub(CalcLists.Mul(StartPrimes, [6]),[1]); //5
            while (true)
            {
                CalcConvert.LongListToString(PrimeExpected, "000000000000000000", out string PrimeExpectedString);
                Console.WriteLine("Prime expected: " + PrimeExpectedString);
                ListPrimesPar(PrimeExpected);
                PrimeExpected = CalcLists.Add(PrimeExpected, [step]);
                step = 6 - step;
            }

        
        }
        public static void ListPrimesPar(List<long> CheckPrime)
        {
            List<long> DigList = CheckPrime;
            //string n = "410208303256140177411418173882702218858474481623710975733";
            CalcConvert.LongListToString(DigList, "000000000000000000", out string n);
            //string n = "273874657892";
            //string n = "13436790520670073527238076749162231404431";
            //string n = "21";
            //CalcConvert.StringToLongList(in n, 18, out List<long> DigList);

            string MyString4 = n;

            List<List<long>> factors = [];
            List<long> Num1 = [1];
            List<long> Num2 = [2];
            //List<long> Num6 = [6];
            List<long> FactorFactor = [];

            int l = 0;

            //List<long> lLongAdd = [PrimesToCheckPool];
            // factor=167636999999
            // step=2
            if (DigList.Count == 1 && DigList[0] == 1) //prime 1
            { factors.Add(Num1); l++; }
            else
            {

                //List<long> factor = [292625999999]; //[2]
                List<long> factor = [2]; //[2]
                long step = 2;
                FactorFactor = CalcLists.Mul(in factor, in factor);
                CalcCompare.ListBigger(DigList, DigList.Count, FactorFactor, FactorFactor.Count, out int FactorBigger);

                //for small digs (factor < 5) to eliminate next else if
                while (FactorBigger != 2 && factor.Count == 1 && factor[0] < 5) //factor * factor <= DigList
                {
                    l++;
                    List<List<long>> DigModFactor = CalcLists.Div(DigList, factor);
                    if (DigModFactor[1].Count == 1 && DigModFactor[1][0] == 0) //n % factor == 0
                    {
                        MyAllPrimes.Add(factor);
                        DigList = DigModFactor[0];
                        DigListAsync.Value = DigList;
                        CalcConvert.LongListToString(in DigList, "000000000000000000", out MyString4);
                    }
                    else if (factor.Count == 1 && factor[0] < 3)
                    {
                        factor = CalcLists.Add(factor, Num1);
                    }
                    else if (factor.Count == 1 && factor[0] < 5)
                    {
                        factor = CalcLists.Add(factor, Num2);
                    }
                    else
                    {
                        factor = CalcLists.Add(factor, [step]);
                        step = 6 - step;
                    }
                } //from now we can try to do it in parallel manner


                List<long> PrimesCheckedLocal = [0];
                PrimesChecked = PrimesCheckedLocal;
                PrimesToCheckAsync.Value = PrimesCheckedLocal;
                DigListAsync.Value = DigList;
                FactorListAsync.Value = factor;
                FactorListGlobal = factor;
                DigListGlobal = DigList;
                //StepAsync.Value = step;
                List<long> FactorCount = [1]; //any prime number greater than 3 can be written as (6n + 1) or (6n - 1), so start from n==1
                                              //for bigger digs, now without else if checking


                int TaskCount = 6;
                int PrimesToCheckPool = 1000000;
                List<Task> TaskList = new List<Task>(TaskCount);
                while (true)//(FactorBigger != 2) //factor * factor <= DigList
                {

                    while (TaskList.Count < TaskCount && ResetPrimes == 0)
                    {
                        FactorListAsync.Value = FactorCount;
                        TaskList.Add(Task.Run(async () => await CheckPrimesPar(PrimesToCheckAsync.Value, PrimesToCheckPool, DigListAsync.Value, FactorListAsync.Value).ConfigureAwait(false)));
                        PrimesToCheckAsync.Value = CalcLists.Add(PrimesToCheckAsync.Value, [PrimesToCheckPool]);
                        //FactorListAsync.Value = CalcLists.Add(FactorListAsync.Value, CalcLists.Mul([3], [PrimesToCheckPool]));
                        FactorCount = CalcLists.Add(FactorCount, [PrimesToCheckPool / 2]); //make the factor for next task
                        //Thread.Sleep(200);
                    }

                    if (ResetPrimes == 1)
                    {
                        Task.WhenAll(TaskList);
                        TaskList.Clear();
                        PrimesToCheckAsync.Value = PrimesAfterReset;
                        DigListAsync.Value = DigListAfterReset;
                        //FactorCount = CalcLists.Add(FactorAfterReset, [PrimesToCheckPool]); // FactorAfterReset;
                        FactorCount = FactorAfterReset;
                        //StepAsync.Value = StepAfterReset;
                        ResetPrimes = 0;
                    }
                    else
                    {
                        while (TaskList.Count >= TaskCount)
                        {
                            Thread.Sleep(5);
                            //Task.WhenAny(TaskList);
                            TaskList.RemoveAll(x => x.IsCompleted);
                        }
                    }

                    FactorFactor = CalcLists.Mul(in FactorListGlobal, in FactorListGlobal);
                    CalcCompare.ListBigger(DigListGlobal, DigListGlobal.Count, FactorFactor, FactorFactor.Count, out FactorBigger);
                    if (FactorBigger == 2 || FactorBigger == 0)
                    { EndPrimes = 1; }

                    if (EndPrimes == 1)
                    {
                        Task.WhenAll(TaskList);
                        TaskList.Clear();
                        MyAllPrimes.Add(DigListGlobal);
                        break;

                    }
                }
            }
            Console.WriteLine("Dig: " + n + " primes:");
            for (int i = 0; i < MyAllPrimes.Count; i++)
            {
                CalcConvert.LongListToString(MyAllPrimes[i], "000000000000000000", out string MyString);
                Console.WriteLine(MyString);
            }
            MyAllPrimes.Clear();
        }

        public static async Task CheckPrimesPar(List<long> MyCount, int Pool, List<long> DigList, List<long> FactorList)
        {
            if (ResetPrimes == 1)
            { return; } //break if any other parallel task found any prime
            List<long> ThisChecked = CalcLists.Add(MyCount, [Pool]); //PrimesToCheckAsync

            List<List<long>> TempFactors = new();
            List<long> FactorListLocal = CalcLists.Sub(CalcLists.Mul(FactorList, [6]), [1]);
            List<List<long>> TestFactors = new();
            //FactorListLocal = CalcLists.Sub(FactorList, [1]);
            List<long> DigListLocal = new(DigList);
            long StepLocal = 2;
            CalcConvert.LongListToString(in DigListLocal, "000000000000000000", out string MyString4);
            int PoolDone = 0;
            while (true) //for (int l = 1; l <= Pool; l++)
            {
                if (ResetPrimes == 1)
                { return; } //break if any other parallel task found any prime -ok


                //CalcConvert.LongListToString(in FactorListLocal, "000000000000000000", out string MyString9);
                //Console.WriteLine(MyString9);
                List<List<long>> DigModFactor = CalcLists.Div(DigListLocal, FactorListLocal);
                if (DigModFactor[1].Count == 1 && DigModFactor[1][0] == 0) //n % factor == 0
                {
                    TempFactors.Add(FactorListLocal);
                    DigListLocal = DigModFactor[0];
                    CalcConvert.LongListToString(in DigListLocal, "000000000000000000", out MyString4);
                }
                else
                {
                    PoolDone++;
                    if (PoolDone == Pool) { break; }
                    FactorListLocal = CalcLists.Add(FactorListLocal, [StepLocal]);
                    StepLocal = 6 - StepLocal;
                }

            }

            while (true) //trap for tasks
            {
                if (ResetPrimes == 1)
                { return; } //break if any other parallel task found any prime
                lock (locking_Checked)
                {
                    List<long> AllChecked = CalcLists.Add(PrimesChecked, [Pool]); //FIFO, check if previous task ended
                    CalcCompare.ListBigger(ThisChecked, ThisChecked.Count, AllChecked, AllChecked.Count, out int BiggerList);
                    if (BiggerList == 0)
                    {
                        if (ResetPrimes == 1)
                        { return; } //break if any other parallel task found any prime
                        break;
                    }
                }

                //await Task.Delay(1);
                Thread.Sleep(2);
            }
            if (ResetPrimes == 1)
            { return; } //break if any other parallel task found any prime



            if (TempFactors.Count > 0 && ResetPrimes == 0) //if released from trap and found factors, add to global list
            {
                ResetPrimes = 1;
                MyAllPrimes.AddRange(TempFactors); //Add factors to list
                //now we must abandon others parallel tasks, update DigList, Factor and Step.
                PrimesAfterReset = ThisChecked;
                DigListAfterReset = DigListLocal;
                FactorAfterReset = CalcLists.Div(CalcLists.Add(FactorListLocal, [StepLocal + 1]), [6])[0];
            }


            //Display Checked pool info
            CalcConvert.LongListToString(ThisChecked, "000000000000000000", out string MyString2);
            CalcConvert.LongListToString(in FactorListLocal, "000000000000000000", out string MyString3);
            CalcConvert.LongListToString(in FactorList, "000000000000000000", out string MyString5);
            MyString2 = CalcStringExt.SplitString(MyString2);
            MyString3 = CalcStringExt.SplitString(MyString3);
            MyString5 = CalcStringExt.SplitString(MyString5);
            //Console.WriteLine("Count: " + MyString2 + "   Dig: " + MyString4 + "   Factor: " + MyString3 + "   FactorN: " + MyString5);
            //Display All calculated primes
            if (MyAllPrimes.Count > 0)
            {
                for (int i = 0; i < MyAllPrimes.Count; i++)
                {
                    CalcConvert.LongListToString(MyAllPrimes[i], "000000000000000000", out string MyString);
                    //Console.WriteLine("Factors: " + MyString);
                }
            }


            lock (locking_Checked) //release trap
            {

                FactorListGlobal = FactorListLocal;
                DigListGlobal = DigListLocal;
                PrimesChecked = ThisChecked;
                //CalcConvert.LongListToString(in ThisChecked, "000000000000000000", out string MyString0);
                //Console.WriteLine("Checked: " + MyString0);

            }
            //var a = TestFactors[999999];
        }

        public static bool CheckPrime(int num)
        {
            int a = 0;
            for (int i = 1; i <= num; i++)
            {
                if (num % i == 0)
                {
                    a++;
                }
            }

            if (a == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void primeFactors(List<long> n)
        {
            // Print the number of 2s that divide n 
            List<long> Dig2 = [2];
            List<long> Dig3 = [3];
            List<List<long>> MyPrimes = [];
            List<List<long>> TempDiv = [];
            int l = 0;
            int lDisplay = 1000000;
            List<long> lLong = [0];
            List<long> lLongAdd = [lDisplay];
            while (true)
            {
                l++;
                TempDiv = CalcLists.Div(in n, in Dig2);
                if (TempDiv[1].Count == 1 && TempDiv[1][0] == 0)
                {
                    MyPrimes.Add(Dig2); n = TempDiv[0];
                    CalcConvert.LongListToString(in Dig2, "000000000000000000", out string MyString);
                    Console.WriteLine(MyString);
                }
                else
                { break; }
                //Console.Write(2 + " ");
                if (l == lDisplay)
                {
                    lLong = CalcLists.Add(lLong, lLongAdd);
                    CalcConvert.LongListToString(in lLong, "000000000000000000", out string MyString2);
                    Console.WriteLine(MyString2);
                    l = 0;
                }

            }

            // n must be odd at this point. So we can 
            // skip one element (Note i = i +2) 
            /*
            for (int i = 3; i <= Math.Sqrt(n); i += 2)
            {
                // While i divides n, print i and divide n 
                while (n % i == 0)
                {
                    Console.Write(i + " ");
                    n /= i;
                }
            }
            */
            List<long> Divider = [3];
            while (true)
            {
                l++;
                while (true)
                {
                    TempDiv = CalcLists.Div(in n, in Divider);
                    if (TempDiv[1].Count == 1 && TempDiv[1][0] == 0)
                    {
                        MyPrimes.Add(Divider); n = TempDiv[0];
                        CalcConvert.LongListToString(in Divider, "000000000000000000", out string MyString);
                        Console.WriteLine(MyString);
                    }
                    else
                    { break; }
                }
                Divider = CalcLists.Add(Divider, Dig2);
                CalcCompare.ListBigger(Divider, Divider.Count, n, n.Count, out int ListBigger);
                if (ListBigger == 1) { break; }
                if (ListBigger == 0)
                {
                    MyPrimes.Add(Divider);
                    CalcConvert.LongListToString(in Divider, "000000000000000000", out string MyString);
                    Console.WriteLine(MyString);
                    break;
                }

                if (l == lDisplay)
                {
                    lLong = CalcLists.Add(lLong, lLongAdd);
                    CalcConvert.LongListToString(in lLong, "000000000000000000", out string MyString2);
                    Console.WriteLine(MyString2);
                    l = 0;
                }
            }

            // This condition is to handle the case when 
            // n is a prime number greater than 2 
            //if (n > 2)????
            //Console.Write(n);
            for (int i = 0; i < MyPrimes.Count; i++)
            {
                CalcConvert.LongListToString(MyPrimes[i], "000000000000000000", out string MyString2);
                Console.WriteLine(MyString2);
            }


        }

        // Driver Code 
        public static void GetPrimes()
        {
            string n = "3755694380586030003065247222678916480543336423203954689";
            //string n = "235823564568";
            CalcConvert.StringToLongList(in n, 18, out List<long> DigList);
            ;
            primeFactors(DigList);
        }


    }

}

namespace Polynomials
{
    class Polynomials
    {
        public static List<List<char>> MaskTableP = new();
        public static List<List<char>> MaskTableQ = new();
        public static List<List<char>> MaskTableR = new();
        public static char[][] MaskArrayP = new char[1][];
        public static char[][] MaskArrayQ = new char[1][];
        public static char[][] MaskArrayR = new char[1][];

        //public static List<List<List<long>>> ConvTableP = new();
        //public static List<List<List<long>>> ConvTableQ = new();
        //public static List<List<List<long>>> ConvTableR = new();
        public static List<long>[][] ConvListArrayP = new List<long>[1][];
        public static List<long>[][] ConvListArrayQ = new List<long>[1][];
        public static List<long>[][] ConvListArrayR = new List<long>[1][];

        public static int IsUnaryP = 0;
        public static int IsUnaryQ = 0;
        public static int IsUnaryR = 0;

        //public static List<List<long>> AllUnarysValueListP = new();
        //public static List<List<long>> AllUnarysValueListQ = new();
        //public static List<List<long>> AllUnarysValueListR = new();
        public static List<long>[] AllUnarysValueListArrayP = new List<long>[1];
        public static List<long>[] AllUnarysValueListArrayQ = new List<long>[1];
        public static List<long>[] AllUnarysValueListArrayR = new List<long>[1];

        //public static List<List<long>> DigLenListP = new();
        //public static List<List<long>> DigLenListQ = new();
        //public static List<List<long>> DigLenListR = new();
        public static long[][] DigLenArrayP = new long[1][];
        public static long[][] DigLenArrayQ = new long[1][];
        public static long[][] DigLenArrayR = new long[1][];

        public static string LastPossibleDecStringP = "";
        public static string LastPossibleDecStringQ = "";
        public static string LastPossibleDecStringR = "";

        public static int PosForDigsP;
        public static long PosForDigsMultP;
        public static int PosForDigsQ;
        public static long PosForDigsMultQ;
        public static int PosForDigsR;
        public static long PosForDigsMultR;

        public static void ReadConfig()
        {
            string ProgramDir = Environment.CurrentDirectory;
            //string ReadString;
            int pLength = 0;
            int qLength = 0;
            int rLength = 0;
            string pMaskTableOther = "";
            string qMaskTableOther = "";
            string rMaskTableOther = "";

            string ConfigFile = System.IO.Path.Combine(ProgramDir, "Config", "Config.txt");

            List<string> ConfigStringList = new List<string>(File.ReadAllLines(ConfigFile));

            for (int i = 0; i < ConfigStringList.Count; i++)
            {
                if (ConfigStringList[i].Length != 0 && ConfigStringList[i].Substring(0, 1) != "#")
                {
                    if (ConfigStringList[i].Contains("pMaxLength"))
                    {
                        pLength = Convert.ToInt32(ConfigStringList[i].Trim().Substring(11));
                        for (int j = 1; j <= pLength; j++)
                        { MaskTableP.Add(new List<char> { }); }
                    }
                    if (ConfigStringList[i].Contains("qMaxLength"))
                    {
                        qLength = Convert.ToInt32(ConfigStringList[i].Trim().Substring(11));
                        for (int j = 1; j <= qLength; j++)
                        { MaskTableQ.Add(new List<char> { }); }
                    }
                    if (ConfigStringList[i].Contains("rMaxLength"))
                    {
                        rLength = Convert.ToInt32(ConfigStringList[i].Trim().Substring(11));
                        for (int j = 1; j <= rLength; j++)
                        { MaskTableR.Add(new List<char> { }); }
                    }
                    if (ConfigStringList[i].Contains("pCharacters"))
                    {
                        pMaskTableOther = ConfigStringList[i].Trim().Substring(12);
                    }
                    if (ConfigStringList[i].Contains("qCharacters"))
                    {
                        qMaskTableOther = ConfigStringList[i].Trim().Substring(12);
                    }
                    if (ConfigStringList[i].Contains("rCharacters"))
                    {
                        rMaskTableOther = ConfigStringList[i].Trim().Substring(12);
                    }
                }
            }

            for (int i = 0; i < pLength; i++)
            {
                MaskTableP[i] = pMaskTableOther.ToArray().ToList();
            }
            for (int i = 0; i < qLength; i++)
            {
                MaskTableQ[i] = qMaskTableOther.ToArray().ToList();
            }
            for (int i = 0; i < rLength; i++)
            {
                MaskTableR[i] = rMaskTableOther.ToArray().ToList();
            }


            for (int i = 0; i < ConfigStringList.Count; i++)
            {
                if (ConfigStringList[i].Length != 0 && ConfigStringList[i].Substring(0, 1) != "#")
                {
                    if (ConfigStringList[i].Contains("p?"))
                    {
                        int InStr = ConfigStringList[i].IndexOf('=');
                        int MaskPosInt = Convert.ToInt32(ConfigStringList[i].Trim().Substring(2, InStr - 2));
                        string MaskPosString = ConfigStringList[i].Trim().Substring(InStr + 1);
                        MaskTableP[MaskPosInt - 1] = MaskPosString.ToArray().ToList();
                    }
                }
            }

            for (int i = 0; i < ConfigStringList.Count; i++)
            {
                if (ConfigStringList[i].Length != 0 && ConfigStringList[i].Substring(0, 1) != "#")
                {
                    if (ConfigStringList[i].Contains("q?"))
                    {
                        int InStr = ConfigStringList[i].IndexOf('=');
                        int MaskPosInt = Convert.ToInt32(ConfigStringList[i].Trim().Substring(2, InStr - 2));
                        string MaskPosString = ConfigStringList[i].Trim().Substring(InStr + 1);
                        MaskTableQ[MaskPosInt - 1] = MaskPosString.ToArray().ToList();
                    }
                }
            }

            for (int i = 0; i < ConfigStringList.Count; i++)
            {
                if (ConfigStringList[i].Length != 0 && ConfigStringList[i].Substring(0, 1) != "#")
                {
                    if (ConfigStringList[i].Contains("r?"))
                    {
                        int InStr = ConfigStringList[i].IndexOf('=');
                        int MaskPosInt = Convert.ToInt32(ConfigStringList[i].Trim().Substring(2, InStr - 2));
                        string MaskPosString = ConfigStringList[i].Trim().Substring(InStr + 1);
                        MaskTableR[MaskPosInt - 1] = MaskPosString.ToArray().ToList();
                    }
                }
            }

            Console.WriteLine("Config loaded...");
        }
        public static void MakeConvTable()
        {
            Console.WriteLine("Conversion tables - generating...");

            List<List<List<long>>> ConvTableLocalP = new();
            List<List<List<long>>> ConvTableLocalQ = new();
            List<List<List<long>>> ConvTableLocalR = new();

            List<List<long>> PosMultiplierP = new();
            List<List<long>> PosMultiplierQ = new();
            List<List<long>> PosMultiplierR = new();

            PosMultiplierP.Add(new List<long>(1) { 1 }); //on ones place always Multipier==1
            PosMultiplierQ.Add(new List<long>(1) { 1 }); //on ones place always Multipier==1
            PosMultiplierR.Add(new List<long>(1) { 1 }); //on ones place always Multipier==1

            // to create polynomial numbering system P
            Array.Resize(ref MaskArrayP, MaskTableP.Count);
            for (int i = 0; i < MaskTableP.Count; i++)
            {
                Array.Resize(ref MaskArrayP[i], MaskTableP[i].Count);
                MaskArrayP[i] = MaskTableP[i].ToArray();
            }
            // to create polynomial numbering system Q
            Array.Resize(ref MaskArrayQ, MaskTableQ.Count);
            for (int i = 0; i < MaskTableQ.Count; i++)
            {
                Array.Resize(ref MaskArrayQ[i], MaskTableQ[i].Count);
                MaskArrayQ[i] = MaskTableQ[i].ToArray();
            }
            // to create polynomial numbering system R
            Array.Resize(ref MaskArrayR, MaskTableR.Count);
            for (int i = 0; i < MaskTableR.Count; i++)
            {
                Array.Resize(ref MaskArrayR[i], MaskTableR[i].Count);
                MaskArrayR[i] = MaskTableR[i].ToArray();
            }

            // calculate Multipliers for Conversion tables P, Q, R
            for (int i = 1; i < MaskTableP.Count; i++) //on higher places is:
            {
                List<long> CharCount = new(1) { (long)MaskTableP[i - 1].Count }; //previous place characters count *
                PosMultiplierP.Add(Miki.CalcLists.Mul(PosMultiplierP[i - 1], CharCount)); // * previous PosMultiplier
            }

            for (int i = 1; i < MaskTableQ.Count; i++) //on higher places is:
            {
                List<long> CharCount = new(1) { (long)MaskTableQ[i - 1].Count }; //previous place characters count *
                PosMultiplierQ.Add(Miki.CalcLists.Mul(PosMultiplierQ[i - 1], CharCount)); // * previous PosMultiplier
            }

            for (int i = 1; i < MaskTableR.Count; i++) //on higher places is:
            {
                List<long> CharCount = new(1) { (long)MaskTableR[i - 1].Count }; //previous place characters count *
                PosMultiplierR.Add(Miki.CalcLists.Mul(PosMultiplierR[i - 1], CharCount)); // * previous PosMultiplier
            }

            // calculate Conversion tables for P, Q, R
            Array.Resize(ref ConvListArrayP, MaskTableP.Count);
            for (int CharPos = 0; CharPos < MaskTableP.Count; CharPos++)
            {
                Array.Resize(ref ConvListArrayP[CharPos], MaskTableP[CharPos].Count);
                for (int CharValue = 0; CharValue < MaskTableP[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableP[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except ones place, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0) //       && CharValue == 0
                        {
                            CharIntToMul = new() { 0 }; //unary zero in ones place
                        }
                        else
                        {
                            CharIntToMul = new() { 1 }; //rest of unaries, they has values
                        }
                        IsUnaryP = 1; //to inform converter, that there is unary
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, PosMultiplierP[CharPos]);
                    //ConvRow.Add(MultResult);
                    ConvListArrayP[CharPos][CharValue] = MultResult;
                    //Console.WriteLine(ConvListArray[CharPos][CharValue][^1]);
                }
                //ConvTableLocal.Add(ConvRow);
            }
            Array.Resize(ref ConvListArrayQ, MaskTableQ.Count);
            for (int CharPos = 0; CharPos < MaskTableQ.Count; CharPos++)
            {
                Array.Resize(ref ConvListArrayQ[CharPos], MaskTableQ[CharPos].Count);
                for (int CharValue = 0; CharValue < MaskTableQ[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableQ[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except ones place, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0) //       && CharValue == 0
                        {
                            CharIntToMul = new() { 0 }; //unary zero in ones place
                        }
                        else
                        {
                            CharIntToMul = new() { 1 }; //rest of unaries, they has values
                        }
                        IsUnaryQ = 1; //to inform converter, that there is unary
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, PosMultiplierQ[CharPos]);
                    //ConvRow.Add(MultResult);
                    ConvListArrayQ[CharPos][CharValue] = MultResult;
                    //Console.WriteLine(ConvListArray[CharPos][CharValue][^1]);
                }
                //ConvTableLocal.Add(ConvRow);
            }
            Array.Resize(ref ConvListArrayR, MaskTableR.Count);
            for (int CharPos = 0; CharPos < MaskTableR.Count; CharPos++)
            {
                Array.Resize(ref ConvListArrayR[CharPos], MaskTableR[CharPos].Count);
                for (int CharValue = 0; CharValue < MaskTableR[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableR[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except ones place, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0) //       && CharValue == 0
                        {
                            CharIntToMul = new() { 0 }; //unary zero in ones place
                        }
                        else
                        {
                            CharIntToMul = new() { 1 }; //rest of unaries, they has values
                        }
                        IsUnaryR = 1; //to inform converter, that there is unary
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, PosMultiplierR[CharPos]);
                    //ConvRow.Add(MultResult);
                    ConvListArrayR[CharPos][CharValue] = MultResult;
                    //Console.WriteLine(ConvListArray[CharPos][CharValue][^1]);
                }
                //ConvTableLocal.Add(ConvRow);
            }

            /*
            for (int CharPos = 0; CharPos < MaskTableP.Count; CharPos++)
            {
                List<List<long>> ConvRow = new();
                for (int CharValue = 0; CharValue < MaskTableP[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableP[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except ones place, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0) //       && CharValue == 0
                        {
                            CharIntToMul = new() { 0 }; //unary zero in ones place
                        }
                        else
                        {
                            CharIntToMul = new() { 1 }; //rest of unaries, they has values
                        }
                        IsUnaryP = 1; //to inform converter, that there is unary
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, PosMultiplierP[CharPos]);
                    ConvRow.Add(MultResult);

                }
                ConvTableLocalP.Add(ConvRow);
            }
            ConvTableP = ConvTableLocalP;

            
            for (int CharPos = 0; CharPos < MaskTableQ.Count; CharPos++)
            {
                List<List<long>> ConvRow = new();
                for (int CharValue = 0; CharValue < MaskTableQ[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableQ[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except ones place, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0) //       && CharValue == 0
                        {
                            CharIntToMul = new() { 0 }; //unary zero in ones place
                        }
                        else
                        {
                            CharIntToMul = new() { 1 }; //rest of unaries, they has values
                        }
                        IsUnaryQ = 1; //to inform converter, that there is unary
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, PosMultiplierQ[CharPos]);
                    ConvRow.Add(MultResult);

                }
                ConvTableLocalQ.Add(ConvRow);
            }
            ConvTableQ = ConvTableLocalQ;

            for (int CharPos = 0; CharPos < MaskTableR.Count; CharPos++)
            {
                List<List<long>> ConvRow = new();
                for (int CharValue = 0; CharValue < MaskTableR[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableR[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except ones place, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0) //       && CharValue == 0
                        {
                            CharIntToMul = new() { 0 }; //unary zero in ones place
                        }
                        else
                        {
                            CharIntToMul = new() { 1 }; //rest of unaries, they has values
                        }
                        IsUnaryR = 1; //to inform converter, that there is unary
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, PosMultiplierR[CharPos]);
                    ConvRow.Add(MultResult);

                }
                ConvTableLocalR.Add(ConvRow);
            }
            ConvTableR = ConvTableLocalR;
            */

            //Create tables for Unarys P, Q, R
            List<long> AllUnarysValueLocalP = new() { 0 };
            Array.Resize(ref AllUnarysValueListArrayP, MaskTableP.Count);
            for (int CharPos = 0; CharPos < MaskTableP.Count; CharPos++)
            {
                if (MaskTableP[CharPos].Count == 1)
                {
                    AllUnarysValueLocalP = Miki.CalcLists.Add(AllUnarysValueLocalP, ConvListArrayP[CharPos][0]);
                    AllUnarysValueListArrayP[CharPos] = AllUnarysValueLocalP;
                }
                else
                {
                    AllUnarysValueListArrayP[CharPos] = AllUnarysValueLocalP;
                }
            }
            List<long> AllUnarysValueLocalQ = new() { 0 };
            Array.Resize(ref AllUnarysValueListArrayQ, MaskTableQ.Count);
            for (int CharPos = 0; CharPos < MaskTableQ.Count; CharPos++)
            {
                if (MaskTableQ[CharPos].Count == 1)
                {
                    AllUnarysValueLocalQ = Miki.CalcLists.Add(AllUnarysValueLocalQ, ConvListArrayQ[CharPos][0]);
                    AllUnarysValueListArrayQ[CharPos] = AllUnarysValueLocalQ;
                }
                else
                {
                    AllUnarysValueListArrayQ[CharPos] = AllUnarysValueLocalQ;
                }
            }
            List<long> AllUnarysValueLocalR = new() { 0 };
            Array.Resize(ref AllUnarysValueListArrayR, MaskTableR.Count);
            for (int CharPos = 0; CharPos < MaskTableR.Count; CharPos++)
            {
                if (MaskTableR[CharPos].Count == 1)
                {
                    AllUnarysValueLocalR = Miki.CalcLists.Add(AllUnarysValueLocalR, ConvListArrayR[CharPos][0]);
                    AllUnarysValueListArrayR[CharPos] = AllUnarysValueLocalR;
                }
                else
                {
                    AllUnarysValueListArrayR[CharPos] = AllUnarysValueLocalR;
                }
            }

            /*
            List<long> AllUnarysValuePLocal = new() { 0 };
            List<List<long>> AllUnarysValuePListLocal = new();

            for (int CharPos = 0; CharPos < MaskTableP.Count; CharPos++)
            {
                if (MaskTableP[CharPos].Count == 1)
                {
                    AllUnarysValuePLocal = Miki.CalcLists.Add(AllUnarysValuePLocal, ConvTableP[CharPos][0]);
                    AllUnarysValuePListLocal.Add(AllUnarysValuePLocal);
                }
                else
                { AllUnarysValuePListLocal.Add(AllUnarysValuePLocal); }
            }
            AllUnarysValueListP = AllUnarysValuePListLocal;

            List<long> AllUnarysValueQLocal = new() { 0 };
            List<List<long>> AllUnarysValueQListLocal = new();

            for (int CharPos = 0; CharPos < MaskTableQ.Count; CharPos++)
            {
                if (MaskTableQ[CharPos].Count == 1)
                {
                    AllUnarysValueQLocal = Miki.CalcLists.Add(AllUnarysValueQLocal, ConvTableQ[CharPos][0]);
                    AllUnarysValueQListLocal.Add(AllUnarysValueQLocal);
                }
                else
                { AllUnarysValueQListLocal.Add(AllUnarysValueQLocal); }
            }
            AllUnarysValueListQ = AllUnarysValueQListLocal;

            List<long> AllUnarysValueRLocal = new() { 0 };
            List<List<long>> AllUnarysValueRListLocal = new();

            for (int CharPos = 0; CharPos < MaskTableR.Count; CharPos++)
            {
                if (MaskTableR[CharPos].Count == 1)
                {
                    AllUnarysValueRLocal = Miki.CalcLists.Add(AllUnarysValueRLocal, ConvTableR[CharPos][0]);
                    AllUnarysValueRListLocal.Add(AllUnarysValueRLocal);
                }
                else
                { AllUnarysValueRListLocal.Add(AllUnarysValueRLocal); }
            }
            AllUnarysValueListR = AllUnarysValueRListLocal;
            */

            //Create tables for speed up conversion
            int LastDigLen2 = 0;
            long LastDigFromConv = 0;
            long ActDigFromConv = 0;

            //Concept, make long list to compare (DigLenList) similar to ConvTable, before compare with ConvTable,
            //it will contain XXXX...YYYY numbers, where
            //XXXX... full number length, YYYY - first digits from this number
            //look in PassListFromDigMyTab (variable ActDigToCompare)

            List<long> LongestListP = ConvListArrayP[ConvListArrayP.Length - 1][ConvListArrayP[ConvListArrayP.Length - 1].Length - 1];
            Miki.CalcIntExt.LongLength(LongestListP[^1], out int LongestListDigLenP);
            LongestListDigLenP = LongestListDigLenP + (LongestListP.Count - 1) * 18;
            Miki.CalcIntExt.LongLength(LongestListDigLenP, out int PosForLengthLocalP);
            int PosForDigsLocalP = 18 - PosForLengthLocalP;
            Miki.CalcIntExt.LongPower(10, in PosForDigsLocalP, out long PosForDigsMultLocalP);

            PosForDigsP = PosForDigsLocalP;
            PosForDigsMultP = PosForDigsMultLocalP;

            Array.Resize(ref DigLenArrayP, MaskTableP.Count);

            for (int CharPos = 0; CharPos < MaskTableP.Count; CharPos++)
            {
                Array.Resize(ref DigLenArrayP[CharPos], MaskTableP[CharPos].Count);
                for (int CharValue = 0; CharValue < MaskTableP[CharPos].Count; CharValue++)
                {
                    int ConvListArrCount = ConvListArrayP[CharPos][CharValue].Count;
                    if (ConvListArrCount > 1)
                    {
                        Miki.CalcIntExt.LongLength(ConvListArrayP[CharPos][CharValue][^1], out int DigLen); // First step to make XXXX....
                        Miki.CalcCompare.GetLongFromList(ConvListArrayP[CharPos][CharValue], PosForDigsLocalP, in DigLen, out ActDigFromConv); //four digits to add later (YYYY)
                        DigLen = 18 * (ConvListArrCount - 1) + DigLen; //Now we have XXXX, full digit length

                        //to replace zeros with proper length and digits
                        if (DigLen >= LastDigLen2)
                        { LastDigLen2 = DigLen; }
                        //int.MaxValue = 2 147 483 647 -> DigLen (XXXX) is int, LastDigFromConv is long
                        //so to long.MaxValue in this concept 18digits we can add max 5 places to DigLen making LastDigFromConv,
                        //I add 4 places, that's enough, then add four digits (YYYY) from earlier calculations
                        LastDigFromConv = (LastDigLen2 * PosForDigsMultLocalP) + ActDigFromConv; //now we have XXXX...YYYY
                        DigLenArrayP[CharPos][CharValue] = LastDigFromConv + 1000000000000000000;
                    }
                    else
                    { DigLenArrayP[CharPos][CharValue] = ConvListArrayP[CharPos][CharValue][^1]; }
                }
            }

            LastDigLen2 = 0;
            LastDigFromConv = 0;
            ActDigFromConv = 0;

            List<long> LongestListQ = ConvListArrayQ[ConvListArrayQ.Length - 1][ConvListArrayQ[ConvListArrayQ.Length - 1].Length - 1];
            Miki.CalcIntExt.LongLength(LongestListQ[^1], out int LongestListDigLenQ);
            LongestListDigLenQ = LongestListDigLenQ + (LongestListQ.Count - 1) * 18;
            Miki.CalcIntExt.LongLength(LongestListDigLenQ, out int PosForLengthLocalQ);
            int PosForDigsLocalQ = 18 - PosForLengthLocalQ;
            Miki.CalcIntExt.LongPower(10, in PosForDigsLocalQ, out long PosForDigsMultLocalQ);

            PosForDigsQ = PosForDigsLocalQ;
            PosForDigsMultQ = PosForDigsMultLocalQ;

            Array.Resize(ref DigLenArrayQ, MaskTableQ.Count);

            for (int CharPos = 0; CharPos < MaskTableQ.Count; CharPos++)
            {
                Array.Resize(ref DigLenArrayQ[CharPos], MaskTableQ[CharPos].Count);
                for (int CharValue = 0; CharValue < MaskTableQ[CharPos].Count; CharValue++)
                {
                    int ConvListArrCount = ConvListArrayQ[CharPos][CharValue].Count;
                    if (ConvListArrCount > 1)
                    {
                        Miki.CalcIntExt.LongLength(ConvListArrayQ[CharPos][CharValue][^1], out int DigLen); // First step to make XXXX....
                        Miki.CalcCompare.GetLongFromList(ConvListArrayQ[CharPos][CharValue], PosForDigsLocalQ, in DigLen, out ActDigFromConv); //four digits to add later (YYYY)
                        DigLen = 18 * (ConvListArrCount - 1) + DigLen; //Now we have XXXX, full digit length

                        //to replace zeros with proper length and digits
                        if (DigLen >= LastDigLen2)
                        { LastDigLen2 = DigLen; }
                        //int.MaxValue = 2 147 483 647 -> DigLen (XXXX) is int, LastDigFromConv is long
                        //so to long.MaxValue in this concept 18digits we can add max 5 places to DigLen making LastDigFromConv,
                        //I add 4 places, that's enough, then add four digits (YYYY) from earlier calculations
                        LastDigFromConv = (LastDigLen2 * PosForDigsMultLocalQ) + ActDigFromConv; //now we have XXXX...YYYY
                        DigLenArrayQ[CharPos][CharValue] = LastDigFromConv + 1000000000000000000;
                    }
                    else
                    { DigLenArrayQ[CharPos][CharValue] = ConvListArrayQ[CharPos][CharValue][^1]; }
                }
            }

            LastDigLen2 = 0;
            LastDigFromConv = 0;
            ActDigFromConv = 0;

            List<long> LongestListR = ConvListArrayR[ConvListArrayR.Length - 1][ConvListArrayR[ConvListArrayR.Length - 1].Length - 1];
            Miki.CalcIntExt.LongLength(LongestListR[^1], out int LongestListDigLenR);
            LongestListDigLenR = LongestListDigLenR + (LongestListR.Count - 1) * 18;
            Miki.CalcIntExt.LongLength(LongestListDigLenR, out int PosForLengthLocalR);
            int PosForDigsLocalR = 18 - PosForLengthLocalR;
            Miki.CalcIntExt.LongPower(10, in PosForDigsLocalR, out long PosForDigsMultLocalR);

            PosForDigsR = PosForDigsLocalR;
            PosForDigsMultR = PosForDigsMultLocalR;

            Array.Resize(ref DigLenArrayR, MaskTableR.Count);

            for (int CharPos = 0; CharPos < MaskTableR.Count; CharPos++)
            {
                Array.Resize(ref DigLenArrayR[CharPos], MaskTableR[CharPos].Count);
                for (int CharValue = 0; CharValue < MaskTableR[CharPos].Count; CharValue++)
                {
                    int ConvListArrCount = ConvListArrayR[CharPos][CharValue].Count;
                    if (ConvListArrCount > 1)
                    {
                        Miki.CalcIntExt.LongLength(ConvListArrayR[CharPos][CharValue][^1], out int DigLen); // First step to make XXXX....
                        Miki.CalcCompare.GetLongFromList(ConvListArrayR[CharPos][CharValue], PosForDigsLocalR, in DigLen, out ActDigFromConv); //four digits to add later (YYYY)
                        DigLen = 18 * (ConvListArrCount - 1) + DigLen; //Now we have XXXX, full digit length

                        //to replace zeros with proper length and digits
                        if (DigLen >= LastDigLen2)
                        { LastDigLen2 = DigLen; }
                        //int.MaxValue = 2 147 483 647 -> DigLen (XXXX) is int, LastDigFromConv is long
                        //so to long.MaxValue in this concept 18digits we can add max 5 places to DigLen making LastDigFromConv,
                        //I add 4 places, that's enough, then add four digits (YYYY) from earlier calculations
                        LastDigFromConv = (LastDigLen2 * PosForDigsMultLocalQ) + ActDigFromConv; //now we have XXXX...YYYY
                        DigLenArrayR[CharPos][CharValue] = LastDigFromConv + 1000000000000000000;
                    }
                    else
                    { DigLenArrayR[CharPos][CharValue] = ConvListArrayR[CharPos][CharValue][^1]; }
                }
            }
            /*
            int LastDigLen2 = 0;
            long LastDigFromConv = 0;
            long ActDigFromConv = 0;
            List<List<long>> DigLenListPLocal = new();
            //Concept, make long list to compare (DigLenList) similar to ConvTable, before compare with ConvTable,
            //it will contain XXXX...YYYY numbers, where
            //XXXX... full number length, YYYY - first digits from this number
            //look in PassListFromDigMyTab (variable ActDigToCompare)
            for (int CharPos = 0; CharPos < MaskTableP.Count; CharPos++)
            {
                List<long> RowLenList2 = new();
                for (int CharValue = 0; CharValue < MaskTableP[CharPos].Count; CharValue++)
                {
                    Miki.CalcIntExt.LongLength(ConvTableP[CharPos][CharValue][^1], out int DigLen); // First step to make XXXX....
                    Miki.CalcCompare.GetLongFromList(ConvTableP[CharPos][CharValue], 4, DigLen, out ActDigFromConv); //four digits to add later (YYYY)
                    DigLen = 18 * (ConvTableP[CharPos][CharValue].Count - 1) + DigLen; //Now we have XXXX, full digit length

                    //to replace zeros with proper length and digits
                    if (DigLen >= LastDigLen2)
                    { LastDigLen2 = DigLen; }
                    if (ActDigFromConv > 0)
                    { LastDigFromConv = ActDigFromConv; }
                    //int.MaxValue = 2 147 483 647 -> DigLen (XXXX) is int, LastDigFromConv is long
                    //so to long.MaxValue in this concept 18digits we can add max 5 places to DigLen making LastDigFromConv,
                    //I add 4 places, that's enough, then add four digits (YYYY) from earlier calculations
                    LastDigFromConv = (LastDigLen2 * 10000) + ActDigFromConv; //now we have XXXX...YYYY
                                                                              //Console.WriteLine(k + "   " + LastDigLen2 + "   " + LastDigFromConv2);
                    RowLenList2.Add(LastDigFromConv);
                }
                DigLenListPLocal.Add(RowLenList2);
            }
            DigLenListP = DigLenListPLocal;

            LastDigLen2 = 0;
            LastDigFromConv = 0;
            ActDigFromConv = 0;
            List<List<long>> DigLenListQLocal = new();

            for (int CharPos = 0; CharPos < MaskTableQ.Count; CharPos++)
            {
                List<long> RowLenList2 = new();
                for (int CharValue = 0; CharValue < MaskTableQ[CharPos].Count; CharValue++)
                {
                    Miki.CalcIntExt.LongLength(ConvTableQ[CharPos][CharValue][^1], out int DigLen);
                    Miki.CalcCompare.GetLongFromList(ConvTableQ[CharPos][CharValue], 4, DigLen, out ActDigFromConv);
                    DigLen = 18 * (ConvTableQ[CharPos][CharValue].Count - 1) + DigLen;

                    if (DigLen >= LastDigLen2)
                    { LastDigLen2 = DigLen; }
                    if (ActDigFromConv > 0)
                    { LastDigFromConv = ActDigFromConv; }

                    LastDigFromConv = (LastDigLen2 * 10000) + ActDigFromConv;
                    RowLenList2.Add(LastDigFromConv);
                }
                DigLenListQLocal.Add(RowLenList2);
            }
            DigLenListQ = DigLenListQLocal;

            LastDigLen2 = 0;
            LastDigFromConv = 0;
            ActDigFromConv = 0;
            List<List<long>> DigLenListRLocal = new();

            for (int CharPos = 0; CharPos < MaskTableR.Count; CharPos++)
            {
                List<long> RowLenList2 = new();
                for (int CharValue = 0; CharValue < MaskTableR[CharPos].Count; CharValue++)
                {
                    Miki.CalcIntExt.LongLength(ConvTableR[CharPos][CharValue][^1], out int DigLen);
                    Miki.CalcCompare.GetLongFromList(ConvTableR[CharPos][CharValue], 4, DigLen, out ActDigFromConv);
                    DigLen = 18 * (ConvTableR[CharPos][CharValue].Count - 1) + DigLen;

                    if (DigLen >= LastDigLen2)
                    { LastDigLen2 = DigLen; }
                    if (ActDigFromConv > 0)
                    { LastDigFromConv = ActDigFromConv; }

                    LastDigFromConv = (LastDigLen2 * 10000) + ActDigFromConv;
                    RowLenList2.Add(LastDigFromConv);
                }
                DigLenListRLocal.Add(RowLenList2);
            }
            DigLenListR = DigLenListRLocal;
            */
            //string FirstPossiblePassP = "";
            string LastPossiblePassP = "";
            for (int CharPos = MaskTableP.Count - 1; CharPos >= 0; CharPos--)
            {
                LastPossiblePassP += MaskTableP[CharPos][^1];
                //FirstPossiblePassP += MaskTableP[CharPos][0];
            }


            //string FirstPossiblePassQ = "";
            string LastPossiblePassQ = "";
            for (int CharPos = MaskTableQ.Count - 1; CharPos >= 0; CharPos--)
            {
                LastPossiblePassQ += MaskTableQ[CharPos][^1];
                //FirstPossiblePassQ += MaskTableQ[CharPos][0];
            }

            //string FirstPossiblePassR = "";
            string LastPossiblePassR = "";
            for (int CharPos = MaskTableR.Count - 1; CharPos >= 0; CharPos--)
            {
                LastPossiblePassR += MaskTableR[CharPos][^1];
                //FirstPossiblePassR += MaskTableR[CharPos][0];
            }


            //Convert to string decimal First and Last Possible Password
            //DigFromWordByTab(in FirstPossiblePassP, MaskTableP, ConvTableP, out string FirstPossibleDecStringPLocal);
            DigFromWordByTab(in LastPossiblePassP, MaskArrayP, ConvListArrayP, out string LastPossibleDecStringPLocal);
            //DigFromWordByTab(in FirstPossiblePassQ, MaskTableQ, ConvTableQ, out string FirstPossibleDecStringQLocal);
            DigFromWordByTab(in LastPossiblePassQ, MaskArrayQ, ConvListArrayQ, out string LastPossibleDecStringQLocal);
            //DigFromWordByTab(in FirstPossiblePassR, MaskTableR, ConvTableR, out string FirstPossibleDecStringRLocal);
            DigFromWordByTab(in LastPossiblePassR, MaskArrayR, ConvListArrayR, out string LastPossibleDecStringRLocal);

            LastPossibleDecStringP = LastPossibleDecStringPLocal;
            LastPossibleDecStringQ = LastPossibleDecStringQLocal;
            LastPossibleDecStringR = LastPossibleDecStringRLocal;

            Console.WriteLine();
            Console.WriteLine("Max first (p) word value:                  " + LastPossiblePassP + " ->");
            Console.WriteLine("In Decimal:                                " + Miki.CalcStringExt.SplitString(LastPossibleDecStringPLocal));
            Console.WriteLine("Max second (q) word value:                 " + LastPossiblePassQ + " ->");
            Console.WriteLine("In Decimal:                                " + Miki.CalcStringExt.SplitString(LastPossibleDecStringQLocal));
            Console.WriteLine("Max third (r) word value:                  " + LastPossiblePassR + " ->");
            Console.WriteLine("In Decimal:                                " + Miki.CalcStringExt.SplitString(LastPossibleDecStringRLocal)); //From Progress (LastBaseDec - FirstBaseDec)+1
            Console.WriteLine();
        }

        public static void DigFromWordByTab(in string MyWord, in char[][] MyMaskArray, in List<long>[][] MyConvArray, out string DigString)
        {
            //convert n-char password to string-number
            List<char> CharList = new(MyWord);
            int k = 0;
            char[][] MaskArrayLocal = MyMaskArray;

            List<long> DigList = new List<long>() { 0 };
            List<long>[][] ConvArrayLocal = MyConvArray;

            for (int CharPos = CharList.Count - 1; CharPos >= 0; CharPos--)
            {
                int CharValue = Array.IndexOf(MaskArrayLocal[CharPos], (CharList[k]));
                List<long> CharValueList = ConvArrayLocal[CharPos][CharValue];
                DigList = Miki.CalcLists.Add(DigList, CharValueList);
                k++;
            }
            Miki.CalcConvert.LongListToString(in DigList, "000000000000000000", out string Dig);
            DigString = Dig;
            return;
        }


        public static void WordFromDigByTab(in string DigString, in char[][] MyMaskArray, in List<long>[][] MyConvArray, in long[][] MyDigLenArray, in int MyPosForDigs, in long MyPosForDigsMult, in List<long>[] MyAllUnarysValueListArray, in int IsUnary, out string WordString)
        {
            long PosForDigsMultLocal = MyPosForDigsMult;
            int PosForDigsLocal = MyPosForDigs;
            Miki.CalcConvert.StringToLongList(in DigString, 18, out List<long> DigPassCurrListLocal);

            char[][] MaskArrayLocal = MyMaskArray;
            List<long>[][] ConvArrayLocal = MyConvArray;
            long[][] DigLenArrayLocal = MyDigLenArray;

            List<long>[] AllUnarysValueListArrayLocal = MyAllUnarysValueListArray;
            int IsUnaryLocal = IsUnary;
            List<long> UnaryToCompare = new();

            List<long> IterPlus = new() { 1 };

            int CompareBigger = 10;

            int FirstNonZero = 0;

            int CharPosMax = MyMaskArray.Length - 1;
            int CharPosForUnary = MyMaskArray.Length - 1;
            List<long> ToCompare = DigPassCurrListLocal;
            List<long> ToCompareFull = DigPassCurrListLocal;
            List<long> UnarySubtracted = new();
            int IsUnarySubtracted = 0;
            //int IsBigger = 10; //temporary
            if (IsUnary == 1)
            {
                for (int i = CharPosMax; i > 0; i--)
                {
                    UnaryToCompare = AllUnarysValueListArrayLocal[i];
                    Miki.CalcCompare.ListBigger(in ToCompare, ToCompare.Count, in UnaryToCompare, UnaryToCompare.Count, out int IsBigger);
                    if (IsBigger != 2) // if AllUnaryValueListLocal[CharPos] is equal bigger than ToCompare
                    {
                        ToCompare = Miki.CalcLists.Sub(ToCompare, UnaryToCompare); //we subtract UnaryToCompare
                        IsUnarySubtracted = 1;
                        //now we must tell converter, which CharPos can take unary value
                        CharPosForUnary = i;
                        break;
                    }
                }
            }
            List<char> MyCharList = new(); //new password in char
            List<string> PassFromDigList = new(); //new password in string

            //string NewPass;
            FirstNonZero = 0;

            while (CharPosMax >= 0)
            {
                if (IsUnarySubtracted == 1 && MaskArrayLocal[CharPosMax].Length == 1)
                {
                    {
                        if (CharPosMax <= CharPosForUnary)
                        {
                            MyCharList.Add(MaskArrayLocal[CharPosMax][0]); //here unary value is possible
                            FirstNonZero = 1;
                        }
                    }
                }
                else
                {
                    int ToCompareCount = ToCompare.Count - 1;
                    long ActDigToCompare;
                    if (ToCompareCount > 0)
                    {
                        Miki.CalcIntExt.LongLength(ToCompare[^1], out int ToCompareLen); //Last pos ToCompare length                                                   
                        Miki.CalcCompare.GetLongFromList(in ToCompare, in PosForDigsLocal, in ToCompareLen, out ActDigToCompare); //YYYY
                        ToCompareLen = 18 * (ToCompareCount) + ToCompareLen; //Full Length XXXX....
                        ActDigToCompare = (ToCompareLen * PosForDigsMultLocal) + ActDigToCompare + 1000000000000000000; //Make 4 places and add ActDigToCompare -> XXXX...YYYY
                    }
                    else
                    {
                        ActDigToCompare = ToCompare[0];
                    }
                    //St1.Start();
                    int StartCompare = 0;
                    int CharPosLength = MaskArrayLocal[CharPosMax].Length - 1;

                    long[] DigLenArrayLocalPart = DigLenArrayLocal[CharPosMax];

                    int mid = (1 + (CharPosLength - 1)) / 2; // ???? int mid = (1 + (CharPosLength - 1)) / 2
                    long ActDigToCompareT = DigLenArrayLocalPart[mid];
                    if (ActDigToCompareT > ActDigToCompare) //zawęź do połowy i szukaj od mid w dół
                    {
                        mid--;
                        for (int a = mid; a >= 1; a--)
                        {
                            ActDigToCompareT = DigLenArrayLocalPart[a];
                            if (ActDigToCompareT <= ActDigToCompare)
                            {
                                StartCompare = a;
                                if (ActDigToCompareT == ActDigToCompare && ActDigToCompare > 999999999999999999)
                                {
                                    //Equal++;
                                    int s = ToCompareCount;

                                    while (s >= 0)
                                    {
                                        if (ToCompare[s] < ConvArrayLocal[CharPosMax][StartCompare][s])
                                        { break; }
                                        else if (ToCompare[s] > ConvArrayLocal[CharPosMax][StartCompare][s])
                                        {
                                            StartCompare--;
                                            if (ToCompareCount + 1 > ConvArrayLocal[CharPosMax][StartCompare].Count)
                                            { break; }
                                            else
                                            {
                                                s = ToCompareCount;
                                                if (StartCompare == 0)
                                                { break; }
                                            }
                                        }
                                        else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                        { s--; }
                                    }

                                }
                                break;
                            }
                        }
                    }
                    else if (ActDigToCompareT < ActDigToCompare) //zawęź do połowy i szukaj od CharPosLength w dół
                    {
                        //mid++;
                        for (int a = CharPosLength; a >= mid; a--)
                        {
                            ActDigToCompareT = DigLenArrayLocalPart[a];
                            if (ActDigToCompareT <= ActDigToCompare)
                            {
                                StartCompare = a;
                                if (ActDigToCompareT == ActDigToCompare && ActDigToCompare > 999999999999999999)
                                {
                                    //Equal++;
                                    int s = ToCompareCount;
                                    while (s >= 0)
                                    {
                                        if (ToCompare[s] < ConvArrayLocal[CharPosMax][StartCompare][s])
                                        { break; }
                                        else if (ToCompare[s] > ConvArrayLocal[CharPosMax][StartCompare][s])
                                        {
                                            StartCompare--;
                                            if (ToCompareCount + 1 > ConvArrayLocal[CharPosMax][StartCompare].Count)
                                            { break; }
                                            else
                                            {
                                                s = ToCompareCount;
                                                if (StartCompare == 0)
                                                { break; }
                                            }
                                        }
                                        else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                        { s--; }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        StartCompare = mid;
                        //Equal++;
                        if (ActDigToCompare > 999999999999999999)
                        {
                            int s = ToCompareCount;
                            while (s >= 0)
                            {
                                if (ToCompare[s] < ConvArrayLocal[CharPosMax][StartCompare][s])
                                { break; }
                                else if (ToCompare[s] > ConvArrayLocal[CharPosMax][StartCompare][s])
                                {
                                    StartCompare--;
                                    if (ToCompareCount + 1 > ConvArrayLocal[CharPosMax][StartCompare].Count)
                                    { break; }
                                    else
                                    {
                                        s = ToCompareCount;
                                        if (StartCompare == 0)
                                        { break; }
                                    }
                                }
                                else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                { s--; }
                            }
                        }
                    }

                    //St2.Stop();
                    //if (MyEnd != StartCompare)
                    //{ Console.WriteLine("MyEnd != StartCompare"); }

                    if (StartCompare > 0 || FirstNonZero == 1 || CharPosMax == 0)
                    {
                        //St1.Start();
                        ToCompare = Miki.CalcLists.Sub(in ToCompare, in ConvArrayLocal[CharPosMax][StartCompare]);
                        //St2.Stop();
                        MyCharList.Add(MaskArrayLocal[CharPosMax][StartCompare]);
                        FirstNonZero = 1;
                        //St1.Stop();
                    }
                }
                CharPosMax--;
            } //end loop for char position in password (from up)
            WordString = new string(MyCharList.ToArray());
        }

        public static void DigToWord()
        {

            while (true)
            {
                Console.WriteLine("#####################################################################################");
                bool isIntDig = false;
                int LineRev = 0;
                string DigString = "";

                while (isIntDig == false)
                {
                    Console.WriteLine("Enter decimal number:");
                    DigString = Console.ReadLine();
                    isIntDig = Miki.Miki.isNumeric(DigString);
                    LineRev += 1;
                    if (isIntDig == false)
                    { Console.WriteLine("Digits only!!!... try again..."); LineRev += 2; }
                }


                while (DigString[..1] == "0") //first digit can't be 0
                {
                    if (DigString.Length == 1) { break; }
                    DigString = DigString[1..];
                }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", DigString);

                Miki.CalcCompare.StringBigger(in LastPossibleDecStringP, in DigString, out int BiggerP);
                if (BiggerP != 2)
                {
                    WordFromDigByTab(in DigString, in MaskArrayP, in ConvListArrayP, in DigLenArrayP, in PosForDigsP, in PosForDigsMultP, in AllUnarysValueListArrayP, in IsUnaryP, out string WordStringP);
                    Console.WriteLine("(p) word is:");
                    Console.WriteLine(WordStringP);
                }
                else { Console.WriteLine("Your number=" + DigString + "  is bigger than last possible decimal (p)=" + LastPossibleDecStringP); }

                Miki.CalcCompare.StringBigger(in LastPossibleDecStringQ, in DigString, out int BiggerQ);
                if (BiggerQ != 2)
                {
                    WordFromDigByTab(in DigString, in MaskArrayQ, in ConvListArrayQ, in DigLenArrayQ, in PosForDigsQ, in PosForDigsMultQ, in AllUnarysValueListArrayQ, in IsUnaryQ, out string WordStringQ);
                    Console.WriteLine("(q) word is:");
                    Console.WriteLine(WordStringQ);
                }
                else { Console.WriteLine("Your number=" + DigString + "  is bigger than last possible decimal (q)=" + LastPossibleDecStringQ); }

                Miki.CalcCompare.StringBigger(in LastPossibleDecStringR, in DigString, out int BiggerR);
                if (BiggerR != 2)
                {
                    WordFromDigByTab(in DigString, in MaskArrayR, in ConvListArrayR, in DigLenArrayR, in PosForDigsR, in PosForDigsMultR, in AllUnarysValueListArrayR, in IsUnaryR, out string WordStringR);
                    Console.WriteLine("(r) word is:");
                    Console.WriteLine(WordStringR);
                }
                else { Console.WriteLine("Your number=" + DigString + "  is bigger than last possible decimal (r)=" + LastPossibleDecStringR); }
            }
        }

        public static void WordToDig()
        {
            while (true)
            {
                Console.WriteLine("#####################################################################################");

                int FitToMaskP = 10;
                int FitToMaskQ = 10;
                int FitToMaskR = 10;

                string Word = "";


                Console.WriteLine("Enter word that fits into any mask (p, q or r):");
                Word = Console.ReadLine();

                FitToMaskP = FitToMask(Word, MaskTableP);
                FitToMaskQ = FitToMask(Word, MaskTableQ);
                FitToMaskR = FitToMask(Word, MaskTableR);

                if (FitToMaskP == 0)
                {
                    DigFromWordByTab(in Word, in MaskArrayP, in ConvListArrayP, out string DigStringP);
                    Console.WriteLine("(p) decimal is:");
                    Console.WriteLine(DigStringP);
                }
                else
                {
                    if (FitToMaskP == 1)
                    { Console.WriteLine("Word (p) is to long!!!"); }
                    if (FitToMaskP == 2)
                    { Console.WriteLine("Word (p) doe's not match the mask!!!"); }
                }


                if (FitToMaskQ == 0)
                {
                    DigFromWordByTab(in Word, in MaskArrayQ, in ConvListArrayQ, out string DigStringQ);
                    Console.WriteLine("(q) decimal is:");
                    Console.WriteLine(DigStringQ);
                }
                else
                {
                    if (FitToMaskQ == 1)
                    { Console.WriteLine("Word (q) is to long!!!"); }
                    if (FitToMaskQ == 2)
                    { Console.WriteLine("Word (q) doe's not match the mask!!!"); }
                }


                if (FitToMaskR == 0)
                {
                    DigFromWordByTab(in Word, in MaskArrayR, in ConvListArrayR, out string DigStringR);
                    Console.WriteLine("(r) decimal is:");
                    Console.WriteLine(DigStringR);
                }
                else
                {
                    if (FitToMaskR == 1)
                    { Console.WriteLine("Word (r) is to long!!!"); }
                    if (FitToMaskR == 2)
                    { Console.WriteLine("Word (r) doe's not match the mask!!!"); }
                }


            }

        }

        public static int FitToMask(string Dig, List<List<char>> Mask)
        {
            List<char> DigList = Dig.ToArray().Reverse().ToList();
            //int Output = 0;
            //int NoChar = 0;
            int ToLong = 0;
            //0 - fit to mask, 1 - to long, 2 - no char

            if (DigList.Count > Mask.Count)
            { ToLong++; return 1; }
            else
            {
                if (DigList.Count == 0)
                { return 2; }

                for (int i = 0; i < DigList.Count; i++)
                {
                    if (Mask[i].IndexOf(DigList[i]) == -1)
                    { return 2; }
                }
            }
            return 0;
        }
        public static void CalculatorPoly()
        {
            string Dig1 = "";
            string Word1 = "";
            string Dig2 = "";
            string Word2 = "";
            string Word3 = "";
            string CalcType = "";
            int FitToMaskP = 10;
            int FitToMaskQ = 10;
            bool isCalcType = false;
            int LineRev = 0;
            while (true)
            {
                Console.WriteLine("##############################################################################################");
                while (FitToMaskP != 0)
                {
                    Console.WriteLine("Enter word that fit to the (p) mask, confirm Enter:");
                    //while (Dig1 == "")
                    { Word1 = Console.ReadLine(); }

                    FitToMaskP = FitToMask(Word1, MaskTableP);
                    LineRev += 1;
                    if (FitToMaskP == 1)
                    { Console.WriteLine("Word is to long!!!... try again..."); LineRev += 2; }
                    if (FitToMaskP == 2)
                    { Console.WriteLine("Word doe's not match the (p) mask!!!... try again..."); LineRev += 2; }

                }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", Word1);
                LineRev = 0;

                while (isCalcType == false)
                {
                    Console.WriteLine("Enter +, -, *, /, ^ , confirm Enter:");
                    CalcType = Console.ReadLine();
                    LineRev += 1;
                    if (CalcType != "+" && CalcType != "-" && CalcType != "*" && CalcType != "/" && CalcType != "^")
                    { Console.WriteLine("Only +, -, *, /, ^ ... try again..."); LineRev += 2; }
                    else { isCalcType = true; }
                }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", CalcType);
                LineRev = 0;


                while (FitToMaskQ != 0)
                {
                    Console.WriteLine("Enter word that fit to the (q) mask, confirm Enter:");
                    //while (Dig1 == "")
                    { Word2 = Console.ReadLine(); }

                    FitToMaskQ = FitToMask(Word2, MaskTableQ);
                    LineRev += 1;
                    if (FitToMaskQ == 1)
                    { Console.WriteLine("Word is to long!!!... try again..."); LineRev += 2; }
                    if (FitToMaskQ == 2)
                    { Console.WriteLine("Word doe's not match the (q) mask!!!... try again..."); LineRev += 2; }

                }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", Word2);
                LineRev = 0;



                List<string> MyResult;
                List<string> MyResultCheck;
                string Check = "Check = FALSE";

                DigFromWordByTab(in Word1, in MaskArrayP, in ConvListArrayP, out Dig1);
                DigFromWordByTab(in Word2, in MaskArrayQ, in ConvListArrayQ, out Dig2);

                if (CalcType == "+")
                {
                    Console.WriteLine("###DIGS###");
                    Console.WriteLine(Dig1);
                    Console.WriteLine("+");
                    Console.WriteLine(Dig2);
                    Console.WriteLine("##########");
                    MyResult = CalcStrings.Add(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Sub(MyResult[0], Dig2);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Miki.CalcCompare.StringBigger(in LastPossibleDecStringP, MyResult[0], out int BiggerP);
                    if (BiggerP != 1)
                    {
                        WordFromDigByTab(MyResult[0], in MaskArrayR, in ConvListArrayR, in DigLenArrayR, in PosForDigsR, in PosForDigsMultR, in AllUnarysValueListArrayR, in IsUnaryR, out Word3);
                        Console.WriteLine(Word3);
                        Console.WriteLine(Check);
                    }
                    else
                    { Console.WriteLine("Result is bigger then last possible decimal (r)..."); }
                }

                if (CalcType == "-")
                {
                    Console.WriteLine("###DIGS###");
                    Console.WriteLine(Dig1);
                    Console.WriteLine("-");
                    Console.WriteLine(Dig2);
                    Console.WriteLine("##########");
                    MyResult = CalcStrings.Sub(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Add(MyResult[0], Dig2);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Miki.CalcCompare.StringBigger(in LastPossibleDecStringP, MyResult[0], out int BiggerP);
                    if (BiggerP != 1)
                    {
                        WordFromDigByTab(MyResult[0], in MaskArrayR, in ConvListArrayR, in DigLenArrayR, in PosForDigsR, in PosForDigsMultR, in AllUnarysValueListArrayR, in IsUnaryR, out Word3);
                        Console.WriteLine(Word3);
                        Console.WriteLine(Check);
                    }
                    else
                    { Console.WriteLine("Result is bigger then last possible decimal (r)..."); }
                }

                if (CalcType == "*")
                {
                    Console.WriteLine("###DIGS###");
                    Console.WriteLine(Dig1);
                    Console.WriteLine("*");
                    Console.WriteLine(Dig2);
                    Console.WriteLine("##########");
                    MyResult = CalcStrings.Mul(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Div(MyResult[0], Dig2);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Miki.CalcCompare.StringBigger(in LastPossibleDecStringP, MyResult[0], out int BiggerP);
                    if (BiggerP != 1)
                    {
                        WordFromDigByTab(MyResult[0], in MaskArrayR, in ConvListArrayR, in DigLenArrayR, in PosForDigsR, in PosForDigsMultR, in AllUnarysValueListArrayR, in IsUnaryR, out Word3);
                        Console.WriteLine(Word3);
                        Console.WriteLine(Check);
                    }
                    else
                    { Console.WriteLine("Result is bigger then last possible decimal (r)..."); }
                }

                if (CalcType == "/")
                {
                    Console.WriteLine("###DIGS###");
                    Console.WriteLine(Dig1);
                    Console.WriteLine("/");
                    Console.WriteLine(Dig2);
                    Console.WriteLine("##########");
                    MyResult = CalcStrings.Div(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Mul(MyResult[0], Dig2);
                    MyResultCheck = CalcStrings.Add(MyResultCheck[0], MyResult[1]);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0] + "  R  " + MyResult[1]);

                    Miki.CalcCompare.StringBigger(in LastPossibleDecStringP, MyResult[0], out int BiggerP);
                    Miki.CalcCompare.StringBigger(in LastPossibleDecStringP, MyResult[1], out int BiggerPR);
                    if (BiggerP != 1 && BiggerPR != 1)
                    {
                        WordFromDigByTab(MyResult[0], in MaskArrayR, in ConvListArrayR, in DigLenArrayR, in PosForDigsR, in PosForDigsMultR, in AllUnarysValueListArrayR, in IsUnaryR, out Word3);
                        WordFromDigByTab(MyResult[0], in MaskArrayR, in ConvListArrayR, in DigLenArrayR, in PosForDigsR, in PosForDigsMultR, in AllUnarysValueListArrayR, in IsUnaryR, out string WordR);
                        Console.WriteLine(Word3 + "  R  " + WordR);
                        Console.WriteLine(Check);
                    }
                    else
                    { Console.WriteLine("Quotient or Rest is bigger then last possible decimal (r)..."); }
                }

                if (CalcType == "^")
                {
                    Console.WriteLine("###DIGS###");
                    Console.WriteLine(Dig1);
                    Console.WriteLine("^");
                    Console.WriteLine(Dig2);
                    Console.WriteLine("##########");
                    MyResult = CalcStrings.Pow(Dig1, Dig2);

                    string Dig2Rev = Dig2;
                    MyResultCheck = MyResult;
                    while (true)
                    {
                        MyResultCheck = CalcStrings.Div(MyResultCheck[0], Dig1);
                        Dig2Rev = CalcStrings.Sub(Dig2Rev, "1")[0];
                        if (Dig2Rev == "1")
                        { break; }
                    }

                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Miki.CalcCompare.StringBigger(in LastPossibleDecStringP, MyResult[0], out int BiggerP);
                    if (BiggerP != 1)
                    {
                        WordFromDigByTab(MyResult[0], in MaskArrayR, in ConvListArrayR, in DigLenArrayR, in PosForDigsR, in PosForDigsMultR, in AllUnarysValueListArrayR, in IsUnaryR, out Word3);
                        Console.WriteLine(Word3);
                        Console.WriteLine(Check);
                    }
                    else
                    { Console.WriteLine("Result is bigger then last possible decimal (r)..."); }
                }


                Dig1 = "";
                Dig2 = "";
                CalcType = "";
                FitToMaskP = 10;
                FitToMaskQ = 10;
                isCalcType = false;
            }

        }

    }
}

namespace LehmerGen
{
    class Passwords
    {
        public static List<long> IncrementList18; //Increment list needed for adding
        public static List<long> IncrementList9; //Increment list needed for multiplication
        public static List<long> DigPassCurrList; //needed for password generator
        //public static List<char> CharactersList; //read config, MakeConvTable
        public static string ProgressIterDoneString = ""; // Saved in SessionProgress.txt
        public static string ProgressIterToDoString = ""; // Next to do = ProgressIterDoneString+1
        public static string ProgressIterStartString = ""; //First iteration in Session, for SaveProgress
        public static List<long> ProgressIterDoneList18; //main and generator
        public static List<long> WorkingIter;

        public static List<long> ProgressPoolMaxList; //main and generator
        public static List<long> ProgressIterToDoList18;
        public static int PassGeneratedBase = 0; //pass base generated from generator
        public static string ProgressAllCountString = ""; //Total password count if fixed length for Initialize and generator
        public static string ProgressPoolMaxString = ""; //Max value to iterate in session, main and generatedictionary

        public static string IncrementBase; //Increment base string
        public static string IncrementPower; //Increment power string
        public static string Increment = ""; //Increment string. Only InitializeGenerator
        public static int PassMinLength; //Password min length int
        public static int PassMaxLength; //Password max length int
        public static int HCDictSize;
        public static int JTRDictSize;
        public static int A7zDictSize;
        public static int H7zDictSize;
        public static int TestPassCheck;
        public static int TestPassLoops;
        public static int CounterDisplay;
        public static int IsUnary = 0;
        public static List<List<char>> MaskTable = new();
        public static char[][] MaskArray = new char[1][];
        //public static List<List<List<long>>> ConvTable = new();
        public static List<long>[][] ConvListArray = new List<long>[1][];
        //public static List<List<long>> AllUnarysValueList = new();
        public static List<long>[] AllUnarysValueListArray = new List<long>[1];
        //public static List<List<long>> DigLenList = new();
        //public static List<List<List<long>>> DigLenListNew = new();
        public static long[][] DigLenArray = new long[1][];
        //public static long[][] DigFirstArray = new long[1][];
        //public static long[][] DigLArray = new long[1][];
        //public static long[][] DigFArray = new long[1][];

        public static List<long> FirstPossibleDecList = new();
        public static List<long> LastPossibleDecList = new(); //only for MakeConvTable
        public static List<long> RealPossibleCountList = new();
        public static string TotalPossibleCountString = "";
        public static string RealPossibleCountString = "";
        public static int FillToLength;
        public static string GeneratorExitReasonPool = "";
        public static string GeneratorExitReasonTotal = "";
        public static List<string> TempRemoved = new();

        public static int PosForDigs;
        public static long PosForDigsMult;
        public static char[] ZeroPassN = [];
        public static char[] ZeroPassU = [];

        public static bool CheckPrime(int num)
        {
            int a = 0;
            for (int i = 1; i <= num; i++)
            {
                if (num % i == 0)
                {
                    a++;
                }
            }
            if (a == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<List<long>> PrimeDecomp(List<long> number)
        {
            var primes = new List<List<long>>();
            List<long> div = new List<long>(10) { 2 };
            //int div = 2; div <= number; div++
            while (true)
            {
                while (true)
                {
                    List<List<long>> MyDiv = Miki.CalcLists.Div(number, div);
                    //Miki.CalcConvert.LongListToString(MyDiv[1], "000000000000000000", out string Rest);
                    if (MyDiv[1].Count != 1)
                    { break; }
                    if (MyDiv[1][0] != 0)
                    { break; }
                    primes.Add(div);
                    Miki.CalcConvert.LongListToString(div, "000000000000000000", out string MyPrime);
                    Console.WriteLine(MyPrime);
                    number = MyDiv[0];
                }
                div = Miki.CalcLists.Add(div, new List<long>(1) { 1 });
                Miki.CalcCompare.ListBigger(in number, number.Count, in div, div.Count, out int Bigger);
                //Miki.CalcConvert.LongListToString(in div, "000000000000000000", out string MyOut);
                //Console.WriteLine(MyOut);
                if (Bigger == 2) { break; }
            }
            return primes;
        }
        public static void InitializeGenerator()
        {

            Increment = Miki.CalcStrings.Pow(IncrementBase, IncrementPower)[0];
            Miki.CalcConvert.StringToLongList(in Increment, 18, out IncrementList18);
            Miki.CalcConvert.StringToLongList(in Increment, 9, out IncrementList9);
            //List<List<long>> aaa = Generate(IncrementList18);
            //Miki.CalcConvert.StringToLongList("775474784289619438457444226490432340170844772643169136033", 18, out List<long> list);
            //List<List<long>> aaa = PrimeDecomp(list);
            MakeConvTable();

            Console.WriteLine("Checking Increment value...");
            Console.WriteLine("IncrementBase value = " + IncrementBase);
            if (IncrementBase == "1")
            { Console.WriteLine("IncrementBase IS NOT a Prime Number, but 1 is OK..."); }
            else
            {
                bool IncrementBaseIsPrime = CheckPrime(Convert.ToInt32(IncrementBase));
                if (IncrementBaseIsPrime)
                { Console.WriteLine("IncrementBase is a Prime Number -> OK..."); }
                else
                {
                    Console.WriteLine("IncrementBase IS NOT a Prime Number -> Change IncrementBase value...");
                    System.Environment.Exit(10);
                }
            }


            Miki.CalcConvert.LongListToString(in RealPossibleCountList, "000000000000000000", out ProgressAllCountString);
            string String1 = Miki.CalcStringExt.SplitString(Increment);

            //Console.WriteLine("Increment = " + String1); było: RealPossibleCountString 
            if (Miki.CalcStrings.Div(TotalPossibleCountString, IncrementBase)[1] != "0" || Increment == "1") //Check if Increment value is good for LCG
            {
                if (Increment == "1")
                {
                    Console.WriteLine("Total Real Pool Count % Increment == 0, but ->");
                    Console.WriteLine("Increment value == 1 -> OK...");
                }
                else
                { Console.WriteLine("Total Real Pool Count % IncrementBase != 0 -> OK..."); }
            }
            else
            {
                Console.WriteLine("Total Pool Real Count % IncrementBase == 0 -> Wrong Increment value...");
                Console.WriteLine("Change the Increment configuration in GENERATOR CONFIGURATION section in Config.txt file...");
                System.Environment.Exit(10);
            }

            List<long> WorkingIterLocal = Miki.CalcLists.Mul(ProgressIterToDoList18, IncrementList9);
            //WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, FirstPossibleDecList);
            WorkingIter = WorkingIterLocal;
            List<long> WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountList)[1];
            WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecList);
            DigPassCurrList = new(WorkingDigList);

        }


        public static void MakeConvTable()
        {
            Console.WriteLine("Conversion tables - generating...");

            //List<char> CharactersListLocal = CharactersList;

            List<List<char>> MaskTableLocal = MaskTable; // to create polynomial numbering system
            //List<List<List<long>>> ConvTableLocal = new();
            int PassMaxLenLocal = PassMaxLength;

            Array.Resize(ref MaskArray, MaskTable.Count);
            Array.Resize(ref ZeroPassN, PassMaxLength);
            Array.Resize(ref ZeroPassU, PassMaxLength);
            
            for (int i = 0; i < MaskTable.Count; i++)
            {
                Array.Resize(ref MaskArray[i], MaskTable[i].Count);
                MaskArray[i] = MaskTable[i].ToArray();
                ZeroPassN[i] = MaskArray[i][0];
                ZeroPassU[i] = MaskArray[i][0];
            }
            Array.Reverse(ZeroPassU);
            Array.Reverse(ZeroPassN);
            string aaa = new string(ZeroPassU);
            /////
            List<List<long>> PosMultiplier = new();
            List<List<long>> PosMultiplier18Local = new();
            PosMultiplier.Add(new List<long>(1) { 1 }); //on ones place always Multipier==1

            for (int i = 1; i < MaskTableLocal.Count; i++) //on higher places is:
            {
                List<long> CharCount = new(1) { (long)MaskTableLocal[i - 1].Count }; //previous place characters count *
                PosMultiplier.Add(Miki.CalcLists.Mul(PosMultiplier[i - 1], CharCount)); // * previous PosMultiplier
            }

            //List<long>[][] ConvListArrayLocal = new List<long>[1][];
            int LastUnaryPos = -1;
            Array.Resize(ref ConvListArray, MaskTableLocal.Count);
            for (int CharPos = 0; CharPos < MaskTableLocal.Count; CharPos++)
            {
                //List<List<long>> ConvRow = new();
                Array.Resize(ref ConvListArray[CharPos], MaskTableLocal[CharPos].Count);
                for (int CharValue = 0; CharValue < MaskTableLocal[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableLocal[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except ones place, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0) //       && CharValue == 0
                        {
                            CharIntToMul = new() { 0 }; //unary zero in ones place
                        }
                        else
                        {
                            CharIntToMul = new() { 1 }; //rest of unaries, they has values
                        }
                        IsUnary = 1; //to inform converter, that there is unary
                        LastUnaryPos = CharPos;
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, PosMultiplier[CharPos]);
                    //ConvRow.Add(MultResult);
                    ConvListArray[CharPos][CharValue] = MultResult;
                    //Console.WriteLine(ConvListArray[CharPos][CharValue][^1]);
                }
                //ConvTableLocal.Add(ConvRow);
            }
            //ConvTable = ConvTableLocal;

            List<long> AllUnarysValueLocal = new() { 0 };
            //List<List<long>> AllUnarysValueListLocal = new();
            //List<long>[] AllUnarysValueListArrayLocal = new List<long>[MaskTableLocal.Count];
            Array.Resize(ref AllUnarysValueListArray, MaskTableLocal.Count);
            for (int CharPos = 0; CharPos < MaskTableLocal.Count; CharPos++)
            {
                if (MaskTableLocal[CharPos].Count == 1)
                {
                    AllUnarysValueLocal = Miki.CalcLists.Add(AllUnarysValueLocal, ConvListArray[CharPos][0]);
                    //AllUnarysValueListLocal.Add(AllUnarysValueLocal);
                    AllUnarysValueListArray[CharPos] = AllUnarysValueLocal;
                }
                else
                {
                    //AllUnarysValueListLocal.Add(AllUnarysValueLocal);
                    AllUnarysValueListArray[CharPos] = AllUnarysValueLocal;
                }
            }
            //AllUnarysValueList = AllUnarysValueListLocal;

            int LastDigLen2 = 0;
            long LastDigFromConv = 0;
            long ActDigFromConv = 0;

            //Concept, make long list to compare (DigLenList) similar to ConvTable, before compare with ConvTable,
            //it will contain XXXX...YYYY numbers, where
            //XXXX... full number length, YYYY - first digits from this number
            //look in PassListFromDigMyTab (variable ActDigToCompare)

            List<long> LongestList = ConvListArray[ConvListArray.Length - 1][ConvListArray[ConvListArray.Length - 1].Length - 1];
            Miki.CalcIntExt.LongLength(LongestList[^1], out int LongestListDigLen);
            LongestListDigLen = LongestListDigLen + (LongestList.Count - 1) * 18;
            Miki.CalcIntExt.LongLength(LongestListDigLen, out int PosForLengthLocal);
            int PosForDigsLocal = 18 - PosForLengthLocal;
            Miki.CalcIntExt.LongPower(10, in PosForDigsLocal, out long PosForDigsMultLocal);

            PosForDigs = PosForDigsLocal;
            PosForDigsMult = PosForDigsMultLocal;

            Array.Resize(ref DigLenArray, MaskTableLocal.Count);

            for (int CharPos = 0; CharPos < MaskTableLocal.Count; CharPos++)
            {

                Array.Resize(ref DigLenArray[CharPos], MaskTableLocal[CharPos].Count);
                //Array.Resize(ref DigLenArray2[CharPos], MaskTableLocal[CharPos].Count);

                for (int CharValue = 0; CharValue < MaskTableLocal[CharPos].Count; CharValue++)
                {
                    int ConvListArrCount = ConvListArray[CharPos][CharValue].Count;
                    if (ConvListArrCount > 1)
                    {
                        Miki.CalcIntExt.LongLength(ConvListArray[CharPos][CharValue][^1], out int DigLen); // First step to make XXXX....
                        Miki.CalcCompare.GetLongFromList(ConvListArray[CharPos][CharValue], PosForDigsLocal, in DigLen, out ActDigFromConv); //four digits to add later (YYYY)
                        DigLen = 18 * (ConvListArrCount - 1) + DigLen; //Now we have XXXX, full digit length

                        //to replace zeros with proper length and digits
                        if (DigLen >= LastDigLen2)
                        { LastDigLen2 = DigLen; }
                        //if (ActDigFromConv > 0)
                        //{ LastDigFromConv = ActDigFromConv; }
                        //int.MaxValue = 2 147 483 647 -> DigLen (XXXX) is int, LastDigFromConv is long
                        //so to long.MaxValue in this concept 18digits we can add max 5 places to DigLen making LastDigFromConv,
                        //I add 4 places, that's enough, then add four digits (YYYY) from earlier calculations
                        LastDigFromConv = (LastDigLen2 * PosForDigsMultLocal) + ActDigFromConv; //now we have XXXX...YYYY
                                                                                                //Console.WriteLine(k + "   " + LastDigLen2 + "   " + LastDigFromConv2);
                        DigLenArray[CharPos][CharValue] = LastDigFromConv + 1000000000000000000;
                    }
                    else
                    { DigLenArray[CharPos][CharValue] = ConvListArray[CharPos][CharValue][^1]; }
                    //Console.WriteLine(DigLenArray[CharPos][CharValue]);
                }
            }

            string FirstPossiblePass = "";
            string LastPossiblePass = "";

            for (int CharPos = PassMaxLength - 1; CharPos >= 0; CharPos--)
            {
                LastPossiblePass += MaskTableLocal[CharPos][^1];
            }
            if (IsUnary != 1)
            {
                for (int CharPos = PassMinLength - 1; CharPos >= 0; CharPos--)
                {
                    FirstPossiblePass += MaskTableLocal[CharPos][0];
                }
            }
            else 
            {
                //Array.Reverse(ZeroPassU);
                //FirstPossiblePass = new string(ZeroPassU[0..(LastUnaryPos+1)]);
                FirstPossiblePass = new string(ZeroPassU[(PassMaxLenLocal - LastUnaryPos-1)..]);
                //var FirstPossiblePassRevArr = FirstPossiblePass.ToArray();
                //Array.Reverse(FirstPossiblePassRevArr);
                //FirstPossiblePass = new string(FirstPossiblePassRevArr);

            }
            List<long> OnPosComb = new() { MaskTableLocal[0].Count };
            List<List<long>> OnPosCombList = new();
            OnPosCombList.Add(OnPosComb);

            for (int i = 1; i < MaskTableLocal.Count; i++) //9 digit list
            {
                OnPosComb = Miki.CalcLists.Mul(OnPosCombList[i - 1], new List<long>() { MaskTableLocal[i].Count }); //out na 18 i usunąć następną pętlę
                OnPosCombList.Add(OnPosComb);
            }

            /*
            for (int i = 0; i < OnPosCombList.Count; i++) //Convert 9 digit list to 18 digit list
            {
                Miki.CalcConvert.ConvertList9To18(OnPosCombList[i], out List<long> OnPosComb18);
                OnPosCombList[i] = OnPosComb18;
            }
            */
            List<List<long>> OnPosCombTotal = new();
            OnPosCombTotal.Add(OnPosCombList[0]);

            for (int i = 1; i < OnPosCombList.Count; i++) //Adding all 18 digit lists
            {
                OnPosCombTotal.Add(Miki.CalcLists.Add(OnPosCombList[i], OnPosCombTotal[i - 1]));
            }
            List<long> TotalPossibleCountListLocal = new();

            //Convert to string decimal First and Last Possible Password
            DigFromPassByTab(in FirstPossiblePass, out string FirstPossibleDecStringLocal);
            DigFromPassByTab(in LastPossiblePass, out string LastPossibleDecStringLocal);
            //Convert string decimals to lists
            Miki.CalcConvert.StringToLongList(in FirstPossibleDecStringLocal, 18, out FirstPossibleDecList);
            Miki.CalcConvert.StringToLongList(in LastPossibleDecStringLocal, 18, out LastPossibleDecList);
            //Counting possible Real iterations

            List<long> RealPossibleCountListLocal = Miki.CalcLists.Sub(LastPossibleDecList, FirstPossibleDecList);
            RealPossibleCountListLocal = Miki.CalcLists.Add(RealPossibleCountListLocal, new List<long> { 1 }); //Real possible passwords count list, = Pool
            RealPossibleCountList = RealPossibleCountListLocal;
            Miki.CalcConvert.LongListToString(in RealPossibleCountListLocal, "000000000000000000", out string RealPossibleCountStringLocal); //Real
            RealPossibleCountString = RealPossibleCountStringLocal;
            //Counting Progress iterations
            string ProgressAllCountStringLocal = LastPossibleDecStringLocal; //////
            ProgressAllCountStringLocal = Miki.CalcStrings.Add(ProgressAllCountStringLocal, "1")[0];
            Miki.CalcConvert.StringToLongList(in ProgressAllCountStringLocal, 18, out List<long> ProgressAllCountListLocal);
            ProgressAllCountString = ProgressAllCountStringLocal;

            string TotalPossibleCountStringLocal; //Total = Total password count Fixed + Non Fixed
            if (PassMinLength == PassMaxLength)
            {
                TotalPossibleCountStringLocal = RealPossibleCountStringLocal;  // OK
            }
            else
            {
                if (PassMinLength > 1)
                {
                    TotalPossibleCountListLocal = new(OnPosCombTotal[^1]);
                    for (int i = 0; i < PassMinLength - 1; i++)
                    { TotalPossibleCountListLocal = Miki.CalcLists.Sub(TotalPossibleCountListLocal, OnPosCombList[i]); }
                }
                else
                { TotalPossibleCountListLocal = OnPosCombTotal[^1]; } //ok
                Miki.CalcConvert.LongListToString(TotalPossibleCountListLocal, "000000000000000000", out TotalPossibleCountStringLocal);
            }
            int LongestString = CalcStringExt.SplitString(RealPossibleCountStringLocal).Length;

            string FirstIsPossibleCount = Miki.CalcStrings.Add(LastPossibleDecStringLocal, "1")[0]; //Only for info
            TotalPossibleCountString = FirstIsPossibleCount;
            Console.WriteLine();
            Console.WriteLine("If First possible Password is:             " + MaskTableLocal[0][0] + " ->");
            Console.WriteLine("First Base Password:                       " + MaskTableLocal[0][0]); //From Progress first password
            Console.WriteLine("-> First Base Decimal:                     " + "0".PadLeft(LongestString)); //From Progress first decimal
            Console.WriteLine("Last Base Password:                        " + LastPossiblePass); //From Progress last password
            Console.WriteLine("-> Last Base Decimal:                      " + CalcStringExt.SplitString(LastPossibleDecStringLocal).PadLeft(LongestString)); //From Progress last decimal
            Console.WriteLine("Base Iterations (Passwords) count:         " + CalcStringExt.SplitString(FirstIsPossibleCount).PadLeft(LongestString)); //From Progress (LastBaseDec - FirstBaseDec)+1
            Console.WriteLine();
            Console.WriteLine("First possible Password:                   " + FirstPossiblePass + " ->");//Real first password
            Console.WriteLine("-> First possible Password in Decimal:     " + CalcStringExt.SplitString(FirstPossibleDecStringLocal).PadLeft(LongestString)); //Real First decimal
            Console.WriteLine("Last possible Password:                    " + LastPossiblePass); //Real last password, equal to From Progress last password
            Console.WriteLine("-> Last possible Password in Decimal:      " + CalcStringExt.SplitString(LastPossibleDecStringLocal).PadLeft(LongestString)); //Real last decimal equal to From Progress last decimal
            Console.WriteLine("Base Iterations (Passwords) count:         " + CalcStringExt.SplitString(RealPossibleCountStringLocal)); //Real to do = (LastPossibleDec-FirstPossibleDec) + 1

            Console.WriteLine("Total Full possible Passwords in Decimal:  " + CalcStringExt.SplitString(TotalPossibleCountStringLocal).PadLeft(LongestString)); //Real all length passwords count
            Console.WriteLine("Increment:                                 " + CalcStringExt.SplitString(Increment).PadLeft(LongestString));
            Console.WriteLine();
            Console.WriteLine("Conversion tables - done...");
            Console.WriteLine();
            MaskTable.Clear();
        }


        public static void ListArrayPerf()
        {
            char[][] MaskArrayLocal = MaskArray;
            //List<List<char>> MaskTableLocal = MaskTable;
            List<long>[][] ConvListArrayLocal = ConvListArray;
            //List<List<List<long>>> ConvTableLocal = ConvTable;
            Stopwatch St1 = new Stopwatch();
            Stopwatch St2 = new Stopwatch();
            Stopwatch St3 = new Stopwatch();
            long[][] DigLenArrayLocal = DigLenArray;

            char[] ArrChar = new char[32];
            List<long> List1 = ConvListArrayLocal[31][60];
            List<long> List2 = ConvListArrayLocal[22][60];
            List<long> MyOutput2 = new();
            //List<long> MyOutput = new ();

            for (int k = 0; k < 1000; k++)
            {

                St1.Start();
                for (int i = 0; i < 10000; i++)
                {
                    List<long> MyOutput = Miki.CalcLists.Sub(in List1, in List2);
                }
                St1.Stop();


                St2.Start();
                for (int i = 0; i < 10000; i++)
                {
                    //List<long> MyOutput = Miki.CalcLists.SubNew(ref List1, ref List2);
                }
                St2.Stop();

                St3.Start();
                for (int i = 0; i < 10000; i++)
                {
                    //Miki.CalcLists.SubV(List1, List2, out List<long> MyOutput);
                }
                St3.Stop();

            }
            Console.WriteLine("St1 = " + St1.Elapsed.TotalMilliseconds);
            Console.WriteLine("St2 = " + St2.Elapsed.TotalMilliseconds);
            Console.WriteLine("St3 = " + St3.Elapsed.TotalMilliseconds);
            //Lis.Reset();
            //Arr.Reset();

        }



        public static List<string> PassListFromDigByTabN(int PassContToBeGen, int TestMode = 0)
        {
            //ListArrayPerf();
            int PassMinLengthLocal = PassMinLength;
            int PassMaxLengthLocal = PassMaxLength;
            int FillToLengthLocal = FillToLength;
            char[][] MaskArrayLocal = MaskArray;
            //char [] MyCharArray = new char[PassMaxLengthLocal];
            List<long>[][] ConvListArrayLocal = ConvListArray;
            List<string> PassTextListLocal = new(PassContToBeGen);
            List<string> TempRemovedLocal = new();
            //List<List<char>> PassFromDigListChar = new(PassToBeGen);
            int PosForDigsLocal = PosForDigs;
            long PosForDigsMultLocal = PosForDigsMult;
            char [] ZeroPassLocalN = new char [PassMaxLengthLocal];
            ZeroPassN.CopyTo(ZeroPassLocalN, 0);
            if (TempRemoved.Count > 0)
            {
                PassTextListLocal.AddRange(TempRemoved);
                TempRemoved.Clear();
            }

            int PassGeneratedBaseLocal = 0;
            long PassGeneratedFull = 0;

            int PassOnePercent = PassContToBeGen / 100;
            int CounterDisplayLocal = CounterDisplay;
            if (PassOnePercent == 0) { PassOnePercent++; }
            Stopwatch LoopStopwatchFull;

            List<long> DigPassCurrListLocal;// = DigPassCurrList;
            List<long> DigPassNextListLocal;// = new();
            List<long> WorkingIterLocal;// = WorkingIter;

            long[][] DigLenArrayLocal = DigLenArray;
            //Span<long[]> DigLenArrayLocalSpan = DigLenArray;

            List<long> IncrementList18Local = IncrementList18;
            //List<long> IncrementList9Local = IncrementList9;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            List<long> RealPossibleCountListLocal = RealPossibleCountList;

            List<long> Iter1 = [1];

            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> ProgressPoolMaxListLocal = ProgressPoolMaxList;
            int RealPossibleCountListLocalCount = RealPossibleCountListLocal.Count;
            int ProgressPoolMaxListLocalCount = ProgressPoolMaxListLocal.Count;
            int FirstPossibleDecListLocalCount = FirstPossibleDecListLocal.Count;
            long LastDisplayCount = 0;

            int CompareBigger = 10;

            //Stopwatch BaseGen = new();
            //check if pool limit in SessionProgress.txt is bigger then TotalBasePassCount list
            Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, RealPossibleCountListLocalCount, in ProgressPoolMaxListLocal, in ProgressPoolMaxListLocalCount, out int BiggerPool);
            //here generator will change second value in SessionProgress.txt file (ProgressPoolMaxList)
            if (BiggerPool == 2)
            {
                ProgressPoolMaxListLocal = RealPossibleCountListLocal;
                ProgressPoolMaxList = ProgressPoolMaxListLocal;
            }

            //at start generator we must use ProgressIterToDoList18 to generate WorkingIter and WorkingDigList == DigPassCurListLocal
            //in main loop we can do it easier: WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, IncrementList18Local);
            WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoList18, in IncrementList18Local);
            List<long> WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
            DigPassCurrListLocal = new(WorkingDigList);

            int FirstNonZero = 0;
            LoopStopwatchFull = Stopwatch.StartNew();
            //Stopwatch St1 = new Stopwatch();
            //Stopwatch St2 = new Stopwatch();
            //Stopwatch St3 = new Stopwatch();

            while (true)
            {
                int CharPosMax = PassMaxLengthLocal - 1;
                List<long> ToCompare = DigPassCurrListLocal;

                //List<char> MyCharList = new(PassMaxLengthLocal); //new password in char
                //List<string> PassFromDigList = new(PassMaxLengthLocal); //new password in string

                FirstNonZero = 0;
                char[] MyCharArray = new char [PassMaxLengthLocal];
                ZeroPassLocalN.CopyTo(MyCharArray, 0);
                int AddedChar = -1;
                int CharToAdd = -1;
                while (CharPosMax >= 0)
                {
                    //St1.Start();
                    //prepare XXXX...YYYY (ActDigToCompare) from ToCompare diglist
                    int ToCompareCount = ToCompare.Count - 1;
                    long ActDigToCompare;
                    if (ToCompareCount > 0)
                    {
                        Miki.CalcIntExt.LongLength(ToCompare[^1], out int ToCompareLen); //Last pos ToCompare length                                                   
                        Miki.CalcCompare.GetLongFromList(in ToCompare, in PosForDigsLocal, in ToCompareLen, out ActDigToCompare); //YYYY
                        ToCompareLen = 18 * (ToCompareCount) + ToCompareLen; //Full Length XXXX....
                        ActDigToCompare = (ToCompareLen * PosForDigsMultLocal) + ActDigToCompare + 1000000000000000000; //Make 4 places and add ActDigToCompare -> XXXX...YYYY
                    }
                    else
                    {
                        ActDigToCompare = ToCompare[0];
                    }

                    int StartCompare = 0;
                    int CharPosLength = MaskArrayLocal[CharPosMax].Length - 1;

                    long[] DigLenArrayLocalPart = DigLenArrayLocal[CharPosMax];

                    int low = 1;
                    int high = CharPosLength;
                    int mid = 0;

                    while (low <= high)
                    {
                        mid = (low + high) / 2;
                        if (DigLenArrayLocalPart[mid] < ActDigToCompare) //yes or no
                        {
                            if (mid == CharPosLength)
                            { StartCompare = mid; break; }
                            else
                            { mid++;}

                            if (DigLenArrayLocalPart[mid] < ActDigToCompare)//no
                            { low = mid+1; StartCompare = mid; }
                            else if (DigLenArrayLocalPart[mid] > ActDigToCompare)//yes
                            { StartCompare = mid-1; break; }
                            else //yes, but if...
                            {
                                if (ActDigToCompare > 999999999999999999)
                                {
                                    int s = ToCompareCount;
                                    while (s >= 0)
                                    {
                                        if (ToCompare[s] < ConvListArrayLocal[CharPosMax][mid][s])//ok
                                        { StartCompare = mid-1; break; }
                                        else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][mid][s])
                                        {
                                            low = mid+1; break;
                                        }
                                        else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                        { s--; }
                                    }
                                }
                                else
                                { StartCompare = mid; break; }
                            }
                        }
                        else if (DigLenArrayLocalPart[mid] > ActDigToCompare) //no at all
                        { high = mid - 1; }
                        else//yes, but if...
                        {
                            if (ActDigToCompare > 999999999999999999)
                            {
                                int s = ToCompareCount;
                                while (s >= 0)
                                {
                                    if (ToCompare[s] < ConvListArrayLocal[CharPosMax][mid][s])//ok
                                    { StartCompare = mid; break; }
                                    else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][mid][s])
                                    {
                                        low = mid + 1; break;
                                    }
                                    else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                    { s--; }
                                }
                            }
                            else { StartCompare = mid; break; }
                        }
                    }


                    CharToAdd++;
                    if (StartCompare > 0 || FirstNonZero == 1 || CharPosMax == 0)
                    {
                        ToCompare = Miki.CalcLists.Sub(in ToCompare, in ConvListArrayLocal[CharPosMax][StartCompare]);
                        AddedChar++;
                        MyCharArray[CharToAdd] = MaskArrayLocal[CharPosMax][StartCompare];
                        FirstNonZero = 1;
                    }
                    CharPosMax--;
                } //end loop for char position in password (from up)
                PassGeneratedBaseLocal++;

                //here we have a password which is converted from decimal iteration, eg. XXXXX
                //we can fill now rest of characters, but characters must represent zeros -> YYYYYXXXXX

                if (FillToLengthLocal == 1)
                {
                    if (PassMaxLengthLocal == PassMinLengthLocal)//add full MyCharArray
                    {
                        PassTextListLocal.Add(new string(MyCharArray));
                        PassGeneratedFull++;
                    } 
                    else
                    {
                        while (AddedChar < PassMaxLengthLocal) //add from min length fo Full MyCharArray
                        {
                            PassTextListLocal.Add(new string(MyCharArray[(PassMaxLengthLocal - AddedChar - 1)..]));
                            AddedChar++;
                            PassGeneratedFull++;
                        }
                    }
                }
                else
                {
                    //if we don't want to fill
                    PassTextListLocal.Add(new string(MyCharArray[(PassMaxLengthLocal - AddedChar - 1)..])); //ok
                    PassGeneratedFull++;
                }

                //NEW NUMBER (seed), classic from seed because from ProgressIterToDoLocal is slower, ProgressIterToDoLocal can be calculated at the end
                WorkingIterLocal = Miki.CalcLists.Add(in DigPassCurrListLocal, in IncrementList18Local);
                DigPassNextListLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                //WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                //DigPassNextListLocal = (WorkingDigList);
                //NEW NUMBER DONE...

                //int TotalEndCheck = 1;
                //Check if Total End. If Increment is good, then return to seed=0 (ThisDigPassNextList) means that all iteration are done....

                Miki.CalcCompare.ListBigger(in DigPassNextListLocal, DigPassNextListLocal.Count, in FirstPossibleDecListLocal, in FirstPossibleDecListLocalCount, out int TotalEndCheck);
                if (TotalEndCheck == 0)
                {
                    GeneratorExitReasonTotal = "Total";
                }

                long RealPassGeneratedFull = PassGeneratedFull;

                if (CounterDisplayLocal == 1)
                {
                    LoopStopwatchFull.Stop();
                    if (LastDisplayCount != PassGeneratedBaseLocal && PassGeneratedBaseLocal % PassOnePercent == 0)
                    {
                        LastDisplayCount = PassGeneratedBaseLocal;
                        int PassPercent = (int)(PassGeneratedBaseLocal * 100 / PassContToBeGen);
                        long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                        long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                        long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                        long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                        long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                        long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                        long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                        string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  in  {3}:{4}:{5}:{6} SpeedB:{7} SpeedF:{8} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, PassPercent, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                    LoopStopwatchFull.Start();
                }

                if (PassGeneratedFull >= PassContToBeGen || TotalEndCheck == 0)
                {
                    LoopStopwatchFull.Stop();
                    //here we can add PassGeneratedBaseLocal and check if Pool ends
                    List<long> PassGeneratedBaseLocalList = new List<long>() { PassGeneratedBaseLocal };
                    ProgressIterToDoListLocal = Miki.CalcLists.Add(in ProgressIterToDoListLocal, in PassGeneratedBaseLocalList);
                    //Check if Pool Ends, if ToDo biger PoolMax
                    Miki.CalcCompare.ListBigger(in ProgressPoolMaxListLocal, ProgressPoolMaxListLocalCount, in ProgressIterToDoListLocal, ProgressIterToDoListLocal.Count, out int PoolEndCheck);
                    if (PoolEndCheck == 2)
                    {
                        GeneratorExitReasonPool = "Pool";
                    }

                    if (PassGeneratedFull > PassContToBeGen && PoolEndCheck != 2 && TotalEndCheck != 0 && TestMode == 1) //to maintain the same length of each wordlist, only if not GeneratorExitReason is Pool or Total
                    {
                        int ToRemoveCount = PassTextListLocal.Count - PassContToBeGen;
                        TempRemoved.AddRange(PassTextListLocal[PassContToBeGen..]); //TempRemoved will be added in next PassListFromDigByTab run
                        PassTextListLocal.RemoveRange(PassContToBeGen, ToRemoveCount); //Remove TempRemoved from PassTextListLocal
                        PassGeneratedFull = PassContToBeGen; //it can  lower a little the speed, but it must be....
                    }


                    ProgressIterToDoList18 = ProgressIterToDoListLocal;
                    Miki.CalcConvert.LongListToString(in ProgressIterToDoListLocal, "000000000000000000", out ProgressIterToDoString);
                    //here ToDo is bigger than Max and we will not use ToDo because we break, so we must subtract 1
                    ProgressIterDoneList18 = Miki.CalcLists.Sub(ProgressIterToDoListLocal, Iter1);
                    Miki.CalcConvert.LongListToString(in ProgressIterDoneList18, "000000000000000000", out ProgressIterDoneString);

                    DigPassCurrList = DigPassCurrListLocal;
                    long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                    long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                    long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                    long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                    long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                    long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                    long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                    string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  {3} - {4} in  {5}:{6}:{7}:{8} SpeedB:{9} SpeedF:{10} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, 100, PassTextListLocal[0], PassTextListLocal[^1], TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                    Console.Write("\r{0}   ", PassGeneratedString);
                    PassGeneratedBase = PassGeneratedBaseLocal;
                    //Console.WriteLine();
                    //Console.WriteLine(St1.Elapsed.TotalMilliseconds);
                    //Console.WriteLine(St2.Elapsed.TotalMilliseconds);
                    //Console.WriteLine(St1.Elapsed.TotalMilliseconds - St2.Elapsed.TotalMilliseconds);
                    //Console.WriteLine(St3.Elapsed.TotalMilliseconds);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    //List<string> PPP = PassCharToString(PassFromDigListChar);
                    return PassTextListLocal;
                    //return PassCharToString(PassFromDigListChar);
                }
                DigPassCurrListLocal = DigPassNextListLocal;

            }
        }

        public static List<string> PassListFromDigByTabNGood(int PassToBeGen, int TestMode = 0)
        {
            //ListArrayPerf();
            int PassMinLengthLocal = PassMinLength;
            int PassMaxLengthLocal = PassMaxLength;
            int FillToLengthLocal = FillToLength;
            char[][] MaskArrayLocal = MaskArray;
            //char [] MyCharArray = new char[PassMaxLengthLocal];
            List<long>[][] ConvListArrayLocal = ConvListArray;
            List<string> PassTextListLocal = new(PassToBeGen);
            List<string> TempRemovedLocal = new();
            //List<List<char>> PassFromDigListChar = new(PassToBeGen);
            int PosForDigsLocal = PosForDigs;
            long PosForDigsMultLocal = PosForDigsMult;

            if (TempRemoved.Count > 0)
            {
                PassTextListLocal.AddRange(TempRemoved);
                TempRemoved.Clear();
            }

            int PassGeneratedBaseLocal = 0;
            long PassGeneratedFull = 0;

            int PassOnePercent = PassToBeGen / 100;
            int CounterDisplayLocal = CounterDisplay;
            if (PassOnePercent == 0) { PassOnePercent++; }
            Stopwatch LoopStopwatchFull;

            List<long> DigPassCurrListLocal;// = DigPassCurrList;
            List<long> DigPassNextListLocal;// = new();
            List<long> WorkingIterLocal;// = WorkingIter;

            long[][] DigLenArrayLocal = DigLenArray;
            //Span<long[]> DigLenArrayLocalSpan = DigLenArray;

            List<long> IncrementList18Local = IncrementList18;
            //List<long> IncrementList9Local = IncrementList9;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            List<long> RealPossibleCountListLocal = RealPossibleCountList;

            List<long> Iter1 = [1];

            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> ProgressPoolMaxListLocal = ProgressPoolMaxList;
            int RealPossibleCountListLocalCount = RealPossibleCountListLocal.Count;
            int ProgressPoolMaxListLocalCount = ProgressPoolMaxListLocal.Count;
            int FirstPossibleDecListLocalCount = FirstPossibleDecListLocal.Count;
            long LastDisplayCount = 0;

            int CompareBigger = 10;

            //Stopwatch BaseGen = new();
            //check if pool limit in SessionProgress.txt is bigger then TotalBasePassCount list
            Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, RealPossibleCountListLocalCount, in ProgressPoolMaxListLocal, in ProgressPoolMaxListLocalCount, out int BiggerPool);
            //here generator will change second value in SessionProgress.txt file (ProgressPoolMaxList)
            if (BiggerPool == 2)
            {
                ProgressPoolMaxListLocal = RealPossibleCountListLocal;
                ProgressPoolMaxList = ProgressPoolMaxListLocal;
            }

            //at start generator we must use ProgressIterToDoList18 to generate WorkingIter and WorkingDigList == DigPassCurListLocal
            //in main loop we can do it easier: WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, IncrementList18Local);
            WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoList18, in IncrementList18Local);
            List<long> WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
            DigPassCurrListLocal = new(WorkingDigList);

            int FirstNonZero = 0;
            LoopStopwatchFull = Stopwatch.StartNew();
            Stopwatch St1 = new Stopwatch();
            Stopwatch St2 = new Stopwatch();
            //Stopwatch St3 = new Stopwatch();

            while (true)
            {
                int CharPosMax = PassMaxLengthLocal - 1;
                List<long> ToCompare = DigPassCurrListLocal;

                List<char> MyCharList = new(PassMaxLengthLocal); //new password in char
                List<string> PassFromDigList = new(PassMaxLengthLocal); //new password in string

                FirstNonZero = 0;

                while (CharPosMax >= 0)
                {
                    //St1.Start();
                    //prepare XXXX...YYYY (ActDigToCompare) from ToCompare diglist

                    int ToCompareCount = ToCompare.Count - 1;
                    long ActDigToCompare;
                    if (ToCompareCount > 0)
                    {
                        Miki.CalcIntExt.LongLength(ToCompare[^1], out int ToCompareLen); //Last pos ToCompare length                                                   
                        Miki.CalcCompare.GetLongFromList(in ToCompare, in PosForDigsLocal, in ToCompareLen, out ActDigToCompare); //YYYY
                        ToCompareLen = 18 * (ToCompareCount) + ToCompareLen; //Full Length XXXX....
                        ActDigToCompare = (ToCompareLen * PosForDigsMultLocal) + ActDigToCompare + 1000000000000000000; //Make 4 places and add ActDigToCompare -> XXXX...YYYY
                    }
                    else
                    {
                        ActDigToCompare = ToCompare[0];
                    }
                    St1.Start();
                    int StartCompare = 0;
                    int CharPosLength = MaskArrayLocal[CharPosMax].Length - 1;

                    long[] DigLenArrayLocalPart = DigLenArrayLocal[CharPosMax];

                    int mid = (1 + (CharPosLength - 1)) >> 1;


                    long ActDigToCompareT = DigLenArrayLocalPart[mid];
                    if (ActDigToCompareT > ActDigToCompare) //zawęź do połowy i szukaj od mid w dół
                    {
                        mid--;
                        for (int a = mid; a >= 1; a--)
                        {
                            ActDigToCompareT = DigLenArrayLocalPart[a];
                            if (ActDigToCompareT <= ActDigToCompare)
                            {
                                StartCompare = a;
                                if (ActDigToCompareT == ActDigToCompare && ActDigToCompare > 999999999999999999)
                                {
                                    //Equal++;
                                    int s = ToCompareCount;

                                    while (s >= 0)
                                    {
                                        if (ToCompare[s] < ConvListArrayLocal[CharPosMax][StartCompare][s])
                                        { break; }
                                        else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][StartCompare][s])
                                        {
                                            StartCompare--;
                                            if (ToCompareCount + 1 > ConvListArrayLocal[CharPosMax][StartCompare].Count)
                                            { break; }
                                            else
                                            {
                                                s = ToCompareCount;
                                                if (StartCompare == 0)
                                                { break; }
                                            }
                                        }
                                        else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                        { s--; }
                                    }

                                }
                                break;
                            }
                        }
                    }
                    else if (ActDigToCompareT < ActDigToCompare) //zawęź do połowy i szukaj od CharPosLength w dół
                    {
                        //mid++;
                        for (int a = CharPosLength; a >= mid; a--)
                        {
                            ActDigToCompareT = DigLenArrayLocalPart[a];
                            if (ActDigToCompareT <= ActDigToCompare)
                            {
                                StartCompare = a;
                                if (ActDigToCompareT == ActDigToCompare && ActDigToCompare > 999999999999999999)
                                {
                                    //Equal++;
                                    int s = ToCompareCount;
                                    while (s >= 0)
                                    {
                                        if (ToCompare[s] < ConvListArrayLocal[CharPosMax][StartCompare][s])
                                        { break; }
                                        else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][StartCompare][s])
                                        {
                                            StartCompare--;
                                            if (ToCompareCount + 1 > ConvListArrayLocal[CharPosMax][StartCompare].Count)
                                            { break; }
                                            else
                                            {
                                                s = ToCompareCount;
                                                if (StartCompare == 0)
                                                { break; }
                                            }
                                        }
                                        else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                        { s--; }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        StartCompare = mid;
                        //Equal++;
                        if (ActDigToCompare > 999999999999999999)
                        {
                            int s = ToCompareCount;
                            while (s >= 0)
                            {
                                if (ToCompare[s] < ConvListArrayLocal[CharPosMax][StartCompare][s])
                                { break; }
                                else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][StartCompare][s])
                                {
                                    StartCompare--;
                                    if (ToCompareCount + 1 > ConvListArrayLocal[CharPosMax][StartCompare].Count)
                                    { break; }
                                    else
                                    {
                                        s = ToCompareCount;
                                        if (StartCompare == 0)
                                        { break; }
                                    }
                                }
                                else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                { s--; }
                            }
                        }
                    }

                    St1.Stop();
                    //if (MyEnd != StartCompare)
                    //{ Console.WriteLine("MyEnd != StartCompare"); }
                    St2.Start();
                    int low = 0;
                    int high = CharPosLength - 1;
                    int mid2 = 0;
                    int test = 0;

                    while (low <= high)
                    {
                        mid2 = (low + high) >> 1;
                        if (DigLenArrayLocalPart[mid2] < ActDigToCompare) //yes or no
                        {
                            if (DigLenArrayLocalPart[mid2 + 1] < ActDigToCompare)//no
                            { low = mid2 + 1; test = low; }
                            else if (DigLenArrayLocalPart[mid2 + 1] > ActDigToCompare)//yes
                            { test = mid2; break; }
                            else //yes, but if...
                            {
                                if (ActDigToCompare > 999999999999999999)
                                {
                                    int s = ToCompareCount;
                                    while (s >= 0)
                                    {
                                        if (ToCompare[s] < ConvListArrayLocal[CharPosMax][mid2 + 1][s])//ok
                                        { test = mid2; break; }
                                        else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][mid2 + 1][s])
                                        {
                                            low = mid2 + 1; break;
                                        }
                                        else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                        { s--; }
                                    }
                                }
                                else
                                { test = mid2 + 1; break; }

                            }
                        }
                        else if (DigLenArrayLocalPart[mid2] > ActDigToCompare) //no at all
                        { high = mid2 - 1; }
                        else//yes, but if...
                        {
                            if (ActDigToCompare > 999999999999999999)
                            {
                                int s = ToCompareCount;
                                while (s >= 0)
                                {
                                    if (ToCompare[s] < ConvListArrayLocal[CharPosMax][mid2][s])//ok
                                    { test = mid2; break; }
                                    else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][mid2][s])
                                    {
                                        low = mid2 + 1; break;
                                    }
                                    else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                    { s--; }
                                }
                            }
                            else { test = mid2; break; }
                        }
                    }
                    St2.Stop();
                    _ = mid2;

                    if (test != StartCompare)
                    {

                    }

                    if (StartCompare > 0 || FirstNonZero == 1 || CharPosMax == 0)
                    {
                        //St1.Start();
                        ToCompare = Miki.CalcLists.Sub(in ToCompare, in ConvListArrayLocal[CharPosMax][StartCompare]);
                        //St2.Stop();
                        MyCharList.Add(MaskArrayLocal[CharPosMax][StartCompare]);
                        FirstNonZero = 1;
                        //St1.Stop();
                    }

                    //BaseGen.Stop();
                    //PassGen.Stop();
                    CharPosMax--;
                } //end loop for char position in password (from up)
                PassGeneratedBaseLocal++;
                //St2.Stop();
                //here we have a password which is converted from decimal iteration, eg. XXXXX
                //we can fill now rest of characters, but characters must represent zeros -> YYYYYXXXXX
                //St3.Start();
                if (FillToLengthLocal == 1)
                {
                    int MyCharListCount = MyCharList.Count;

                    if (MyCharListCount == PassMinLengthLocal) //Fixed or not, but first pass is ok
                    {
                        if (PassMaxLengthLocal == PassMinLengthLocal) //Fixed, first pass length ok
                        {
                            //we can always add first pass
                            PassFromDigList.Add(new string(MyCharList.ToArrayPar()));
                            //PassFromDigListChar.Add(MyCharList);
                            //MyCharList.CopyTo(MyCharArray);
                            //PassFromDigList.Add(new string(MyCharArray));
                            PassGeneratedFull++;
                        }
                        else //Not fixed, first pass length is ok
                        {
                            //we can always add first pass
                            PassFromDigList.Add(new string(MyCharList.ToArrayPar()));
                            //PassFromDigListChar.Add(MyCharList);
                            PassGeneratedFull++;
                            //fill next characters
                            while (MyCharListCount < PassMaxLengthLocal)//add to max length, and add to PassFromDigList
                            {
                                MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                PassFromDigList.Add(new string(MyCharList.ToArrayPar()));
                                //PassFromDigListChar.Add(MyCharList);
                                PassGeneratedFull++;
                                MyCharListCount++;
                            }
                        }
                    }
                    else //Fixed or not, but first pass is to short
                    {
                        if (PassMaxLengthLocal == PassMinLengthLocal) //Fixed length
                        {
                            //we can add next character if we check it is zero
                            while (MyCharListCount < PassMaxLengthLocal)
                            {
                                MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                MyCharListCount++;
                            }

                            //finally add to passlist
                            PassFromDigList.Add(new string(MyCharList.ToArrayPar()));
                            //PassFromDigListChar.Add(MyCharList);
                            //PassFromDigList.Add(new string(MyCharArray));
                            PassGeneratedFull++;

                        }
                        else //Not fixed
                        {
                            //fill to min length and add
                            while (MyCharListCount < PassMinLengthLocal)//add to min length
                            {
                                MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                MyCharListCount++;
                            }
                            PassFromDigList.Add(new string(MyCharList.ToArrayPar()));
                            //PassFromDigListChar.Add(MyCharList);
                            PassGeneratedFull++;

                            //fill from min to max, then add
                            while (MyCharListCount < PassMaxLengthLocal)
                            {
                                MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                PassFromDigList.Add(new string(MyCharList.ToArrayPar()));
                                //PassFromDigListChar.Add(MyCharList);
                                PassGeneratedFull++;
                                MyCharListCount++;
                            }

                        }
                    }
                }
                else
                {
                    //if we don't want to fill
                    PassFromDigList.Add(new string(MyCharList.ToArrayPar()));
                    //PassFromDigListChar.Add(MyCharList);
                    PassGeneratedFull++;
                }
                //St3.Stop();
                //All pass count in PassFromDigList, we can add now

                PassTextListLocal.AddRange(PassFromDigList);

                //NEW NUMBER (seed), classic from seed because from ProgressIterToDoLocal is slower, ProgressIterToDoLocal can be calculated at the end
                WorkingIterLocal = Miki.CalcLists.Add(in DigPassCurrListLocal, in IncrementList18Local);
                DigPassNextListLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                //WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                //DigPassNextListLocal = (WorkingDigList);
                //NEW NUMBER DONE...

                //int TotalEndCheck = 1;
                //Check if Total End. If Increment is good, then return to seed=0 (ThisDigPassNextList) means that all iteration are done....

                Miki.CalcCompare.ListBigger(in DigPassNextListLocal, DigPassNextListLocal.Count, in FirstPossibleDecListLocal, in FirstPossibleDecListLocalCount, out int TotalEndCheck);
                if (TotalEndCheck == 0)
                {
                    GeneratorExitReasonTotal = "Total";
                }

                long RealPassGeneratedFull = PassGeneratedFull;

                if (CounterDisplayLocal == 1)
                {
                    LoopStopwatchFull.Stop();
                    if (LastDisplayCount != PassGeneratedBaseLocal && PassGeneratedBaseLocal % PassOnePercent == 0)
                    {
                        LastDisplayCount = PassGeneratedBaseLocal;
                        int PassPercent = (int)(PassGeneratedBaseLocal * 100 / PassToBeGen);
                        long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                        long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                        long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                        long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                        long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                        long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                        long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                        string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  in  {3}:{4}:{5}:{6} SpeedB:{7} SpeedF:{8} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, PassPercent, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                    LoopStopwatchFull.Start();
                }

                if (PassGeneratedFull >= PassToBeGen || TotalEndCheck == 0)
                {
                    LoopStopwatchFull.Stop();
                    //here we can add PassGeneratedBaseLocal and check if Pool ends
                    List<long> PassGeneratedBaseLocalList = new List<long>() { PassGeneratedBaseLocal };
                    ProgressIterToDoListLocal = Miki.CalcLists.Add(in ProgressIterToDoListLocal, in PassGeneratedBaseLocalList);
                    //Check if Pool Ends, if ToDo biger PoolMax
                    Miki.CalcCompare.ListBigger(in ProgressPoolMaxListLocal, ProgressPoolMaxListLocalCount, in ProgressIterToDoListLocal, ProgressIterToDoListLocal.Count, out int PoolEndCheck);
                    if (PoolEndCheck == 2)
                    {
                        GeneratorExitReasonPool = "Pool";
                    }

                    if (PassGeneratedFull > PassToBeGen && PoolEndCheck != 2 && TotalEndCheck != 0 && TestMode == 1) //to maintain the same length of each wordlist, only if not GeneratorExitReason is Pool or Total
                    {
                        int ToRemoveCount = PassTextListLocal.Count - PassToBeGen;
                        TempRemoved.AddRange(PassTextListLocal[PassToBeGen..]); //TempRemoved will be added in next PassListFromDigByTab run
                        PassTextListLocal.RemoveRange(PassToBeGen, ToRemoveCount); //Remove TempRemoved from PassTextListLocal
                        PassGeneratedFull = PassToBeGen; //it can  lower a little the speed, but it must be....
                    }


                    ProgressIterToDoList18 = ProgressIterToDoListLocal;
                    Miki.CalcConvert.LongListToString(in ProgressIterToDoListLocal, "000000000000000000", out ProgressIterToDoString);
                    //here ToDo is bigger than Max and we will not use ToDo because we break, so we must subtract 1
                    ProgressIterDoneList18 = Miki.CalcLists.Sub(ProgressIterToDoListLocal, Iter1);
                    Miki.CalcConvert.LongListToString(in ProgressIterDoneList18, "000000000000000000", out ProgressIterDoneString);

                    DigPassCurrList = DigPassCurrListLocal;
                    long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                    long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                    long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                    long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                    long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                    long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                    long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                    string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  {3} - {4} in  {5}:{6}:{7}:{8} SpeedB:{9} SpeedF:{10} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, 100, PassTextListLocal[0], PassTextListLocal[^1], TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                    Console.Write("\r{0}   ", PassGeneratedString);
                    PassGeneratedBase = PassGeneratedBaseLocal;
                    Console.WriteLine();
                    Console.WriteLine(St1.Elapsed.TotalMilliseconds);
                    Console.WriteLine(St2.Elapsed.TotalMilliseconds);
                    //Console.WriteLine(St1.Elapsed.TotalMilliseconds - St2.Elapsed.TotalMilliseconds);
                    //Console.WriteLine(St3.Elapsed.TotalMilliseconds);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    //List<string> PPP = PassCharToString(PassFromDigListChar);
                    return PassTextListLocal;
                    //return PassCharToString(PassFromDigListChar);
                }
                DigPassCurrListLocal = DigPassNextListLocal;

            }
        }
        public static List<string> PassListFromDigByTabU(int PassCountToBeGen, int TestMode = 0)
        {
            //ListArrayPerf();
            int PassMinLengthLocal = PassMinLength;
            int PassMaxLengthLocal = PassMaxLength;
            int FillToLengthLocal = FillToLength;
            char[][] MaskArrayLocal = MaskArray;
            List<long>[][] ConvListArrayLocal = ConvListArray;
            List<string> PassTextListLocal = new(PassCountToBeGen);
            List<string> TempRemovedLocal = new();

            int PosForDigsLocal = PosForDigs;
            long PosForDigsMultLocal = PosForDigsMult;

            char[] ZeroPassLocalU = new char[PassMaxLengthLocal];
            ZeroPassU.CopyTo(ZeroPassLocalU, 0);

            if (TempRemoved.Count > 0)
            {
                PassTextListLocal.AddRange(TempRemoved);
                TempRemoved.Clear();
            }

            int PassGeneratedBaseLocal = 0;
            long PassGeneratedFull = 0;

            int PassOnePercent = PassCountToBeGen / 100;
            int CounterDisplayLocal = CounterDisplay;
            if (PassOnePercent == 0) { PassOnePercent++; }
            Stopwatch LoopStopwatchFull;

            List<long> DigPassCurrListLocal = DigPassCurrList;
            List<long> DigPassNextListLocal = new();
            List<long> WorkingIterLocal = WorkingIter;

            long[][] DigLenArrayLocal = DigLenArray;
            List<long>[] AllUnarysValueListArrayLocal = AllUnarysValueListArray;

            int IsUnaryLocal = IsUnary;

            List<long> IncrementList18Local = IncrementList18;
            //List<long> IncrementList9Local = IncrementList9;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            //List<long> TotalPossibleCountListLocal = TotalPossibleCountList;
            //Miki.CalcConvert.StringToLongList(TotalPossibleCountString, 18, out List<long> TotalPossibleCountListLocal);
            List<long> RealPossibleCountListLocal = RealPossibleCountList;

            List<long> Iter1 = new() { 1 };

            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> ProgressPoolMaxListLocal = ProgressPoolMaxList;
            int ProgressPoolMaxListLocalCount = ProgressPoolMaxListLocal.Count;
            int RealPossibleCountListLocalCount = RealPossibleCountListLocal.Count;
            int FirstPossibleDecListLocalCount = FirstPossibleDecListLocal.Count;
            long LastDisplayCount = 0;

            //int CompareBigger = 10;


            //check if pool limit in SessionProgress.txt is bigger then TotalBasePassCount list
            Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, in RealPossibleCountListLocalCount, in ProgressPoolMaxListLocal, in ProgressPoolMaxListLocalCount, out int BiggerPool);
            //here generator will change second value in SessionProgress.txt file (ProgressPoolMaxList)
            if (BiggerPool == 2)
            {
                ProgressPoolMaxListLocal = RealPossibleCountListLocal;
                ProgressPoolMaxList = ProgressPoolMaxListLocal;
            }

            //at start generator we must use ProgressIterToDoList18 to generate WorkingIter and WorkingDigList == DigPassCurListLocal
            //in main loop we can do it easier: WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, IncrementList18Local);
            WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoList18, in IncrementList18Local);
            List<long> WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
            DigPassCurrListLocal = new(WorkingDigList);

            int FirstNonZero = 0;
            LoopStopwatchFull = Stopwatch.StartNew();

            while (true)
            {
                int CharPosMax = PassMaxLengthLocal - 1;
                List<long> ToCompare = DigPassCurrListLocal;

                int IsUnarySubtracted = 0;

                List<long> UnaryToCompare = AllUnarysValueListArrayLocal[CharPosMax];
                Miki.CalcCompare.ListBigger(in ToCompare, ToCompare.Count, in UnaryToCompare, UnaryToCompare.Count, out int IsBigger);
                if (IsBigger != 2) // if AllUnaryValueListLocal[CharPos] is equal or bigger than ToCompare
                {
                    ToCompare = Miki.CalcLists.Sub(in ToCompare, in UnaryToCompare); //we subtract UnaryToCompare
                    IsUnarySubtracted = 1;
                }

                List<char> MyCharList = new(PassMaxLengthLocal); //new password in char
                char[] MyCharArray = new char[PassMaxLengthLocal];
                ZeroPassLocalU.CopyTo(MyCharArray, 0);
                int AddedChar = -1;
                int CharToAdd = -1;


                List<string> PassFromDigList = new(PassMaxLengthLocal); //new password in string

                FirstNonZero = 0;

                while (CharPosMax >= 0)
                {
                    CharToAdd++;
                    //CharToAdd = PassMaxLengthLocal - CharPosMax-1;
                    if (IsUnarySubtracted == 1 && MaskArrayLocal[CharPosMax].Length == 1)
                    {
                        //MyCharList.Add(MaskArrayLocal[CharPosMax][0]); //here unary value is possible
                        //MyCharArray[CharToAdd] = MaskArrayLocal[CharPosMax][0];
                        //Console.WriteLine(CharToAdd + "   " + CharPosMax);
                        FirstNonZero = 1;
                        AddedChar++;
                    }
                    else
                    {
                        //St1.Start();
                        //prepare XXXX...YYYY (ActDigToCompare) from ToCompare diglist
                        int ToCompareCount = ToCompare.Count - 1;
                        long ActDigToCompare;
                        if (ToCompareCount > 0)
                        {
                            Miki.CalcIntExt.LongLength(ToCompare[^1], out int ToCompareLen); //Last pos ToCompare length                                                   
                            Miki.CalcCompare.GetLongFromList(in ToCompare, in PosForDigsLocal, in ToCompareLen, out ActDigToCompare); //YYYY
                            ToCompareLen = 18 * (ToCompareCount) + ToCompareLen; //Full Length XXXX....
                            ActDigToCompare = (ToCompareLen * PosForDigsMultLocal) + ActDigToCompare + 1000000000000000000; //Make 4 places and add ActDigToCompare -> XXXX...YYYY
                        }
                        else
                        {
                            ActDigToCompare = ToCompare[0];
                        }

                        int StartCompare = 0;
                        int CharPosLength = MaskArrayLocal[CharPosMax].Length - 1;

                        long[] DigLenArrayLocalPart = DigLenArrayLocal[CharPosMax];

                        int low = 1;
                        int high = CharPosLength;
                        int mid = 0;

                        while (low <= high)
                        {
                            mid = (low + high) / 2;
                            if (DigLenArrayLocalPart[mid] < ActDigToCompare) //yes or no
                            {
                                if (mid == CharPosLength)
                                { StartCompare = mid; break; }
                                else
                                { mid++; }

                                if (DigLenArrayLocalPart[mid] < ActDigToCompare)//no
                                { low = mid + 1; StartCompare = mid; }
                                else if (DigLenArrayLocalPart[mid] > ActDigToCompare)//yes
                                { StartCompare = mid - 1; break; }
                                else //yes, but if...
                                {
                                    if (ActDigToCompare > 999999999999999999)
                                    {
                                        int s = ToCompareCount;
                                        while (s >= 0)
                                        {
                                            if (ToCompare[s] < ConvListArrayLocal[CharPosMax][mid][s])//ok
                                            { StartCompare = mid - 1; break; }
                                            else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][mid][s])
                                            {
                                                low = mid + 1; break;
                                            }
                                            else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                            { s--; }
                                        }
                                    }
                                    else
                                    { StartCompare = mid; break; }
                                }
                            }
                            else if (DigLenArrayLocalPart[mid] > ActDigToCompare) //no at all
                            { high = mid - 1; }
                            else//yes, but if...
                            {
                                if (ActDigToCompare > 999999999999999999)
                                {
                                    int s = ToCompareCount;
                                    while (s >= 0)
                                    {
                                        if (ToCompare[s] < ConvListArrayLocal[CharPosMax][mid][s])//ok
                                        { StartCompare = mid; break; }
                                        else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][mid][s])
                                        {
                                            low = mid + 1; break;
                                        }
                                        else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                        { s--; }
                                    }
                                }
                                else { StartCompare = mid; break; }
                            }
                        }

                        if (StartCompare > 0 || FirstNonZero == 1 || CharPosMax == 0)
                        {
                            ToCompare = Miki.CalcLists.Sub(in ToCompare, in ConvListArrayLocal[CharPosMax][StartCompare]);
                            AddedChar++;
                            MyCharArray[CharToAdd] = MaskArrayLocal[CharPosMax][StartCompare]; //CharPosMax odwraca, nie stosować
                            FirstNonZero = 1;
                            //Console.WriteLine(CharToAdd + "   " + CharPosMax);
                        }
                    }
                    CharPosMax--;
                } //end loop for char position in password (from up)
                //string abc = new string(MyCharArray);
                //here we have a password which is converted from decimal iteration, eg. XXXXX
                //we can fill now rest of characters, but characters must represent zeros -> YYYYYXXXXX
                PassGeneratedBaseLocal++;
                //if we have unary, characters above AddedChar position can be zero or not. But we know, that all from AddedChar down is good in our password, so...
                //if we take rest of CharArray we must watch to take only true zeros (MaskArrayLocal(CharPosMax).Length > 0.
                if (FillToLengthLocal == 1)
                {
                    if (PassMaxLengthLocal == PassMinLengthLocal)//we can take full MyCharArray only if AddedChar+1 = PassMaxLengthLocal
                    {
                        if (AddedChar + 1 != PassMaxLengthLocal) //here is easy, but can be a lot of discarded passwords
                        { }
                            PassTextListLocal.Add(new string(MyCharArray));
                            PassGeneratedFull++;
                        
                    }
                    else
                    {
                        while (AddedChar < PassMaxLengthLocal) //add from min length fo Full MyCharArray
                        {
                            PassTextListLocal.Add(new string(MyCharArray[(PassMaxLengthLocal - AddedChar - 1)..]));
                            AddedChar++;
                            PassGeneratedFull++;
                        }
                    }
                }
                else
                {
                    //if we don't want to fill
                    PassTextListLocal.Add(new string(MyCharArray[(PassMaxLengthLocal - AddedChar - 1)..])); //always ok, because we take only added chars
                    PassGeneratedFull++;
                }

                //All pass count in PassFromDigList, we can add now

                //PassTextListLocal.AddRange(PassFromDigList);

                //NEW NUMBER (seed)
                //WorkingIterLocal = Miki.CalcLists.Add(in DigPassCurrListLocal, in IncrementList18Local);
                //DigPassNextListLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];

                //ProgressIterToDoListLocal = Miki.CalcLists.Add(ProgressIterToDoListLocal, Iter1);
                //WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoListLocal, 18, in IncrementList9Local, 9, 18);

                WorkingIterLocal = Miki.CalcLists.Add(in WorkingIterLocal, in IncrementList18Local);
                Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, in RealPossibleCountListLocalCount, in WorkingIterLocal, WorkingIterLocal.Count, out int WorkingBigger);
                if (WorkingBigger < 2)
                {
                    WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                }
                else
                {
                    //Console.WriteLine("MOD");
                    WorkingIterLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                }

                DigPassNextListLocal = new(WorkingDigList);
                //NEW NUMBER DONE...


                //Check if Total End. If Increment is good, then return to seed=0 (ThisDigPassNextList) means that all iteration are done....
                Miki.CalcCompare.ListBigger(in DigPassNextListLocal, DigPassNextListLocal.Count, in FirstPossibleDecListLocal, in FirstPossibleDecListLocalCount, out int TotalEndCheck);
                if (TotalEndCheck == 0)
                {
                    //Console.WriteLine("Total");
                    GeneratorExitReasonTotal = "Total";
                }

                long RealPassGeneratedFull = PassGeneratedFull;
                if (CounterDisplayLocal == 1)
                {
                    LoopStopwatchFull.Stop();
                    if (LastDisplayCount != PassGeneratedBaseLocal && PassGeneratedBaseLocal % PassOnePercent == 0)
                    {
                        LastDisplayCount = PassGeneratedBaseLocal;
                        int PassPercent = (int)(PassGeneratedBaseLocal * 100 / PassCountToBeGen);
                        long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                        long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                        long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                        long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                        long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                        long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                        long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                        string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  in  {3}:{4}:{5}:{6} SpeedB:{7} SpeedF:{8} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, PassPercent, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                    LoopStopwatchFull.Start();
                }

                if (PassGeneratedFull >= PassCountToBeGen || TotalEndCheck == 0)
                {
                    LoopStopwatchFull.Stop();
                    //here we can add PassGeneratedBaseLocal and check if Pool ends
                    List<long> PassGeneratedBaseLocalList = new List<long>() { PassGeneratedBaseLocal };
                    ProgressIterToDoListLocal = Miki.CalcLists.Add(in ProgressIterToDoListLocal, in PassGeneratedBaseLocalList);
                    //Check if Pool Ends, if ToDo biger PoolMax
                    Miki.CalcCompare.ListBigger(in ProgressPoolMaxListLocal, ProgressPoolMaxListLocalCount, in ProgressIterToDoListLocal, ProgressIterToDoListLocal.Count, out int PoolEndCheck);
                    if (PoolEndCheck == 2)
                    {
                        GeneratorExitReasonPool = "Pool";
                    }

                    if (PassGeneratedFull > PassCountToBeGen && PoolEndCheck != 2 && TotalEndCheck != 0 && TestMode == 1) //to maintain the same length of each wordlist, only if not GeneratorExitReason is Pool or Total
                    {
                        int ToRemoveCount = PassTextListLocal.Count - PassCountToBeGen;
                        TempRemoved.AddRange(PassTextListLocal[PassCountToBeGen..]); //TempRemoved will be added in next PassListFromDigByTab run
                        PassTextListLocal.RemoveRange(PassCountToBeGen, ToRemoveCount); //Remove TempRemoved from PassTextListLocal
                        PassGeneratedFull = PassCountToBeGen; //it can  lower a little the speed, but it must be....
                    }


                    ProgressIterToDoList18 = ProgressIterToDoListLocal;
                    Miki.CalcConvert.LongListToString(in ProgressIterToDoListLocal, "000000000000000000", out ProgressIterToDoString);
                    //here ToDo is bigger than Max and we will not use ToDo because we break, so we must subtract 1
                    ProgressIterDoneList18 = Miki.CalcLists.Sub(ProgressIterToDoListLocal, Iter1);
                    Miki.CalcConvert.LongListToString(in ProgressIterDoneList18, "000000000000000000", out ProgressIterDoneString);

                    DigPassCurrList = DigPassCurrListLocal;
                    long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                    long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                    long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                    long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                    long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                    long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                    long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                    string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  {3} - {4} in  {5}:{6}:{7}:{8} SpeedB:{9} SpeedF:{10} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, 100, PassTextListLocal[0], PassTextListLocal[^1], TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                    Console.Write("\r{0}   ", PassGeneratedString);
                    PassGeneratedBase = PassGeneratedBaseLocal;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    return PassTextListLocal;
                }
                DigPassCurrListLocal = DigPassNextListLocal;

            }
        }


        public static List<string> PassListFromDigByTabUGood(int PassToBeGen, int TestMode = 0)
        {
            //ListArrayPerf();
            int PassMinLengthLocal = PassMinLength;
            int PassMaxLengthLocal = PassMaxLength;
            int FillToLengthLocal = FillToLength;
            char[][] MaskArrayLocal = MaskArray;
            List<long>[][] ConvListArrayLocal = ConvListArray;
            List<string> PassTextListLocal = new(PassToBeGen);
            List<string> TempRemovedLocal = new();

            int PosForDigsLocal = PosForDigs;
            long PosForDigsMultLocal = PosForDigsMult;

            if (TempRemoved.Count > 0)
            {
                PassTextListLocal.AddRange(TempRemoved);
                TempRemoved.Clear();
            }

            int PassGeneratedBaseLocal = 0;
            long PassGeneratedFull = 0;

            int PassOnePercent = PassToBeGen / 100;
            int CounterDisplayLocal = CounterDisplay;
            if (PassOnePercent == 0) { PassOnePercent++; }
            Stopwatch LoopStopwatchFull;

            List<long> DigPassCurrListLocal = DigPassCurrList;
            List<long> DigPassNextListLocal = new();
            List<long> WorkingIterLocal = WorkingIter;

            long[][] DigLenArrayLocal = DigLenArray;
            List<long>[] AllUnarysValueListArrayLocal = AllUnarysValueListArray;

            int IsUnaryLocal = IsUnary;

            List<long> IncrementList18Local = IncrementList18;
            //List<long> IncrementList9Local = IncrementList9;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            //List<long> TotalPossibleCountListLocal = TotalPossibleCountList;
            //Miki.CalcConvert.StringToLongList(TotalPossibleCountString, 18, out List<long> TotalPossibleCountListLocal);
            List<long> RealPossibleCountListLocal = RealPossibleCountList;

            List<long> Iter1 = new() { 1 };

            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> ProgressPoolMaxListLocal = ProgressPoolMaxList;
            int ProgressPoolMaxListLocalCount = ProgressPoolMaxListLocal.Count;
            int RealPossibleCountListLocalCount = RealPossibleCountListLocal.Count;
            int FirstPossibleDecListLocalCount = FirstPossibleDecListLocal.Count;
            long LastDisplayCount = 0;

            //int CompareBigger = 10;


            //check if pool limit in SessionProgress.txt is bigger then TotalBasePassCount list
            Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, in RealPossibleCountListLocalCount, in ProgressPoolMaxListLocal, in ProgressPoolMaxListLocalCount, out int BiggerPool);
            //here generator will change second value in SessionProgress.txt file (ProgressPoolMaxList)
            if (BiggerPool == 2)
            {
                ProgressPoolMaxListLocal = RealPossibleCountListLocal;
                ProgressPoolMaxList = ProgressPoolMaxListLocal;
            }

            //at start generator we must use ProgressIterToDoList18 to generate WorkingIter and WorkingDigList == DigPassCurListLocal
            //in main loop we can do it easier: WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, IncrementList18Local);
            WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoList18, in IncrementList18Local);
            List<long> WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
            DigPassCurrListLocal = new(WorkingDigList);

            int FirstNonZero = 0;
            LoopStopwatchFull = Stopwatch.StartNew();

            while (true)
            {
                int CharPosMax = PassMaxLengthLocal - 1;
                //int CharPosForUnary = PassMaxLengthLocal - 1;
                List<long> ToCompare = DigPassCurrListLocal;

                int IsUnarySubtracted = 0;
                //int IsBigger = 10; //temporary

                //for (int i = CharPosMax; i > 0; i--)
                //{
                List<long> UnaryToCompare = AllUnarysValueListArrayLocal[CharPosMax];
                Miki.CalcCompare.ListBigger(in ToCompare, ToCompare.Count, in UnaryToCompare, UnaryToCompare.Count, out int IsBigger);
                if (IsBigger != 2) // if AllUnaryValueListLocal[CharPos] is equal or bigger than ToCompare
                {
                    ToCompare = Miki.CalcLists.Sub(in ToCompare, in UnaryToCompare); //we subtract UnaryToCompare
                    IsUnarySubtracted = 1;
                    //now we must tell converter, which CharPos can take unary value
                    //CharPosForUnary = i;
                    //break;
                }
                //}

                List<char> MyCharList = new(PassMaxLengthLocal); //new password in char
                //char[] MyCharArray = new char[PassMaxLengthLocal];
                List<string> PassFromDigList = new(PassMaxLengthLocal); //new password in string

                FirstNonZero = 0;

                while (CharPosMax >= 0)
                {
                    if (IsUnarySubtracted == 1 && MaskArrayLocal[CharPosMax].Length == 1)
                    {
                        MyCharList.Add(MaskArrayLocal[CharPosMax][0]); //here unary value is possible
                                                                       //MyCharArray[CharPosMax] = MaskArrayLocal[CharPosMax][0];
                        FirstNonZero = 1;
                    }
                    else
                    {
                        int ToCompareCount = ToCompare.Count - 1;
                        long ActDigToCompare;
                        if (ToCompareCount > 0)
                        {
                            Miki.CalcIntExt.LongLength(ToCompare[^1], out int ToCompareLen); //Last pos ToCompare length                                                   
                            Miki.CalcCompare.GetLongFromList(in ToCompare, in PosForDigsLocal, in ToCompareLen, out ActDigToCompare); //YYYY
                            ToCompareLen = 18 * (ToCompareCount) + ToCompareLen; //Full Length XXXX....
                            ActDigToCompare = (ToCompareLen * PosForDigsMultLocal) + ActDigToCompare + 1000000000000000000; //Make 4 places and add ActDigToCompare -> XXXX...YYYY
                        }
                        else
                        {
                            ActDigToCompare = ToCompare[0];
                        }
                        //St1.Start();
                        int StartCompare = 0;
                        int CharPosLength = MaskArrayLocal[CharPosMax].Length - 1;

                        long[] DigLenArrayLocalPart = DigLenArrayLocal[CharPosMax];

                        int mid = (1 + (CharPosLength - 1)) / 2;
                        long ActDigToCompareT = DigLenArrayLocalPart[mid];
                        if (ActDigToCompareT > ActDigToCompare) //zawęź do połowy i szukaj od mid w dół
                        {
                            mid--;
                            for (int a = mid; a >= 1; a--)
                            {
                                ActDigToCompareT = DigLenArrayLocalPart[a];
                                if (ActDigToCompareT <= ActDigToCompare)
                                {
                                    StartCompare = a;
                                    if (ActDigToCompareT == ActDigToCompare && ActDigToCompare > 999999999999999999)
                                    {
                                        //Equal++;
                                        int s = ToCompareCount;

                                        while (s >= 0)
                                        {
                                            if (ToCompare[s] < ConvListArrayLocal[CharPosMax][StartCompare][s])
                                            { break; }
                                            else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][StartCompare][s])
                                            {
                                                StartCompare--;
                                                if (ToCompareCount + 1 > ConvListArrayLocal[CharPosMax][StartCompare].Count)
                                                { break; }
                                                else
                                                {
                                                    s = ToCompareCount;
                                                    if (StartCompare == 0)
                                                    { break; }
                                                }
                                            }
                                            else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                            { s--; }
                                        }

                                    }
                                    break;
                                }
                            }
                        }
                        else if (ActDigToCompareT < ActDigToCompare) // take half and look from CharPosLength down
                        {
                            //mid++;
                            for (int a = CharPosLength; a >= mid; a--)
                            {
                                ActDigToCompareT = DigLenArrayLocalPart[a];
                                if (ActDigToCompareT <= ActDigToCompare)
                                {
                                    StartCompare = a;
                                    if (ActDigToCompareT == ActDigToCompare && ActDigToCompare > 999999999999999999)
                                    {
                                        //Equal++;
                                        int s = ToCompareCount;
                                        while (s >= 0)
                                        {
                                            if (ToCompare[s] < ConvListArrayLocal[CharPosMax][StartCompare][s])
                                            { break; }
                                            else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][StartCompare][s])
                                            {
                                                StartCompare--;
                                                if (ToCompareCount + 1 > ConvListArrayLocal[CharPosMax][StartCompare].Count)
                                                { break; }
                                                else
                                                {
                                                    s = ToCompareCount;
                                                    if (StartCompare == 0)
                                                    { break; }
                                                }
                                            }
                                            else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                            { s--; }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            StartCompare = mid;
                            //Equal++;
                            if (ActDigToCompare > 999999999999999999)
                            {
                                int s = ToCompareCount;
                                while (s >= 0)
                                {
                                    if (ToCompare[s] < ConvListArrayLocal[CharPosMax][StartCompare][s])
                                    { break; }
                                    else if (ToCompare[s] > ConvListArrayLocal[CharPosMax][StartCompare][s])
                                    {
                                        StartCompare--;
                                        if (ToCompareCount + 1 > ConvListArrayLocal[CharPosMax][StartCompare].Count)
                                        { break; }
                                        else
                                        {
                                            s = ToCompareCount;
                                            if (StartCompare == 0)
                                            { break; }
                                        }
                                    }
                                    else //ToCompare[s] == ConvListArrayLocal[CharPosMax][StartCompare][s]
                                    { s--; }
                                }
                            }
                        }

                        //St2.Stop();
                        //if (MyEnd != StartCompare)
                        //{ Console.WriteLine("MyEnd != StartCompare"); }

                        if (StartCompare > 0 || FirstNonZero == 1 || CharPosMax == 0)
                        {
                            //St1.Start();
                            ToCompare = Miki.CalcLists.Sub(in ToCompare, in ConvListArrayLocal[CharPosMax][StartCompare]);
                            //St2.Stop();
                            MyCharList.Add(MaskArrayLocal[CharPosMax][StartCompare]);
                            FirstNonZero = 1;
                            //St1.Stop();
                        }
                    }
                    CharPosMax--;
                } //end loop for char position in password (from up)
                //string abc = new string(MyCharArray);
                //here we have a password which is converted from decimal iteration, eg. XXXXX
                //we can fill now rest of characters, but characters must represent zeros -> YYYYYXXXXX
                PassGeneratedBaseLocal++;
                //char[] PassArray;// = new char[1];
                if (FillToLengthLocal == 1)
                {
                    int MyCharListCount = MyCharList.Count;

                    if (MyCharListCount == PassMinLengthLocal) //Fixed or not, but first pass is ok
                    {
                        if (PassMaxLengthLocal == PassMinLengthLocal) //Fixed, first pass length ok
                        {
                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;
                        }
                        else //Not fixed, first pass length is ok
                        {
                            //we can always add first pass
                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;
                            //fill next characters
                            while (MyCharListCount < PassMaxLengthLocal)//add to max length, and add to PassFromDigList
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1) //if next character is zero we can add, if not we must break adding
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    PassFromDigList.Add(new string(MyCharList.ToArray()));
                                    PassGeneratedFull++;
                                    MyCharListCount++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else //Fixed or not, but first pass is to short
                    {
                        if (PassMaxLengthLocal == PassMinLengthLocal) //Fixed length
                        {
                            //we can add next character if we check it is zero
                            while (MyCharListCount < PassMaxLengthLocal)
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1) //if next character is zero we can add, if not we must break adding
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    MyCharListCount++;
                                }
                                else
                                { break; }
                            }

                            //finally add to passlist
                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;

                        }
                        else //Not fixed
                        {
                            //fill to min length and add
                            while (MyCharListCount < PassMinLengthLocal)//add to min length
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1) //if zero we can add
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    MyCharListCount++;
                                }
                                else
                                { break; } // break if no zero

                            }

                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;

                            //fill from min to max, then add
                            while (MyCharListCount < PassMaxLengthLocal)
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1)
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    PassFromDigList.Add(new string(MyCharList.ToArray()));
                                    PassGeneratedFull++;
                                    MyCharListCount++;
                                }
                                else
                                { break; }
                            }

                        }
                    }
                }
                else
                {
                    //if we don't want to fill
                    PassFromDigList.Add(new string(MyCharList.ToArray()));
                    PassGeneratedFull++;
                }

                //All pass count in PassFromDigList, we can add now

                PassTextListLocal.AddRange(PassFromDigList);

                //NEW NUMBER (seed)
                //WorkingIterLocal = Miki.CalcLists.Add(in DigPassCurrListLocal, in IncrementList18Local);
                //DigPassNextListLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];

                //ProgressIterToDoListLocal = Miki.CalcLists.Add(ProgressIterToDoListLocal, Iter1);
                //WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoListLocal, 18, in IncrementList9Local, 9, 18);

                WorkingIterLocal = Miki.CalcLists.Add(in WorkingIterLocal, in IncrementList18Local);
                Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, in RealPossibleCountListLocalCount, in WorkingIterLocal, WorkingIterLocal.Count, out int WorkingBigger);
                if (WorkingBigger < 2)
                {
                    WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                }
                else
                {
                    //Console.WriteLine("MOD");
                    WorkingIterLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                }

                DigPassNextListLocal = new(WorkingDigList);
                //NEW NUMBER DONE...


                //Check if Total End. If Increment is good, then return to seed=0 (ThisDigPassNextList) means that all iteration are done....
                Miki.CalcCompare.ListBigger(in DigPassNextListLocal, DigPassNextListLocal.Count, in FirstPossibleDecListLocal, in FirstPossibleDecListLocalCount, out int TotalEndCheck);
                if (TotalEndCheck == 0)
                {
                    //Console.WriteLine("Total");
                    GeneratorExitReasonTotal = "Total";
                }

                long RealPassGeneratedFull = PassGeneratedFull;
                if (CounterDisplayLocal == 1)
                {
                    LoopStopwatchFull.Stop();
                    if (LastDisplayCount != PassGeneratedBaseLocal && PassGeneratedBaseLocal % PassOnePercent == 0)
                    {
                        LastDisplayCount = PassGeneratedBaseLocal;
                        int PassPercent = (int)(PassGeneratedBaseLocal * 100 / PassToBeGen);
                        long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                        long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                        long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                        long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                        long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                        long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                        long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                        string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  in  {3}:{4}:{5}:{6} SpeedB:{7} SpeedF:{8} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, PassPercent, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                    LoopStopwatchFull.Start();
                }

                if (PassGeneratedFull >= PassToBeGen || TotalEndCheck == 0)
                {
                    LoopStopwatchFull.Stop();
                    //here we can add PassGeneratedBaseLocal and check if Pool ends
                    List<long> PassGeneratedBaseLocalList = new List<long>() { PassGeneratedBaseLocal };
                    ProgressIterToDoListLocal = Miki.CalcLists.Add(in ProgressIterToDoListLocal, in PassGeneratedBaseLocalList);
                    //Check if Pool Ends, if ToDo biger PoolMax
                    Miki.CalcCompare.ListBigger(in ProgressPoolMaxListLocal, ProgressPoolMaxListLocalCount, in ProgressIterToDoListLocal, ProgressIterToDoListLocal.Count, out int PoolEndCheck);
                    if (PoolEndCheck == 2)
                    {
                        GeneratorExitReasonPool = "Pool";
                    }

                    if (PassGeneratedFull > PassToBeGen && PoolEndCheck != 2 && TotalEndCheck != 0 && TestMode == 1) //to maintain the same length of each wordlist, only if not GeneratorExitReason is Pool or Total
                    {
                        int ToRemoveCount = PassTextListLocal.Count - PassToBeGen;
                        TempRemoved.AddRange(PassTextListLocal[PassToBeGen..]); //TempRemoved will be added in next PassListFromDigByTab run
                        PassTextListLocal.RemoveRange(PassToBeGen, ToRemoveCount); //Remove TempRemoved from PassTextListLocal
                        PassGeneratedFull = PassToBeGen; //it can  lower a little the speed, but it must be....
                    }


                    ProgressIterToDoList18 = ProgressIterToDoListLocal;
                    Miki.CalcConvert.LongListToString(in ProgressIterToDoListLocal, "000000000000000000", out ProgressIterToDoString);
                    //here ToDo is bigger than Max and we will not use ToDo because we break, so we must subtract 1
                    ProgressIterDoneList18 = Miki.CalcLists.Sub(ProgressIterToDoListLocal, Iter1);
                    Miki.CalcConvert.LongListToString(in ProgressIterDoneList18, "000000000000000000", out ProgressIterDoneString);

                    DigPassCurrList = DigPassCurrListLocal;
                    long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                    long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                    long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                    long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                    long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                    long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                    long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                    string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  {3} - {4} in  {5}:{6}:{7}:{8} SpeedB:{9} SpeedF:{10} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, 100, PassTextListLocal[0], PassTextListLocal[^1], TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                    Console.Write("\r{0}   ", PassGeneratedString);
                    PassGeneratedBase = PassGeneratedBaseLocal;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    return PassTextListLocal;
                }
                DigPassCurrListLocal = DigPassNextListLocal;

            }
        }


        public static List<string> PassListFromDigByTabUOld(int PassToBeGen, int TestMode = 0)
        {
            //ListArrayPerf();
            int PassMinLengthLocal = PassMinLength;
            int PassMaxLengthLocal = PassMaxLength;
            int FillToLengthLocal = FillToLength;
            char[][] MaskArrayLocal = MaskArray;
            List<long>[][] ConvListArrayLocal = ConvListArray;
            List<string> PassTextListLocal = new(PassToBeGen);
            List<string> TempRemovedLocal = new();

            int PosForDigsLocal = PosForDigs;
            long PosForDigsMultLocal = PosForDigsMult;

            if (TempRemoved.Count > 0)
            {
                PassTextListLocal.AddRange(TempRemoved);
                TempRemoved.Clear();
            }

            int PassGeneratedBaseLocal = 0;
            long PassGeneratedFull = 0;

            int PassOnePercent = PassToBeGen / 100;
            int CounterDisplayLocal = CounterDisplay;
            if (PassOnePercent == 0) { PassOnePercent++; }
            Stopwatch LoopStopwatchFull;

            List<long> DigPassCurrListLocal = DigPassCurrList;
            List<long> DigPassNextListLocal = new();
            List<long> WorkingIterLocal = WorkingIter;

            long[][] DigLenArrayLocal = DigLenArray;
            List<long>[] AllUnarysValueListArrayLocal = AllUnarysValueListArray;

            int IsUnaryLocal = IsUnary;

            List<long> IncrementList18Local = IncrementList18;
            //List<long> IncrementList9Local = IncrementList9;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            //List<long> TotalPossibleCountListLocal = TotalPossibleCountList;
            //Miki.CalcConvert.StringToLongList(TotalPossibleCountString, 18, out List<long> TotalPossibleCountListLocal);
            List<long> RealPossibleCountListLocal = RealPossibleCountList;

            List<long> Iter1 = new() { 1 };

            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> ProgressPoolMaxListLocal = ProgressPoolMaxList;
            int ProgressPoolMaxListLocalCount = ProgressPoolMaxListLocal.Count;
            int RealPossibleCountListLocalCount = RealPossibleCountListLocal.Count;
            int FirstPossibleDecListLocalCount = FirstPossibleDecListLocal.Count;
            long LastDisplayCount = 0;

            int CompareBigger = 10;


            //check if pool limit in SessionProgress.txt is bigger then TotalBasePassCount list
            Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, in RealPossibleCountListLocalCount, in ProgressPoolMaxListLocal, in ProgressPoolMaxListLocalCount, out int BiggerPool);
            //here generator will change second value in SessionProgress.txt file (ProgressPoolMaxList)
            if (BiggerPool == 2)
            {
                ProgressPoolMaxListLocal = RealPossibleCountListLocal;
                ProgressPoolMaxList = ProgressPoolMaxListLocal;
            }

            //at start generator we must use ProgressIterToDoList18 to generate WorkingIter and WorkingDigList == DigPassCurListLocal
            //in main loop we can do it easier: WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, IncrementList18Local);
            WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoList18, in IncrementList18Local);
            List<long> WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
            DigPassCurrListLocal = new(WorkingDigList);

            int FirstNonZero = 0;
            LoopStopwatchFull = Stopwatch.StartNew();

            while (true)
            {
                int CharPosMax = PassMaxLengthLocal - 1;
                int CharPosForUnary = PassMaxLengthLocal - 1;
                List<long> ToCompare = DigPassCurrListLocal;

                int IsUnarySubtracted = 0;
                int IsBigger = 10; //temporary

                //for (int i = CharPosMax; i > 0; i--)
                //{
                List<long> UnaryToCompare = AllUnarysValueListArrayLocal[CharPosMax];
                Miki.CalcCompare.ListBigger(in ToCompare, ToCompare.Count, in UnaryToCompare, UnaryToCompare.Count, out IsBigger);
                if (IsBigger != 2) // if AllUnaryValueListLocal[CharPos] is equal bigger than ToCompare
                {
                    ToCompare = Miki.CalcLists.Sub(in ToCompare, in UnaryToCompare); //we subtract UnaryToCompare
                    IsUnarySubtracted = 1;
                    //now we must tell converter, which CharPos can take unary value
                    //CharPosForUnary = i;
                    //break;
                }
                //}

                List<char> MyCharList = new(PassMaxLengthLocal); //new password in char
                //char[] MyCharArray = new char[PassMaxLengthLocal];
                List<string> PassFromDigList = new(PassMaxLengthLocal); //new password in string

                FirstNonZero = 0;

                while (CharPosMax >= 0)
                {
                    if (IsUnarySubtracted == 1 && MaskArrayLocal[CharPosMax].Length == 1)
                    {
                        //UnaryToCompare = AllUnarysValueListArrayLocal[CharPosMax];
                        //Miki.CalcCompare.ListBigger(in ToCompare, in UnaryToCompare, out IsBigger);

                        //if (IsBigger != 2)
                        {
                            MyCharList.Add(MaskArrayLocal[CharPosMax][0]); //here unary value is possible
                                                                           //MyCharArray[CharPosMax] = MaskArrayLocal[CharPosMax][0];
                            FirstNonZero = 1;
                        }

                    }
                    else
                    {
                        if (CharPosMax == 1)
                        { }
                        //prepare XXXX...YYYY (ActDigToCompare) from ToCompare diglist
                        int ToCompareCount = ToCompare.Count;
                        Miki.CalcIntExt.LongLength(ToCompare[^1], out int ToCompareLen); //Last pos ToCompare length                                                   
                        Miki.CalcCompare.GetLongFromList(in ToCompare, in PosForDigsLocal, in ToCompareLen, out long ActDigToCompare); //YYYY
                        ToCompareLen = 18 * (ToCompareCount - 1) + ToCompareLen; //Full Length XXXX....
                        ActDigToCompare = (ToCompareLen * PosForDigsMultLocal) + ActDigToCompare; //Make 4 places and add ActDigToCompare -> XXXX...YYYY

                        int EndCompare = 0;
                        int StartCompare = MaskArrayLocal[CharPosMax].Length - 1;
                        int Found = 0;
                        for (int i = StartCompare; i >= 0; i--)
                        {
                            if (DigLenArrayLocal[CharPosMax][i] <= ActDigToCompare)
                            {
                                StartCompare = i;
                                Found++;
                                break;
                            }

                        }
                        if (Found == 0) { StartCompare = 0; }

                        //int InnerLoop = 0;
                        for (int CharIntTab = StartCompare; CharIntTab >= EndCompare; CharIntTab--)
                        {
                            if (CharPosMax == 0)
                            { FirstNonZero = 1; }
                            //InnerLoop++;
                            //if (InnerLoop > 1) 
                            //{ Console.WriteLine(InnerLoop); }
                            List<long> FromConvToCompare = ConvListArrayLocal[CharPosMax][CharIntTab];
                            Miki.CalcCompare.ListBigger(in FromConvToCompare, FromConvToCompare.Count, in ToCompare, in ToCompareCount, out CompareBigger);

                            if (CompareBigger != 1)
                            {
                                if (CharIntTab > 0 || FirstNonZero == 1)
                                {
                                    if (CharPosMax == 1 && CharIntTab != 0)
                                    { }
                                    ToCompare = Miki.CalcLists.Sub(in ToCompare, in FromConvToCompare);
                                    MyCharList.Add(MaskArrayLocal[CharPosMax][CharIntTab]);
                                    //MyCharArray[CharPosMax] = MaskArrayLocal[CharPosMax][CharInt];
                                    FirstNonZero = 1;
                                }
                                break;
                            }
                        } //end loop for looking for character
                    }
                    CharPosMax--;
                } //end loop for char position in password (from up)
                //string abc = new string(MyCharArray);
                //here we have a password which is converted from decimal iteration, eg. XXXXX
                //we can fill now rest of characters, but characters must represent zeros -> YYYYYXXXXX
                PassGeneratedBaseLocal++;
                //char[] PassArray;// = new char[1];
                if (FillToLengthLocal == 1)
                {
                    int MyCharListCount = MyCharList.Count;

                    if (MyCharListCount == PassMinLengthLocal) //Fixed or not, but first pass is ok
                    {
                        if (PassMaxLengthLocal == PassMinLengthLocal) //Fixed, first pass length ok
                        {
                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;
                        }
                        else //Not fixed, first pass length is ok
                        {
                            //we can always add first pass
                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;
                            //fill next characters
                            while (MyCharListCount < PassMaxLengthLocal)//add to max length, and add to PassFromDigList
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1) //if next character is zero we can add, if not we must break adding
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    PassFromDigList.Add(new string(MyCharList.ToArray()));
                                    PassGeneratedFull++;
                                    MyCharListCount++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else //Fixed or not, but first pass is to short
                    {
                        if (PassMaxLengthLocal == PassMinLengthLocal) //Fixed length
                        {
                            //we can add next character if we check it is zero
                            while (MyCharListCount < PassMaxLengthLocal)
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1) //if next character is zero we can add, if not we must break adding
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    MyCharListCount++;
                                }
                                else
                                { break; }
                            }

                            //finally add to passlist
                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;

                        }
                        else //Not fixed
                        {
                            //fill to min length and add
                            while (MyCharListCount < PassMinLengthLocal)//add to min length
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1) //if zero we can add
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    MyCharListCount++;
                                }
                                else
                                { break; } // break if no zero

                            }

                            PassFromDigList.Add(new string(MyCharList.ToArray()));
                            PassGeneratedFull++;

                            //fill from min to max, then add
                            while (MyCharListCount < PassMaxLengthLocal)
                            {
                                if (MaskArrayLocal[MyCharListCount].Length > 1)
                                {
                                    MyCharList.Insert(0, MaskArrayLocal[MyCharListCount][0]);
                                    PassFromDigList.Add(new string(MyCharList.ToArray()));
                                    PassGeneratedFull++;
                                    MyCharListCount++;
                                }
                                else
                                { break; }
                            }

                        }
                    }
                }
                else
                {
                    //if we don't want to fill
                    PassFromDigList.Add(new string(MyCharList.ToArray()));
                    PassGeneratedFull++;
                }

                //All pass count in PassFromDigList, we can add now

                PassTextListLocal.AddRange(PassFromDigList);

                //NEW NUMBER (seed)
                //ProgressIterToDoListLocal = Miki.CalcLists.Add(ProgressIterToDoListLocal, Iter1);
                //WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterToDoListLocal, 18, in IncrementList9Local, 9, 18);
                WorkingIterLocal = Miki.CalcLists.Add(in WorkingIterLocal, in IncrementList18Local);
                Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, in RealPossibleCountListLocalCount, in WorkingIterLocal, WorkingIterLocal.Count, out int WorkingBigger);
                if (WorkingBigger < 2)
                {
                    WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                }
                else
                {
                    //Console.WriteLine("MOD");
                    WorkingIterLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                    WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                }
                DigPassNextListLocal = new(WorkingDigList);
                //NEW NUMBER DONE...


                //Check if Total End. If Increment is good, then return to seed=0 (ThisDigPassNextList) means that all iteration are done....
                Miki.CalcCompare.ListBigger(in DigPassNextListLocal, DigPassNextListLocal.Count, in FirstPossibleDecListLocal, in FirstPossibleDecListLocalCount, out int TotalEndCheck);
                if (TotalEndCheck == 0)
                {
                    //Console.WriteLine("Total");
                    GeneratorExitReasonTotal = "Total";
                }

                long RealPassGeneratedFull = PassGeneratedFull;
                if (CounterDisplayLocal == 1)
                {
                    LoopStopwatchFull.Stop();
                    if (LastDisplayCount != PassGeneratedBaseLocal && PassGeneratedBaseLocal % PassOnePercent == 0)
                    {
                        LastDisplayCount = PassGeneratedBaseLocal;
                        int PassPercent = (int)(PassGeneratedBaseLocal * 100 / PassToBeGen);
                        long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                        long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                        long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                        long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                        long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                        long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                        long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                        string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  in  h:{3} min:{4} sec:{5} ms:{6} SpeedB:{7} SpeedF:{8} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, PassPercent, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                    LoopStopwatchFull.Start();
                }

                if (PassGeneratedFull >= PassToBeGen || TotalEndCheck == 0)
                {
                    LoopStopwatchFull.Stop();
                    //here we can add PassGeneratedBaseLocal and check if Pool ends
                    List<long> PassGeneratedBaseLocalList = new List<long>() { PassGeneratedBaseLocal };
                    ProgressIterToDoListLocal = Miki.CalcLists.Add(in ProgressIterToDoListLocal, in PassGeneratedBaseLocalList);
                    //Check if Pool Ends, if ToDo biger PoolMax
                    Miki.CalcCompare.ListBigger(in ProgressPoolMaxListLocal, ProgressPoolMaxListLocalCount, in ProgressIterToDoListLocal, ProgressIterToDoListLocal.Count, out int PoolEndCheck);
                    if (PoolEndCheck == 2)
                    {
                        GeneratorExitReasonPool = "Pool";
                    }

                    if (PassGeneratedFull > PassToBeGen && PoolEndCheck != 2 && TotalEndCheck != 0 && TestMode == 1) //to maintain the same length of each wordlist, only if not GeneratorExitReason is Pool or Total
                    {
                        int ToRemoveCount = PassTextListLocal.Count - PassToBeGen;
                        TempRemoved.AddRange(PassTextListLocal[PassToBeGen..]); //TempRemoved will be added in next PassListFromDigByTab run
                        PassTextListLocal.RemoveRange(PassToBeGen, ToRemoveCount); //Remove TempRemoved from PassTextListLocal
                        PassGeneratedFull = PassToBeGen; //it can  lower a little the speed, but it must be....
                    }


                    ProgressIterToDoList18 = ProgressIterToDoListLocal;
                    Miki.CalcConvert.LongListToString(in ProgressIterToDoListLocal, "000000000000000000", out ProgressIterToDoString);
                    //here ToDo is bigger than Max and we will not use ToDo because we break, so we must subtract 1
                    ProgressIterDoneList18 = Miki.CalcLists.Sub(ProgressIterToDoListLocal, Iter1);
                    Miki.CalcConvert.LongListToString(in ProgressIterDoneList18, "000000000000000000", out ProgressIterDoneString);

                    DigPassCurrList = DigPassCurrListLocal;
                    long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                    long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                    long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                    long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                    long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                    long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                    long SpeedFull = (RealPassGeneratedFull * 1000000000 / TimeNsFTotal);

                    string PassGeneratedString = string.Format("B:{0} F:{1} {2} %  in  h:{3} min:{4} sec:{5} ms:{6} SpeedB:{7} SpeedF:{8} /s", PassGeneratedBaseLocal, RealPassGeneratedFull, 100, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                    Console.Write("\r{0}   ", PassGeneratedString);
                    PassGeneratedBase = PassGeneratedBaseLocal;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    return PassTextListLocal;
                }
                DigPassCurrListLocal = DigPassNextListLocal;

            }
        }
        public static void GetDigPassListNext(in List<long> ProgressIterDoneList, out List<long> ProgressIterToDoList)
        {
            List<long> WorkingIterLocal = Miki.CalcLists.Mul(in ProgressIterDoneList, in IncrementList9);
            WorkingIterLocal = Miki.CalcLists.Add(in WorkingIterLocal, in FirstPossibleDecList);
            List<long> WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, in RealPossibleCountList)[1];
            WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecList);
            ProgressIterToDoList = new(WorkingDigList);
        }

        public static void DigFromPassByTab(in string MyPass, out string DigString)
        {
            //convert n-char password to string-number
            List<char> CharList = new(MyPass);
            int k = 0;
            int CharPosMax = CharList.Count - 1;

            List<long> DigList = new List<long>() { 0 };

            char[][] MaskArrayLocal = MaskArray;
            List<long>[][] ConvListArrayLocal = ConvListArray;

            for (int CharPos = CharPosMax; CharPos >= 0; CharPos--)
            {
                int CharValue = Array.IndexOf(MaskArrayLocal[CharPos], (CharList[k]));
                DigList = Miki.CalcLists.Add(in DigList, ConvListArrayLocal[CharPos][CharValue]);
                k++;
            }
            Miki.CalcConvert.LongListToString(in DigList, "000000000000000000", out string Dig);
            DigString = Dig;
            return;
        }


        public static void TestGenerator(int WithCheck, int LoopCount)
        {
            Console.WriteLine("");
            Console.WriteLine("Generator started, wait...");

            long Errors = 0;
            TempRemoved.Clear();

            long GenTimePassMs;
            long GenTimeCheckMs;
            long CheckTimeMs;
            long GenTimeFull;

            long ThisCounterBase = 0;
            long ThisCounterFull = 0;
            int PassMinLengthLocal = PassMinLength;
            int PassMaxLengthLocal = PassMaxLength;

            Stopwatch StopwatchGenerator;
            Stopwatch StopwatchCheckList;
            Stopwatch StopwatchControl;
            Stopwatch StopwatchGenFull = Stopwatch.StartNew();
            StopwatchGenFull.Stop();
            //List<long> IncrementList9Local = IncrementList9;
            List<long> IncrementList18Local = IncrementList18;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            List<long> RealPossibleCountListLocal = RealPossibleCountList;
            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> PassDigListAct = DigPassCurrList; //Temporary, later it is calculated form iteration


            List<long> IterPlus = new() { 1 };

            try
            {
                Console.WriteLine("Warming up...");
                List<string> PassTestList;
                if (IsUnary != 1)
                { PassTestList = PassListFromDigByTabN(100000); } //build List of Passwords
                else
                { PassTestList = PassListFromDigByTabU(100000); }
                for (int i = 0; i < PassTestList.Count; i++)
                { DigFromPassByTab(PassTestList[i], out string Warming); } //here ProgressIterToDoList18 will be changed
                Console.WriteLine("");
                Console.WriteLine("Let's start...");
                Console.WriteLine("");
                ProgressIterToDoList18 = ProgressIterToDoListLocal; //now we restore original ProgressIterToDoList
                DigPassCurrList = PassDigListAct; //And restore DigPassCurrList
            }
            catch { }



            //It is not necessary but if DigPassCurList == WorkingDigList (PassDigListAct) everything is ok.
            List<long> WorkingIterLocal = Miki.CalcLists.Mul(ProgressIterToDoListLocal, IncrementList18Local);
            //WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, FirstPossibleDecListLocal);
            List<long> WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecListLocal);
            PassDigListAct = new(WorkingDigList); //Calculated from iteration

            //PassDigListAct = DigPassCurrList;
            while (true)
            {

                //Generate PassTestList
                StopwatchGenerator = Stopwatch.StartNew();
                StopwatchGenFull.Start();
                List<string> PassTestList;
                if (IsUnary != 1)
                { PassTestList = PassListFromDigByTabN(LoopCount); } //build List of Passwords
                else
                { PassTestList = PassListFromDigByTabU(LoopCount); }
                StopwatchGenFull.Stop();
                StopwatchGenerator.Stop();
                GenTimePassMs = (long)StopwatchGenerator.Elapsed.TotalMilliseconds;
                GenTimeFull = (long)StopwatchGenFull.Elapsed.TotalMilliseconds;
                //End generating PassTestList

                ThisCounterFull += PassTestList.Count;
                ThisCounterBase += PassGeneratedBase; //base from generator
                if (WithCheck == 1)
                {
                    //Generate CheckList
                    StopwatchCheckList = Stopwatch.StartNew();
                    List<List<string>> ControlDigTest = new();
                    string PassString;
                    int PassTestListCount = PassTestList.Count;

                    for (int i = 0; i < PassTestListCount; i++)
                    {
                        Miki.CalcConvert.LongListToString(in PassDigListAct, "000000000000000000", out string PassDigListActS);

                        if (PassMinLengthLocal == PassMaxLengthLocal)
                        {
                            PassString = PassTestList[i];
                            //string DigString;
                            DigFromPassByTab(in PassString, out string DigString); //change each password to dig
                            List<string> TempPassStringTriple = new(3) { PassString, DigString, PassDigListActS };
                            ControlDigTest.Add(TempPassStringTriple);
                        }
                        else
                        {
                            int pos = 0;
                            string PassStringTest = PassTestList[i];
                            DigFromPassByTab(in PassStringTest, out string DigStringTest);
                            while (true) //if non fixed length
                            {
                                if (i < PassTestListCount)
                                {
                                    PassString = PassTestList[i];
                                    DigFromPassByTab(in PassString, out string DigString); //change each password to dig
                                    if (DigStringTest == DigString)
                                    {
                                        //PassString is our Password, PassDigListActS is calculated from iteration, DigString is converted from PassString
                                        List<string> TempPassStringTriple = new(3) { PassString, DigString, PassDigListActS };
                                        ControlDigTest.Add(TempPassStringTriple);
                                        pos++;
                                        i++;
                                    }
                                    else
                                    { i--; break; }
                                }
                                else
                                { break; }

                            }
                        }

                        //Calculate next PassDigList
                        if (IsUnary != 1)
                        {
                            WorkingIterLocal = Miki.CalcLists.Add(in PassDigListAct, in IncrementList18Local); //faster, ProgressIterToDoListLocal we can count at the end
                                                                                                               //WorkingIterLocal = Miki.CalcLists.Add(in WorkingIterLocal, in IncrementList18Local);
                            WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                            //WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                            PassDigListAct = new(WorkingDigList); //Calculated from seed
                        }
                        else
                        {

                            WorkingIterLocal = Miki.CalcLists.Add(in WorkingIterLocal, in IncrementList18Local);
                            Miki.CalcCompare.ListBigger(RealPossibleCountListLocal, RealPossibleCountListLocal.Count, WorkingIterLocal, WorkingIterLocal.Count, out int WorkingBigger);
                            if (WorkingBigger < 2)
                            {
                                WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                                WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                            }
                            else
                            {
                                //Console.WriteLine("MOD");
                                WorkingIterLocal = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                                WorkingDigList = Miki.CalcLists.Div(in WorkingIterLocal, in RealPossibleCountListLocal)[1];
                                WorkingDigList = Miki.CalcLists.Add(in WorkingDigList, in FirstPossibleDecListLocal);
                            }

                            PassDigListAct = new(WorkingDigList);


                        }

                    }
                    StopwatchCheckList.Stop();
                    GenTimeCheckMs = (long)StopwatchCheckList.Elapsed.TotalMilliseconds;
                    //End generate CheckList

                    //Start checking
                    StopwatchControl = Stopwatch.StartNew();
                    for (int c = 0; c < ControlDigTest.Count; c++)
                    {
                        if (ControlDigTest[c][1] != ControlDigTest[c][2])
                        {
                            Console.WriteLine("ERROR");
                            Console.WriteLine(c);
                            Console.WriteLine(ControlDigTest[c][0]);
                            Console.WriteLine(ControlDigTest[c][1]);
                            Console.WriteLine(ControlDigTest[c][2]);
                            Errors += 1;
                        }
                    }
                    StopwatchControl.Stop();
                    CheckTimeMs = (long)StopwatchControl.Elapsed.TotalMilliseconds;
                    Console.WriteLine("");
                    long FullSpeed = ThisCounterFull * 1000 / GenTimeFull;
                    Console.WriteLine(string.Format("    Total: B:{0}  F:{1}  L:{2} PassGenTime: {3} CheckGenTime: {4}  CheckTime: {5} ms  Errors: {6}  FullSpeed: {7}", ThisCounterBase, ThisCounterFull, PassTestList.Count, GenTimePassMs, GenTimeCheckMs, CheckTimeMs, Errors, FullSpeed));
                    //Console.WriteLine(string.Format("                                FullSpeed: {0}  CheckGenTime: {1}  CheckTime: {2}  ms  Errors: {3}", FullSpeed, GenTimeCheckMs, CheckTimeMs, Errors));
                }
                else
                {
                    long FullSpeed = ThisCounterFull * 1000 / GenTimeFull;
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("    Total: B:{0}  F:{1}  L:{2} PassGenTime: {3} CheckGenTime: {4}  CheckTime: {5} ms  Errors: {6}  FullSpeed: {7}", ThisCounterBase, ThisCounterFull, PassTestList.Count, GenTimePassMs, "N/A", "N/A", "N/A", FullSpeed));
                }
                if (GeneratorExitReasonTotal == "Total")
                { Console.WriteLine("Total pool checked, exit..."); break; }
                if (GeneratorExitReasonPool == "Pool")
                { Console.WriteLine("Progress pool checked, checking next pool..."); }
            }
        }
    }
}

namespace Miki
{
    class Miki
    {
        //Mega-Integer-string (K)Caluculator Intefrace - Miki...
        //for my son, Miki :-*

        public static void Calculator()
        {
            string Dig1 = "";
            string Dig2 = "";
            string CalcType = "";
            bool isIntDig1 = false;
            bool isIntDig2 = false;
            bool isCalcType = false;
            int LineRev = 0;
            while (true)
            {
                Console.WriteLine("##############################################################################################");
                while (isIntDig1 == false)
                {
                    Console.WriteLine("Enter first number, confirm Enter:");
                    Dig1 = Console.ReadLine();
                    isIntDig1 = isNumeric(Dig1);
                    LineRev += 1;
                    if (isIntDig1 == false)
                    { Console.WriteLine("Digits only!!!... try again..."); LineRev += 2; }
                }

                while (Dig1[..1] == "0") //first digit can't be 0
                {
                    if (Dig1.Length == 1) { break; }
                    Dig1 = Dig1[1..];
                }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", Dig1);
                LineRev = 0;

                while (isCalcType == false)
                {
                    Console.WriteLine("Enter +, -, *, /, ^ , confirm Enter:");
                    CalcType = Console.ReadLine();
                    LineRev += 1;
                    if (CalcType != "+" && CalcType != "-" && CalcType != "*" && CalcType != "/" && CalcType != "^")
                    { Console.WriteLine("Only +, -, *, /, ^ ... try again..."); LineRev += 2; }
                    else { isCalcType = true; }
                }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", CalcType);
                LineRev = 0;


                while (isIntDig2 == false)
                {
                    Console.WriteLine("Enter second number, confirm Enter:");
                    Dig2 = Console.ReadLine();
                    LineRev += 1;
                    isIntDig2 = isNumeric(Dig2);
                    if (isIntDig2 == false)
                    { Console.WriteLine("Digits only... try again..."); LineRev += 2; }
                }

                while (Dig2[..1] == "0") //first digit can't be 0
                {
                    if (Dig1.Length == 1) { break; }
                    Dig1 = Dig1[1..];
                }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", Dig2);
                LineRev = 0;



                List<string> MyResult;
                List<string> MyResultCheck;
                string Check = "Check = FALSE";

                if (CalcType == "+")
                {
                    MyResult = CalcStrings.Add(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Sub(MyResult[0], Dig2);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Console.WriteLine(Check);
                }

                if (CalcType == "-")
                {
                    MyResult = CalcStrings.Sub(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Add(MyResult[0], Dig2);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Console.WriteLine(Check);
                }

                if (CalcType == "*")
                {
                    MyResult = CalcStrings.Mul(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Div(MyResult[0], Dig2);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Console.WriteLine(Check);
                }

                if (CalcType == "/")
                {
                    MyResult = CalcStrings.Div(Dig1, Dig2);
                    MyResultCheck = CalcStrings.Mul(MyResult[0], Dig2);
                    MyResultCheck = CalcStrings.Add(MyResultCheck[0], MyResult[1]);
                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0] + "  R  " + MyResult[1]);
                    Console.WriteLine(Check);
                }

                if (CalcType == "^")
                {
                    MyResult = CalcStrings.Pow(Dig1, Dig2);

                    string Dig2Rev = Dig2;
                    MyResultCheck = MyResult;
                    while (true)
                    {
                        MyResultCheck = CalcStrings.Div(MyResultCheck[0], Dig1);
                        Dig2Rev = CalcStrings.Sub(Dig2Rev, "1")[0];
                        if (Dig2Rev == "1")
                        { break; }
                    }

                    if (MyResultCheck[0] == Dig1)
                    { Check = "Check = TRUE"; }
                    Console.WriteLine("=");
                    Console.WriteLine(MyResult[0]);
                    Console.WriteLine(Check);
                }


                Dig1 = "";
                Dig2 = "";
                CalcType = "";
                isIntDig1 = false;
                isIntDig2 = false;
                isCalcType = false;
            }

        }

        public static void TestRandom()
        {
            string Dig1 = "";
            string Dig2 = "";
            bool isIntDig1 = false;
            bool isIntDig2 = false;
            int LineRev = 0;
            string MinDigStringLengthS = "0";
            int MinDigStringLength = 0;
            string MaxDigStringLengthS = "0";
            int MaxDigStringLength = 0;
            int CheckError = 0;
            long Randoms = 0;

            Console.WriteLine("##############################################################################################");
            while (isIntDig1 == false)
            {
                Console.WriteLine("Enter minimal DigString length:");
                MinDigStringLengthS = Console.ReadLine();
                isIntDig1 = isNumeric(MinDigStringLengthS);
                LineRev += 1;
                if (isIntDig1 == false)
                { Console.WriteLine("Digits only!!!... try again..."); LineRev += 2; }
            }
            MinDigStringLength = Convert.ToInt32(MinDigStringLengthS);

            while (isIntDig2 == false)
            {
                Console.WriteLine("Enter maximal DigString length:");
                MaxDigStringLengthS = Console.ReadLine();
                isIntDig2 = isNumeric(MaxDigStringLengthS);
                LineRev += 1;
                if (isIntDig2 == false)
                { Console.WriteLine("Digits only!!!... try again..."); LineRev += 2; }
            }
            MaxDigStringLength = Convert.ToInt32(MaxDigStringLengthS);


            while (true)
            {
                Console.WriteLine(string.Format("####################################################################################   Randoms: {0}    Errors: {1}", Randoms, CheckError));
                Dig1 = Miki.RandomStringDig(MinDigStringLength, MaxDigStringLength);
                Dig2 = Miki.RandomStringDig(MinDigStringLength, MaxDigStringLength);
                Console.WriteLine(string.Format("Dig1={0}", Dig1));
                Console.WriteLine(string.Format("Dig2={0}", Dig2));

                List<string> MyResult;
                List<string> MyResultCheck;
                string Check = "Check = FALSE";
                //++++++++++++++++++++++
                Console.WriteLine("++++++++++++++++++++++++++++++");
                MyResult = CalcStrings.Add(Dig1, Dig2);
                Console.WriteLine(string.Format("DigR={0}", MyResult[0]));
                Console.WriteLine("----");
                Console.WriteLine(string.Format("Dig2={0}", Dig2));
                Console.WriteLine("====");
                MyResultCheck = CalcStrings.Sub(MyResult[0], Dig2);
                Console.WriteLine(string.Format("Dig1={0}", MyResultCheck[0]));
                if (MyResultCheck[0] == Dig1)
                { Check = "Check = TRUE"; }
                else { CheckError += 1; Check = "Check = FALSE"; }

                Console.WriteLine(Check);


                //-----------------------
                Console.WriteLine("------------------------------");
                MyResult = CalcStrings.Sub(Dig1, Dig2);


                Console.WriteLine(string.Format("DigR={0}", MyResult[0]));
                Console.WriteLine("++++");
                Console.WriteLine(string.Format("Dig2={0}", Dig2));
                Console.WriteLine("====");
                MyResultCheck = CalcStrings.Add(MyResult[0], Dig2);
                Console.WriteLine(string.Format("Dig1={0}", MyResultCheck[0]));
                if (MyResultCheck[0] == Dig1)
                { Check = "Check = TRUE"; }
                else { CheckError += 1; Check = "Check = FALSE"; }
                Console.WriteLine(Check);

                //***********************
                Console.WriteLine("******************************");
                MyResult = CalcStrings.Mul(Dig1, Dig2);
                /*
                CalcConvert.StringsToLongLists(Dig1, Dig2, 18, out List<List<long>>TestList);
                List<long> Test = CalcLists.MulGood(TestList[0], 18, TestList[1], 18, 18);
                CalcConvert.LongListToString(Test, "000000000000000000", out string TestString);
                if (TestString != MyResult[0])
                {
                    Console.WriteLine(MyResult[0]);
                    Console.WriteLine(TestString);
                }
                */

                Console.WriteLine(string.Format("DigR={0}", MyResult[0]));
                Console.WriteLine("////");
                Console.WriteLine(string.Format("Dig2={0}", Dig2));
                Console.WriteLine("====");
                MyResultCheck = CalcStrings.Div(MyResult[0], Dig2);
                Console.WriteLine(string.Format("Dig1={0}", MyResultCheck[0]));
                if (MyResultCheck[0] == Dig1)
                { Check = "Check = TRUE"; }
                else { CheckError += 1; Check = "Check = FALSE"; }
                Console.WriteLine(Check);

                // ///////////////////////
                Console.WriteLine("//////////////////////////////");
                MyResult = CalcStrings.Div(Dig1, Dig2);
                Console.WriteLine(string.Format("DigR={0}  R  {1}", MyResult[0], MyResult[1]));
                Console.WriteLine("****");
                Console.WriteLine(string.Format("Dig2={0}", Dig2));
                Console.WriteLine("====");
                MyResultCheck = CalcStrings.Mul(MyResult[0], Dig2);
                MyResultCheck = CalcStrings.Add(MyResultCheck[0], MyResult[1]);
                Console.WriteLine(string.Format("Dig1={0}", MyResultCheck[0]));
                if (MyResultCheck[0] == Dig1)
                { Check = "Check = TRUE"; }
                else { CheckError += 1; Check = "Check = FALSE"; }

                Console.WriteLine(Check);
                Randoms += 1;
            }
        }

        public static string RandomStringDig(int StrMin, int StrMax)
        {
            Random RandomLength = new Random();
            int RandomLengtInt = RandomLength.Next(StrMin, StrMax);

            var chars = "0123456789";
            char[] stringChars = new char[RandomLengtInt];
            Random RandomChars = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                char NewChar = chars[RandomChars.Next(chars.Length)];
                while (i == 0 && NewChar == '0')
                {
                    NewChar = chars[RandomChars.Next(chars.Length)];
                }
                stringChars[i] = NewChar;
            }
            string finalString = new string(stringChars);

            return finalString;
        }
        public static bool isNumeric(string s)
        {
            string CleanString;

            if (s[..1] == "+" || s[..1] == "-")
            { CleanString = s[1..]; }
            else
            { CleanString = s; }

            char[] MyString = CleanString.ToCharArray();
            for (int i = 0; i < MyString.Length; i++)
            {
                if (int.TryParse(MyString[i].ToString(), out int n) == false)
                { return false; }
            }
            return true;
        }
    }

    public static class CalcIntExt
    {
        public static void LongLength(in long i, out int IntLen)
        {
            IntLen =
                (i >= 100000000000000000) ? 18 : (i >= 10000000000000000) ? 17 :
                (i >= 1000000000000000) ? 16 : (i >= 100000000000000) ? 15 :
                (i >= 10000000000000) ? 14 : (i >= 1000000000000) ? 13 :
                (i >= 100000000000) ? 12 : (i >= 10000000000) ? 11 :
                (i >= 1000000000) ? 10 : (i >= 100000000) ? 9 :
                (i >= 10000000) ? 8 : (i >= 1000000) ? 7 :
                (i >= 100000) ? 6 : (i >= 10000) ? 5 :
                (i >= 1000) ? 4 : (i >= 100) ? 3 :
                (i >= 10) ? 2 : 1;

        }
        public static void DecLength(in decimal i, out int DecLen)
        {
            if (i == 0)
                DecLen = 1;

            DecLen = (int)Math.Floor(Math.Log10((double)i)) + 1;
        }
        static void Mylog10(in long v, out int Mylog)
        {
            Mylog = (v >= 1000000000000000000) ? 18 :
                (v >= 100000000000000000) ? 17 : (v >= 10000000000000000) ? 16 :
                (v >= 1000000000000000) ? 15 : (v >= 100000000000000) ? 14 :
                (v >= 10000000000000) ? 13 : (v >= 1000000000000) ? 12 :
                (v >= 100000000000) ? 11 : (v >= 10000000000) ? 10 :
                (v >= 1000000000) ? 9 : (v >= 100000000) ? 8 :
                (v >= 10000000) ? 7 : (v >= 1000000) ? 6 :
                (v >= 100000) ? 5 : (v >= 10000) ? 4 :
                (v >= 1000) ? 3 : (v >= 100) ? 2 :
                (v >= 10) ? 1 : 0;
        }
        public static void LongPower(in long MyBase, in int MyPower, out long MyLongPower)
        {
            if (MyPower == 0) { MyLongPower = 1; return; }

            long MyResult = MyBase;

            for (int i = 1; i < MyPower; i++)
            {
                MyResult *= MyBase;
            }
            MyLongPower = MyResult;

        }
        public static void IntAbs(in int Dig, out int MyAbs)
        {
            //NOT USED
            MyAbs = (Dig + (Dig >> 31)) ^ (Dig >> 31);
        }


        public static void DecimalPower(in decimal MyBase, in int MyPower, out decimal MyDecimalPower)
        {
            if (MyPower == 0) { MyDecimalPower = 1; return; }
            decimal MyResult = MyBase;

            for (int i = 1; i < MyPower; i++)
            {
                MyResult *= MyBase;
            }
            MyDecimalPower = MyResult;

        }
        public static void MyModuloIntToInt(in int Dig, in int Divider, out int Full, out int MyMod)
        {
            if (Dig > Divider) { Full = (Dig / Divider); MyMod = Dig - (Full * Divider); return; }
            else if (Dig < Divider) { Full = 0; MyMod = Dig; return; }
            else { Full = 1; MyMod = 0; return; }
        }
        public static void MyModuloLongToLong(in long Dig, in long Divider, out long Full, out long MyMod)
        {

            if (Dig > Divider) { Full = (Dig / Divider); MyMod = Dig - (Full * Divider); return; }
            else if (Dig < Divider) { Full = 0; MyMod = Dig; return; }
            else { Full = 1; MyMod = 0; return; }
        }

        public static void MyModuloDecToLong(in decimal Dig, in long Divider, out long Full, out long MyMod)
        {
            if (Dig > Divider) { Full = (long)(Dig / Divider); MyMod = (long)(Dig - ((decimal)Full * Divider)); return; }
            else if (Dig < Divider) { Full = 0; MyMod = (long)Dig; return; }
            else { Full = 1; MyMod = 0; return; }
        }

        public static void MyModuloDecToDec(in decimal Dig, in decimal Divider, out decimal Full, out decimal MyMod)
        {

            if (Dig > Divider) { Full = decimal.Truncate(Dig / Divider); MyMod = Dig - (Full * Divider); return; }
            else if (Dig < Divider) { Full = 0; MyMod = Dig; return; }
            else { Full = 1; MyMod = 0; return; }
        }
    }

    public static class CalcStringExt
    {
        public static string Left(this string @this, int count)
        {
            if (@this.Length <= count)
            {
                return @this;
            }
            else
            {
                return @this[..count];
            }
        }
        public static string Right(this string input, int count)
        {
            return input.Substring(Math.Max(input.Length - count, 0), Math.Min(count, input.Length));
        }

        public static string Mid(this string input, int start, int count)
        {
            return input.Substring(Math.Min(start, input.Length), Math.Min(count, Math.Max(input.Length - start, 0)));
        }

        public static string SplitString(string DigString)
        {
            string TempString = DigString;
            TempString.Replace(" ", "");
            for (int i = DigString.Length - 3; i >= 0; i -= 3)
            {
                TempString = TempString.Insert(i, " ");
            }
            if (TempString[..1] == " ")
            { TempString = TempString[1..]; }
            return TempString;
        }

        public static string DeSplitString(string DigString)
        {
            string TempString = DigString.Replace(" ", "");
            return TempString;
        }

        public static string MyPadLeft(int LongestString, string StringToPad)
        {
            int PlacesToPad = LongestString - StringToPad.Length;
            return StringToPad.PadLeft(LongestString);
        }

    }

    public static class CalcLists
    {




        public static List<long> Add(in List<long> Dig1List, in List<long> Dig2List)
        {
            int Dig1ListCount = Dig1List.Count;
            int Dig2ListCount = Dig2List.Count;
            int MaxLoops = Dig1ListCount;
            int MinLoops = Dig2ListCount;
            int BiggerDig = 0;


            if (Dig1ListCount < Dig2ListCount)
            {
                MaxLoops = Dig2ListCount;
                MinLoops = Dig1ListCount;
                BiggerDig = 2;
            }

            if (Dig1ListCount > Dig2ListCount)
            { BiggerDig = 1; }

            int i = 0;
            long tempUp = 0;
            long temp;

            List<long> MyOutput = new(MaxLoops);

            while (true)
            {
                if (i < MinLoops)
                {
                    temp = Dig1List[i] + Dig2List[i] + tempUp;

                    if (temp >= 1000000000000000000)
                    {
                        tempUp = 1;
                        temp -= 1000000000000000000;
                        MyOutput.Add(temp);
                        if (i == MaxLoops - 1)
                        {
                            MyOutput.Add(tempUp);
                            break;
                        }
                    }
                    else
                    {
                        tempUp = 0;
                        MyOutput.Add(temp);
                    }
                }
                else
                {
                    if (tempUp == 0)
                    {
                        if (BiggerDig == 1)
                        {
                            MyOutput.AddRange(Dig1List[i..]); break;
                        }
                        else if (BiggerDig == 2)
                        {
                            MyOutput.AddRange(Dig2List[i..]); break;
                        }
                        else // if (BiggerDig == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (BiggerDig == 1)
                        {
                            temp = Dig1List[i] + tempUp;
                        }
                        else
                        {
                            temp = Dig2List[i] + tempUp;
                        }

                        if (temp >= 1000000000000000000)
                        {
                            tempUp = 1;
                            temp -= 1000000000000000000;
                            MyOutput.Add(temp);
                            if (i == MaxLoops - 1)
                            {
                                MyOutput.Add(tempUp);
                                break;
                            }
                        }
                        else
                        {
                            tempUp = 0;
                            MyOutput.Add(temp);
                        }
                    }
                }
                i++;
            }
            return MyOutput;
        }



        public static List<long> Sub(in List<long> Dig1List, in List<long> Dig2List)
        {

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }


            CalcCompare.ListBigger(in Dig1List, in Dig1ListCount, in Dig2List, Dig2ListCount, out int DigBigger);


            if (DigBigger == 0)
            { return new List<long>(1) { 0 }; }

            int i = 0;
            int debt = 0;
            long temp;
            List<long> MyOutput;
            int OutputCount = 0; // Full MyOutput.Count
            int OutputPos = 0; // Last MyOutput[#]>0

            if (DigBigger == 1)
            {
                MyOutput = new(Dig1ListCount);
                while (true)
                {
                    if (i < Dig2ListCount)
                    {
                        temp = (Dig1List[i] - Dig2List[i]) - debt;
                        if (temp > 0)
                        {
                            debt = 0;
                            MyOutput.Add(temp);
                            OutputCount++;
                            OutputPos = OutputCount;
                        }//
                        else if (temp < 0)
                        {
                            debt = 1;
                            MyOutput.Add(temp + 1000000000000000000);
                            OutputCount++;
                            OutputPos = OutputCount;
                        }//
                        else
                        {
                            debt = 0;
                            if (i == Dig1ListCount - 1)
                            {
                                //if (MyOutput[^1] == 0) //
                                //{ }
                                break;
                            }
                            else
                            {
                                MyOutput.Add(temp);
                                OutputCount++;
                            }

                        }//
                    }
                    else
                    {
                        if (debt == 0)
                        {
                            if (i < Dig1ListCount)
                            {
                                MyOutput.AddRange(Dig1List[i..]);
                                OutputPos += Dig1ListCount - i;
                            }

                            //if (MyOutput[^1] == 0)//
                            //{ }
                            break;
                        }
                        else
                        {
                            temp = Dig1List[i] - debt;
                            if (temp > 0)
                            {
                                debt = 0;
                                MyOutput.Add(temp);
                                OutputCount++;
                                OutputPos = OutputCount;
                            }//
                            else if (temp < 0)
                            {
                                debt = 1;
                                MyOutput.Add(temp + 1000000000000000000);
                                OutputCount++;
                                OutputPos = OutputCount;
                            }//
                            else
                            {
                                debt = 0;
                                MyOutput.Add(temp);
                                OutputCount++;
                            }//
                        }
                    }
                    i++;
                }
            }
            else
            {
                MyOutput = new(Dig2ListCount);
                while (true)
                {
                    if (i < Dig1ListCount)
                    {
                        temp = (Dig2List[i] - Dig1List[i]) - debt;
                        if (temp > 0)
                        {
                            debt = 0;
                            MyOutput.Add(temp);
                            OutputCount++;
                            OutputPos = OutputCount;
                        }//
                        else if (temp < 0)
                        {
                            debt = 1;
                            MyOutput.Add(temp + 1000000000000000000);
                            OutputCount++;
                            OutputPos = OutputCount;
                        }//
                        else
                        {
                            debt = 0;
                            if (i == Dig2ListCount - 1)
                            {
                                //if (MyOutput[^1] == 0) //
                                //{ }
                                break;
                            }
                            else
                            { MyOutput.Add(temp); OutputCount++; }
                        }//
                    }
                    else
                    {
                        if (debt == 0)
                        {
                            if (i < Dig2ListCount)
                            {
                                MyOutput.AddRange(Dig2List[i..]);
                                OutputPos += Dig2ListCount - i;
                            }

                            //if (MyOutput[^1] == 0) //Cant be????
                            //{ }
                            break;
                        }
                        else
                        {
                            temp = Dig2List[i] - debt;
                            if (temp > 0)
                            {
                                debt = 0;
                                MyOutput.Add(temp);
                                OutputCount++;
                                OutputPos = OutputCount;
                            }//
                            else if (temp < 0)
                            {
                                debt = 1;
                                MyOutput.Add(temp + 1000000000000000000);
                                OutputCount++;
                                OutputPos = OutputCount;
                            }//
                            else
                            {
                                debt = 0;
                                MyOutput.Add(temp);
                                OutputCount++;
                            }//
                        }
                    }
                    i++;
                }
            }

            /*
            if (MyOutput[^1] == 0 && MyOutput.Count > 1)
            {
                MyOutput.RemoveAt(MyOutput.Count - 1);

                while (MyOutput[^1] == 0 && MyOutput.Count > 1)
                {
                    MyOutput.RemoveAt(MyOutput.Count - 1);

                }
            }
            //return MyOutput;
            */

            if (MyOutput[^1] == 0 && OutputCount > 1)
            { return MyOutput[..(OutputPos)]; }
            else
            { return MyOutput; }


        }


        public static List<long> SubOld(in List<long> Dig1List, in List<long> Dig2List)
        {

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }

            CalcCompare.ListBigger(in Dig1List, in Dig1ListCount, in Dig2List, Dig2ListCount, out int DigBigger);

            if (DigBigger == 0)
            { return new List<long>(1) { 0 }; }

            int i = 0;
            int debt = 0;
            long temp;
            List<long> MyOutput;
            int OutputCount = 0;
            int OutputPos = 0;

            if (DigBigger == 1)
            {
                //int MaxLoops = Dig1ListCount;
                //int MinLoops = Dig2ListCount;
                MyOutput = new(Dig1ListCount);
                while (true)
                {
                    if (i < Dig2ListCount)
                    {
                        temp = (Dig1List[i] - Dig2List[i]) - debt;
                        if (temp < 0)
                        { }

                        if (temp < 0)
                        {
                            debt = 1;
                            MyOutput.Add(temp + 1000000000000000000);
                            OutputCount++;
                        }
                        else
                        {
                            debt = 0;
                            if (temp > 0)
                            { MyOutput.Add(temp); OutputCount++; }
                            else
                            {
                                if (i == Dig1ListCount - 1)
                                {
                                    if (MyOutput[^1] == 0) //
                                    { }
                                    break;
                                }
                                else
                                { MyOutput.Add(temp); OutputCount++; }
                            }

                        }
                    }
                    else
                    {
                        if (debt == 0)
                        {
                            if (i < Dig1ListCount)
                            {
                                //var aaa = Dig1List[i..];
                                MyOutput.AddRange(Dig1List[i..]);
                            }

                            if (MyOutput[^1] == 0)//
                            { }
                            break;
                        }
                        else
                        {
                            temp = Dig1List[i] - debt;
                            if (temp < 0)
                            {
                                debt = 1;
                                MyOutput.Add(temp + 1000000000000000000);
                                OutputCount++;
                            }
                            else
                            {
                                debt = 0;
                                MyOutput.Add(temp);
                                OutputCount++;
                            }
                        }
                    }
                    i++;
                }
            }
            else
            {
                //int MaxLoops = Dig2ListCount;
                //int MinLoops = Dig1ListCount;
                MyOutput = new(Dig2ListCount);
                while (true)
                {
                    if (i < Dig1ListCount)
                    {
                        temp = (Dig2List[i] - Dig1List[i]) - debt;
                        if (temp < 0)
                        { }

                        if (temp < 0)
                        {
                            debt = 1;
                            MyOutput.Add(temp + 1000000000000000000);
                            OutputCount++;
                        }
                        else
                        {
                            debt = 0;
                            if (temp > 0)
                            { MyOutput.Add(temp); OutputCount++; }
                            else
                            {
                                if (i == Dig2ListCount - 1)
                                {
                                    if (MyOutput[^1] == 0) //
                                    { }
                                    break;
                                }
                                else
                                { MyOutput.Add(temp); OutputCount++; }
                            }

                        }
                    }
                    else
                    {
                        if (debt == 0)
                        {
                            if (i < Dig2ListCount)
                            {
                                MyOutput.AddRange(Dig2List[i..]);
                            }

                            if (MyOutput[^1] == 0) //Nie występuje????
                            { }
                            break;
                        }
                        else
                        {
                            temp = Dig2List[i] - debt;

                            if (temp < 0)
                            {
                                debt = 1;
                                MyOutput.Add(temp + 1000000000000000000);
                                OutputCount++;
                            }
                            else
                            {
                                debt = 0;
                                MyOutput.Add(temp);
                                OutputCount++;
                            }
                        }
                    }
                    i++;
                }
            }

            if (MyOutput[^1] == 0 && MyOutput.Count > 1)
            {
                MyOutput.RemoveAt(MyOutput.Count - 1);

                while (MyOutput[^1] == 0 && MyOutput.Count > 1)
                {
                    MyOutput.RemoveAt(MyOutput.Count - 1);

                }
            }

            return MyOutput;
        }

        public static List<long> Mul(in List<long> Dig1List, in List<long> Dig2List)
        {
            //list of 18


            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1)
            {
                if (Dig1List[0] == 1)
                { return Dig2List; }
                else if (Dig1List[0] == 0)
                { return Dig1List; }

            }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1)
            {
                if (Dig2List[0] == 1)
                { return Dig1List; }
                else if (Dig2List[0] == 0)
                { return Dig2List; }
            }

            int MyPoss = 0;
            //long tempUp;

            int ResultCount = (Dig1ListCount + Dig2ListCount) - 1;

            List<long> ResultList = new(Dig1ListCount + Dig2ListCount);
            //ResultList = new(new long[ResultCount]); // Product (ResultList) can be max Dig1List.Count + Dig2List.Count digit long or 1 less
            ResultList.AddRange(new long[ResultCount]);
            for (int i = 0; i < Dig2ListCount; i++)
            {
                long Dig2ListI = Dig2List[i];
                for (int k = 0; k < Dig1ListCount; k++)
                {
                    long Dig1ListK = Dig1List[k];
                    if (Dig1ListK > 999999999 && Dig2ListI > 999999999) //ok, max 36dig
                    {
                        CalcIntExt.MyModuloLongToLong(Dig1ListK, 1000000000, out long Dig1ListU, out long Dig1ListD);
                        CalcIntExt.MyModuloLongToLong(Dig2ListI, 1000000000, out long Dig2ListU, out long Dig2ListD);
                        long Frag1 = Dig2ListD * Dig1ListD; //max 999999998000000001
                        long Frag2 = (Dig2ListD * Dig1ListU) + (Dig2ListU * Dig1ListD); //max 1999999996000000002
                        long Frag3 = Dig2ListU * Dig1ListU; //max 999999998000000001
                        CalcIntExt.MyModuloLongToLong(Frag2, 1000000000, out long ToFrag3, out long ToFrag1);
                        Frag1 += ToFrag1 * 1000000000; //in worse case ToFrag1=999999999 => Frag1=1999999997000000001

                        if (Frag1 > 999999999999999999) // so we can simplify, instead use modulo
                        {
                            Frag1 -= 1000000000000000000; //max 18dig
                            Frag3 += ToFrag3 + 1; //max 18dig
                        }
                        else
                        {
                            Frag3 += ToFrag3;
                        }

                        Frag1 += ResultList[MyPoss]; //ResultList[MyPoss] is tempUp in worse case 1999999999999999998 if we add 999999999999999999, we have 29...........
                        if (Frag1 > 999999999999999999) //temp
                        {
                            CalcIntExt.MyModuloLongToLong(Frag1, 1000000000000000000, out ToFrag3, out Frag1); //so we must use modulo
                            //Frag1 -= 1000000000000000000;
                            //if (ToFrag3 >= 3) //1, 2
                            //{ }
                            Frag3 += ToFrag3; //tempUp
                        }
                        ResultList[MyPoss] = Frag1; //Always

                        if (MyPoss + 1 == ResultCount)
                        {
                            if (Frag3 > 0) { ResultList.Add(Frag3); } //at the end if is something to add, we add, it is always 18dig or less
                        }
                        else
                        {
                            ResultList[MyPoss + 1] += Frag3; //tempUp, here we don't validate if it is 18digits, so if we add two 18dig numbers it can be 19dig number eg. 18..........
                        }

                    }
                    else if (Dig1ListK > 999999999 && Dig2ListI < 1000000000) //ok, max 27dig
                    {

                        CalcIntExt.MyModuloLongToLong(Dig1ListK, 1000000000, out long Dig1ListU, out long Dig1ListD);
                        long Frag1 = Dig2ListI * Dig1ListD;//max 999999998000000001
                        long Frag3 = Dig2ListI * Dig1ListU;//max 999999998000000001
                        CalcIntExt.MyModuloLongToLong(Frag3, 1000000000, out long ToFrag3, out long ToFrag1); //ToFrag3 max 9dig

                        Frag1 += ToFrag1 * 1000000000; //in worse case ToFrag1=999999999 => Frag1=1999999997000000001

                        if (Frag1 > 999999999999999999) //as above, simplify
                        {
                            Frag1 -= 1000000000000000000; //max 18dig
                            Frag3 = ToFrag3 + 1; //max 9dig
                        }
                        else
                        {
                            Frag3 = ToFrag3;
                        }

                        Frag1 += ResultList[MyPoss];
                        if (Frag1 > 999999999999999999) //temp
                        {
                            //CalcIntExt.MyModuloLongToLong(Frag1, 1000000000000000000, out ToFrag3, out Frag1);
                            Frag1 -= 1000000000000000000;
                            //if (ToFrag3 >= 2) //1, 
                            //{ }
                            //Frag3 += ToFrag3; //tempUp
                            Frag3++;
                        }
                        ResultList[MyPoss] = Frag1; //Always

                        if (MyPoss + 1 == ResultCount)
                        {
                            if (Frag3 > 0) { ResultList.Add(Frag3); }
                        }
                        else
                        {
                            ResultList[MyPoss + 1] += Frag3;
                        }

                    }
                    else if (Dig1ListK < 1000000000 && Dig2ListI < 1000000000) //ok???
                    {
                        long Frag1 = Dig1ListK * Dig2ListI;
                        long Frag3 = 0;

                        Frag1 += ResultList[MyPoss];
                        /*
                        if (Frag1 > 999999999999999999) //temp
                        {
                            CalcIntExt.MyModuloLongToLong(Frag1, 1000000000000000000, out long ToFrag3, out Frag1);
                            //Frag1 -= 1000000000000000000;
                            if (ToFrag3 >= 1)
                            { }
                            Frag3 += ToFrag3; //tempUp
                        }
                        */
                        ResultList[MyPoss] = Frag1; //Always

                        if (MyPoss + 1 == ResultCount)
                        {
                            if (Frag3 > 0) { ResultList.Add(Frag3); }
                        }
                        else
                        {
                            ResultList[MyPoss + 1] += Frag3;
                        }
                    }
                    else if (Dig1ListK < 1000000000 && Dig2ListI > 999999999)
                    {
                        CalcIntExt.MyModuloLongToLong(Dig2ListI, 1000000000, out long Dig2ListU, out long Dig2ListD);
                        long Frag1 = Dig2ListD * Dig1ListK;
                        long Frag3 = Dig2ListU * Dig1ListK;

                        CalcIntExt.MyModuloLongToLong(Frag3, 1000000000, out long ToFrag3, out long ToFrag1);
                        Frag1 += ToFrag1 * 1000000000;

                        if (Frag1 > 999999999999999999)
                        {
                            Frag1 -= 1000000000000000000;
                            Frag3 = ToFrag3 + 1;
                        }
                        else
                        {
                            Frag3 = ToFrag3;
                        }

                        Frag1 += ResultList[MyPoss];
                        if (Frag1 > 999999999999999999) //temp
                        {
                            //CalcIntExt.MyModuloLongToLong(Frag1, 1000000000000000000, out ToFrag3, out Frag1);
                            Frag1 -= 1000000000000000000;
                            //if (ToFrag3 >= 2) //1,
                            //{ }
                            //Frag3 += ToFrag3; //tempUp
                            Frag3++;
                        }
                        ResultList[MyPoss] = Frag1; //Always

                        if (MyPoss + 1 == ResultCount)
                        {
                            if (Frag3 > 0) { ResultList.Add(Frag3); }
                        }
                        else
                        {
                            ResultList[MyPoss + 1] += Frag3;
                        }
                    }
                    MyPoss += 1;
                }
                MyPoss = i + 1;
            }

            return ResultList;

        }

        public static List<long> MulGood(in List<long> Dig1List, in int Dig1ListFrag, in List<long> Dig2List, in int Dig2ListFrag, in int ProductFrag)
        {
            //list of 18

            List<long> MyOutput;
            List<long> ResultList = [];
            int IsResult = 0;
            int OutFrag = 0;

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1)
            {
                if (Dig1List[0] == 1)
                { ResultList = Dig2List; IsResult = 1; OutFrag = Dig2ListFrag; }
                else if (Dig1List[0] == 0)
                { ResultList = Dig1List; IsResult = 1; OutFrag = Dig1ListFrag; }

            }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1)
            {
                if (Dig2List[0] == 1)
                { ResultList = Dig1List; IsResult = 1; OutFrag = Dig1ListFrag; }
                else if (Dig2List[0] == 0)
                { ResultList = Dig2List; IsResult = 1; OutFrag = Dig2ListFrag; }
            }

            if (IsResult == 0)
            {
                List<long> Dig1List9;
                List<long> Dig2List9;
                if (Dig1ListFrag == 18)
                { CalcConvert.ConvertList18To9(in Dig1List, out Dig1List9); Dig1ListCount = Dig1List9.Count; }
                else
                { Dig1List9 = Dig1List; }
                if (Dig2ListFrag == 18)
                { CalcConvert.ConvertList18To9(in Dig2List, out Dig2List9); Dig2ListCount = Dig2List9.Count; }
                else
                { Dig2List9 = Dig2List; }

                int MyPoss = 0;
                long tempUp;

                int ResultCount = (Dig1ListCount + Dig2ListCount) - 1;
                ResultList = new(new long[ResultCount]); // Product (ResultList) can be max Dig1List.Count + Dig2List.Count digit long or 1 less

                if (Dig1ListCount < Dig2ListCount)
                {
                    for (int i = 0; i < Dig2ListCount; i++)
                    {
                        long Dig2List9I = Dig2List9[i];
                        for (int k = 0; k < Dig1ListCount; k++)
                        {
                            long temp = (Dig1List9[k] * Dig2List9I) + ResultList[MyPoss];
                            if (temp >= 1000000000)
                            {
                                CalcIntExt.MyModuloLongToLong(temp, 1000000000, out tempUp, out temp);

                                if (MyPoss + 1 == ResultCount)
                                { ResultList.Add(tempUp); }
                                else
                                {
                                    ResultList[MyPoss + 1] += tempUp;
                                }
                            }

                            ResultList[MyPoss] = temp;

                            MyPoss += 1;
                        }

                        MyPoss = i + 1;
                    }

                    OutFrag = 9;

                }
                else
                {

                    for (int i = 0; i < Dig2ListCount; i++)
                    {
                        long Dig2List9I = Dig2List9[i];
                        for (int k = 0; k < Dig1ListCount; k++)
                        {
                            long temp = (Dig2List9I * Dig1List9[k]) + ResultList[MyPoss];
                            if (temp >= 1000000000)
                            {
                                CalcIntExt.MyModuloLongToLong(temp, 1000000000, out tempUp, out temp);

                                if (MyPoss + 1 == ResultCount)
                                { ResultList.Add(tempUp); }
                                else
                                {
                                    ResultList[MyPoss + 1] += tempUp;
                                }
                            }

                            ResultList[MyPoss] = temp;

                            MyPoss += 1;
                        }

                        MyPoss = i + 1;
                    }

                    OutFrag = 9;
                }
            }

            if (ProductFrag != OutFrag)
            {
                if (ProductFrag == 18 && OutFrag == 9)
                { CalcConvert.ConvertList9To18(in ResultList, out MyOutput); }
                else
                { CalcConvert.ConvertList18To9(in ResultList, out MyOutput); }
            }
            else
            { MyOutput = ResultList; }

            return MyOutput;

        }

        public static List<long> MulNewConcept(in List<long> Dig1List, in int Dig1ListFrag, in List<long> Dig2List, in int Dig2ListFrag, in int ProductFrag)
        {
            //list of 18

            List<long> MyOutput;
            List<long> ResultList = [];
            int IsResult = 0;
            int OutFrag = 0;

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1)
            {
                if (Dig1List[0] == 1)
                { ResultList = Dig2List; IsResult = 1; OutFrag = Dig2ListFrag; }
                else if (Dig1List[0] == 0)
                { ResultList = Dig1List; IsResult = 1; OutFrag = Dig1ListFrag; }

            }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1)
            {
                if (Dig2List[0] == 1)
                { ResultList = Dig1List; IsResult = 1; OutFrag = Dig1ListFrag; }
                else if (Dig2List[0] == 0)
                { ResultList = Dig2List; IsResult = 1; OutFrag = Dig2ListFrag; }
            }


            if (IsResult == 0)
            {
                List<long> Dig1List9;
                List<long> Dig2List9;
                /*
                if (Dig1ListFrag == 18)
                { CalcConvert.ConvertList18To9(in Dig1List, out Dig1List9); Dig1ListCount = Dig1List9.Count; }
                else
                { Dig1List9 = Dig1List; }
                if (Dig2ListFrag == 18)
                { CalcConvert.ConvertList18To9(in Dig2List, out Dig2List9); Dig2ListCount = Dig2List9.Count; }
                else
                { Dig2List9 = Dig2List; }
                */
                int MyPoss = 0;
                long tempUp;

                int ResultCount = (Dig1ListCount + Dig2ListCount) - 1;
                //ResultList = new(new long[ResultCount]); // Product (ResultList) can be max Dig1List.Count + Dig2List.Count digit long or 1 less

                int ResultListCountN = (Dig1ListCount + Dig2ListCount) - 1;
                //int ResultListCountRev = ResultListCountN;
                ResultList = new(ResultListCountN + 1);
                int d2 = Dig2ListCount - 1;
                int d1 = Dig1ListCount - 1;
                int d1w = 0;
                int d2w = 0;
                Console.WriteLine("Start:   " + Dig1ListCount + "    " + Dig2ListCount);
                long PartPosUp = 0;
                for (int s = 0; s < ResultListCountN; s++)
                {

                    if (s > d1)
                    { d1w = d1; d2w = s - d1; }
                    else
                    { d1w = s; d2w = 0; }
                    Console.WriteLine("---------------------");
                    long PartPosResult = PartPosUp;
                    PartPosUp = 0;

                    while (true)
                    {
                        ////////


                        Console.WriteLine(d1w + "    " + d2w);
                        if (Dig1List[d1w] > 999999999 && Dig2List[d2w] > 999999999)
                        {
                            CalcIntExt.MyModuloLongToLong(Dig1List[d1w], 1000000000, out long d1wU, out long d1wD);
                            CalcIntExt.MyModuloLongToLong(Dig2List[d2w], 1000000000, out long d2wU, out long d2wD);


                            long d1wDxd2wD = d1wD * d2wD;
                            long d1wUxd2wD = d1wU * d2wD;
                            long d1wDxd2wU = d1wD * d2wU;
                            long d1wUxd2wU = d1wU * d2wU;

                            long TempSum = d1wUxd2wD + d1wDxd2wU;
                            if (TempSum >= 1000000000000000000)
                            { TempSum -= 1000000000000000000; d1wUxd2wU++; }
                            CalcIntExt.MyModuloLongToLong(TempSum, 1000000000, out long TempSumU, out long TempSumD);

                            long LastSumD = (TempSumD * 1000000000) + d1wDxd2wD;
                            long LastSumU = TempSumU + d1wUxd2wU;
                            PartPosResult = LastSumD;
                            PartPosUp = LastSumU;

                        }
                        else if (Dig1List[d1w] < 1000000000 && Dig2List[d2w] < 1000000000)
                        {
                            PartPosResult += (Dig1List[d1w] * Dig2List[d2w]);
                            if (PartPosResult > 999999999999999999)
                            {
                                CalcIntExt.MyModuloLongToLong(PartPosResult, 1000000000000000000, out long PartPosResultU, out long PartPosResultD);
                                PartPosResult += PartPosResultD;
                                PartPosUp += PartPosResultU;
                            }
                        }
                        else if (Dig1List[d1w] > 999999999 && Dig2List[d2w] < 1000000000)
                        { }
                        else if (Dig1List[d1w] < 1000000000 && Dig2List[d2w] > 999999999)
                        {
                            CalcIntExt.MyModuloLongToLong(Dig2List[d2w], 1000000000, out long d2wU, out long d2wD);

                            PartPosResult = (Dig1List[d1w] * Dig2List[d2w]);
                        }



                        ////////
                        d2w++;
                        d1w--;

                        if (d2w > s || d2w > d2)
                        { break; }

                        if (d1w + d2w > s)
                        { break; }
                    }
                    ResultList.Add(PartPosResult);
                    //Console.WriteLine("--------------");
                }


            }
            /*
            if (ProductFrag != OutFrag)
            {
                if (ProductFrag == 18 && OutFrag == 9)
                { CalcConvert.ConvertList9To18(in ResultList, out MyOutput); }
                else
                { CalcConvert.ConvertList18To9(in ResultList, out MyOutput); }
            }
            else
            { MyOutput = ResultList; }
            */
            MyOutput = ResultList;
            return MyOutput;

        }
        /*
        public static List<long> MulNewOn9(in List<long> Dig1List, in int Dig1ListFrag, in List<long> Dig2List, in int Dig2ListFrag, in int ProductFrag)
        {
            //list of 18

            List<long> MyOutput;
            List<long> ResultList = [];
            int IsResult = 0;
            int OutFrag = 0;

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1)
            {
                if (Dig1List[0] == 1)
                { ResultList = Dig2List; IsResult = 1; OutFrag = Dig2ListFrag; }
                else if (Dig1List[0] == 0)
                { ResultList = Dig1List; IsResult = 1; OutFrag = Dig1ListFrag; }

            }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1)
            {
                if (Dig2List[0] == 1)
                { ResultList = Dig1List; IsResult = 1; OutFrag = Dig1ListFrag; }
                else if (Dig2List[0] == 0)
                { ResultList = Dig2List; IsResult = 1; OutFrag = Dig2ListFrag; }
            }

            if (IsResult == 0)
            {
                List<long> Dig1List9;
                List<long> Dig2List9;
                if (Dig1ListFrag == 18)
                { CalcConvert.ConvertList18To9(in Dig1List, out Dig1List9); Dig1ListCount = Dig1List9.Count; }
                else
                { Dig1List9 = Dig1List; }
                if (Dig2ListFrag == 18)
                { CalcConvert.ConvertList18To9(in Dig2List, out Dig2List9); Dig2ListCount = Dig2List9.Count; }
                else
                { Dig2List9 = Dig2List; }

                int MyPoss = 0;
                long tempUp;

                int ResultCount = (Dig1ListCount + Dig2ListCount) - 1;
                ResultList = new(new long[ResultCount]); // Product (ResultList) can be max Dig1List.Count + Dig2List.Count digit long or 1 less

                int ResultListCountN = (Dig1ListCount + Dig2ListCount) - 1;
                //int ResultListCountRev = ResultListCountN;
                ResultList = new(ResultListCountN + 1);
                int d2 = Dig2ListCount - 1;
                int d1 = Dig1ListCount - 1;
                int d1w = 0;
                int d2w = 0;
                Console.WriteLine("Start:   " + Dig1ListCount + "    " + Dig2ListCount);

                for (int s = 0; s < ResultListCountN; s++)
                {

                    if (s > d1)
                    { d1w = d1; d2w = s - d1; }
                    else
                    { d1w = s; d2w = 0; }
                    Console.WriteLine("---------------------");
                    while (true)
                    {


                        Console.WriteLine(d1w + "    " + d2w);


                        d2w++;
                        d1w--;

                        if (d2w > s || d2w > d2)
                        { break; }

                        if (d1w + d2w > s)
                        { break; }
                    }

                    //Console.WriteLine("--------------");
                }


            }

            if (ProductFrag != OutFrag)
            {
                if (ProductFrag == 18 && OutFrag == 9)
                { CalcConvert.ConvertList9To18(in ResultList, out MyOutput); }
                else
                { CalcConvert.ConvertList18To9(in ResultList, out MyOutput); }
            }
            else
            { MyOutput = ResultList; }

            return MyOutput;

        }
        */
        public static List<List<long>> Div(in List<long> Dig1ListOrig, in List<long> Dig2List, in int CheckDL = 0)
        {
            List<long> Dig1List = Dig1ListOrig;
            //Best with MySafeMultiplier, long digs, long multiplier
            int Dig1ListCount = Dig1List.Count;
            int Dig2ListCount = Dig2List.Count;
            if (Dig1ListCount < Dig2ListCount) // fast pass
            {
                //List<List<long>> MyOutput1 = new(2) { new List<long>(1) { 0 }, Dig1List };
                return new(2) { new List<long>(1) { 0 }, Dig1List };
            }

            if (Dig1ListCount == 1 && Dig2ListCount == 1) // sometimes we can simply divide two longs - fast pass
            {
                if (Dig2List[0] != 0)
                {
                    CalcIntExt.MyModuloLongToLong(Dig1List[0], Dig2List[0], out long Full, out long MyMod);
                    //List<List<long>> MyOutput1 = new(2) { new List<long>(1) { Full }, new List<long>(1) { MyMod } };
                    return new(2) { new List<long>(1) { Full }, new List<long>(1) { MyMod } };
                }
                else
                {
                    //List<List<long>> MyOutput1 = new(2) { new List<long>(1) { 0 }, new List<long>(1) { 0 } };
                    Console.WriteLine("ERROR - Div by 0 - Return: 0");
                    return new(2) { new List<long>(1) { 0 }, new List<long>(1) { 0 } };
                }
            }


            //int Dig2ListFirstLength;
            //int Dig2ListLength;

            //int Dig1ListFirstLength;
            //int Dig1ListLength;

            int BiggerList = 1;
            long Dig1FirstDig = Dig1List[^1];
            CalcIntExt.LongLength(Dig2List[^1], out int Dig2ListFirstLength); //static
            int Dig2ListLength = (Dig2ListCount - 1) * 18 + Dig2ListFirstLength; //static

            CalcIntExt.LongLength(in Dig1FirstDig, out int Dig1ListFirstLength); //must be counted in each loop and it is necessary here
            int Dig1ListLength = (Dig1ListCount - 1) * 18 + Dig1ListFirstLength; //must be counted in each loop and it is necessary here

            if (Dig1ListLength < Dig2ListLength)
            {
                //MyOutput.Add(new List<long>(1) { 0 });
                //MyOutput.Add(Dig1List);
                //List<List<long>> MyOutput1 = new(2) { new List<long>(1) { 0 }, Dig1List };
                return new(2) { new List<long>(1) { 0 }, Dig1List };
            }

            if (Dig1ListLength == Dig2ListLength)
            {
                CalcCompare.ListBigger(in Dig1List, Dig1List.Count, in Dig2List, in Dig2ListCount, out BiggerList);

                switch (BiggerList)
                {
                    case 0:
                        //MyOutput.Add(new List<long>(1) { 1 });
                        //MyOutput.Add(new List<long>(1) { 0 });
                        //List<List<long>> MyOutput1 = new(2) { new List<long>(1) { 1 }, new List<long>(1) { 0 } };
                        return new(2) { new List<long>(1) { 1 }, new List<long>(1) { 0 } };
                    case 2:
                        //MyOutput.Add(new List<long>(1) { 0 });
                        //MyOutput.Add(Dig1List);
                        //List<List<long>> MyOutput2 = new(2) { new List<long>(1) { 0 }, Dig1List };
                        return new(2) { new List<long>(1) { 0 }, Dig1List };
                }

            }

            if (CheckDL == 1 && Dig1ListLength < 29 && Dig2ListLength < 29)
            {
                CalcCompare.GetDecimalFromList(in Dig1List, out decimal DecimalDig1);
                CalcCompare.GetDecimalFromList(in Dig2List, out decimal DecimalDig2);

                CalcIntExt.MyModuloDecToDec(in DecimalDig1, in DecimalDig2, out decimal FullD, out decimal MyModD);

                List<long> LongResult;
                if (FullD >= 1000000000000000000)
                {
                    CalcIntExt.MyModuloDecToLong(in FullD, 1000000000000000000, out long FullL, out long MyModL);
                    LongResult = new List<long>(2) { MyModL, FullL };
                }
                else
                { LongResult = new List<long>(1) { (long)(FullD) }; }

                List<long> LongRest;
                if (MyModD >= 1000000000000000000)
                {
                    CalcIntExt.MyModuloDecToLong(in FullD, 1000000000000000000, out long FullL, out long MyModL);
                    LongRest = new List<long>(2) { MyModL, FullL };
                }
                else
                { LongRest = new List<long>(1) { (long)(MyModD) }; }

                //List<List<long>> MyOutput1 = new(2) { LongResult, LongRest };
                return new(2) { LongResult, LongRest };
            }

            List<long> TempDig2List18 = new();
            //CalcConvert.ConvertList18To9(in Dig2List, out List<long> Dig2OrigList9); //static, necessary for multiplication

            //prepare number from Dig2 to estimate multiplier, take 17 digits
            CalcCompare.GetLongFromList(in Dig2List, 17, in Dig2ListFirstLength, out long First17DigDig2); //static, necessary to find "safe multiplier"

            if (Dig2ListLength > 17) //if full digit was taken (necessary????)
            {
                First17DigDig2 += 1; //only if we cant't get full divisor

            }

            //List<long> MultiplyList = new() { 0 };
            List<long> MultiplyList = [0];

            while (BiggerList < 2) //division by repeated subtraction
            {
                List<long> SafeMultiplierList18 = new();

                if (CheckDL == 1)
                {
                    if (Dig1ListLength < 19 && Dig2ListLength < 19) // or on longs
                    {
                        long LongDig1 = Dig1List[0];
                        long LongDig2 = Dig2List[0];
                        long LongOutput = LongDig1 / LongDig2;
                        long LongTempDig2 = LongOutput * LongDig2;

                        SafeMultiplierList18 = new List<long>(1) { LongOutput };
                        TempDig2List18 = new List<long>(1) { LongTempDig2 };
                    }
                    else if (Dig1ListLength < 29 && Dig2ListLength < 29)
                    {
                        CalcCompare.GetDecimalFromList(in Dig1List, out decimal DecimalDig1);
                        CalcCompare.GetDecimalFromList(in Dig2List, out decimal DecimalDig2);
                        decimal DecDecOutput = decimal.Truncate((DecimalDig1 / DecimalDig2));
                        if (DecDecOutput >= 1000000000000000000)
                        {
                            CalcIntExt.MyModuloDecToLong(in DecDecOutput, 1000000000000000000, out long Full, out long MyMod);
                            SafeMultiplierList18 = new List<long>(2) { MyMod, Full };
                        }
                        else
                        { SafeMultiplierList18 = new List<long>(1) { (long)(DecDecOutput) }; }

                        decimal DecTempDig2 = DecDecOutput * DecimalDig2;
                        if (DecTempDig2 >= 1000000000000000000)
                        {
                            CalcIntExt.MyModuloDecToLong(in DecTempDig2, 1000000000000000000, out long Full, out long MyMod);
                            TempDig2List18 = new List<long>(2) { MyMod, Full };
                        }
                        else
                        { TempDig2List18 = new List<long>(1) { (long)(DecTempDig2) }; }
                    }
                    else
                    {
                        MySafeMultiplier(in Dig1List, in Dig1FirstDig, in Dig2List, in First17DigDig2, in Dig1ListLength, in Dig1ListFirstLength, in Dig2ListLength, out List<List<long>> MySafeMultiplierResult);
                        TempDig2List18 = MySafeMultiplierResult[0];
                        SafeMultiplierList18 = MySafeMultiplierResult[1];
                    }
                }
                else
                {
                    MySafeMultiplier(in Dig1List, in Dig1FirstDig, in Dig2List, in First17DigDig2, in Dig1ListLength, in Dig1ListFirstLength, in Dig2ListLength, out List<List<long>> MySafeMultiplierResult);
                    TempDig2List18 = MySafeMultiplierResult[0];
                    SafeMultiplierList18 = MySafeMultiplierResult[1];
                }

                Dig1List = CalcLists.Sub(Dig1List, TempDig2List18);
                MultiplyList = CalcLists.Add(MultiplyList, SafeMultiplierList18);
                //And we must check if we can break calculation. Checking is "time waster", it's the best place for it
                Dig1ListCount = Dig1List.Count;
                CalcCompare.ListBigger(in Dig1List, Dig1ListCount, in Dig2List, in Dig2ListCount, out BiggerList);

                if (BiggerList == 2)
                { break; }
                Dig1FirstDig = Dig1List[^1];
                CalcIntExt.LongLength(in Dig1FirstDig, out Dig1ListFirstLength); //must be counted in each loop
                Dig1ListLength = (Dig1ListCount - 1) * 18 + Dig1ListFirstLength; //must be counted in each loop

            }

            //List<List<long>> MyOutput3 = new(2) { MultiplyList, Dig1List };
            return new(2) { MultiplyList, Dig1List };
        }



        public static void MySafeMultiplier(in List<long> Dig1List18, in long Dig1FirstDig, in List<long> Dig2List18, in long First17DigDig2, in int Dig1ListLength, in int Dig1List18FirstLength, in int Dig2ListLength, out List<List<long>> MyOutput)
        {
            List<long> SafeMultiplierList18;

            CalcCompare.GetLongFromList(in Dig1List18, 18, in Dig1List18FirstLength, out long First18DigDig1);

            long SafeMultiplier = First18DigDig1 / First17DigDig2;

            if (Dig2ListLength == Dig1ListLength) // This is necessary.
            {
                //SafeMultiplier /= 10;
                if (SafeMultiplier < 10) { SafeMultiplierList18 = new(1) { 1 }; }
                else { SafeMultiplierList18 = new(1) { (SafeMultiplier /= 10) }; }

            }
            else
            {
                if (SafeMultiplier >= 1000000000000000000) //faster
                {
                    CalcIntExt.MyModuloLongToLong(SafeMultiplier, 1000000000000000000, out long Full, out long MyMod);
                    SafeMultiplierList18 = new(2) { MyMod, Full };
                }
                else
                { SafeMultiplierList18 = new(1) { SafeMultiplier }; }
            }

            List<long> TempDig2List18 = (CalcLists.Mul(Dig2List18, SafeMultiplierList18));
            CalcIntExt.LongLength(TempDig2List18[^1], out int LenToAdd);
            int TempDig2ListLength = (TempDig2List18.Count - 1) * 18 + LenToAdd; //must be counted in each loop

            int ZerosToAdd = Dig1ListLength - TempDig2ListLength;


            if (ZerosToAdd > 0) //if 0 then we multiply by 1
            {
                long Temp2DigFirst = TempDig2List18[^1];
                CalcIntExt.LongLength(in Temp2DigFirst, out int TempDig2List18First);
                CalcCompare.GetLongDigFromDig(in Temp2DigFirst, 1, in TempDig2List18First, out long FirstTempDig2List);
                CalcCompare.GetLongDigFromDig(in Dig1FirstDig, 1, in Dig1List18FirstLength, out long FirstDig1List);

                if (FirstDig1List - FirstTempDig2List < 0)
                { ZerosToAdd -= 1; }

                CalcIntExt.MyModuloIntToInt(in ZerosToAdd, 18, out int ZerosD, out int ZerosU);
                CalcIntExt.LongPower(10, in ZerosU, out long Result);
                List<long> MultiplyBy10List = new(1) { Result };

                List<long> TempDig2List18Temp = CalcLists.Mul(TempDig2List18, MultiplyBy10List);
                //TempDig2List9 = CalcLists.Mul(TempDig2List9, 9, MultiplyBy10List, 9, 9);
                List<long> SafeMultiplierList18Temp = CalcLists.Mul(SafeMultiplierList18, MultiplyBy10List);
                //SafeMultiplierList9 = CalcLists.Mul(SafeMultiplierList9, 9, MultiplyBy10List, 9, 9);
                if (ZerosD > 0)
                {
                    TempDig2List18 = new(TempDig2List18Temp.Count + ZerosD);
                    SafeMultiplierList18 = new(SafeMultiplierList18Temp.Count + ZerosD);
                    //List<long> ZerosAddList = new(ZerosD);
                    long[] ZerosAddList = new long[ZerosD];
                    //for (int t = 0; t < ZerosD; t++)
                    //{
                    //    ZerosAddList.Add(0);
                    //}

                    TempDig2List18.AddRange(ZerosAddList);
                    TempDig2List18.AddRange(TempDig2List18Temp);
                    SafeMultiplierList18.AddRange(ZerosAddList);
                    SafeMultiplierList18.AddRange(SafeMultiplierList18Temp);
                }
                else
                {
                    TempDig2List18 = TempDig2List18Temp;
                    SafeMultiplierList18 = SafeMultiplierList18Temp;

                }
            }

            //CalcConvert.ConvertList9To18(in TempDig2List9, out List<long> TempDig2List18);
            //CalcConvert.ConvertList9To18(in SafeMultiplierList9, out List<long> SafeMultiplierList18);

            MyOutput = new(2) { TempDig2List18, SafeMultiplierList18 };
        }

        public static List<long> Pow(in List<long> Dig1List, in List<long> Dig2List)
        {
            //You can write condition to calculate power with proper result sign
            //or remember, if -Dig1Orig and Dig2Orig % 2 != 0 ResultSign = "-"
            int Dig2ListCount = Dig2List.Count;

            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            {
                //List<long> MyOutput1 = new(1) { 1 };
                return new(1) { 1 };
            }

            List<long> MyOutputList = [];

            if (Dig2ListCount == 1 && Dig2List[0] == 1)
            {
                return Dig1List;
            }

            List<long> MyPowerList = new(1) { 1 };
            List<long> MyPowerListAdd = new(1) { 1 };

            MyOutputList = Dig1List;
            int Result;
            CalcCompare.ListBigger(in MyPowerList, MyPowerList.Count, in Dig2List, Dig2ListCount, out Result);
            while (Result == 2)
            {
                MyOutputList = CalcLists.Mul(MyOutputList, Dig1List);
                MyPowerList = CalcLists.Add(MyPowerList, MyPowerListAdd);
                CalcCompare.ListBigger(in MyPowerList, MyPowerList.Count, in Dig2List, Dig2ListCount, out Result);
            }

            return MyOutputList;

        }
    }
    public class CalcStrings
    {
        public static List<string> Add(string Dig1Orig, string Dig2Orig)
        {

            List<string> MyOutput = new(2);

            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;

            if (Dig1Orig[..1] == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig[1..];
            }
            if (Dig2Orig[..1] == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig[1..];
            }

            if (Dig1 == "0")
            {
                MyOutput.Add(Dig2Orig);
                MyOutput.Add(Dig2);
                return MyOutput;
            }

            if (Dig2 == "0")
            {
                MyOutput.Add(Dig1Orig);
                MyOutput.Add(Dig1);
                return MyOutput;
            }

            if (Dig1.Length < 29 && Dig2.Length < 29)
            {
                decimal IntOutput = Convert.ToDecimal(Dig1Orig) + Convert.ToDecimal(Dig2Orig);
                MyOutput.Add(Convert.ToString(IntOutput));
                MyOutput.Add(Convert.ToString(Math.Abs(IntOutput)));
                return MyOutput;
            }

            CalcConvert.StringsToLongLists(in Dig1, in Dig2, 18, out List<List<long>> DigsLists);

            List<long> Dig1List = DigsLists[0];
            List<long> Dig2List = DigsLists[1];
            CalcCompare.ListBigger(in Dig1List, Dig1List.Count, in Dig2List, Dig2List.Count, out int DigBiggerTemp);
            string TempResult;

            if (Dig1Sign == "-" && Dig2Sign != "-")
            {
                if (DigBiggerTemp == 1)
                { ResultSign = "-"; }
                CalcConvert.LongListToString(CalcLists.Sub(Dig1List, Dig2List), "000000000000000000", out TempResult);
                MyOutput.Add(ResultSign + TempResult);
                MyOutput.Add(TempResult);
                return MyOutput;
            }

            if (Dig1Sign != "-" && Dig2Sign == "-")
            {
                if (DigBiggerTemp == 2)
                { ResultSign = "-"; }
                CalcConvert.LongListToString(CalcLists.Sub(Dig1List, Dig2List), "000000000000000000", out TempResult);
                MyOutput.Add(ResultSign + TempResult);
                MyOutput.Add(TempResult);
                return MyOutput;
            }
            CalcConvert.LongListToString(CalcLists.Add(Dig1List, Dig2List), "000000000000000000", out TempResult);

            if (TempResult == "")
            {
                TempResult = "0";
            }

            if (Dig1Sign == "-" && Dig2Sign == "-")
            { ResultSign = "-"; }

            MyOutput.Add(ResultSign + TempResult);

            return MyOutput;
        }

        public static List<string> Sub(string Dig1Orig, string Dig2Orig)
        {

            List<string> MyOutput = new(3);
            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;

            if (Dig1[..1] == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig[1..];
            }
            if (Dig2[..1] == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig[1..];
            }

            if (Dig1 == "0" && Dig2 == "0")
            {
                MyOutput.Add(Dig2);
                MyOutput.Add(Dig2);
                return MyOutput;
            }

            if (Dig1 == "0")
            {
                if (Dig2Sign == "-")
                {
                    MyOutput.Add(Dig2);
                    MyOutput.Add(Dig2);
                }
                else
                {
                    MyOutput.Add("-" + Dig2);
                    MyOutput.Add(Dig2);
                }
                return MyOutput;
            }

            if (Dig2 == "0")
            {
                MyOutput.Add(Dig1Orig);
                MyOutput.Add(Dig1);
                return MyOutput;
            }

            if (Dig1.Length < 29 && Dig2.Length < 29)
            {
                decimal IntOutput = Convert.ToDecimal(Dig1Orig) - Convert.ToDecimal(Dig2Orig);
                MyOutput.Add(Convert.ToString(IntOutput));
                MyOutput.Add(Convert.ToString(Math.Abs(IntOutput)));
                return MyOutput;
            }
            CalcConvert.StringsToLongLists(in Dig1, in Dig2, 18, out List<List<long>> DigsLists);

            List<long> Dig1List = DigsLists[0];
            List<long> Dig2List = DigsLists[1];
            string TempOutput;
            CalcCompare.ListBigger(in Dig1List, Dig1List.Count, in Dig2List, Dig2List.Count, out int DigBiggerTemp);

            if (Dig1Sign == "-" && Dig2Sign != "-")
            {
                CalcConvert.LongListToString(CalcLists.Add(Dig1List, Dig2List), "000000000000000000", out TempOutput);
                ResultSign = "-";
                MyOutput.Add(ResultSign + TempOutput);
                MyOutput.Add(TempOutput);
                return MyOutput;
            }

            if (Dig1Sign != "-" && Dig2Sign == "-")
            {
                CalcConvert.LongListToString(CalcLists.Add(Dig1List, Dig2List), "000000000000000000", out TempOutput);
                ResultSign = "";
                MyOutput.Add(ResultSign + TempOutput);
                MyOutput.Add(TempOutput);
                return MyOutput;
            }

            if (Dig1Sign != "-" && Dig2Sign != "-")
            {
                if (DigBiggerTemp == 2)
                { ResultSign = "-"; }
            }

            if (Dig1Sign == "-" && Dig2Sign == "-")
            {
                if (DigBiggerTemp == 1)
                { ResultSign = "-"; }
            }
            CalcConvert.LongListToString(CalcLists.Sub(Dig1List, Dig2List), "000000000000000000", out TempOutput);

            if (TempOutput == "")
            { TempOutput = "0"; }

            MyOutput.Add(ResultSign + TempOutput);
            MyOutput.Add(TempOutput);

            return MyOutput;
        }

        public static List<string> Mul(string Dig1Orig, string Dig2Orig)
        {

            List<string> MyOutput = new(3);
            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;

            if (Dig1[..1] == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig[1..];
            }
            if (Dig2[..1] == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig[1..];
            }

            if (Dig2 == "0" || Dig2 == "0")
            {
                MyOutput.Add("0");
                MyOutput.Add("0");
                return MyOutput;
            }

            if (Dig1.Length + Dig2.Length < 29)
            {
                decimal IntOutput = Convert.ToDecimal(Dig1Orig) * Convert.ToDecimal(Dig2Orig);
                MyOutput.Add(Convert.ToString(IntOutput));
                MyOutput.Add(Convert.ToString(Math.Abs(IntOutput)));
                return MyOutput;
            }

            if (Dig1Sign != Dig2Sign)
            {
                ResultSign = "-";
            }
            CalcConvert.StringsToLongLists(in Dig1, in Dig2, 18, out List<List<long>> DigsLists); //9

            List<long> Dig1List = DigsLists[0];
            List<long> Dig2List = DigsLists[1];
            CalcConvert.LongListToString(CalcLists.Mul(Dig1List, Dig2List), "000000000000000000", out string TempOutput); //"000000000"

            if (TempOutput == "")
            {
                TempOutput = "0";
            }

            MyOutput.Add(ResultSign + TempOutput);

            return MyOutput;
        }

        public static List<string> Pow(string Dig1Orig, string Dig2Orig)
        {
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;

            List<string> MyOutput = new(3);

            if (Dig2 == "0")
            {
                MyOutput.Add("1");
                return MyOutput;
            }

            if (Dig2 == "1")
            {
                MyOutput.Add(Dig1Orig);
                return MyOutput;
            }
            CalcConvert.StringToLongList(in Dig1, 18, out List<long> Dig1List);
            CalcConvert.StringToLongList(in Dig2, 18, out List<long> Dig2List);
            List<long> MyOutputList = CalcLists.Pow(Dig1List, Dig2List);

            CalcConvert.LongListToString(in MyOutputList, "000000000000000000", out string Output);// 000000000
            MyOutput.Add(Output);
            return MyOutput;
        }

        public static List<string> Div(string Dig1Orig, string Dig2Orig)
        {
            //most difficult part of job
            List<string> MyOutput = new(3);
            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;
            int Dig1Length = Dig1.Length;
            int Dig2Length = Dig2.Length;

            if (Dig1[..1] == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig[1..];
            }
            if (Dig2[..1] == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig[1..];
            }

            if (Dig2 == "0")
            {
                MyOutput.Add("ERROR Div by 0");
                MyOutput.Add("ERROR Div by 0");
                return MyOutput;
            }

            if (Dig1Sign != Dig2Sign)
            {
                ResultSign = "-";
            }

            if (Dig1Length < Dig2Length)
            {
                MyOutput.Add("0");
                MyOutput.Add(Dig2Sign + Dig1);
                return MyOutput;
            }

            if (Dig2 == "1")
            {
                MyOutput.Add(ResultSign + Dig1);
                MyOutput.Add("0");
                return MyOutput;
            }

            if (Dig1 == "0")
            {
                MyOutput.Add("0");
                MyOutput.Add("0");
                return MyOutput;
            }

            if (Dig1Length < 29 && Dig2Length < 29)
            {
                decimal Dig1Dec = Convert.ToDecimal(Dig1Orig);
                decimal Dig2Dec = Convert.ToDecimal(Dig2Orig);
                CalcIntExt.MyModuloDecToDec(in Dig1Dec, in Dig2Dec, out decimal DecDecOutput, out decimal TempRestDec);
                MyOutput.Add(Convert.ToString(DecDecOutput));
                MyOutput.Add(Convert.ToString(TempRestDec));
                return MyOutput;
            }

            CalcConvert.StringsToLongLists(in Dig1, in Dig2, 18, out List<List<long>> DigsList);
            List<long> Dig1List = DigsList[0];
            List<long> Dig2List = DigsList[1];
            CalcCompare.ListBigger(in Dig1List, Dig1List.Count, in Dig2List, Dig2List.Count, out int BiggerList);

            if (Dig1Length == Dig2Length)
            {

                if (BiggerList == 2)
                {
                    MyOutput.Add("0");
                    MyOutput.Add(Dig1Sign + Dig1);
                    return MyOutput;
                }

                if (BiggerList == 0 && Dig1 != "0")
                {
                    MyOutput.Add(ResultSign + "1");
                    MyOutput.Add("0");
                    return MyOutput;
                }

            }


            if (Dig1Length >= Dig2Length)
            {
                List<List<long>> MyResult = CalcLists.Div(Dig1List, Dig2List);

                CalcConvert.LongListToString(MyResult[0], "000000000000000000", out string Multiply);
                CalcConvert.LongListToString(MyResult[1], "000000000000000000", out string Rest);

                //last conditions to avoid empty lists
                if (Multiply == "") { Multiply = "0"; }
                if (Rest == "") { Rest = "0"; }

                //propably not nedded, but to avoid incomplete subtraction
                if (Rest == Dig2)
                {
                    MyOutput = CalcStrings.Add(Multiply, "1");
                    Rest = "0";
                }

                MyOutput.Add(ResultSign + Multiply); //Quotient
                MyOutput.Add(Dig1Sign + Rest); //Rest
                //add proper sign for quotient and rest (REST NOT MODULO)
                return MyOutput;
            }

            return MyOutput;
        }

    }

    class CalcCompare
    {

        public static void StringBigger(in string Dig1Orig, in string Dig2Orig, out int StringBigger)
        {

            long DigsDiff = Dig1Orig.Length - Dig2Orig.Length;

            if (DigsDiff > 0)
            { StringBigger = 1; return; }
            if (DigsDiff < 0)
            { StringBigger = 2; return; }
            if (DigsDiff == 0)
            {
                if (Dig1Orig == Dig2Orig)
                { StringBigger = 0; return; }

                CalcConvert.StringsToLongLists(in Dig1Orig, in Dig2Orig, 18, out List<List<long>> ListTest);

                List<long> Dig1List = ListTest[0];
                List<long> Dig2List = ListTest[1];
                int Dig1ListCount = Dig1List.Count;
                for (int i = Dig1ListCount - 1; i >= 0; i--)
                {
                    if (Dig1List[i] > Dig2List[i])
                    {
                        StringBigger = 1; return;
                    }
                    if (Dig1List[i] < Dig2List[i])
                    {
                        StringBigger = 2; return;
                    }
                }

                StringBigger = 0; return;
            }

            StringBigger = 0; ; return;
        }

        public static void ListBigger(in List<long> Dig1List, in int Dig1ListCount, in List<long> Dig2List, in int Dig2ListCount, out int BiggerList)
        {
            //int Dig1ListCount = Dig1List.Count;
            //int Dig2ListCount = Dig2List.Count;
            int DigsDiff = Dig1ListCount - Dig2ListCount;

            switch (DigsDiff)
            {
                case > 0:
                    BiggerList = 1;
                    return;
                case < 0:
                    BiggerList = 2;
                    return;
                case 0:
                    for (int i = Dig1ListCount - 1; i >= 0; i--)
                    {
                        long Dig1 = Dig1List[i];
                        long Dig2 = Dig2List[i];
                        if (Dig1 > Dig2)
                        {
                            BiggerList = 1;
                            return;
                        }
                        if (Dig1 < Dig2)
                        {
                            BiggerList = 2;
                            return;
                        }
                    }
                    BiggerList = 0;
                    return;
            }

        }

        public static void ListBiggerEqual(in List<long> Dig1List, in List<long> Dig2List, in int ListLength, out int BiggerList)
        {

            for (int i = ListLength - 1; i >= 0; i--)
            {
                long Dig1 = Dig1List[i];
                long Dig2 = Dig2List[i];
                if (Dig1 > Dig2)
                {
                    BiggerList = 1;
                    return;
                }
                if (Dig1 < Dig2)
                {
                    BiggerList = 2;
                    return;
                }
            }
            BiggerList = 0;
            return;


        }

        public static void GetLongFromList(in List<long> DigList, in int DigCount, in int DigListFirstLength, out long LongDig)
        {

            if (DigListFirstLength == DigCount)
            { LongDig = DigList[^1]; return; }


            if (DigListFirstLength > DigCount)
            {
                int Diff = DigListFirstLength - DigCount;
                CalcIntExt.LongPower(10, in Diff, out long Result);
                LongDig = (DigList[^1] / Result);
                return;
            }
            else
            {

                if (DigList.Count > 1)
                {
                    long Result;
                    int Diff = 18 - DigListFirstLength;
                    CalcIntExt.LongPower(10, in Diff, out Result);
                    long FullLong = (DigList[^1] * Result);
                    CalcIntExt.LongPower(10, in DigListFirstLength, out Result);
                    long AddLong = (DigList[^2] / Result);
                    FullLong += AddLong;
                    Diff = 18 - DigCount;
                    CalcIntExt.LongPower(10, in Diff, out Result);
                    LongDig = (FullLong / Result);
                    return;
                }
                else
                {
                    LongDig = DigList[^1];
                    return;
                }

            }

        }

        public static void GetLongDigFromDig(in long Dig1, in int DigPosFromUp, in int AllDigLength, out long MyLongDig)
        {
            //get one digit from Up Dig1
            int DigPosRev = (AllDigLength - DigPosFromUp);
            CalcIntExt.LongPower(10, in DigPosRev, out long Result);
            MyLongDig = (Dig1 / Result);
        }
        public static void GetDecimalFromList(in List<long> DigList, out decimal MyDecimalDig)
        {
            if (DigList.Count > 1)
            {
                decimal FulDec = (DigList[^1] * (decimal)1000000000000000000);
                FulDec += DigList[^2];
                MyDecimalDig = FulDec;
                return;
            }
            else
            {
                MyDecimalDig = DigList[^1];
                return;
            }
        }
    }

    public class CalcConvert
    {
        public static void StringsToLongLists(in string Dig1Orig, in string Dig2Orig, in int FragLength, out List<List<long>> MyOutput)
        {
            MyOutput = new(2);

            string Dig1;
            string Dig2;

            int Dig1OrigLength = Dig1Orig.Length;
            int Dig2OrigLength = Dig2Orig.Length;

            int DigLengthDiff = Dig1OrigLength - Dig2OrigLength;

            int CommonLength = Dig1OrigLength;
            int Dig2Length = Dig2OrigLength;

            if (DigLengthDiff < 0)
            {
                Dig1 = Dig2Orig;
                Dig2 = Dig1Orig;
                CommonLength = Dig2OrigLength;
                Dig2Length = Dig1OrigLength;
            }
            else
            {
                Dig1 = Dig1Orig;
                Dig2 = Dig2Orig;
            }

            List<long> Dig1List = new((Dig1.Length / FragLength) + 1);
            List<long> Dig2List = new((Dig2.Length / FragLength) + 1);


            if (CommonLength > FragLength) //cut number string into n-digit int array (from right to left)
            {
                CalcIntExt.MyModuloIntToInt(in CommonLength, in FragLength, out int PartCount1, out int ModLength1);
                CalcIntExt.MyModuloIntToInt(in Dig2Length, in FragLength, out int PartCount2, out int ModLength2);

                for (int i = PartCount1 - 1; i >= 0; i--)
                {
                    int start1 = ModLength1 + (i * FragLength);
                    int start2 = ModLength2 + (i * FragLength);
                    Dig1List.Add(Convert.ToInt64(Dig1.Substring(start1, FragLength))); //get n-digit string to list
                    if (PartCount2 - 1 >= i && PartCount2 > 0)
                    {
                        Dig2List.Add(Convert.ToInt64(Dig2.Substring(start2, FragLength)));
                    }
                }

                if (ModLength1 > 0)
                {
                    Dig1List.Add(Convert.ToInt64(Dig1[..ModLength1])); //get n-digit string to list
                }
                if (ModLength2 > 0)
                {
                    Dig2List.Add(Convert.ToInt64(Dig2[..ModLength2]));
                }
            }
            else
            {
                Dig1List.Add(Convert.ToInt64(Dig1)); //if number is smaller then n digit
                Dig2List.Add(Convert.ToInt64(Dig2));
            }

            if (DigLengthDiff < 0)
            {
                MyOutput.Add(Dig2List);
                MyOutput.Add(Dig1List);
            }
            else
            {
                MyOutput.Add(Dig1List);
                MyOutput.Add(Dig2List);
            }
        }




        public static void StringToLongList(in string Dig1Orig, in int FragLength, out List<long> Dig1List)
        {
            int CommonLength = Dig1Orig.Length;

            Dig1List = new((CommonLength / FragLength) + 1);
            if (CommonLength > FragLength) //cut number string into n-digit int array (from right to left)
            {
                CalcIntExt.MyModuloIntToInt(in CommonLength, in FragLength, out int PartCount, out int ModLength);

                for (int i = PartCount - 1; i >= 0; i--)
                {
                    int start = ModLength + (i * FragLength);
                    Dig1List.Add(Convert.ToInt64(Dig1Orig.Substring(start, FragLength))); //get n-digit string to list
                }

                if (ModLength > 0)
                {
                    Dig1List.Add(Convert.ToInt64(Dig1Orig[..ModLength])); //get n-digit string to list
                }
            }
            else
            {
                Dig1List.Add(Convert.ToInt64(Dig1Orig)); //if number is smaller then n digit
            }
        }

        public static void LongListToString(in List<long> DigList, in string StringPlaces, out string MyOutput)
        {
            int loops = DigList.Count - 1;
            List<string> MyOutputList = new(loops + 1);

            for (int i = loops; i >= 0; i--)
            {

                if (i == loops)
                {
                    MyOutputList.Add(Convert.ToString(DigList[i]));
                }
                else
                {
                    MyOutputList.Add(DigList[i].ToString(StringPlaces));
                }

            }

            MyOutput = string.Join("", MyOutputList);

            if (MyOutput == "")
            { MyOutput = "0"; }
        }

        public static void ConvertList18To9(in List<long> DigList, out List<long> MyResult)
        {
            int ResultCount;
            ResultCount = DigList.Count * 2;

            MyResult = new(ResultCount);
            int Loops = DigList.Count - 1;

            for (int i = 0; i <= Loops; i++)
            {
                //long Full, MyMod;
                CalcIntExt.MyModuloLongToLong(DigList[i], 1000000000, out long Full, out long MyMod);
                MyResult.Add(MyMod);
                long NextDig = Full;

                if (NextDig > 0 && i <= Loops)
                { MyResult.Add(NextDig); }

            }
            //return;
        }

        public static void ConvertList9To18(in List<long> DigList, out List<long> MyResult)
        {
            int ResultCount;
            ResultCount = DigList.Count / 2 + 1;
            MyResult = new(ResultCount);
            int Loops = DigList.Count - 1;

            for (int i = 0; i <= Loops; i++)
            {
                MyResult.Add(DigList[i]);

                if (i < Loops && DigList[i + 1] > 0)
                { MyResult[^1] += DigList[i + 1] * 1000000000; }
                i += 1;
            }
            //return;
        }
    }

}
