using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using LehmerGen;
using System.ComponentModel;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.IO.Compression;
using Miki;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

//format code ctrl-k + ctrl-f

namespace RandomPassword
{

    class RandomPassword
    {

        public static int DecoderMode = 100;
        public static int SysEx = 0;
        public static int OtherCode = 0;
        public static int FalsePosCount = 0;

        public static int DisplayMode = 0;
        public static int ActTaskCount = 0;
        public static int GlobalTaskCount = 0;
        public static int SetupTaskCount = 0;

        public static string SessionPassCountDoneS = "0";

        private static AsyncLocal<string> ExtrMyPass = new AsyncLocal<string>();
        private static AsyncLocal<ProcessStartInfo> Process7z = new AsyncLocal<ProcessStartInfo>();
        private static AsyncLocal<string> SesPassCountGenSAsync = new AsyncLocal<string>();

        public static long MySpeedMin = 0;
        public static long MySpeedTempMin = 0;
        public static long MySpeedSec = 0;
        public static long MySpeedTempSec = 0;

        public static DateTime StartTime = DateTime.Now;
        public static DateTime EndTime = DateTime.Now;
        public static Stopwatch Stopwatch7zLap;
        public static Stopwatch Stopwatch7zFull;
        public static int LapCount7z;
        public static int Speed7zLoop;

        public static string MyExit = "n";
        public static string CurrentOS = "";
        public static string ProgramDir = "";
        //public static string Add7z = "";
        public static Stopwatch StopwatchJob;
        public static double ElapsNsJob = 0;
        public static string DictExitReason = "";

        public static string HashcatParam = "";
        public static string JTRParam = "";
        public static string ZipParam = "";
        public static int Task7zInProgress = 0;
        public static int Task7zWaiting = 0;
        public static List<string> PassRunList = new();
        public static object locking_object = new object();

        public static string SessionDictGenPasswords = "0";
        public static string DictionaryFirstIteration = "0";
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding("ISO-8859-1");
            Console.WriteLine("Miki Calculator and Pseudo-Random Password Generator for decoding 7z (Qlocker)...");
            Console.WriteLine("");
            //TestCalculation();
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("1.RndTo7z,   2.RndToHashcat,    3.RndToJTR,    8.Calculator,    9.Calculator Radom,    0.Test Generator");
                DecoderMode = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

                if (DecoderMode != 0 & DecoderMode != 1 & DecoderMode != 2 & DecoderMode != 3 & DecoderMode != 8 & DecoderMode != 9)
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


            ProgramDir = Environment.CurrentDirectory;
            ReadConfig();

            string ProgressFile = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.txt");

            //List<string> ProgressStringList = new();

            List<string> ProgressStringList = new List<string>(File.ReadAllLines(ProgressFile));

            /*
            using (System.IO.StreamReader Pfile = new System.IO.StreamReader(ProgressFile))
            {
                string ReadString;
                while ((ReadString = Pfile.ReadLine()) != null)
                { ProgressStringList.Add(CalcStringExt.DeSplitString(ReadString)); }
                Pfile.Dispose();
                Pfile.Close();
            }
            */


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
            //Miki.CalcConvert.StringToLongList(in LehmerGen.Passwords.ProgressIterToDoString, 9, out LehmerGen.Passwords.ProgressIterToDoList9);

            LehmerGen.Passwords.InitializeGenerator();
            //LehmerGen.Passwords.PrepareGeneratorNext(true);

            Console.WriteLine("Session restored...");
            Console.WriteLine(string.Format("Iteration to do:   {0}", CalcStringExt.SplitString(LehmerGen.Passwords.ProgressIterToDoString)));

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
                    Task.Run(() => RunHashcatProgram()).ConfigureAwait(false);
                    //LehmerGen.Passwords.DictSize = LehmerGen.Passwords.HCDictSize;
                }

                if (DecoderMode == 3)
                {
                    Task.Run(() => RunJTRProgram()).ConfigureAwait(false);
                    //LehmerGen.Passwords.DictSize = LehmerGen.Passwords.JTRDictSize;
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
                    LehmerGen.Passwords.HCDictSize = Convert.ToInt32(CalcStringExt.DeSplitString(Console.ReadLine()));
                    Console.WriteLine();
                    Task.Run(() => RunHashcatProgram()).ConfigureAwait(false);
                }

                if (DecoderMode == 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much passwords in Dictionary file???");
                    LehmerGen.Passwords.HCDictSize = Convert.ToInt32(CalcStringExt.DeSplitString(Console.ReadLine()));
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
                    LehmerGen.Passwords.TestPassLoops = Convert.ToInt32(CalcStringExt.DeSplitString(CalcStringExt.DeSplitString(Console.ReadLine())));
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
            Stopwatch sw = new Stopwatch();
            string Dig1 = "2676543456765432345676543234567652345654321234567875434567890987654321234567876543";
            string Dig2 = "1";

            Miki.CalcConvert.StringToLongList(Dig1, 18, out List<long> Dig1List);
            Miki.CalcConvert.StringToLongList(Dig2, 18, out List<long> Dig2List);
            int s = 1000000;
            int l = 10;
            double sumtime = 0;
            double MeanTime = 0;

            sw.Start();
            for (int k = 1; k <= l; k++)
            {
                for (int i = 0; i <= s; i++)
                { List<long> Result = Miki.CalcLists.SubCh(Dig1List, Dig2List); }
                for (int i = 0; i <= s; i++)
                { List<long> Result = Miki.CalcLists.SubOld(Dig1List, Dig2List); }
            }
            sw.Stop();
            //Console.WriteLine("New time1: " + sw.Elapsed.TotalNanoseconds);
            sw.Reset();


            for (int k = 1; k <= l; k++)
            {
                sw.Start();
                for (int i = 0; i <= s; i++)
                { List<long> Result = Miki.CalcLists.SubOld(Dig1List, Dig2List); }
                sw.Stop();
                Console.WriteLine("Old time" + k + ": " + sw.Elapsed.TotalNanoseconds);
                sumtime += sw.Elapsed.TotalNanoseconds;
                sw.Reset();
            }
            MeanTime = sumtime / l;
            Console.WriteLine("MeanTime: " + MeanTime);
            sumtime = 0;

            for (int k = 1; k <= l; k++)
            {
                sw.Start();
                for (int i = 0; i <= s; i++)
                { List<long> Result = Miki.CalcLists.SubCh(Dig1List, Dig2List); }
                sw.Stop();
                Console.WriteLine("New time" + k + ": " + sw.Elapsed.TotalNanoseconds);
                sumtime += sw.Elapsed.TotalNanoseconds;
                sw.Reset();
            }
            MeanTime = sumtime / l;
            Console.WriteLine("MeanTime: " + MeanTime);
            sumtime = 0;

            
        }
        public static void ReadConfig()
        {
            string ReadString;
            List<string> ConfigStringList = new();
            List<List<char>> MaskTableTemp = new();
            string ConfigFile = System.IO.Path.Combine(ProgramDir, "Config", "Config.txt");

            using (System.IO.StreamReader Cfile = new System.IO.StreamReader(ConfigFile))
            {
                while ((ReadString = Cfile.ReadLine()) != null)
                {
                    if (ReadString != "" && ReadString[..1] != "#")
                    {
                        if (ReadString.Contains("Characters="))
                        {
                            string TempString = ReadString.Replace("Characters=", "").Trim();
                            char[] characters = TempString.ToCharArray();
                            Array.Sort(characters);
                            LehmerGen.Passwords.CharactersList = new List<char>(characters);
                            //LehmerGen.Passwords.PassCharCount = Convert.ToString(LehmerGen.Passwords.CharactersList.Count);
                        }
                        if (ReadString.Contains("PassMinLength="))
                        {
                            LehmerGen.Passwords.PassMinLength = Convert.ToInt32(ReadString.Replace("PassMinLength=", "").Trim());
                        }
                        if (ReadString.Contains("PassMaxLength="))
                        {
                            LehmerGen.Passwords.PassMaxLength = Convert.ToInt32(ReadString.Replace("PassMaxLength=", "").Trim());
                            for (int c = 0; c < LehmerGen.Passwords.PassMaxLength; c++)
                            { LehmerGen.Passwords.MaskTable.Add(LehmerGen.Passwords.CharactersList); }
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
                        if (ReadString.Contains("ZipParam="))
                        {
                            ZipParam = ReadString.Replace("ZipParam=", "").Trim();
                        }
                        if (ReadString.Contains("Speed7zLoop="))
                        {
                            Speed7zLoop = Convert.ToInt32(ReadString.Replace("Speed7zLoop=", "").Trim());
                        }
                        if (ReadString.Contains("ProcCount="))
                        {
                            RandomPassword.SetupTaskCount = Convert.ToInt32(ReadString.Replace("ProcCount=", "").Trim());
                            RandomPassword.GlobalTaskCount = RandomPassword.SetupTaskCount;

                        }
                        if (ReadString.Contains("7zDictSize="))
                        {
                            LehmerGen.Passwords.ZipDictSize = Convert.ToInt32(ReadString.Replace("7zDictSize=", "").Trim());
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
            string InfoString = "";
            if (DecoderMode == 1) { InfoString2 = "+. 7z Task++, -. 7z Task--, /. Task=2, *. Task=From Config"; }
            InfoString3 = "n. End if cracked, e. Safe End, Ctrl+s. Reload Config";
            if (DecoderMode == 1) { InfoString4 = string.Format("Number of active tasks is set to: {0}", RandomPassword.GlobalTaskCount); }

            InfoString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", InfoString1, Environment.NewLine, InfoString2, Environment.NewLine, InfoString3, Environment.NewLine, InfoString4, Environment.NewLine, InfoString5);

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(InfoString);
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");


            while (true)
            {
                // true hides the pressed character from the console
                ConsoleKeyInfo cki;
                bool DisplayInfo = false;

                cki = Console.ReadKey(true);
                if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((cki.Key & ConsoleKey.S) != 0)
                    {
                        ReadConfig();
                        break;
                    }
                }

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
                        RandomPassword.GlobalTaskCount = RandomPassword.SetupTaskCount;
                        DisplayInfo = true;
                        break;
                    case '/':
                        RandomPassword.GlobalTaskCount = 2;
                        DisplayInfo = true;
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
                            //Console.WriteLine("Key pressed...");
                            Thread.Sleep(100);
                        }
                        else
                        {
                            Console.WriteLine("End waiting for key...");
                        }
                        break;

                }

                if (DecoderMode == 1) { InfoString4 = string.Format("Number of active tasks is set to: {0}", RandomPassword.GlobalTaskCount); }

                if (MyExit == "n")
                { InfoString5 = "End program if cracked..."; }
                if (MyExit == "e")
                { InfoString5 = "Sheduled Safe End..."; }


                if (DisplayInfo == true)
                {
                    Console.WriteLine("");
                    InfoString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", InfoString1, Environment.NewLine, InfoString2, Environment.NewLine, InfoString3, Environment.NewLine, InfoString4, Environment.NewLine, InfoString5);
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
                Task.WaitAny(GenDict);
                DictExitReason = GenDict.Result;
                return;

            }

            if (!File.Exists(DictFile) && File.Exists(DictFileNext))
            {
                Console.WriteLine("No Dictionary.txt, rename DictionaryNext.txt to Dictionary.txt...");
                System.IO.File.Move(DictFileNext, DictFile);
                DictExitReason = " - OK.";
                return;
            }

            if (File.Exists(DictFile) && !File.Exists(DictFileNext))
            {
                Console.WriteLine("No DictionaryNext.txt, Generating in progress...");
                Task<string> GenDict = GenerateDictionary(DictFileNext, "DictionaryNext.txt", DictionarySize, JobKind, SesPassCountDoneS);
                Task.WaitAny(GenDict);
                DictExitReason = GenDict.Result;
                return;
            }
            DictExitReason = " - OK.";
            return;
        }

        public static async Task<string> GenerateDictionary(string DictFile, string DictionaryName, int DictionarySize, string JobKind, string SesPassCountDoneS)
        {

            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            string IterAll = "";

            Console.WriteLine("");
            Console.WriteLine("New " + DictionaryName + " file is generated: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            List<string> PassList = LehmerGen.Passwords.PassListFromDigByTab(DictionarySize);
            File.WriteAllLines(DictFile, PassList.ToArray(), isoLatin1Encoding);

            PassList.Clear();

            //IterAll = Miki.CalcStrings.Add(LehmerGen.Passwords.ProgressIterDoneString, Convert.ToString(DictionarySize))[0]; // not correct
            IterAll = LehmerGen.Passwords.ProgressIterDoneString; //ok
            string IterDoneLocal = Miki.CalcStrings.Sub(IterAll, LehmerGen.Passwords.ProgressIterStartString)[0];
            IterDoneLocal = Miki.CalcStrings.Add(IterDoneLocal, "1")[0];
            Console.WriteLine("");
            Console.WriteLine("New " + DictionaryName + " file is done:      " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("");
            //int i = await CalculateSpeed().ConfigureAwait(true);
            SaveProgress(IterAll, LehmerGen.Passwords.ProgressPoolMaxString, JobKind);
            string String1 = CalcStringExt.SplitString(LehmerGen.Passwords.ProgressPoolMaxString);
            int String1Len = String1.Length;
            string String2 = CalcStringExt.SplitString(LehmerGen.Passwords.ProgressIterDoneString).PadLeft(String1Len);
            string String3 = CalcStringExt.SplitString(LehmerGen.Passwords.ProgressIterToDoString).PadLeft(String1Len);
            string String4 = CalcStringExt.SplitString(IterAll).PadLeft(String1Len);
            string String5 = CalcStringExt.SplitString(IterDoneLocal).PadLeft(String1Len);
            string String6 = CalcStringExt.SplitString(Convert.ToString(DictionarySize)).PadLeft(String1Len);
            string String7 = CalcStringExt.SplitString(SesPassCountDoneS).PadLeft(String1Len);
            SessionDictGenPasswords = Miki.CalcStrings.Add(SessionDictGenPasswords, Convert.ToString(DictionarySize))[0];
            string String8 = CalcStringExt.SplitString(SessionDictGenPasswords).PadLeft(String1Len);
            string String9 = CalcStringExt.SplitString(DictionaryFirstIteration).PadLeft(String1Len);

            //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "    " + JobKind + " Act/Full: " + MySpeedTemp + " / " + MySpeed + " /m    FalsePos: " + FalsePosCount);
            Console.WriteLine("DICTIONARIES AND SESSION INFO ->");
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

            DictionaryFirstIteration = LehmerGen.Passwords.ProgressIterToDoString;
            LehmerGen.Passwords.ProgressIterDoneString = IterAll;
            LehmerGen.Passwords.ProgressIterToDoString = Miki.CalcStrings.Add(LehmerGen.Passwords.ProgressIterDoneString, "1")[0];

            Miki.CalcCompare.StringBigger(in IterAll, in LehmerGen.Passwords.ProgressPoolMaxString, out int StringBigger);
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

            string ExitReason1 = "Safe exit";

            await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.ZipDictSize, DictFile, DictFileNext, "7z", SessionPassCountDoneS)).ConfigureAwait(true);
            string ExitReason2 = DictExitReason;
            List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

            StartTime = DateTime.Now;
            Stopwatch7zLap = Stopwatch.StartNew();
            Stopwatch7zFull = Stopwatch.StartNew();
            LapCount7z = 0;

            while (true)
            {

                await Task.Delay(1);
                Task7zCount = RandomPassword.GlobalTaskCount;

                if (RandomPassword.MyExit == "e")
                {
                    RandomPassword.GlobalTaskCount = 0;
                    if (PassRunList.Count == 0)
                    {
                        ExitReason2 = DictExitReason;
                        Console.WriteLine(ExitReason1 + ExitReason2);
                        return;
                    }
                }

                if (RandomPassword.MyExit == "ex" && PassRunList.Count == 0)
                {
                    ExitReason2 = DictExitReason;
                    Console.WriteLine(ExitReason1 + ExitReason2);
                    return;
                }

                while (PassRunList.Count < Task7zCount)
                {
                    if (l == 0) //there is a time to generate DictionaryNext.txt, not waiting...
                    {
                        Task.Run(async () => await CheckForDictionary(LehmerGen.Passwords.ZipDictSize, DictFile, DictFileNext, "7z", SesPassCountGenSAsync.Value)).ConfigureAwait(false);
                    }

                    ExtrMyPass.Value = TempDictList[l];
                    SesPassCountGenSAsync.Value = Miki.CalcStrings.Add("1", SesPassCountGenSAsync.Value)[0];
                    Task.Run(async () => await ExtractFile(CodedFile, ExtrMyPass.Value, SesPassCountGenSAsync.Value).ConfigureAwait(false));
                    lock (locking_object)
                    {
                        PassRunList.Add(ExtrMyPass.Value);
                    }
                    SessionPassCountDoneS = SesPassCountGenSAsync.Value;
                    l++;

                    if (l == TempDictList.Count) //here we must have Dictionary.txt, so rename DictionaryNext.txt or generate it
                    {
                        File.Delete(DictFile);
                        Console.WriteLine();
                        Console.WriteLine("Dictionary.txt checked... deleted...");
                        await Task.Run(() => CheckForDictionary(LehmerGen.Passwords.ZipDictSize, DictFile, DictFileNext, "7z", SesPassCountGenSAsync.Value)).ConfigureAwait(true);
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
            string ZipFile;

            if (RandomPassword.CurrentOS == "Windows")
            {
                JTRFile = System.IO.Path.Combine(ProgramDir, "JTR", "run", "john.exe");
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za.exe");
            }
            else
            {
                JTRFile = System.IO.Path.Combine(ProgramDir, "JTR", "run", "john");
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za");
            }
            string CodedFile = System.IO.Path.Combine(ProgramDir, "Coded", "Coded.7z");
            string DictFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
            string DictFileNext = System.IO.Path.Combine(ProgramDir, "Dictionary", "DictionaryNext.txt");
            string JTRDir = System.IO.Path.Combine(ProgramDir, "JTR", "run");
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            string ExitReason1 = "Safe exit";
            string ExitReason2 = "";

            //ProgressString is a real password number, eg. 1 is for 000000000...0, and it is last chcecked password
            //but password 000000...0 cames from iteration 0, so if we want to check iteration 0, ProgressString should be -1

            StopwatchJob = Stopwatch.StartNew();
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
                    TimeSpan span = EndTime.Subtract(RandomPassword.StartTime);
                    CalculateSpeed(SessionPassCountDoneS, span.TotalNanoseconds, LehmerGen.Passwords.JTRDictSize, swHC.Elapsed.TotalNanoseconds);
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " JTR Speed Act/Full: " + MySpeedTempMin + " / " + MySpeedMin + " /m  " + MySpeedTempSec + " / " + MySpeedSec + " /s" + "   FalsePos: " + FalsePosCount);

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
                            ProcessStartInfo pro7z = new ProcessStartInfo
                            {
                                WindowStyle = ProcessWindowStyle.Hidden,
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                CreateNoWindow = false,
                                FileName = ZipFile,
                                Arguments = string.Format("t \"{0}\" -p\"{1}\" -y", CodedFile, MyPasss)
                            };
                            Process x7z = Process.Start(pro7z);

                            x7z.WaitForExit();
                            string errorOutput = x7z.StandardError.ReadToEnd();
                            string standardOutput = x7z.StandardOutput.ReadToEnd();


                            if (x7z.ExitCode == 0) // password is true positive
                            {
                                Console.WriteLine("");
                                Console.WriteLine(string.Format("Password is: {0}", MyPasss));
                                SavePassword(MyPasss);
                                DeleteDictFile = false;
                                Console.WriteLine("");
                                Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                                x7z.Dispose();
                                x7z.Close();
                                return;

                            }
                            if (x7z.ExitCode == 2)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("FALSE POSITIVE - password, cleaning password list....");
                                Console.WriteLine("");
                                List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

                                TempDictList.Remove(MyPasss);

                                File.Delete(DictFile);
                                File.WriteAllLines(DictFile, TempDictList.ToArray(), isoLatin1Encoding);
                                TempDictList.Clear();
                                string MyPasswordFalseFilename = "FalsePassword-" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt";
                                string MyPasswordFalse = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);

                                Console.WriteLine("Move john.pot file to FalsePassword....");
                                System.IO.File.Move(MyResult, MyPasswordFalse);

                                Console.WriteLine("");
                                Console.WriteLine("Return to check rest of the list....");
                                Console.WriteLine("");
                                x7z.Dispose();
                                x7z.Close();
                                DeleteDictFile = false;
                                FalsePosCount++;
                            }
                        }

                    }

                    if (DeleteDictFile == true) //don't delete file if after false positive
                    {
                        File.Delete(DictFile);
                        Console.WriteLine();
                        SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.JTRDictSize), SessionPassCountDoneS)[0];
                    }

                    if (RandomPassword.MyExit == "e")
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

                    RandomPassword.ElapsNsJob = RandomPassword.StopwatchJob.Elapsed.TotalNanoseconds;
                    RandomPassword.StopwatchJob = Stopwatch.StartNew();
                    RandomPassword.EndTime = DateTime.Now;
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
            string ZipFile;

            if (RandomPassword.CurrentOS == "Windows")
            {
                HascatFile = System.IO.Path.Combine(ProgramDir, "Hashcat", "hashcat.exe");
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za.exe");
            }
            else
            {
                HascatFile = System.IO.Path.Combine(ProgramDir, "Hashcat", "hashcat");
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za");
            }

            string CodedFile = System.IO.Path.Combine(ProgramDir, "Coded", "Coded.7z");
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

            StopwatchJob = Stopwatch.StartNew();
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
                    TimeSpan span = EndTime.Subtract(RandomPassword.StartTime);
                    CalculateSpeed(SessionPassCountDoneS, span.TotalNanoseconds, LehmerGen.Passwords.HCDictSize, swHC.Elapsed.TotalNanoseconds);
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " HC Speed Act/Full: " + MySpeedTempMin + " / " + MySpeedMin + " /m  " + MySpeedTempSec + " / " + MySpeedSec + " /s" + "   FalsePos: " + FalsePosCount);

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
                        ProcessStartInfo pro7z = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            CreateNoWindow = false,
                            FileName = ZipFile,
                            Arguments = string.Format("t \"{0}\" -p\"{1}\" -y", CodedFile, MyPasss) //x or t, t is much faster, and password is saved to file (it is not necessary to decrypt file)
                        };

                        Process x7z = Process.Start(pro7z);

                        x7z.WaitForExit();
                        string errorOutput = x7z.StandardError.ReadToEnd();
                        string standardOutput = x7z.StandardOutput.ReadToEnd();


                        if (x7z.ExitCode == 0) // password is true positive
                        {
                            Console.WriteLine("");
                            Console.WriteLine(string.Format("Password is: {0}", MyPasss));
                            SavePassword(MyPasss);

                            Console.WriteLine("");
                            Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                            x7z.Dispose();
                            x7z.Close();
                            return;
                        }
                        if (x7z.ExitCode == 2)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("FALSE POSITIVE - password, cleaning password list....");
                            Console.WriteLine("");
                            List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

                            TempDictList.Remove(MyPasss);

                            File.Delete(DictFile);
                            File.WriteAllLines(DictFile, TempDictList.ToArray(), isoLatin1Encoding);
                            TempDictList.Clear();
                            string MyPasswordFalseFilename = "FalsePassword-" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt";
                            string MyPasswordFalse = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);
                            System.IO.File.Move(MyResult, MyPasswordFalse);
                            Console.WriteLine("");
                            Console.WriteLine("Return to check rest of the list....");
                            Console.WriteLine("");
                            x7z.Dispose();
                            x7z.Close();
                            FalsePosCount++;

                        }
                    }

                    if (x.ExitCode == 1)

                    {
                        File.Delete(DictFile);
                        Console.WriteLine();
                        SessionPassCountDoneS = Miki.CalcStrings.Add(Convert.ToString(LehmerGen.Passwords.HCDictSize), SessionPassCountDoneS)[0];
                    }


                    if (RandomPassword.MyExit == "e")
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

                    RandomPassword.ElapsNsJob = RandomPassword.StopwatchJob.Elapsed.TotalNanoseconds;
                    RandomPassword.StopwatchJob = Stopwatch.StartNew();
                    RandomPassword.EndTime = DateTime.Now;
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
            ProcessStartInfo pro = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                FileName = ZipDir,
                Arguments = "",
            };
            RandomPassword.Process7z.Value = pro;
        }
        private static async Task ExtractFile(string CodedDir, string MyPass, string SesPassCountS)
        {
            string SesPassCountLocalS = SesPassCountS;
            int MyExit = 100;
            string LocalPass = MyPass;
            int LoopCount = 0;
            long WaitTime = 0;

            try
            {
                ProcessStartInfo Process7zLocal = Process7z.Value;
                Process7zLocal.Arguments = string.Format("{0} \"{1}\" -p\"{2}\" -y", ZipParam, CodedDir, LocalPass);
                Process x = await Task.Run(async () => Process.Start(Process7zLocal)).ConfigureAwait(false);
                lock (locking_object)
                {
                    Task7zInProgress++;
                }

                x.BeginOutputReadLine();
                x.BeginErrorReadLine();
                x.WaitForExit();
                MyExit = x.ExitCode;
                x.Dispose();

                lock (locking_object)
                {
                    Task7zInProgress--;
                    Task7zWaiting++;
                }
                Stopwatch WaitWatch = Stopwatch.StartNew();
                //FIFO like
                while (true)
                {
                    //while waiting we can do some things
                    if (LoopCount == 0) //only once while waiting
                    {
                        if (Miki.CalcStrings.Div(SesPassCountS, Convert.ToString(Speed7zLoop))[1] == "0")
                        {
                            Stopwatch7zLap.Stop();
                            double FullTime = Stopwatch7zFull.Elapsed.TotalNanoseconds;
                            double LapTime = Stopwatch7zLap.Elapsed.TotalNanoseconds;
                            CalculateSpeed(SesPassCountLocalS, FullTime, Speed7zLoop, LapTime);
                            Console.WriteLine(string.Format("{0}  7z Speed A/F: {1} / {2} /m  {3} / {4} /s  7z: F:{5}/{6} / P:{7} / W:{8} FalsePos:{9}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), MySpeedTempMin, MySpeedMin, MySpeedTempSec, MySpeedSec, PassRunList.Count, GlobalTaskCount, Task7zInProgress, Task7zWaiting, FalsePosCount));
                            Stopwatch7zLap.Restart();
                        }
                    }
                    if (RandomPassword.DisplayMode == 1)
                    {
                        while (LocalPass != PassRunList[0])
                        {
                            await Task.Delay(1);
                            LoopCount++;
                        }
                    }
                    LoopCount++;
                    break;
                }
                lock (locking_object)
                {
                    Task7zWaiting--;
                }
                WaitWatch.Stop();
                WaitTime = WaitWatch.ElapsedMilliseconds;

                if (MyExit == 0)
                {
                    SavePassword(LocalPass);
                    ProcessStartInfo Process7zLocalCheck = Process7z.Value;
                    string CodedFileCheck = System.IO.Path.Combine(ProgramDir, "Coded", "CodedCheck.7z");
                    Process7zLocalCheck.Arguments = string.Format("{0} \"{1}\" -p\"{2}\" -y", ZipParam, CodedFileCheck, LocalPass);
                    Process y = await Task.Run(() => Process.Start(Process7zLocalCheck));

                    y.BeginOutputReadLine();
                    y.BeginErrorReadLine();
                    y.WaitForExit();
                    y.Dispose();

                    if (y.ExitCode == 0) // password is true positive
                    {
                        Console.WriteLine("");
                        Console.WriteLine(string.Format("Password is: {0}", LocalPass));
                        Console.WriteLine("");
                        Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                        System.Environment.Exit(0);
                        Console.ReadKey();
                        return;
                    }
                    if (y.ExitCode == 2)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("FALSE POSITIVE");
                        Console.WriteLine("");
                        string MyPasswordFalseFilename = "FalsePassword-" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt";
                        string MyPasswordFalse = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);
                        string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Output", "MyPassword7z.txt");
                        System.IO.File.Move(MyPasswordFile, MyPasswordFalse);
                        Console.WriteLine("");
                        Console.WriteLine("Return to check rest of the list....");
                        Console.WriteLine("");
                        FalsePosCount++;
                    }
                }

                if (MyExit == 2)
                {
                    switch (RandomPassword.DisplayMode)
                    {
                        case 1:
                            Console.WriteLine(string.Format("{0}  {1}  Speed A/F: {2} / {3} /m  {4} / {5} /s  ExC:{6} ExCx:{7} Err:{8}  7z: F:{9}/{10} / P:{11} / W:{12}  Wait L/T: {13} / {14}", CalcStringExt.SplitString(SesPassCountLocalS), LocalPass, RandomPassword.MySpeedTempMin, RandomPassword.MySpeedMin, RandomPassword.MySpeedTempSec, RandomPassword.MySpeedSec, MyExit, OtherCode, SysEx, PassRunList.Count, RandomPassword.GlobalTaskCount, Task7zInProgress, Task7zWaiting, LoopCount, WaitTime));
                            break;
                        case 2:
                            Console.WriteLine(string.Format("{0}  {1}  Speed A/F: {2} / {3} /m  {4} / {5} /s  ExC:{6} ExCx:{7} Err:{8}  7z: F:{9}/{10} / P:{11} / W:{12}  Wait L/T: {13} / {14}", CalcStringExt.SplitString(SesPassCountLocalS), LocalPass, RandomPassword.MySpeedTempMin, RandomPassword.MySpeedMin, RandomPassword.MySpeedTempSec, RandomPassword.MySpeedSec, MyExit, OtherCode, SysEx, PassRunList.Count, RandomPassword.GlobalTaskCount, Task7zInProgress, Task7zWaiting, LoopCount, WaitTime));
                            RandomPassword.DisplayMode = 3;
                            break;
                        case 3:
                            break;
                    }
                }
                else
                {
                    OtherCode++;
                    Console.WriteLine(string.Format("{0}  {1}  Speed A/F: {2} / {3} /m  {4} / {5} /s  ExC:{6} ExCx:{7} Err:{8}  7z: F:{9}/{10} / P:{11} / W:{12}  Wait L/T: {13} / {14}", CalcStringExt.SplitString(SesPassCountLocalS), LocalPass, RandomPassword.MySpeedTempMin, RandomPassword.MySpeedMin, RandomPassword.MySpeedTempSec, RandomPassword.MySpeedSec, MyExit, OtherCode, SysEx, PassRunList.Count, RandomPassword.GlobalTaskCount, Task7zInProgress, Task7zWaiting, LoopCount, WaitTime));
                }

            }

            catch (System.Exception Ex)
            {
                SysEx++;
                Console.WriteLine(Ex.Message);
                return;

            }


            lock (locking_object) //FIFO like
            {
                PassRunList.Remove(LocalPass);
            }
            return;

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
            //TimeSpan span = EndTime.Subtract(RandomPassword.StartTime);
            string FullSpeedInterval = Convert.ToString(FullNs);
            try
            {
                if (FullSpeedInterval != "0")
                {
                    string SFullSpeedS = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(FullCount, "1000000000")[0], FullSpeedInterval)[0];
                    string SFullSpeed = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(FullCount, "60000000000")[0], FullSpeedInterval)[0];
                    MySpeedMin = Convert.ToInt64(SFullSpeed);
                    MySpeedSec = Convert.ToInt64(SFullSpeedS);
                }

                string LapSpeedInterval = Convert.ToString(LapNs);
                if (LapSpeedInterval != "0")
                {
                    long InSec = LapCount * 1000000000;
                    long InMin = LapCount * 60000000000;
                    string SLapSpeedS = Miki.CalcStrings.Div(Convert.ToString(InSec), LapSpeedInterval)[0];
                    string SLapSpeed = Miki.CalcStrings.Div(Convert.ToString(InMin), LapSpeedInterval)[0];
                    MySpeedTempMin = Convert.ToInt64(SLapSpeed);
                    MySpeedTempSec = Convert.ToInt64(SLapSpeedS);
                }
            }
            catch
            {
                MySpeedMin = -1;
                MySpeedSec = -1;
                MySpeedTempMin = -1;
                MySpeedTempSec = -1;
            }

        }
        public static void SaveProgress(string DoneIter, string MaxIter, string JobKind)
        {
            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");

            string IterAll = "";

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

            if (File.Exists(ProgressFile))
            {
                if (File.Exists(ProgressFileBak)) // delete prev backup
                    File.Delete(ProgressFileBak);

                File.Move(ProgressFile, ProgressFileBak);
            }

            List<string> ProgressList = new ();
            ProgressList.Add(CalcStringExt.SplitString(DoneIter));
            ProgressList.Add(CalcStringExt.SplitString(MaxIter));
            for (int i = 0; i < LehmerGen.Passwords.TempRemoved.Count; i++)
            {
                ProgressList.Add(LehmerGen.Passwords.TempRemoved[i]);
            }
            string [] ProgressArray = ProgressList.ToArray();

            File.WriteAllLines(ProgressFile, ProgressArray, isoLatin1Encoding);
            /*
            if (!File.Exists(ProgressFile))
            {
                // Create a file to write to.
                using Stream fs = new FileStream(ProgressFile, FileMode.Create, FileAccess.Write, FileShare.None, 0x1000, FileOptions.WriteThrough);
                using (StreamWriter shellConfigWriter = new StreamWriter(fs))
                {

                    shellConfigWriter.WriteLine(CalcStringExt.SplitString(DoneIter));
                    shellConfigWriter.Write(String.Join(String.Empty, CalcStringExt.SplitString(MaxIter)));

                    

                    shellConfigWriter.Flush();
                    shellConfigWriter.Dispose();
                    shellConfigWriter.Close();
                }
            }
            */
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }
        public static void SavePassword(string CrackedPass = "")
        {
            if (CrackedPass != "")
            {
                string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Output", "MyPassword7z.txt");

                if (!File.Exists(MyPasswordFile))
                {

                    using (Stream fs = new FileStream(MyPasswordFile, FileMode.Create, FileAccess.Write, FileShare.None, 0x1000, FileOptions.WriteThrough))
                    using (StreamWriter shellConfigWriter = new StreamWriter(fs))
                    {
                        shellConfigWriter.WriteLine("Password is:");
                        shellConfigWriter.Write(string.Join(string.Empty, CrackedPass));

                        shellConfigWriter.Flush();
                        shellConfigWriter.Dispose();
                        shellConfigWriter.Close();
                    }
                }
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
        //public static List<long> TotalBasePassCountList; //Total passwords count list
        public static List<long> DigPassCurrList; //needed for password generator
        //public static List<long> DigPassNextList;
        public static List<char> CharactersList; //read config, MakeConvTable
        public static string ProgressIterDoneString = ""; // Saved in SessionProgress.txt
        public static string ProgressIterToDoString = ""; // Next to do = ProgressIterDoneString+1
        public static string ProgressIterStartString = ""; //First iteration in Session, for SaveProgress
        public static List<long> ProgressIterDoneList18; //main and generator
        public static List<long> WorkingIter;

        public static List<long> ProgressPoolMaxList; //main and generator
        //public static List<long> WorkIterToDoList;
        //public static List<long> ProgressIterToDoList9;
        public static List<long> ProgressIterToDoList18;
        //public static List<long> IterDoneGen = new() { 0 };
        //public static int WhatIs;
        //public static string TotalPassCountString; //Total password count string
        public static int PassGeneratedBase = 0; //pass base generated from generator
        public static string ProgressAllCountString = ""; //Total password count if fixed length for Initialize and generator
        //public static string PassIterMaxValueFix = ""; //Max value in string witch is last password (total - 1)
        public static string ProgressPoolMaxString = ""; //Max value to iterate in session, main and generatedictionary
        //public static string TotalPossibleCountString = "0"; //Passwords count if non fixed length
        

        public static string IncrementBase; //Increment base string
        public static string IncrementPower; //Increment power string
        public static string Increment = ""; //Increment string. Only InitializeGenerator
        public static int PassMinLength; //Password min length int
        public static int PassMaxLength; //Password max length int
        //public static int PassCharListLength; //Length password char list
        //public static string PassCharCount; //Count of all char that can be in password
        public static int HCDictSize;
        public static int JTRDictSize;
        public static int ZipDictSize;
        ///public static int DictSize;
        public static int TestPassCheck;
        public static int TestPassLoops;
        public static int CounterDisplay;
        public static int IsUnary = 0;
        public static List<List<char>> MaskTable = new();
        public static List<List<List<long>>> ConvTable = new();
        public static List<List<long>> AllUnarysValueList = new();
        public static List<List<long>> DigLenList = new();
        public static List<long> FirstPossibleDecList = new();
        public static List<long> LastPossibleDecList = new(); //only for MakeConvTable
        public static List<long> RealPossibleCountList = new();
        public static string RealPossibleCountString = "";
        //public static List<long> FirstIterList = new();
        //public static List<long> LastIterList = new();
        public static int FillToLength;
        //public static List<List<long>> PowerList = new();
        public static string GeneratorExitReason = "";
        public static List<string> TempRemoved = new();
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
        public static void InitializeGenerator()
        {
            MakeConvTable();

            //PassCharListLength = PassMaxLength - 1;
            //TotalPassCountString = TotalPossibleCountString;

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
            Increment = Miki.CalcStrings.Pow(IncrementBase, IncrementPower)[0];
            Miki.CalcConvert.StringToLongList(in Increment, 18, out IncrementList18);
            Miki.CalcConvert.StringToLongList(in Increment, 9, out IncrementList9);

            //Miki.CalcConvert.StringToLongList(in PassIterCountFix, 18, out List<long> PassIterCountFixList);
            //Passwords.TotalBasePassCountList = RealPossibleCountList; //PassPool list było PassIterCountFixList
            Miki.CalcConvert.LongListToString(in RealPossibleCountList, "000000000000000000", out ProgressAllCountString); //nie było tego
            string String1 = CalcStringExt.SplitString(Increment);

            //ProgressTotalIterCountFix
            Console.WriteLine("Increment = " + String1);
            if (Miki.CalcStrings.Div(RealPossibleCountString, Increment)[1] != "0" || Increment == "1") //Check if Increment value is good for LCG
            {
                if (Increment == "1")
                {
                    Console.WriteLine("Total Real Pool Count % Increment == 0, but ->");
                    Console.WriteLine("Increment value == 1 -> OK..."); 
                }
                else
                { Console.WriteLine("Total Real Pool Count % Increment != 0 -> OK..."); }
            }
            else
            {
                Console.WriteLine("Total Pool Real Count % Increment == 0 -> Wrong Increment value...");
                Console.WriteLine("Change the Increment configuration in GENERATOR CONFIGURATION section in Config.txt file...");
                System.Environment.Exit(10);
            }
            //string PassNumber = "";
            //PassNumber = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(ProgressIterToDoString, Increment)[0], ProgressAllCountString)[1];
            //Miki.CalcConvert.StringToLongList(in PassNumber, 18, out DigPassCurrList);


            List<long> WorkingIterLocal = Miki.CalcLists.Mul(ProgressIterToDoList18, 18, IncrementList9, 9, 18);
            WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, FirstPossibleDecList);
            WorkingIter = WorkingIterLocal;
            List<long> WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountList)[1];
            WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecList);
            DigPassCurrList = new(WorkingDigList); //DigPassCurrList
            
        }


        public static void PrepareGeneratorNext(bool GetFirst)
        {

            //string PassNumber = "";
            /*
            switch (WhatIs)
            {
                case 0: //Password
                    DigFromPassByTab(in ProgressIterToDoString, out PassNumber);
                    break;
                case 1: //Iteration, best solution
                    PassNumber = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(ProgressIterToDoString, Increment)[0], ProgressAllCountString)[1];
                    break;
                case 2: //Number
                    PassNumber = ProgressIterToDoString;
                    break;
            }
            */
            //PassNumber = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(ProgressIterToDoString, Increment)[0], ProgressAllCountString)[1];
            //Miki.CalcConvert.StringToLongList(in PassNumber, 18, out DigPassCurrList);

            /*
            if (GetFirst == false)
            {
                GetDigPassListNext(DigPassCurrList, out List<long> DigPassListCurrTemp);
                DigPassCurrList = DigPassListCurrTemp;
                Console.WriteLine("XXXX");
            }
            */
        }

        public static void MakeConvTable()
        {
            Console.WriteLine("Conversion tables - generating...");
            //List<List<List<long>>> ConvTableLocal = new();
            //List<long> DivListLocal = DivList;
            List<char> CharactersListLocal = CharactersList;
            //List<List<int>> LenTableLocal = new();
            List<List<char>> MaskTableLocal = MaskTable; // to create polynomial numbering system
            List<List<List<long>>> ConvTableLocal = new();
            //string PassIterCountNFixLocal = TotalPossibleCountString;
            int PassMaxLenLocal = PassMaxLength;

            /////////NOWE
            List<List<long>> PowerListLocal = new();
            PowerListLocal.Add(new List<long>(1) { 1 });

            for (int i = 1; i < MaskTableLocal.Count; i++)
            {
                List<long> CharCount = new(1) { (long)MaskTableLocal[i - 1].Count };
                PowerListLocal.Add(Miki.CalcLists.Mul(PowerListLocal[i - 1], 9, CharCount, 9, 9));
            }
            /*
            string test = "";
            for (int i = 0; i < CharactersList.Count; i++)
            {
                test = test + CharactersList[i];
            }
            Console.WriteLine(test);
            */
            for (int CharPos = 0; CharPos < MaskTableLocal.Count; CharPos++)
            {
                List<List<long>> ConvRow = new();
                for (int CharValue = 0; CharValue < MaskTableLocal[CharPos].Count; CharValue++)
                {
                    List<long> CharIntToMul = new();
                    if (MaskTableLocal[CharPos].Count == 1)
                    {
                        //unary system is a little tricky, it has no zero, value is or not, except last position, where must be zero
                        //so value can be in 0 position
                        //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                        //because this value can be / is in upper char as 1.
                        if (CharPos == 0 && CharValue == 0) //unary zero in last position
                        {
                            CharIntToMul = new() { 0 };
                        }
                        else
                        { CharIntToMul = new() { 1 }; } //rest of unaries, they has values
                        IsUnary = 1; //to inform converter, that there is unary
                    }
                    else
                    {
                        CharIntToMul = new() { CharValue };
                    }
                    List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, 9, PowerListLocal[CharPos], 9, 9); //out na 18 i usunąć convert
                    Miki.CalcConvert.ConvertList9To18(in MultResult, out List<long> MultResult18);
                    ConvRow.Add(MultResult18);

                }
                ConvTableLocal.Add(ConvRow);
            }
            ConvTable = ConvTableLocal;

            /*
            for (int i = 0; i < CharactersListLocal.Count; i++)
            {
                List<List<long>> ConvRow = new();
                for (int k = 0; k < PassMaxLenLocal; k++)
                {
                    List<long> CharIntToMul = new();

                    int IsInMask;

                    if (MaskTableLocal[k].Count == 1)
                    { IsInMask = 0; }
                    else 
                    { IsInMask = MaskTableLocal[k].IndexOf(CharactersListLocal[i]); }

                    
                    if (IsInMask > -1)
                    {
                        if (MaskTableLocal[k].Count == 1)
                        {
                            //unary system is a little tricky, it has no zero, value is or not, except last position, where must be zero
                            //so value can be in 0 position
                            //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                            //because this value can be / is in upper char as 1.
                            if (i == 0 && k == 0) //unary zero in last position
                            { 
                                CharIntToMul = new() { 0 }; 
                            }
                            else
                            { CharIntToMul = new() { 1 }; } //rest of unaries, they has values
                            IsUnary = 1; //to inform converter, that there is unary
                        }
                        else
                        {
                            if (i < MaskTableLocal[k].Count)
                            { CharIntToMul = new() { i }; }
                            else
                            { CharIntToMul = new() { 1000 }; } // not possible i think, it goes to next else // must be bigger?????? it doesn't mater what is here, so it can be zero
                        }
                        List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, 9, PowerListLocal[k], 9, 9); //out na 18 i usunąć convert
                        Miki.CalcConvert.ConvertList9To18(in MultResult, out List<long> MultResult18);
                        ConvRow.Add(MultResult18);
                    }
                    else
                    { ConvRow.Add(new List<long> { -1 }); }

                }
                ConvTableLocal.Add(ConvRow);
            }
            ConvTable = ConvTableLocal;
            */

            List<long> AllUnarysValueLocal = new() { 0 };
            List<List<long>> AllUnarysValueListLocal = new();

            for (int CharPos = 0; CharPos < MaskTableLocal.Count; CharPos++)
            {
                if (MaskTableLocal[CharPos].Count == 1)
                {
                    AllUnarysValueLocal = Miki.CalcLists.Add(AllUnarysValueLocal, ConvTableLocal[CharPos][0]);
                    AllUnarysValueListLocal.Add(AllUnarysValueLocal);
                }
                else
                { AllUnarysValueListLocal.Add(AllUnarysValueLocal); }
            }
            AllUnarysValueList = AllUnarysValueListLocal;

            int LastDigLen2 = 0;
            long LastDigFromConv = 0;
            long ActDigFromConv = 0;
            List<List<long>> DigLenListLocal = new();
            //Concept, make long list to compare (DigLenList) similar to ConvTable, before compare with ConvTable,
            //it will contain XXXX...YYYY numbers, where
            //XXXX... full number length, YYYY - first digits from this number
            //look in PassListFromDigMyTab (variable ActDigToCompare)
            for (int CharPos = 0; CharPos < MaskTableLocal.Count; CharPos++)
            {
                List<long> RowLenList2 = new();
                for (int CharValue = 0; CharValue < MaskTableLocal[CharPos].Count; CharValue++)
                {
                        Miki.CalcIntExt.LongLength(ConvTableLocal[CharPos][CharValue][^1], out int DigLen); // First step to make XXXX....
                        Miki.CalcCompare.GetLongFromList(ConvTableLocal[CharPos][CharValue], 4, DigLen, out ActDigFromConv); //four digits to add later (YYYY)
                        DigLen = 18 * (ConvTableLocal[CharPos][CharValue].Count - 1) + DigLen; //Now we have XXXX, full digit length

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
                DigLenListLocal.Add(RowLenList2);
            }
            DigLenList = DigLenListLocal;

            string FirstPossiblePass = ""; // MaskTableLocal[0][0].ToString();
            string LastPossiblePass = "";

            for (int CharPos = PassMinLength - 1; CharPos>=0; CharPos--)
            {
                LastPossiblePass += MaskTableLocal[CharPos][^1];
            }

            for (int CharPos = PassMinLength - 1; CharPos >= 0; CharPos--)
            {
                FirstPossiblePass += MaskTableLocal[CharPos][0];
            }
            

            List<long> OnPosComb = new() { MaskTableLocal[0].Count };
            List<List<long>> OnPosCombList = new ();
            OnPosCombList.Add(OnPosComb);

            for (int i = 1; i < MaskTableLocal.Count; i++) //9 digit list
            {
                OnPosComb = Miki.CalcLists.Mul(OnPosCombList[i-1], 9, new List<long>() { MaskTableLocal[i].Count }, 9, 9); //out na 18 i usunąć następną pętlę
                OnPosCombList.Add(OnPosComb);
            }

            for (int i = 0; i < OnPosCombList.Count; i++) //Convert 9 digit list to 18 digit list
            {
                Miki.CalcConvert.ConvertList9To18(OnPosCombList[i], out List<long> OnPosComb18);
                OnPosCombList[i] = OnPosComb18;
            }
            List<List<long>> OnPosCombTotal = new();
            OnPosCombTotal.Add(OnPosCombList[0]);

            for (int i = 1; i < OnPosCombList.Count; i++) //Adding all 18 digit lists
            {
                OnPosCombTotal.Add(Miki.CalcLists.Add(OnPosCombList[i], OnPosCombTotal[i-1]));
            }
            List<long>TotalPossibleCountListLocal = new();

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
            string ProgressAllCountStringLocal = LastPossibleDecStringLocal; // Miki.CalcStrings.Sub(LastPossibleDec, "0")[0];
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
                    TotalPossibleCountListLocal = new (OnPosCombTotal[^1]);
                    for (int i = 0; i < PassMinLength-1; i++)
                    { TotalPossibleCountListLocal = Miki.CalcLists.Sub(TotalPossibleCountListLocal, OnPosCombList[i]); }
                }
                else 
                { TotalPossibleCountListLocal = OnPosCombTotal[^1]; } //ok
                Miki.CalcConvert.LongListToString(TotalPossibleCountListLocal, "000000000000000000", out TotalPossibleCountStringLocal);
            }

            //TotalPossibleCountString = TotalPossibleCountStringLocal;
            //FirstIterList = FirstPossibleDecList;
            //LastIterList = LastPossibleDecList;
            
            
            //Miki.CalcConvert.StringToLongList(in ProgressAllCountStringLocal, 18, out LastPossibleDecList);

            
            string FirstIsPossibleCount = Miki.CalcStrings.Add(LastPossibleDecStringLocal, "1")[0]; //Only for info
            Console.WriteLine();
            Console.WriteLine("If First possible Password is:             " + MaskTableLocal[0][0] + " ->");
            Console.WriteLine("First Base Password:                       " + MaskTableLocal[0][0]); //From Progress first password
            Console.WriteLine("-> First Base Decimal:                     " + "0"); //From Progress first decimal
            Console.WriteLine("Last Base Password:                        " + LastPossiblePass); //From Progress last password
            Console.WriteLine("-> Last Base Decimal:                      " + Miki.CalcStringExt.SplitString(LastPossibleDecStringLocal)); //From Progress last decimal
            Console.WriteLine("Base Iterations (Passwords) count:         " + Miki.CalcStringExt.SplitString(FirstIsPossibleCount)); //From Progress (LastBaseDec - FirstBaseDec)+1
            Console.WriteLine();
            Console.WriteLine("First possible Password:                   " + FirstPossiblePass + " ->");//Real first password
            Console.WriteLine("-> First possible Password in Decimal:     " + Miki.CalcStringExt.SplitString(FirstPossibleDecStringLocal)); //Real First decimal
            Console.WriteLine("Last possible Password:                    " + LastPossiblePass); //Real last password, equal to From Progress last password
            Console.WriteLine("-> Last possible Password in Decimal:      " + Miki.CalcStringExt.SplitString(LastPossibleDecStringLocal)); //Real last decimal equal to From Progress last decimal
            Console.WriteLine("Base Iterations (Passwords) count:         " + Miki.CalcStringExt.SplitString(RealPossibleCountStringLocal)); //Real to do = (LastPossibleDec-FirstPossibleDec) + 1

            Console.WriteLine("Total Full possible Passwords in Decimal:  " + Miki.CalcStringExt.SplitString(TotalPossibleCountStringLocal)); //Real all length passwords count
            Console.WriteLine();
            Console.WriteLine("Conversion tables - done...");
            Console.WriteLine();
        }

        public static void MakeConvTableOld()
        {
            Console.WriteLine("Conversion tables - generating...");
            //List<List<List<long>>> ConvTableLocal = new();
            //List<long> DivListLocal = DivList;
            List<char> CharactersListLocal = CharactersList;
            //List<List<int>> LenTableLocal = new();
            List<List<char>> MaskTableLocal = MaskTable; // to create polynomial numbering system
            List<List<List<long>>> ConvTableLocal = new();
            //string PassIterCountNFixLocal = TotalPossibleCountString;
            int PassMaxLenLocal = PassMaxLength;

            /////////NOWE
            List<List<long>> PowerListLocal = new();
            PowerListLocal.Add(new List<long>(1) { 1 });

            for (int i = 1; i < MaskTableLocal.Count; i++)
            {
                List<long> CharCount = new(1) { (long)MaskTableLocal[i - 1].Count };
                PowerListLocal.Add(Miki.CalcLists.Mul(PowerListLocal[i - 1], 9, CharCount, 9, 9));
            }
            /*
            string test = "";
            for (int i = 0; i < CharactersList.Count; i++)
            {
                test = test + CharactersList[i];
            }
            Console.WriteLine(test);
            */
            for (int i = 0; i < CharactersListLocal.Count; i++)
            {
                List<List<long>> ConvRow = new();
                for (int k = 0; k < PassMaxLenLocal; k++)
                {
                    List<long> CharIntToMul = new();

                    int IsInMask;

                    if (MaskTableLocal[k].Count == 1)
                    { IsInMask = 0; }
                    else
                    { IsInMask = MaskTableLocal[k].IndexOf(CharactersListLocal[i]); }


                    if (IsInMask > -1)
                    {
                        if (MaskTableLocal[k].Count == 1)
                        {
                            //unary system is a little tricky, it has no zero, value is or not, except last position, where must be zero
                            //so value can be in 0 position
                            //we must watch on each unary due conversion (MaskTableLocal[k].Count == 1)
                            //because this value can be / is in upper char as 1.
                            if (i == 0 && k == 0) //unary zero in last position
                            {
                                CharIntToMul = new() { 0 };
                            }
                            else
                            { CharIntToMul = new() { 1 }; } //rest of unaries, they has values
                            IsUnary = 1; //to inform converter, that there is unary
                        }
                        else
                        {
                            if (i < MaskTableLocal[k].Count)
                            { CharIntToMul = new() { i }; }
                            else
                            { CharIntToMul = new() { 1000 }; } // not possible i think, it goes to next else // must be bigger?????? it doesn't mater what is here, so it can be zero
                        }
                        List<long> MultResult = Miki.CalcLists.Mul(CharIntToMul, 9, PowerListLocal[k], 9, 9); //out na 18 i usunąć convert
                        Miki.CalcConvert.ConvertList9To18(in MultResult, out List<long> MultResult18);
                        ConvRow.Add(MultResult18);
                    }
                    else
                    { ConvRow.Add(new List<long> { -1 }); }

                }
                ConvTableLocal.Add(ConvRow);
            }
            ConvTable = ConvTableLocal;


            List<long> AllUnarysValueLocal = new() { 0 };
            List<List<long>> AllUnarysValueListLocal = new();

            for (int i = 0; i < MaskTableLocal.Count; i++)
            {
                if (MaskTableLocal[i].Count == 1)
                {
                    AllUnarysValueLocal = Miki.CalcLists.Add(AllUnarysValueLocal, ConvTableLocal[0][i]);
                    AllUnarysValueListLocal.Add(AllUnarysValueLocal);
                }
                else
                { AllUnarysValueListLocal.Add(AllUnarysValueLocal); }
            }
            AllUnarysValueList = AllUnarysValueListLocal;

            int LastDigLen2 = 0;
            long LastDigFromConv = 0;
            long ActDigFromConv = 0;
            List<List<long>> DigLenListLocal = new();
            //Concept, make long list to compare (DigLenList) similar to ConvTable, before compare with ConvTable,
            //it will contain XXXX...YYYY numbers, where
            //XXXX... full number length, YYYY - first digits from this number
            //look in PassListFromDigMyTab (variable ActDigToCompare)
            for (int k = 0; k < PassMaxLenLocal; k++)
            {
                List<long> RowLenList2 = new();
                for (int i = 0; i < CharactersListLocal.Count; i++)
                {
                    if (ConvTableLocal[i][k][^1] > -1)
                    {
                        Miki.CalcIntExt.LongLength(ConvTableLocal[i][k][^1], out int DigLen); // First step to make XXXX....
                        Miki.CalcCompare.GetLongFromList(ConvTableLocal[i][k], 4, DigLen, out ActDigFromConv); //four digits to add later (YYYY)
                        DigLen = 18 * (ConvTableLocal[i][k].Count - 1) + DigLen; //Now we have XXXX, full digit length

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
                    else
                    { RowLenList2.Add(-1); }
                }
                DigLenListLocal.Add(RowLenList2);
            }
            DigLenList = DigLenListLocal;

            string FirstPossiblePass = ""; // MaskTableLocal[0][0].ToString();
            string LastPossiblePass = "";

            for (int i = PassMaxLength - 1; i >= 0; i--)
            {
                LastPossiblePass += MaskTableLocal[i][^1];
            }

            for (int i = PassMinLength - 1; i >= 0; i--)
            {
                FirstPossiblePass += MaskTableLocal[i][0];
            }


            List<long> OnPosComb = new() { MaskTableLocal[0].Count };
            List<List<long>> OnPosCombList = new();
            OnPosCombList.Add(OnPosComb);

            for (int i = 1; i < MaskTableLocal.Count; i++) //9 digit list
            {
                OnPosComb = Miki.CalcLists.Mul(OnPosCombList[i - 1], 9, new List<long>() { MaskTableLocal[i].Count }, 9, 9); //out na 18 i usunąć następną pętlę
                OnPosCombList.Add(OnPosComb);
            }

            for (int i = 0; i < OnPosCombList.Count; i++) //Convert 9 digit list to 18 digit list
            {
                Miki.CalcConvert.ConvertList9To18(OnPosCombList[i], out List<long> OnPosComb18);
                OnPosCombList[i] = OnPosComb18;
            }
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
            string ProgressAllCountStringLocal = LastPossibleDecStringLocal; // Miki.CalcStrings.Sub(LastPossibleDec, "0")[0];
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

            //TotalPossibleCountString = TotalPossibleCountStringLocal;
            //FirstIterList = FirstPossibleDecList;
            //LastIterList = LastPossibleDecList;


            //Miki.CalcConvert.StringToLongList(in ProgressAllCountStringLocal, 18, out LastPossibleDecList);


            string FirstIsPossibleCount = Miki.CalcStrings.Add(LastPossibleDecStringLocal, "1")[0]; //Only for info
            Console.WriteLine();
            Console.WriteLine("If First possible Password is:             " + MaskTableLocal[0][0] + " ->");
            Console.WriteLine("First Base Password:                       " + MaskTableLocal[0][0]); //From Progress first password
            Console.WriteLine("-> First Base Decimal:                     " + "0"); //From Progress first decimal
            Console.WriteLine("Last Base Password:                        " + LastPossiblePass); //From Progress last password
            Console.WriteLine("-> Last Base Decimal:                      " + Miki.CalcStringExt.SplitString(LastPossibleDecStringLocal)); //From Progress last decimal
            Console.WriteLine("Base Iterations (Passwords) count:         " + Miki.CalcStringExt.SplitString(FirstIsPossibleCount)); //From Progress (LastBaseDec - FirstBaseDec)+1
            Console.WriteLine();
            Console.WriteLine("First possible Password:                   " + FirstPossiblePass + " ->");//Real first password
            Console.WriteLine("-> First possible Password in Decimal:     " + Miki.CalcStringExt.SplitString(FirstPossibleDecStringLocal)); //Real First decimal
            Console.WriteLine("Last possible Password:                    " + LastPossiblePass); //Real last password, equal to From Progress last password
            Console.WriteLine("-> Last possible Password in Decimal:      " + Miki.CalcStringExt.SplitString(LastPossibleDecStringLocal)); //Real last decimal equal to From Progress last decimal
            Console.WriteLine("Base Iterations (Passwords) count:         " + Miki.CalcStringExt.SplitString(RealPossibleCountStringLocal)); //Real to do = (LastPossibleDec-FirstPossibleDec) + 1

            Console.WriteLine("Total Full possible Passwords in Decimal:  " + Miki.CalcStringExt.SplitString(TotalPossibleCountStringLocal)); //Real all length passwords count
            Console.WriteLine();
            Console.WriteLine("Conversion tables - done...");
            Console.WriteLine();
        }
        public static List<string> PassListFromDigByTab(int PassToBeGen)
        {
            int PassMinLengthLocal = PassMinLength;
            int PassMaxLengthLocal = PassMaxLength;
            int FillToLengthLocal = FillToLength;
            List<string> PassTextListLocal = new(PassToBeGen);
            List<string> TempRemovedLocal = new();
            if (TempRemoved.Count > 0)
            {
                PassTextListLocal.AddRange(TempRemoved);
                TempRemoved.Clear();
            }

            int PassGeneratedBaseLocal = 0;
            long PassGeneratedFull = 0;
            long PassDiscardedCount = 0;
            int PassOnePercent = PassToBeGen / 100;
            int CounterDisplayLocal = CounterDisplay;
            if (PassOnePercent == 0) { PassOnePercent++; }
            Stopwatch LoopStopwatchFull;

            List<long> DigPassCurrListLocal = DigPassCurrList;
            List<long> DigPassNextListLocal = new();
            List<long> WorkingIterLocal = WorkingIter;

            //List<char> CharactersListLocal = CharactersList;
            //int CharactersListLocalCount = CharactersListLocal.Count;
            List<List<char>> MaskTableLocal = MaskTable;
            List<List<List<long>>> ConvTableLocal = ConvTable;
            List<List<long>> DigLenListLocal = DigLenList;

            List<List<long>> AllUnarysValueListLocal = AllUnarysValueList;
            int IsUnaryLocal = IsUnary;
            List<long> UnaryToCompare = new();

            List<long> IncrementList18Local = IncrementList18;
            List<long> IncrementList9Local = IncrementList9;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            List<long> RealPossibleCountListLocal = RealPossibleCountList;
            
            List<long> IterPlus = new() { 1 };
            
            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> ProgressPoolMaxListLocal = ProgressPoolMaxList;
            
            long LastDisplayCount = 0;

            int MyOut = 10;


            //check if pool limit in SessionProgress.txt is bigger then TotalBasePassCount list
            Miki.CalcCompare.ListBigger(in RealPossibleCountListLocal, in ProgressPoolMaxListLocal, out int BiggerPool);
            //here generator will change second value in SessionProgress.txt file (ProgressPoolMaxList)
            if (BiggerPool == 2)
            {
                ProgressPoolMaxListLocal = RealPossibleCountListLocal;
                ProgressPoolMaxList = ProgressPoolMaxListLocal;
            }

            //at start generator we must use ProgressIterToDoList18 to generate WorkingIter and WorkingDigList == DigPassCurListLocal
            //in main loop we can do it easier: WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, IncrementList18Local);
            WorkingIterLocal = Miki.CalcLists.Mul(ProgressIterToDoList18, 18, IncrementList9Local, 9, 18);
            //WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, FirstPossibleDecListLocal);
            List<long> WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecListLocal);
            DigPassCurrListLocal = new(WorkingDigList); //DigPassCurrList

            LoopStopwatchFull = Stopwatch.StartNew();
            int FirstNonZero = 0;
            int PoolEndCheck = 1;
            int TotalEndCheck = 1;
            while (true)
            {
                    int CharPosMax = PassMaxLengthLocal - 1;
                    List<long> ToCompare = DigPassCurrListLocal;
                    List<long> ToCompareFull = DigPassCurrListLocal;
                    List<long> UnarySubtracted = new();
                    int IsUnarySubtracted = 0;
                    int IsBigger = 10;
                    if (IsUnary == 1)
                    {
                        for (int i = CharPosMax; i > 0; i--)
                        {
                            UnaryToCompare = AllUnarysValueListLocal[i];
                            Miki.CalcCompare.ListBigger(in ToCompare, in UnaryToCompare, out IsBigger);
                            if (IsBigger != 2)
                            {
                                ToCompare = Miki.CalcLists.Sub(ToCompare, UnaryToCompare);
                                IsUnarySubtracted = 1;
                                break;
                            }
                        }
                    }
                    List<char> MyCharList = new(PassMaxLengthLocal); //new password in char
                    List<string> PassFromDigList = new(PassMaxLengthLocal); //new password in string

                    string NewPass;
                    FirstNonZero = 0;

                    while (CharPosMax >= 0)   //CharPosMax - char position in password, (CharInt-1) char in password
                    {
                        if (IsUnarySubtracted == 1 && MaskTableLocal[CharPosMax].Count == 1)
                        {
                            //List<long> UnaryToCompareFull = AllUnarysValueListLocal[CharPosMax];
                            //Miki.CalcCompare.ListBigger(in ToCompareFull, in UnaryToCompareFull, out int IsBiggerFull);
                            //if (IsBiggerFull != 2)
                            {
                                MyCharList.Add(MaskTableLocal[CharPosMax][0]);
                                FirstNonZero = 1;
                            }
                        }
                        else
                        {
                            Miki.CalcIntExt.LongLength(ToCompare[^1], out int ToCompareLen); //Last pos ToCompare length
                                                                                             //prepare XXXX...YYYY (ActDigToCompare) from ToCompare diglist
                            Miki.CalcCompare.GetLongFromList(in ToCompare, 4, in ToCompareLen, out long ActDigToCompare); //YYYY
                            ToCompareLen = 18 * (ToCompare.Count - 1) + ToCompareLen; //Full Length XXXX....
                            ActDigToCompare = (ToCompareLen * 10000) + ActDigToCompare; //Make 4 places and add ActDigToCompare -> XXXX...YYYY

                            int StartCompare = 0; //if ToCompareLen bigger than DigLenListLocal[CharPosMax][i] - Not possible???
                            int EndCompare = MaskTableLocal[CharPosMax].Count;
                            
                            for (int i = 0; i < MaskTableLocal[CharPosMax].Count; i++)
                            {
                                if (DigLenListLocal[CharPosMax][i] >= ActDigToCompare)
                                {
                                //EndCompare = i;
                                //if (i > 1)
                                //{ StartCompare = i - 1; }
                               // else 
                                //{ StartCompare = 0; }
                                
                                break;
                                }
                                StartCompare = i;
                            }
                        //if (StartCompare > EndCompare)
                        //{ }

                            /*
                        int Found = 0;
                        int ChPos = 0;
                        int ChVal = 0;
                        for (int CharPos = 0; CharPos < DigLenListLocal.Count; CharPos++)
                        {
                            for (int CharValue = 0; CharValue < DigLenListLocal[CharPos].Count; CharValue++)
                            {
                                if (DigLenListLocal[CharPos][CharValue] >= ActDigToCompare)
                                {
                                    Found++;
                                    ChPos = CharPos;
                                    ChVal = CharValue;
                                    break;
                                }
                            }
                            if (Found == 1)
                            { break; }
                        }
                            */
                        //CharPosMax = ChPos;
                        //if (ChVal == 0)
                        //{ StartCompare = 0; }
                        //else 
                        //{ StartCompare = ChVal - 1; }
                        
                            //all above is very fast, and now we have StartCompare value,
                            //so we don't need to search in all ConvTableLocal and do lots of calculation,
                            //moreover StartCompare is probably first and last position
                            //we must look at..., so next loop is as short as needed...
                            //_ = PassTextListLocal;
                            for (int CharInt = StartCompare; CharInt < EndCompare; CharInt++)
                            {
                                if (CharPosMax == 0)
                                { FirstNonZero = 1; }
                                Miki.CalcCompare.ListBigger(ConvTableLocal[CharPosMax][CharInt], in ToCompare, out MyOut);

                                if (MyOut == 1)
                                {
                                    int Char;
                                    if (CharInt > 1)
                                    { Char = CharInt - 1; }
                                    else
                                    { Char = 0; }
                                    if (Char > 0 || FirstNonZero == 1)
                                    {
                                        ToCompare = Miki.CalcLists.Sub(ToCompare, ConvTableLocal[CharPosMax][Char]);
                                        char CharToAdd = MaskTableLocal[CharPosMax][Char];
                                        MyCharList.Add(CharToAdd);
                                        FirstNonZero = 1;
                                    }
                                    break;
                                }
                                if (MyOut == 0)
                                {
                                    if (CharInt > 0 || FirstNonZero == 1)
                                    {
                                        ToCompare = Miki.CalcLists.Sub(ToCompare, ConvTableLocal[CharPosMax][CharInt]);
                                        char CharToAdd = MaskTableLocal[CharPosMax][CharInt];
                                        MyCharList.Add(CharToAdd);
                                        FirstNonZero = 1;
                                    }
                                    break;
                                }

                                if ((CharInt == EndCompare - 1) && (MyOut == 2))
                                {
                                    //if (CharInt > 0 || FirstNonZero == 1)
                                    {
                                        ToCompare = Miki.CalcLists.Sub(ToCompare, ConvTableLocal[CharPosMax][CharInt]);
                                        char CharToAdd = MaskTableLocal[CharPosMax][CharInt];
                                        MyCharList.Add(CharToAdd);
                                        FirstNonZero = 1;
                                    }
                                    break;
                                }
                            } //end loop for looking for character
                        }
                        CharPosMax--;
                    } //end loop for char position in password (from up)

                //here we have a password which is converted from decimal iteration, eg. XXXXX
                //we can fill now rest of characters, but characters must represent zeros -> YYYYYXXXXX
                //int FillToLength = 1; //if we want to fill

                if (FillToLengthLocal == 1)
                    {
                        //DiscardPass = 0; //to check, if character we want to add is zero
                        if (MyCharList.Count == PassMinLengthLocal) //Fixed or not, but first pass is ok
                        {
                            if (PassMaxLengthLocal == PassMinLengthLocal) //Fixed, first pass length ok
                            {
                            //we can always add first pass
                            
                                NewPass = new string(MyCharList.ToArray());
                                PassFromDigList.Add(NewPass);
                                PassGeneratedBaseLocal++;
                                PassGeneratedFull++;
                            }
                            else //Not fixed, first pass length is ok
                            {
                                //we can always add first pass
                                NewPass = new string(MyCharList.ToArray());
                                PassFromDigList.Add(NewPass);
                                PassGeneratedBaseLocal++;
                                PassGeneratedFull++;
                                //fill next characters
                                while (MyCharList.Count < PassMaxLengthLocal)//add to max length, and add to PassFromDigList
                                {
                                    if (MaskTableLocal[MyCharList.Count].Count > 1) //if next character is zero we can add, if not we must break adding
                                    {
                                        char ActZero = MaskTableLocal[MyCharList.Count][0]; //this is zero
                                        MyCharList.Insert(0, ActZero);
                                        NewPass = new string(MyCharList.ToArray());
                                        PassFromDigList.Add(NewPass);
                                        PassGeneratedFull++;
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
                                while (MyCharList.Count < PassMaxLengthLocal)
                                {
                                    if (MaskTableLocal[MyCharList.Count].Count > 1) //if next character is zero we can add, if not we must break adding
                                    {
                                        char ActZero = MaskTableLocal[MyCharList.Count][0]; //this is zero
                                        MyCharList.Insert(0, ActZero);
                                    }
                                    else
                                    { break; }
                                }

                                NewPass = new string(MyCharList.ToArray()); //finally add to passlist
                                PassFromDigList.Add(NewPass);
                                PassGeneratedBaseLocal++;
                                PassGeneratedFull++;

                            }
                            else //Not fixed
                            {
                                //fill to min length and add
                                while (MyCharList.Count < PassMinLengthLocal)//add to min length
                                {
                                    if (MaskTableLocal[MyCharList.Count].Count > 1) //if zero we can add
                                    {
                                        char ActZero = MaskTableLocal[MyCharList.Count][0];
                                        MyCharList.Insert(0, ActZero);
                                    }
                                    else 
                                    { break; } // break if no zero

                                }

                                NewPass = new string(MyCharList.ToArray()); //first pass with min length, we can add
                                PassFromDigList.Add(NewPass);
                                PassGeneratedBaseLocal++;
                                PassGeneratedFull++;

                                //fill from min to max, then add
                                while (MyCharList.Count < PassMaxLengthLocal)
                                {
                                    if (MaskTableLocal[MyCharList.Count].Count > 1)
                                    {
                                        char ActZero = MaskTableLocal[MyCharList.Count][0];
                                        MyCharList.Insert(0, ActZero);
                                        NewPass = new string(MyCharList.ToArray());
                                        PassFromDigList.Add(NewPass);
                                        PassGeneratedFull++;
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
                        NewPass = new string(MyCharList.ToArray());
                        PassFromDigList.Add(NewPass);
                        PassGeneratedBaseLocal++;
                        PassGeneratedFull++;
                    }

                    int PassFromDigListCount = PassFromDigList.Count; //All pass count in PassFromDigList, we can add now
                    if (PassFromDigListCount > 0)
                    {
                        //if added to output list, we can count all passes
                        PassTextListLocal.AddRange(PassFromDigList);
#if DEBUG
                        //Miki.CalcConvert.LongListToString(DigPassCurrListLocal, "000000000000000000", out string Test);
                        //for (int i = 0; i < PassFromDigList.Count; i++)
                        //{ Console.WriteLine(Test + "  " + PassFromDigList[i]); }
#endif
                    }

                //here we have done all password from current iteration, so we must add 1 to ProgressIterToDoListLocal
                ProgressIterToDoListLocal = Miki.CalcLists.Add(ProgressIterToDoListLocal, IterPlus);
                //Check if Pool Ends, if ToDo biger PoolMax
                Miki.CalcCompare.ListBigger(in ProgressPoolMaxListLocal, in ProgressIterToDoListLocal, out PoolEndCheck);
                if (PoolEndCheck == 2)
                {
                    GeneratorExitReason = "Pool";
                }

                //NEW NUMBER (seed)
                //Console.WriteLine(WorkingIterLocal[0]);
                WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, IncrementList18Local);
                WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountListLocal)[1];
                WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecListLocal);
                DigPassNextListLocal = new(WorkingDigList);
                //NEW NUMBER DONE...

                //Check if Total End. If Increment is good, then return to seed=0 (ThisDigPassNextList) means that all iteration are done....
                Miki.CalcCompare.ListBigger(in DigPassNextListLocal, in FirstPossibleDecListLocal, out TotalEndCheck);
                if (TotalEndCheck == 0)
                {
                    GeneratorExitReason = "Total";
                }

                if (PassTextListLocal.Count > PassToBeGen && PoolEndCheck != 2 && TotalEndCheck != 0) //to maintain the same length of each wordlist, only if not GeneratorExitReason is Pool or Total
                {
                    int ToRemoveCount = PassTextListLocal.Count - PassToBeGen;
                    TempRemoved.AddRange(PassTextListLocal[PassToBeGen..]); //TempRemoved will be added in next PassListFromDigByTab run
                    PassTextListLocal.RemoveRange(PassToBeGen, ToRemoveCount); //Remove TempRemoved from PassTextListLocal
                }

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
                        long SpeedFull = (PassGeneratedFull * 1000000000 / TimeNsFTotal);

                        string PassGeneratedString = string.Format("B:{0} F:{1} D:{2} {3} %  in  h:{4} min:{5} sec:{6} ms:{7} SpeedB:{8} SpeedF:{9} /s", PassGeneratedBaseLocal, PassGeneratedFull, PassDiscardedCount, PassPercent, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                    LoopStopwatchFull.Start();
                }
                if (PassTextListLocal.Count == PassToBeGen || PoolEndCheck == 2 || TotalEndCheck == 0) //PassGeneratedBaseLocal
                {
                    LoopStopwatchFull.Stop();
                    ProgressIterToDoList18 = ProgressIterToDoListLocal;
                    Miki.CalcConvert.LongListToString(in ProgressIterToDoListLocal, "000000000000000000", out ProgressIterToDoString);
                    //here ToDo is bigger than Max and we will not use ToDo because we break, so we must subtract 1
                    ProgressIterDoneList18 = Miki.CalcLists.Sub(ProgressIterToDoListLocal, IterPlus);
                    Miki.CalcConvert.LongListToString(in ProgressIterDoneList18, "000000000000000000", out ProgressIterDoneString);

                    //DigPassCurrListLocal = DigPassNextListLocal;
                    DigPassCurrList = DigPassCurrListLocal;
                    //ProgressIterToDoList18 = DigPassNextListLocal;
                    long TimeNsFTotal = (long)LoopStopwatchFull.Elapsed.TotalNanoseconds;
                    long TimeMsF = LoopStopwatchFull.Elapsed.Milliseconds;
                    long TimeSecF = LoopStopwatchFull.Elapsed.Seconds;
                    long TimeMinF = LoopStopwatchFull.Elapsed.Minutes;
                    long TimeHourF = LoopStopwatchFull.Elapsed.Hours;

                    long SpeedBase = ((long)PassGeneratedBaseLocal * 1000000000 / TimeNsFTotal);
                    long SpeedFull = (PassGeneratedFull * 1000000000 / TimeNsFTotal);

                    string PassGeneratedString = string.Format("B:{0} F:{1} D:{2} {3} %  in  h:{4} min:{5} sec:{6} ms:{7} SpeedB:{8} SpeedF:{9} /s", PassGeneratedBaseLocal, PassGeneratedFull, PassDiscardedCount, 100, TimeHourF, TimeMinF, TimeSecF, TimeMsF, SpeedBase, SpeedFull);
                    Console.Write("\r{0}   ", PassGeneratedString);
                    PassGeneratedBase = PassGeneratedBaseLocal;
                    return PassTextListLocal;
                }
                DigPassCurrListLocal = DigPassNextListLocal;

            }
        }

        public static void GetDigPassListNext(in List<long> ProgressIterDoneList, out List<long> ProgressIterToDoList)
        {
            List<long> WorkingIterLocal = Miki.CalcLists.Mul(ProgressIterDoneList, 18, IncrementList9, 9, 18);
            WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, FirstPossibleDecList);
            List<long> WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountList)[1];
            WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecList);
            ProgressIterToDoList = new(WorkingDigList); //DigPassCurrList
        }

        
        public static void DigFromPassByTab(in string MyPass, out string DigString)
        {
            //convert n-char password to string-number
            List<char> CharList = new(MyPass);
            int k = 0;
            //List<char> CharactersListLocal = CharactersList;
            List<List<char>> MaskTableLocal = MaskTable;

            List<long> DigList = new List<long>() { 0 };
            List<List<List<long>>> ConvTableLocal = ConvTable;

            for (int CharPos = CharList.Count - 1; CharPos >= 0; CharPos--)
            {
                int CharValue = MaskTableLocal[CharPos].IndexOf(CharList[k]);
                List<long> CharValueList = ConvTableLocal[CharPos][CharValue];
                DigList = Miki.CalcLists.Add(DigList, CharValueList);
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

            Stopwatch StopwatchGenerator;
            Stopwatch StopwatchCheckList;
            Stopwatch StopwatchControl;
            Stopwatch StopwatchGenFull = Stopwatch.StartNew();
            StopwatchGenFull.Stop();
            List<long> IncrementList9Local = IncrementList9;
            List<long> FirstPossibleDecListLocal = FirstPossibleDecList;
            List<long> RealPossibleCountListLocal = RealPossibleCountList;
            List<long> ProgressIterToDoListLocal = ProgressIterToDoList18;
            List<long> PassDigListAct = DigPassCurrList;

            List<long> IterPlus = new() { 1 };

            try
            {
                Console.WriteLine("Warming up...");
                List<string> PassTestList = PassListFromDigByTab(LoopCount*5); //build List of Passwords
                for (int i = 0; i < PassTestList.Count; i++)
                { DigFromPassByTab(PassTestList[i], out string Warming); }
                Console.WriteLine("");
                Console.WriteLine("Let's start...");
                Console.WriteLine("");
                ProgressIterToDoList18 = ProgressIterToDoListLocal;
                DigPassCurrList = PassDigListAct;
            }
            catch { }

            List<long> WorkingIterLocal = Miki.CalcLists.Mul(ProgressIterToDoListLocal, 18, IncrementList9Local, 9, 18);
            WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, FirstPossibleDecListLocal);
            List<long> WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountListLocal)[1];
            WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecListLocal);
            PassDigListAct = new(WorkingDigList); //DigPassCurrList

            while (true)
            {
                PassDigListAct = DigPassCurrList;

                //Generate PassTestList
                StopwatchGenerator = Stopwatch.StartNew();
                StopwatchGenFull.Start();
                List<string> PassTestList = PassListFromDigByTab(LoopCount); //build List of Passwords
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
                    string TempPassString;
                    for (int i = 0; i < PassTestList.Count; i++)
                    {
                        Miki.CalcConvert.LongListToString(in PassDigListAct, "000000000000000000", out string PassDigListActS);
                        TempPassString = PassTestList[i];
                        string DigString;

                        DigFromPassByTab(TempPassString, out DigString); //change each password to dig
                        int pos = 0;
                        while (DigString == PassDigListActS) //if non fixed length
                        {
                            List<string> TempPassStringPair = new(3) { TempPassString, PassDigListActS, DigString };
                            ControlDigTest.Add(TempPassStringPair);

                            if (i + pos < PassTestList.Count - 1)
                            { pos++; }
                            else
                            { break; }
                            TempPassString = PassTestList[i + pos];
                            DigFromPassByTab(TempPassString, out DigString); //change each password to dig
                        }

                        //GetDigPassListNext(PassDigListAct, out List<long> PassDigListActTemp);

                        ///////
                        ProgressIterToDoListLocal = Miki.CalcLists.Add(ProgressIterToDoListLocal, IterPlus);
                        WorkingIterLocal = Miki.CalcLists.Mul(ProgressIterToDoListLocal, 18, IncrementList9Local, 9, 18);
                        WorkingIterLocal = Miki.CalcLists.Add(WorkingIterLocal, FirstPossibleDecListLocal);
                        WorkingDigList = Miki.CalcLists.Div(WorkingIterLocal, RealPossibleCountListLocal)[1];
                        WorkingDigList = Miki.CalcLists.Add(WorkingDigList, FirstPossibleDecListLocal);
                        PassDigListAct = new(WorkingDigList); //DigPassCurrList
                        /////////

                        //PassDigListAct = PassDigListActTemp;
                    }
                    StopwatchCheckList.Stop();
                    GenTimeCheckMs = (long)StopwatchCheckList.Elapsed.TotalMilliseconds;
                    //End geneerate CheckList

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
                    Console.WriteLine(string.Format("{0}  {1}  ({2})  {3}  PassGenTime: {4}  CheckGenTime: {5}  CheckTime: {6}  ms  FullSpeed: {7}  Errors: {8}", ThisCounterBase, ThisCounterFull, PassTestList.Count, PassTestList[0], GenTimePassMs, GenTimeCheckMs, CheckTimeMs, FullSpeed, Errors));
                }
                else
                {
                    long FullSpeed = ThisCounterFull * 1000 / GenTimeFull;
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("{0}  {1}  ({2})  {3}  PassGenTime: {4}  CheckGenTime: {5}  CheckTime: {6}  ms  FullSpeed: {7}  Errors: {8}", ThisCounterBase, ThisCounterFull, PassTestList.Count, PassTestList[0], GenTimePassMs, "N/A", "N/A", FullSpeed, Errors));
                }
                if (GeneratorExitReason == "Total")
                { Console.WriteLine("Total pool checked, exit..."); break; }
                if (GeneratorExitReason == "Pool")
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
                { Dig1 = Dig1[1..]; }

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
                { Dig2 = Dig2[1..]; }

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
    }

    public static class CalcLists
    {

        public static List<long> AddOld(List<long> Dig1List, List<long> Dig2List)
        {

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }

            List<long> MyOutput = new(Dig1ListCount + 1);

            int DigCountDiff = Dig1ListCount - Dig2ListCount;

            long tempUp = 0;
            long temp;

            int loops = Dig1ListCount - 1;

            switch (DigCountDiff)
            {
                case > 0:
                    Dig2List = new(Dig2List);
                    for (int i = 0; i < DigCountDiff; i++)
                    { Dig2List.Add(0); }
                    break;
                case < 0:
                    Dig1List = new(Dig1List);
                    for (int i = 0; i > DigCountDiff; i--)
                    { Dig1List.Add(0); }
                    loops = Dig2ListCount - 1;
                    break;
            }

            for (int i = 0; i <= loops; i++) //adding Lists
            {
                temp = Dig1List[i] + Dig2List[i] + tempUp;

                if (temp >= 1000000000000000000)
                {
                    tempUp = 1;
                    temp -= 1000000000000000000;
                    MyOutput.Add(temp);
                    if (i == loops)
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
            return MyOutput;
        }
        

        public static List<long> Add(List<long> Dig1List, List<long> Dig2List)
        {
            int Dig1ListCount = Dig1List.Count;
            int Dig2ListCount = Dig2List.Count;
            //int DigListCount = Dig1List.Count;
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
                        if (BiggerDig == 2)
                        {
                            MyOutput.AddRange(Dig2List[i..]); break;
                        }
                        if (BiggerDig == 0)
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

        public static List<long> Sub(List<long> Dig1List, List<long> Dig2List)
        {
            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }

            CalcCompare.ListBigger(in Dig1List, in Dig2List, out int DigBigger);

            int MaxLoops = Dig1ListCount;
            int MinLoops = Dig2ListCount;

            if (DigBigger == 0)
            { return new List<long>(1) { 0 }; }

            if (DigBigger == 2)
            {
                //List<long> DigChangeTemp = new(Dig1List);
                //Dig1List = new(Dig2List);
                //Dig2List = new(DigChangeTemp);
                MaxLoops = Dig2ListCount;
                MinLoops = Dig1ListCount;
                //DigBigger = 1;
            }

            int i = 0;
            int debt = 0;
            long temp;
            List<long> MyOutput = new(MaxLoops);

            while (true)
            {
                if (i < MinLoops)
                {
                    if (DigBigger == 1)
                    { temp = (Dig1List[i] - Dig2List[i]) - debt; }
                    else
                    { temp = (Dig2List[i] - Dig1List[i]) - debt; }

                    if (temp < 0)
                    {
                        debt = 1;
                        MyOutput.Add(temp + 1000000000000000000);
                    }
                    else
                    {
                        debt = 0;
                        MyOutput.Add(temp);
                        if (i == MaxLoops - 1)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (debt == 0)
                    {
                        if (DigBigger == 1)
                        {
                            MyOutput.AddRange(Dig1List[i..]); break;
                        }
                        else
                        //if (DigBigger == 2)
                        {
                            MyOutput.AddRange(Dig2List[i..]); break;
                        }
                        /*
                        if (DigBigger == 0)
                        {
                            break;
                        }
                        */
                    }
                    else
                    {
                        if (DigBigger == 1)
                        {
                            temp = Dig1List[i] - debt;
                        }
                        else
                        {
                            temp = Dig2List[i] - debt;
                        }
                        
                        if (temp < 0)
                        {
                            debt = 1;
                            MyOutput.Add(temp + 1000000000000000000);
                        }
                        else
                        {
                            debt = 0;
                            MyOutput.Add(temp);
                        }
                    }
                }
                i++;
            }

            while (MyOutput[^1] == 0 && MyOutput.Count > 1)
            {
                MyOutput.RemoveAt(MyOutput.Count - 1);
            }

            return MyOutput;
        }

        public static List<long> SubCh(List<long> Dig1List, List<long> Dig2List)
        {
            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }

            CalcCompare.ListBigger(in Dig1List, in Dig2List, out int DigBigger);

            int MaxLoops = Dig1ListCount;
            int MinLoops = Dig2ListCount;

            if (DigBigger == 0)
            { return new List<long>(1) { 0 }; }

            if (DigBigger == 2)
            {
                List<long> DigChangeTemp = new(Dig1List);
                Dig1List = new(Dig2List);
                Dig2List = new(DigChangeTemp);
                MaxLoops = Dig2ListCount;
                MinLoops = Dig1ListCount;
                //DigBigger = 1;
            }

            int i = 0;
            int debt = 0;
            long temp;
            List<long> MyOutput = new(MaxLoops);

            while (true)
            {
                if (i < MinLoops)
                {
                    temp = (Dig1List[i] - Dig2List[i]) - debt;

                    if (temp < 0)
                    {
                        debt = 1;
                        MyOutput.Add(temp + 1000000000000000000);
                    }
                    else
                    {
                        debt = 0;
                        MyOutput.Add(temp);
                        if (i == MaxLoops - 1)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (debt == 0)
                    {
                        //if (DigBigger == 1)
                        {
                            MyOutput.AddRange(Dig1List[i..]); break;
                        }
                        /*
                        if (DigBigger == 2)
                        {
                            MyOutput.AddRange(Dig2List[i..]); break;
                        }
                        if (DigBigger == 0)
                        {
                            break;
                        }
                        */
                    }
                    else
                    {
                        //if (DigBigger == 1)
                        {
                            temp = Dig1List[i] - debt;
                        }
                        /*
                        else
                        {
                            temp = Dig2List[i] - debt;
                        }
                        */
                        if (temp < 0)
                        {
                            debt = 1;
                            MyOutput.Add(temp + 1000000000000000000);
                        }
                        else
                        {
                            debt = 0;
                            MyOutput.Add(temp);
                        }
                    }
                }
                i++;
            }

            while (MyOutput[^1] == 0 && MyOutput.Count > 1)
            {
                MyOutput.RemoveAt(MyOutput.Count - 1);
            }

            return MyOutput;
        }
        public static List<long> SubOld(List<long> Dig1List, List<long> Dig2List)
        {
            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }

            int debt = 0;
            long temp;

            int DigCountDiff = Dig1ListCount - Dig2ListCount;
            CalcCompare.ListBigger(in Dig1List, in Dig2List, out int DigBigger);

            int loops = Dig1ListCount - 1;

            switch (DigBigger)
            {
                case 1:
                    Dig2List = new(Dig2List);
                    for (int i = 0; i < DigCountDiff; i++)
                    { Dig2List.Add(0); }
                    break;
                case 2:
                    List<long> DigChangeTemp = new(Dig1List);
                    for (int i = 0; i > DigCountDiff; i--)
                    { DigChangeTemp.Add(0); }
                    Dig1List = new(Dig2List);
                    Dig2List = new(DigChangeTemp);
                    loops = Dig2ListCount - 1;
                    break;
            }
            List<long> MyOutput = new(loops + 1);

            for (int i = 0; i <= loops; i++) //Subtract arrays
            {
                temp = (Dig1List[i] - Dig2List[i]) - debt;

                if (temp < 0)
                {
                    debt = 1;
                    MyOutput.Add(temp + 1000000000000000000);
                }
                else
                {
                    debt = 0;
                    MyOutput.Add(temp);
                }

            }

            while (MyOutput[^1] == 0 && MyOutput.Count > 1)
            {
                MyOutput.RemoveAt(MyOutput.Count - 1);
            }

            return MyOutput;
        }

        public static List<long> Mul(List<long> Dig1List, int Dig1ListFrag, List<long> Dig2List, int Dig2ListFrag, int ProductFrag)
        {
            //list of 18

            List<long> MyOutput;
            List<long> ResultList = new();
            bool IsResult = false;
            int OutFrag = 0;

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1)
            {
                if (Dig1List[0] == 1)
                { ResultList = Dig2List; IsResult = true; OutFrag = Dig2ListFrag; }
                if (Dig1List[0] == 0)
                { ResultList = Dig1List; IsResult = true; OutFrag = Dig1ListFrag; }

            }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1)
            {
                if (Dig2List[0] == 1)
                { ResultList = Dig1List; IsResult = true; OutFrag = Dig1ListFrag; }
                if (Dig2List[0] == 0)
                { ResultList = Dig2List; IsResult = true; OutFrag = Dig2ListFrag; }
            }

            if (IsResult == false)
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
                int Loop1Count = Dig1ListCount;
                int Loop2Count = Dig2ListCount;

                if (Dig1ListCount < Dig2ListCount)
                {
                    List<long> DigChangeTemp = (Dig1List9);
                    Dig1List9 = new(Dig2List9);
                    Dig2List9 = new(DigChangeTemp);
                    Loop1Count = Dig2ListCount;
                    Loop2Count = Dig1ListCount;
                }

                int ResultCount = (Dig1ListCount + Dig2ListCount) - 1;
                ResultList = new(new long[ResultCount]); // Product (ResultList) can be max Dig1List.Count + Dig2List.Count digit long or 1 less

                for (int i = 0; i < Loop2Count; i++)
                {
                    for (int k = 0; k < Loop1Count; k++)
                    {
                        long temp = (Dig2List9[i] * Dig1List9[k]) + ResultList[MyPoss];
                        if (temp >= 1000000000)
                        {
                            CalcIntExt.MyModuloLongToLong(temp, 1000000000, out tempUp, out temp);

                            if (MyPoss + 1 == ResultCount)
                            { ResultList.Add(tempUp); }
                            else
                            { ResultList[MyPoss + 1] += tempUp; }
                        }

                        ResultList[MyPoss] = temp;

                        MyPoss += 1;
                    }

                    MyPoss = i + 1;
                }

                OutFrag = 9;
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

        public static List<long> MulOld(List<long> Dig1List, List<long> Dig2List)
        {
            //list of 9
            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1)
            {
                if (Dig1List[0] == 1)
                { return Dig2List; }
                if (Dig1List[0] == 0)
                { return Dig1List; }

            }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1)
            {
                if (Dig2List[0] == 1)
                { return Dig1List; }
                if (Dig2List[0] == 0)
                { return Dig2List; }
            }

            int MyPoss = 0;


            long tempUp;
            int Loop1Count = Dig1ListCount;
            int Loop2Count = Dig2ListCount;

            if (Dig1ListCount < Dig2ListCount)
            {
                List<long> DigChangeTemp = (Dig1List);
                Dig1List = new(Dig2List);
                Dig2List = new(DigChangeTemp);
                Loop1Count = Dig2ListCount;
                Loop2Count = Dig1ListCount;
            }

            int ResultCount = (Dig1ListCount + Dig2ListCount) - 1;
            List<long> ResultList = new(new long[ResultCount]); // Product (ResultList) can be max Dig1List.Count + Dig2List.Count digit long or 1 less

            for (int i = 0; i < Loop2Count; i++)
            {
                for (int k = 0; k < Loop1Count; k++)
                {
                    long temp = (Dig2List[i] * Dig1List[k]) + ResultList[MyPoss];
                    if (temp >= 1000000000)
                    {
                        CalcIntExt.MyModuloLongToLong(temp, 1000000000, out tempUp, out temp);

                        if (MyPoss + 1 == ResultCount)
                        { ResultList.Add(tempUp); }
                        else
                        { ResultList[MyPoss + 1] += tempUp; }
                    }

                    ResultList[MyPoss] = temp;

                    MyPoss += 1;
                }

                MyPoss = i + 1;
            }
            return ResultList;

        }
        public static List<List<long>> Div(List<long> Dig1List, List<long> Dig2List)
        {
            //Best with MySafeMultiplier, long digs, long multiplier
            List<List<long>> MyOutput = new(2);
            int Dig1ListCount = Dig1List.Count;
            int Dig2ListCount = Dig2List.Count;
            //int Result = 0;
            if (Dig1ListCount < Dig2ListCount) // fast pass
            {
                MyOutput.Add(new List<long>(1) { 0 });
                MyOutput.Add(Dig1List);

            }

            if (Dig1ListCount == 1 && Dig2ListCount == 1) // sometimes we can simply divide two longs - fast pass
            {
                if (Dig2List[0] != 0)
                {
                    CalcIntExt.MyModuloLongToLong(Dig1List[0], Dig2List[0], out long Full, out long MyMod);
                    MyOutput.Add(new List<long>(1) { Full });
                    MyOutput.Add(new List<long>(1) { MyMod });
                }
                else
                {
                    MyOutput.Add(new List<long>(1) { 0 }); //error Div by 0, but I don't care
                    MyOutput.Add(new List<long>(1) { 0 });
                    Console.WriteLine("ERROR - Div by 0 - Return: 0");
                }
                return MyOutput;
            }


            int Dig2ListFirstLength;
            int Dig2ListLength;

            int Dig1ListFirstLength;
            int Dig1ListLength;

            int BiggerList = 1;
            long Dig1FirstDig = Dig1List[^1];
            CalcIntExt.LongLength(Dig2List[^1], out Dig2ListFirstLength); //static
            Dig2ListLength = (Dig2ListCount - 1) * 18 + Dig2ListFirstLength; //static

            CalcIntExt.LongLength(in Dig1FirstDig, out Dig1ListFirstLength); //must be counted in each loop and it is necessary here
            Dig1ListLength = (Dig1ListCount - 1) * 18 + Dig1ListFirstLength; //must be counted in each loop and it is necessary here

            if (Dig1ListLength < Dig2ListLength)
            {
                MyOutput.Add(new List<long>(1) { 0 });
                MyOutput.Add(Dig1List);
                return MyOutput;
            }

            if (Dig1ListLength == Dig2ListLength)
            {
                CalcCompare.ListBigger(in Dig1List, in Dig2List, out BiggerList);

                switch (BiggerList)
                {
                    case 0:
                        MyOutput.Add(new List<long>(1) { 1 });
                        MyOutput.Add(new List<long>(1) { 0 });
                        return MyOutput;
                    case 2:
                        MyOutput.Add(new List<long>(1) { 0 });
                        MyOutput.Add(Dig1List);
                        return MyOutput;
                }

            }

            if (Dig1ListLength < 29 && Dig2ListLength < 29)
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

                MyOutput.Add(LongResult);
                MyOutput.Add(LongRest);
                return MyOutput;
            }

            List<long> TempDig2List18;
            long First17DigDig2;
            CalcConvert.ConvertList18To9(in Dig2List, out List<long> Dig2OrigList9); //static, necessary for multiplication

            //prepare number from Dig2 to estimate multiplier, take 17 digits
            CalcCompare.GetLongFromList(in Dig2List, 17, in Dig2ListFirstLength, out First17DigDig2); //static, necessary to find "safe multiplier"
            if (Dig2ListLength > 17) //if full digit was taken
            {
                First17DigDig2 += 1; //only if we cant't get full divisor
            }

            List<long> MultiplyList = new() { 0 };

            while (BiggerList < 2) //division by repeated subtraction
            {
                List<long> SafeMultiplierList18;

                if (Dig1ListLength < 29 && Dig2ListLength < 29) //sometimes we can simply calculate on decimals at the end
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
                    else
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
                }
                else //but if numbers are bigger then decimal
                {
                    MySafeMultiplier(in Dig1List, in Dig1FirstDig, in Dig2OrigList9, in First17DigDig2, in Dig1ListLength, in Dig1ListFirstLength, in Dig2ListLength, out List<List<long>> MySafeMultiplierResult);
                    TempDig2List18 = MySafeMultiplierResult[0];
                    SafeMultiplierList18 = MySafeMultiplierResult[1];
                }

                Dig1List = CalcLists.Sub(Dig1List, TempDig2List18);
                MultiplyList = (CalcLists.Add(MultiplyList, SafeMultiplierList18)); ;
                //And we must check if we can break calculation. Checking is "time waster", it's the best place for it
                CalcCompare.ListBigger(in Dig1List, in Dig2List, out BiggerList);

                if (BiggerList == 2)
                { break; }
                Dig1FirstDig = Dig1List[^1];
                CalcIntExt.LongLength(in Dig1FirstDig, out Dig1ListFirstLength); //must be counted in each loop
                Dig1ListLength = (Dig1List.Count - 1) * 18 + Dig1ListFirstLength; //must be counted in each loop

            }

            MyOutput = new(2) { MultiplyList, Dig1List };
            return MyOutput;
        }

        public static void MySafeMultiplier(in List<long> Dig1List18, in long Dig1FirstDig, in List<long> Dig2List9, in long First17DigDig2, in int Dig1ListLength, in int Dig1List18FirstLength, in int Dig2ListLength, out List<List<long>> MyOutput)
        {
            List<long> TempDig2List18;
            List<long> TempDig2List9;
            long SafeMultiplier;
            long First18DigDig1;
            List<long> SafeMultiplierList9;

            CalcCompare.GetLongFromList(in Dig1List18, 18, in Dig1List18FirstLength, out First18DigDig1);

            if (Dig1ListLength > 18)
            {
                First18DigDig1 -= 1; //Only if we cant't get full divident
            }

            SafeMultiplier = First18DigDig1 / First17DigDig2;

            while (true)
            {
                if (Dig2ListLength == Dig1ListLength) // This is necessary.
                {
                    SafeMultiplier /= 10;
                    if (SafeMultiplier == 0) { SafeMultiplierList9 = new(1) { 1 }; break; }
                    { SafeMultiplierList9 = new(1) { SafeMultiplier }; break; }

                }

                if (SafeMultiplier >= 1000000000) //faster
                {
                    CalcIntExt.MyModuloLongToLong(SafeMultiplier, 1000000000, out long Full, out long MyMod);
                    SafeMultiplierList9 = new(2) { MyMod, Full }; break;
                }
                else
                { SafeMultiplierList9 = new(1) { SafeMultiplier }; break; }
            }

            TempDig2List9 = (CalcLists.Mul(Dig2List9, 9, SafeMultiplierList9, 9, 9));
            CalcIntExt.LongLength(TempDig2List9[^1], out int LenToAdd);
            int TempDig2ListLength = (TempDig2List9.Count - 1) * 9 + LenToAdd; //must be counted in each loop

            int ZerosToAdd = Dig1ListLength - TempDig2ListLength;


            if (ZerosToAdd > 0) //if 0 then we multiply by 1
            {

                long Temp2DigFirst = TempDig2List9[^1];
                CalcIntExt.LongLength(in Temp2DigFirst, out int TempDig2List9First);
                CalcCompare.GetLongDigFromDig(in Temp2DigFirst, 1, in TempDig2List9First, out long FirstTempDig2List);
                CalcCompare.GetLongDigFromDig(in Dig1FirstDig, 1, in Dig1List18FirstLength, out long FirstDig1List);

                if (FirstDig1List - FirstTempDig2List < 0)
                { ZerosToAdd -= 1; }

                CalcIntExt.MyModuloIntToInt(in ZerosToAdd, 9, out int ZerosD, out int ZerosU);
                CalcIntExt.LongPower(10, in ZerosU, out long Result);
                List<long> MultiplyBy10List = new(1) { Result };

                List<long> TempDig2List9Temp = CalcLists.Mul(TempDig2List9, 9, MultiplyBy10List, 9, 9); 
                List<long> SafeMultiplierList9Temp = CalcLists.Mul(SafeMultiplierList9, 9, MultiplyBy10List, 9, 9);

                

                if (ZerosD > 0)
                {
                    TempDig2List9 = new(TempDig2List9Temp.Count + ZerosD);
                    SafeMultiplierList9 = new(SafeMultiplierList9Temp.Count + ZerosD);
                    List<long> ZerosAddList = new(ZerosD);
                    for (int t = 0; t < ZerosD; t++)
                    {
                        ZerosAddList.Add(0);
                    }
                    TempDig2List9.AddRange(ZerosAddList);
                    TempDig2List9.AddRange(TempDig2List9Temp);
                    SafeMultiplierList9.AddRange(ZerosAddList);
                    SafeMultiplierList9.AddRange(SafeMultiplierList9Temp);
                }
                else
                {
                    //TempDig2List9.AddRange(TempDig2List9Temp);
                    //SafeMultiplierList9.AddRange(SafeMultiplierList9Temp);
                    TempDig2List9=TempDig2List9Temp;
                    SafeMultiplierList9=SafeMultiplierList9Temp;

                }
            }

            CalcConvert.ConvertList9To18(in TempDig2List9, out TempDig2List18);
            CalcConvert.ConvertList9To18(in SafeMultiplierList9, out List<long> SafeMultiplierList18);

            MyOutput = new(2) { TempDig2List18, SafeMultiplierList18 };
        }
        public static List<long> Pow(List<long> Dig1List, int Dig1ListFrag, List<long> Dig2List, int ExpFrag)
        {
            //You can write condition to calculate power with proper result sign
            //or remember, if -Dig1Orig and Dig2Orig % 2 != 0 ResultSign = "-"
            int Dig2ListCount = Dig2List.Count;

            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            {
                List<long> MyOutput1 = new(1) { 1 };
                return MyOutput1;
            }

            bool IsResult = false;
            int OutFrag = 0;

            List<long> MyOutputList = new();

            if (Dig2ListCount == 1 && Dig2List[0] == 1)
            {
                MyOutputList =  Dig1List; IsResult = true; OutFrag = Dig1ListFrag;
            }

            if (IsResult == false)
            {
                List<long> MyPowerList = new(1) { 1 };
                List<long> MyPowerListAdd = new(1) { 1 };
                List<long> Dig1List9;
                if (Dig1ListFrag == 18)
                { CalcConvert.ConvertList18To9(in Dig1List, out Dig1List9); }
                else
                { Dig1List9 = Dig1List; }

                MyOutputList = Dig1List9;
                int Result;
                CalcCompare.ListBigger(in MyPowerList, in Dig2List, out Result);
                while (Result == 2)
                {
                    MyOutputList = CalcLists.Mul(MyOutputList, 9, Dig1List9, 9, 9);
                    MyPowerList = CalcLists.Add(MyPowerList, MyPowerListAdd);
                    CalcCompare.ListBigger(in MyPowerList, in Dig2List, out Result);
                }
                OutFrag = 9;
            }
            List<long> MyOutput = new();
            if (ExpFrag != OutFrag)
            {
                if (ExpFrag == 18 && OutFrag == 9)
                { CalcConvert.ConvertList9To18(in MyOutputList, out MyOutput); }
                else
                { CalcConvert.ConvertList18To9(in MyOutputList, out MyOutput); }
            }
            else
            { MyOutput = MyOutputList; }

            return MyOutput;

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
            CalcCompare.ListBigger(in Dig1List, in Dig2List, out int DigBiggerTemp);
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
            CalcCompare.ListBigger(in Dig1List, in Dig2List, out int DigBiggerTemp);

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
            CalcConvert.StringsToLongLists(in Dig1, in Dig2, 9, out List<List<long>> DigsLists); //9

            List<long> Dig1List = DigsLists[0];
            List<long> Dig2List = DigsLists[1];
            CalcConvert.LongListToString(CalcLists.Mul(Dig1List, 9, Dig2List, 9, 18), "000000000000000000", out string TempOutput); //"000000000"

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
            CalcConvert.StringToLongList(in Dig1, 9, out List<long> Dig1List);
            CalcConvert.StringToLongList(in Dig2, 18, out List<long> Dig2List);
            List<long> MyOutputList = CalcLists.Pow(Dig1List, 9, Dig2List, 18);

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
            CalcCompare.ListBigger(in Dig1List, in Dig2List, out int BiggerList);

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

        public static void ListBigger(in List<long> Dig1List, in List<long> Dig2List, out int BiggerList)
        {
            int Dig1ListCount = Dig1List.Count;
            int Dig2ListCount = Dig2List.Count;
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
                        long Diff = Dig1List[i] - Dig2List[i];
                        if (Diff > 0)
                        {
                            BiggerList = 1;
                            return;
                        }
                        if (Diff < 0)
                        {
                            BiggerList = 2;
                            return;
                        }
                    }
                    BiggerList = 0;
                    return;
            }

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
                long Full, MyMod;
                CalcIntExt.MyModuloLongToLong(DigList[i], 1000000000, out Full, out MyMod);
                MyResult.Add(MyMod);
                long NextDig = Full;

                if (NextDig > 0 && i <= Loops)
                { MyResult.Add(NextDig); }

            }
            return;
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
            return;
        }
    }
}
