using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
//using System.Linq;
//using System.Collections.Generic;
//using SevenZip;



//dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
//format code ctrl-k + ctrl-f
//<PublishTrimmed>true</PublishTrimmed>

namespace RandomPassword
{

    class RandomPassword
    {

        public static int DecoderMode = 100;
        public static int SysEx = 0;
        public static int OtherCode = 0;
        public static int FalsePosCount = 0;

        public static int DisplayMode = 0;
        public static int ActThreadCount = 0;
        public static int PublicMyTaskCount = 0;
        public static int GlobalTaskCount = 0;
        public static int SetupTaskCount = 0;
        public static int PassCount = 0;
        public static string SessionPassCountDoneS = "0";

        private static AsyncLocal<string> ExtrMyPass = new AsyncLocal<string>();

        public static long MySpeed = 0;
        public static long MySpeedTemp = 0;

        public static DateTime StartTime = DateTime.Now;
        public static DateTime EndTime = DateTime.Now;

        public static string MyExit = "n";
        public static string CurrentOS = "";
        public static string ProgramDir = "";
        public static string Add7z = "";
        public static Stopwatch sw7zTemp = Stopwatch.StartNew();
        public static double ElapsSec7z = 0;
        public static string Session7zPassCount = "0";
        public static string HashcatParam = "";
        public static string JTRParam = "";
        public static string ZipParam = "";

        public static int TempPassCountPublic = 0; //Czasowa liczba haseł, które przeszły weryfikację

        static void Main(string[] args)
        {
            Console.WriteLine("Miki Calculator and Pseudo-Random Password Generator for decoding 7z (Qlocker)...");
            Console.WriteLine("");

            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("1.RndTo7z,   2.RndToHashcat,    3.RndToJTR,    9.Calculator,    0.Test Generator");
                DecoderMode = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

                if (DecoderMode != 0 & DecoderMode != 1 & DecoderMode != 2 & DecoderMode != 3 & DecoderMode != 9)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Wrong option, try again...");
                }
                else { Console.WriteLine(""); break; }
            }

            if (DecoderMode == 9)
            {
                Console.WriteLine("Calculator");
                Miki.Miki.Calculator();
                System.Environment.Exit(0);

            }

            ProgramDir = Environment.CurrentDirectory;



            string ReadString;
            List<string> ConfigStringList = new List<string>();
            string ConfigFile = System.IO.Path.Combine(ProgramDir, "Config", "Config.txt");

            using (System.IO.StreamReader Cfile = new System.IO.StreamReader(ConfigFile))
            {
                while ((ReadString = Cfile.ReadLine()) != null)
                {
                    if (ReadString.Contains("Characters="))
                    {
                        string TempString = ReadString.Replace("Characters=", "").Trim();
                        //char[] characters = TempString.ToArray();
                        char[] characters = TempString.ToCharArray();
                        Array.Sort(characters);
                        LehmerGen.Passwords.CharactersList = new List<char>(characters);
                        LehmerGen.Passwords.PassCharCount = Convert.ToString(LehmerGen.Passwords.CharactersList.Count);
                    }
                    if (ReadString.Contains("PassMinLength="))
                    {
                        LehmerGen.Passwords.PassMinLength = Convert.ToInt32(ReadString.Replace("PassMinLength=", "").Trim());
                    }
                    if (ReadString.Contains("PassMaxLength="))
                    {
                        LehmerGen.Passwords.PassMaxLength = Convert.ToInt32(ReadString.Replace("PassMaxLength=", "").Trim());
                    }
                    if (ReadString.Contains("IncrementBase="))
                    {
                        LehmerGen.Passwords.IncrementBase = ReadString.Replace("IncrementBase=", "").Trim();
                    }
                    if (ReadString.Contains("IncrementPower="))
                    {
                        LehmerGen.Passwords.IncrementPower = ReadString.Replace("IncrementPower=", "").Trim();
                    }
                    if (ReadString.Contains("Mode="))
                    {
                        LehmerGen.Passwords.WhatIs = Convert.ToInt32(ReadString.Replace("Mode=", "").Trim());
                    }
                    if (ReadString.Contains("ZipParam="))
                    {
                        ZipParam = ReadString.Replace("ZipParam=", "").Trim();
                    }
                    if (ReadString.Contains("ProcCount="))
                    {
                        RandomPassword.SetupTaskCount = Convert.ToInt32(ReadString.Replace("ProcCount=", "").Trim());
                        RandomPassword.GlobalTaskCount = RandomPassword.SetupTaskCount;

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
                        LehmerGen.Passwords.HCDictSize = Convert.ToUInt32(ReadString.Replace("HCDictSize=", "").Trim());
                    }
                    if (ReadString.Contains("JTRDictSize="))
                    {
                        LehmerGen.Passwords.JTRDictSize = Convert.ToUInt32(ReadString.Replace("JTRDictSize=", "").Trim());
                    }
                    if (ReadString.Contains("PassCheck="))
                    {
                        LehmerGen.Passwords.TestPassCheck = Convert.ToInt32(ReadString.Replace("PassCheck=", "").Trim());
                    }
                    if (ReadString.Contains("PassLoops="))
                    {
                        LehmerGen.Passwords.TestPassLoops = Convert.ToInt32(ReadString.Replace("PassLoops=", "").Trim());
                    }
                }

                Cfile.Dispose();
                Cfile.Close();
            }
            //LehmerGen.Passwords.WhatIs = ConfigStringList[0];

            string ProgressFile = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.txt");

            List<string> ProgressStringList = new List<string>();


            using (System.IO.StreamReader Pfile = new System.IO.StreamReader(ProgressFile))
            {
                while ((ReadString = Pfile.ReadLine()) != null)
                { ProgressStringList.Add(ReadString); }
                Pfile.Dispose();
                Pfile.Close();
            }

            LehmerGen.Passwords.IterDone = ProgressStringList[0];
            LehmerGen.Passwords.IterToDo = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, "1")[0];
            LehmerGen.Passwords.PassIterPoolMaxFix = ProgressStringList[1];


            LehmerGen.Passwords.InitializeGenerator(); //Prepare generator "62", "32", "3", "108"
            LehmerGen.Passwords.PrepareGeneratorNext(true);

            Console.WriteLine("Session restored...");
            Console.WriteLine(String.Format("Iteration to do:   {0}", LehmerGen.Passwords.IterToDo));

            //LehmerGen.Passwords.MyPerformanceCheck();
            //RandomPassword.CurrentOS = CheckOS();



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
                //RandomPassword.GlobalTaskCount = Convert.ToInt32(Console.ReadLine());
                //MaxLoop = Convert.ToUInt32(Console.ReadLine());
                if (DecoderMode == 1)
                {
                    Task.Run(() => PutRandomPassTo7z(RandomPassword.GlobalTaskCount)).ConfigureAwait(false);
                }

                if (DecoderMode == 2)
                {
                    Task.Run(() => RunHashcatProgram()).ConfigureAwait(false);
                    LehmerGen.Passwords.DictSize = LehmerGen.Passwords.HCDictSize;
                }

                if (DecoderMode == 3)
                {
                    Task.Run(() => RunJTRProgram()).ConfigureAwait(false);
                    LehmerGen.Passwords.DictSize = LehmerGen.Passwords.JTRDictSize;
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
                    LehmerGen.Passwords.HCDictSize = Convert.ToUInt32(Console.ReadLine());
                    Console.WriteLine();
                    Task.Run(() => RunHashcatProgram()).ConfigureAwait(false);
                }

                if (DecoderMode == 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much passwords in Dictionary file???");
                    LehmerGen.Passwords.HCDictSize = Convert.ToUInt32(Console.ReadLine());
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
                    LehmerGen.Passwords.TestPassLoops = Convert.ToInt32(Console.ReadLine());
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

        public static int MonitorKeypress()
        {
            string InfoString1 = "1. Visual Mode, 2. Show status, 3. Silent Mode";
            string InfoString2 = "";
            string InfoString3 = "";
            string InfoString4 = "";
            string InfoString5 = "End program if cracked...";
            string InfoString = "";
            if (DecoderMode == 1 || Add7z == "y") { InfoString2 = "+. 7z Task++, -. 7z Task--, /. Task=2, *. Task=From Config"; }
            InfoString3 = "n. End if cracked, e. Safe End, Ctrl+p. Refresh KeyPress monitor";
            if (DecoderMode == 1 || Add7z == "y") { InfoString4 = string.Format("Number of active tasks is set to: {0}", RandomPassword.GlobalTaskCount); }

            InfoString = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", InfoString1, Environment.NewLine, InfoString2, Environment.NewLine, InfoString3, Environment.NewLine, InfoString4, Environment.NewLine, InfoString5);

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(InfoString);
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");


            while (true)
            {
                // true hides the pressed character from the console
                ConsoleKeyInfo cki = new ConsoleKeyInfo();
                bool DisplayInfo = false;
                //Console.WriteLine(Console.KeyAvailable);
                cki = Console.ReadKey(true);
                if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((cki.Key & ConsoleKey.P) != 0)
                    {
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
                        //CpuCounter();
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
                                //while(Console.KeyAvailable == true) { Console.WriteLine("REM"); }
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
                            //Thread.Sleep(100);
                        }
                        //cki = Console.ReadKey(true);
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

        private static async Task PutRandomPassTo7z(int StartTaskCount)
        {
            //StartTime = DateTime.Now;
            Session7zPassCount = "0";
            string ActOS = CheckOS();

            string ZipFile = "";

            List<Task> ListOfTasks7z = new List<Task>();

            if (ActOS == "Windows")
            {
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za.exe");
            }
            else
            {
                ZipFile = System.IO.Path.Combine(ProgramDir, "7z", "7za");
            }

            string CodedFile = System.IO.Path.Combine(ProgramDir, "Coded", "Coded.7z");

            //int TempPassCount = 0;
            int TaskCount = RandomPassword.GlobalTaskCount;

            Console.WriteLine("Cracking with 7z started...");

            //string IterAll;
            //string IterToDo;
            string SessionPassCountS = "0";
            PassCount = 0;


            //List<string> ProgressStringList = ReadProgress();

            //ProgressString is a real password number, eg. 1 is for 000000000...0, and it is last chcecked password
            //but password 000000...0 cames from iteration 0, so if we want to check iteration 0, ProgressString should be -1
            //var abv = LehmerGen.Passwords.IterToDo;
            //LehmerGen.Passwords.IterDone = LehmerGen.Passwords.IterDone;
            //LehmerGen.Passwords.IterDone = IterLastDone;

            //string MaxPassPool = ProgressStringList[1];
            //IterToDo = Miki.CalcStrings.Add(IterLastDone, "1")[0]; //as above, this is next password to check



            //LehmerGen.Passwords.PrepareGeneratorNext(true); //Prepare generator

            List<Task> ListOfTasks = new List<Task>();

            string ExitReason = "Safe Exit";

            var sw = Stopwatch.StartNew();
            StartTime = DateTime.Now;
            sw7zTemp = Stopwatch.StartNew();
            while (true)
            {
                TaskCount = RandomPassword.GlobalTaskCount;
                RandomPassword.PublicMyTaskCount = TaskCount;
                //Console.WriteLine(ListOfTasks.);

                //now save TempPassCount as a number of last checked password, because each of generated pass will be checked
                if (sw.ElapsedMilliseconds >= 300000)
                {


                    LehmerGen.Passwords.IterDone = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, Convert.ToString(TempPassCountPublic))[0];
                    //Session7zPassCount = Miki.CalcStrings.Add(Session7zPassCount, Convert.ToString(TempPassCount))[0];

                    SaveProgress(LehmerGen.Passwords.IterDone, LehmerGen.Passwords.PassIterPoolMaxFix, "7z");


                    if (Miki.CalcCompare.StringBigger(LehmerGen.Passwords.IterDone, LehmerGen.Passwords.PassIterPoolMaxFix) < 2)
                    {
                        RandomPassword.MyExit = "ex";
                        RandomPassword.GlobalTaskCount = 0;
                        ExitReason += " - Exhausted Pool.";
                    }


                    //TempPassCount = 0;
                    TempPassCountPublic = 0;
                    sw = Stopwatch.StartNew();
                }


                //await Task.WhenAny(ListOfTasks);
                //ListOfTasks.RemoveAll(x => x.IsCompleted);

                RandomPassword.ActThreadCount = ListOfTasks.Count;

                while (RandomPassword.ActThreadCount >= TaskCount)
                {
                    //await Task.Delay(25);
                    await Task.WhenAny(ListOfTasks);
                    ListOfTasks.RemoveAll(x => x.IsCompleted);

                    RandomPassword.ActThreadCount = ListOfTasks.Count;

                    if (RandomPassword.MyExit == "eh") //end Hascat
                    {
                        int TempTaskCount = RandomPassword.GlobalTaskCount;
                        RandomPassword.GlobalTaskCount = 0;
                        await Task.WhenAll(ListOfTasks);
                        ListOfTasks.RemoveAll(x => x.IsCompleted);
                        RandomPassword.ActThreadCount = ListOfTasks.Count;

                        ExitReason += " - Stoped, prepare new password list and run new Hashcat.";

                        Console.WriteLine("Stoped Additional 7z cracking...");
                        if (ListOfTasks.Count == 0)
                        {
                            LehmerGen.Passwords.IterDone = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, Convert.ToString(TempPassCountPublic))[0];
                            //Session7zPassCount = Miki.CalcStrings.Add(Session7zPassCount, Convert.ToString(TempPassCount))[0];
                            SaveProgress(LehmerGen.Passwords.IterDone, LehmerGen.Passwords.PassIterPoolMaxFix, "7z");

                            Console.WriteLine(ExitReason);
                            PassCount = TempPassCountPublic;
                            RandomPassword.GlobalTaskCount = TempTaskCount;
                            return;
                        }
                    }


                    if (RandomPassword.MyExit == "e")
                    {
                        RandomPassword.GlobalTaskCount = 0;
                        await Task.WhenAll(ListOfTasks);
                        ListOfTasks.RemoveAll(x => x.IsCompleted);
                        if (ListOfTasks.Count == 0)
                        {
                            LehmerGen.Passwords.IterDone = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, Convert.ToString(TempPassCountPublic))[0];
                            //Session7zPassCount = Miki.CalcStrings.Add(Session7zPassCount, Convert.ToString(TempPassCount))[0];
                            SaveProgress(LehmerGen.Passwords.IterDone, LehmerGen.Passwords.PassIterPoolMaxFix, "7z");

                            Console.WriteLine(ExitReason);
                            return;
                        }
                    }

                    if (RandomPassword.MyExit == "ex" && ListOfTasks.Count == 0)
                    {
                        //Session7zPassCount = Miki.CalcStrings.Add(Session7zPassCount, Convert.ToString(TempPassCount))[0];
                        Console.WriteLine(ExitReason);
                        return;
                    }

                }


                //List<string> PassList = LehmerGen.Passwords.PassListFromDig(1, false);
                //ExtrMyPass.Value = PassList[0];//MyPass;
                //TempPassCount++;
                //TempPassCountS = Miki.CalcStrings.Add(TempPassCountS, "1")[0];

                while (ListOfTasks.Count < TaskCount)
                {
                    ExtrMyPass.Value = LehmerGen.Passwords.PassFromDig();//PassList[0];//MyPass;
                    SessionPassCountS = Miki.CalcStrings.Add("1", SessionPassCountS)[0];
                    string LocalSessionPassCountS = SessionPassCountS;
                    Session7zPassCount = SessionPassCountS;
                    ListOfTasks.Add(Task.Run(async () => await ExtractFile(ZipFile, CodedFile, ExtrMyPass.Value, LocalSessionPassCountS).ConfigureAwait(false)));

                }

            }
        }


        private static async void RunJTRProgram()
        {
            StartTime = DateTime.Now;
            string JTRFile = "";
            string ZipFile = "";
            List<Task> ListOfTasks7z = new List<Task>();
            //Stopwatch sw7z = new Stopwatch();

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



            string IterAll = "";

            string ExitReason = "Safe Exit";

            List<string> ProgressStringList = ReadProgress();
            //ProgressString is a real password number, eg. 1 is for 000000000...0, and it is last chcecked password
            //but password 000000...0 cames from iteration 0, so if we want to check iteration 0, ProgressString should be -1
            if (!File.Exists(DictFile))
            {
                if (!File.Exists(DictFileNext))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Generate new DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    List<string> PassList = LehmerGen.Passwords.PassListFromDig(LehmerGen.Passwords.JTRDictSize, true);
                    File.WriteAllLines(DictFileNext, PassList.ToArray(), isoLatin1Encoding);

                    PassList.Clear();

                    IterAll = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, Convert.ToString(LehmerGen.Passwords.JTRDictSize))[0];

                    Console.WriteLine("");
                    Console.WriteLine("Done - DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");
                    Console.WriteLine("Last iteration:       " + LehmerGen.Passwords.IterDone);
                    Console.WriteLine("New iteration:        " + LehmerGen.Passwords.IterToDo);
                    Console.WriteLine("End iteration:        " + IterAll);
                    Console.WriteLine("This Pool iteration:  " + LehmerGen.Passwords.JTRDictSize);
                    Console.WriteLine("Max Pool iteration:   " + LehmerGen.Passwords.PassIterPoolMaxFix);
                    Console.WriteLine("");


                    SaveProgress(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix, "JTR");
                    LehmerGen.Passwords.IterDone = IterAll;
                    LehmerGen.Passwords.IterToDo = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, "1")[0];

                    if (Miki.CalcCompare.StringBigger(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix) < 2)
                    {
                        RandomPassword.MyExit = "ex";
                        ExitReason += " - Exhausted Pool.";
                    }
                }
            }

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
                    Console.WriteLine("++++++++++ Start JTR: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");
                    Directory.SetCurrentDirectory(JTRDir);

                    ProcessStartInfo pro = new ProcessStartInfo();
                    pro.WindowStyle = ProcessWindowStyle.Hidden;
                    pro.UseShellExecute = false;
                    pro.RedirectStandardInput = true;
                    pro.CreateNoWindow = false; //false is faster
                    pro.FileName = JTRFile;
                    string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
                    string MyHash = System.IO.Path.Combine(ProgramDir, "Coded", "Hash.txt");
                    string MyResult = System.IO.Path.Combine(ProgramDir, "JTR", "run", "john.pot");
                    pro.Arguments = string.Format("{0} --wordlist={1} {2}", JTRParam, DictFile, MyHash);

                    Process x = Process.Start(pro);

                    if (RandomPassword.MyExit == "n")
                    {

                        //start generate new list, wait for it... then wait for x end....
                        if (!File.Exists(DictFileNext))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Generate new DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            Console.WriteLine("");
                            List<string> PassList = LehmerGen.Passwords.PassListFromDig(LehmerGen.Passwords.JTRDictSize, true);

                            File.WriteAllLines(DictFileNext, PassList.ToArray(), isoLatin1Encoding);
                            PassList.Clear();

                            IterAll = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, Convert.ToString(LehmerGen.Passwords.JTRDictSize))[0];

                            Console.WriteLine("");
                            Console.WriteLine("Done - DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            Console.WriteLine("");
                            Console.WriteLine("Last iteration:       " + LehmerGen.Passwords.IterDone);
                            Console.WriteLine("New iteration:        " + LehmerGen.Passwords.IterToDo);
                            Console.WriteLine("End iteration:        " + IterAll);
                            Console.WriteLine("This Pool iteration:  " + LehmerGen.Passwords.JTRDictSize);
                            Console.WriteLine("Max Pool iteration:   " + LehmerGen.Passwords.PassIterPoolMaxFix);
                            Console.WriteLine("");

                            SaveProgress(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix, "JTR");
                            LehmerGen.Passwords.IterDone = IterAll;
                            LehmerGen.Passwords.IterToDo = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, "1")[0];

                            if (Miki.CalcCompare.StringBigger(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix) < 2)
                            {
                                RandomPassword.MyExit = "ex";
                                ExitReason += " - Exhausted Pool.";
                            }

                        }

                    }

                    x.WaitForExit();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Stop JTR: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");

                    swHC.Stop();
                    long PassPerSec = (LehmerGen.Passwords.JTRDictSize / (swHC.ElapsedMilliseconds / 1000));
                    long PassPerMin = PassPerSec * 60;
                    string InfoString = String.Format("JTR Speed:   {0} Pass/s,    {1} Pass/min", PassPerSec, PassPerMin);
                    Console.WriteLine(InfoString);

                    bool DeleteDictFile = true;

                    //Always ExitCode = 0


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
                            MyPasss = MyPasss.Substring(InString + 1);
                            //MyPasss = Miki.CalcStringExt.Right(MyPasss, 32);
                            ProcessStartInfo pro7z = new ProcessStartInfo();
                            pro7z.WindowStyle = ProcessWindowStyle.Hidden;
                            pro7z.UseShellExecute = false;
                            pro7z.RedirectStandardError = true;
                            pro7z.RedirectStandardOutput = true;
                            pro7z.CreateNoWindow = false; //false is faster
                            pro7z.FileName = ZipFile;

                            pro7z.Arguments = string.Format("t \"{0}\" -p\"{1}\" -y", CodedFile, MyPasss); //x lub t, t działa szybciej a wynik i tak zapisywany jest do pliku

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
                                //Console.ReadKey();
                                x7z.Dispose();
                                x7z.Close();
                                return;
                                //System.Environment.Exit(0);

                            }
                            if (x7z.ExitCode == 2)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("FALSE POSITIVE - password, cleaning password list....");
                                Console.WriteLine("");
                                List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

                                int FindPos = TempDictList.IndexOf(MyPasss);

                                TempDictList.RemoveRange(0, FindPos + 1);

                                File.Delete(DictFile);
                                File.WriteAllLines(DictFile, TempDictList.ToArray(), isoLatin1Encoding);
                                TempDictList.Clear();
                                string MyPasswordFalseFilename = "FalsePassword-" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-.txt";
                                string MyPasswordFalse = System.IO.Path.Combine(ProgramDir, "Output", MyPasswordFalseFilename);

                                Console.WriteLine("Move john.pot file to FalsePassword....");
                                System.IO.File.Move(MyResult, MyPasswordFalse);

                                //File.Delete(MyResult);

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
                        Console.WriteLine(ExitReason);
                        Console.ReadKey();
                        return;
                    }

                    if (RandomPassword.MyExit == "ex")
                    {
                        RandomPassword.MyExit = "e";
                    }
                    x.Dispose();
                    x.Close();

                    RandomPassword.ElapsSec7z = RandomPassword.sw7zTemp.Elapsed.TotalSeconds;
                    RandomPassword.sw7zTemp = Stopwatch.StartNew();
                    RandomPassword.EndTime = DateTime.Now;
                    //CalculateSpeed();
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
            string HascatFile = "";
            string ZipFile = "";
            List<Task> ListOfTasks7z = new List<Task>();
            //Stopwatch sw7z = new Stopwatch();

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



            string IterAll = "";
            //string IterLastDone;
            //string IterToDo;
            string ExitReason = "Safe Exit";
            //List<string> ProgressStringList = new List<string>();
            List<string> ProgressStringList = ReadProgress();
            //ProgressString is a real password number, eg. 1 is for 000000000...0, and it is last chcecked password
            //but password 000000...0 cames from iteration 0, so if we want to check iteration 0, ProgressString should be -1
            if (!File.Exists(DictFile))
            {
                if (!File.Exists(DictFileNext))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Generate new DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    List<string> PassList = LehmerGen.Passwords.PassListFromDig(LehmerGen.Passwords.HCDictSize, true);
                    File.WriteAllLines(DictFileNext, PassList.ToArray(), isoLatin1Encoding);

                    PassList.Clear();

                    IterAll = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, Convert.ToString(LehmerGen.Passwords.HCDictSize))[0];

                    Console.WriteLine("");
                    Console.WriteLine("Done - DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");
                    Console.WriteLine("Last iteration:       " + LehmerGen.Passwords.IterDone);
                    Console.WriteLine("New iteration:        " + LehmerGen.Passwords.IterToDo);
                    Console.WriteLine("End iteration:        " + IterAll);
                    Console.WriteLine("This Pool iteration:  " + LehmerGen.Passwords.HCDictSize);
                    Console.WriteLine("Max Pool iteration:   " + LehmerGen.Passwords.PassIterPoolMaxFix);
                    Console.WriteLine("");


                    SaveProgress(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix, "HC");
                    LehmerGen.Passwords.IterDone = IterAll;
                    LehmerGen.Passwords.IterToDo = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, "1")[0];

                    if (Miki.CalcCompare.StringBigger(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix) < 2)
                    {
                        RandomPassword.MyExit = "ex";
                        ExitReason += " - Exhausted Pool.";
                    }
                }
            }

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

                    ProcessStartInfo pro = new ProcessStartInfo();
                    pro.WindowStyle = ProcessWindowStyle.Hidden;
                    pro.UseShellExecute = false;

                    pro.CreateNoWindow = false; //false is faster
                    pro.FileName = HascatFile;
                    string MyPasswordFile = System.IO.Path.Combine(ProgramDir, "Dictionary", "Dictionary.txt");
                    string MyHash = System.IO.Path.Combine(ProgramDir, "Coded", "Hash.txt");
                    string MyResult = System.IO.Path.Combine(ProgramDir, "Output", "MyPasswordHashcat.txt");
                    pro.Arguments = string.Format("{0} -o {1} {2} {3}", HashcatParam, MyResult, MyHash, DictFile);

                    Process x = Process.Start(pro);

                    //Thread.Sleep(10000);


                    if (RandomPassword.MyExit == "n")
                    {

                        //ProgressStringList = ReadProgress();
                        //IterLastDone = ProgressStringList[0];
                        //ProgressStringList.Clear();
                        //LehmerGen.Passwords.IterDone = IterLastDone;
                        //IterToDo = Miki.CalcStrings.Add(IterLastDone, "1")[0]; //as above, this is next password to check
                        //LehmerGen.Passwords.IterToDo = IterToDo;
                        //LehmerGen.Passwords.PrepareGeneratorNext(true); //Prepare generator

                        //start generate new list, wait for it... then wait for x end....
                        if (!File.Exists(DictFileNext))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Generate new DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            Console.WriteLine("");
                            List<string> PassList = LehmerGen.Passwords.PassListFromDig(LehmerGen.Passwords.HCDictSize, true);

                            File.WriteAllLines(DictFileNext, PassList.ToArray(), isoLatin1Encoding);
                            PassList.Clear();

                            IterAll = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, Convert.ToString(LehmerGen.Passwords.HCDictSize))[0];

                            Console.WriteLine("");
                            Console.WriteLine("Done - DictionaryNext.txt file:  " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            Console.WriteLine("");
                            Console.WriteLine("Last iteration:       " + LehmerGen.Passwords.IterDone);
                            Console.WriteLine("New iteration:        " + LehmerGen.Passwords.IterToDo);
                            Console.WriteLine("End iteration:        " + IterAll);
                            Console.WriteLine("This Pool iteration:  " + LehmerGen.Passwords.HCDictSize);
                            Console.WriteLine("Max Pool iteration:   " + LehmerGen.Passwords.PassIterPoolMaxFix);
                            Console.WriteLine("");

                            SaveProgress(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix, "HC");
                            LehmerGen.Passwords.IterDone = IterAll;
                            LehmerGen.Passwords.IterToDo = Miki.CalcStrings.Add(LehmerGen.Passwords.IterDone, "1")[0];

                            if (Miki.CalcCompare.StringBigger(IterAll, LehmerGen.Passwords.PassIterPoolMaxFix) < 2)
                            {
                                RandomPassword.MyExit = "ex";
                                ExitReason += " - Exhausted Pool.";
                            }

                        }

                    }

                    x.WaitForExit();
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++ Stop Hashcat: {0} ++++++++++", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("");

                    swHC.Stop();


                    Console.WriteLine("HC Speed:    " + (LehmerGen.Passwords.HCDictSize / (swHC.ElapsedMilliseconds / 1000)) + " Pass/s");



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
                        MyPasss = MyPasss.Substring(InString + 1);
                        //MyPasss = Miki.CalcStringExt.Right(MyPasss, 32);
                        ProcessStartInfo pro7z = new ProcessStartInfo();
                        pro7z.WindowStyle = ProcessWindowStyle.Hidden;
                        pro7z.UseShellExecute = false;
                        pro7z.RedirectStandardError = true;
                        pro7z.RedirectStandardOutput = true;
                        pro7z.CreateNoWindow = false; //false is faster
                        pro7z.FileName = ZipFile;

                        pro7z.Arguments = string.Format("t \"{0}\" -p\"{1}\" -y", CodedFile, MyPasss); //x lub t, t działa szybciej a wynik i tak zapisywany jest do pliku
                                                                                                       //x or t, t is much faster, and password is saved to file (it is not necessary to decrypt file)

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
                            //Console.ReadKey();
                            x7z.Dispose();
                            x7z.Close();
                            return;
                            //System.Environment.Exit(0);

                        }
                        if (x7z.ExitCode == 2)
                        {
                            Console.WriteLine("");
                            Console.WriteLine("FALSE POSITIVE - password, cleaning password list....");
                            Console.WriteLine("");
                            List<string> TempDictList = new List<string>(File.ReadAllLines(DictFile));

                            int FindPos = TempDictList.IndexOf(MyPasss);

                            TempDictList.RemoveRange(0, FindPos + 1);


                            File.Delete(DictFile);
                            File.WriteAllLines(DictFile, TempDictList.ToArray(), isoLatin1Encoding);
                            TempDictList.Clear();
                            string MyPasswordFalseFilename = "FalsePassword-" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-.txt";
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
                        Console.WriteLine(ExitReason);
                        Console.ReadKey();
                        return;
                    }

                    if (RandomPassword.MyExit == "ex")
                    {
                        RandomPassword.MyExit = "e";
                    }
                    x.Dispose();
                    x.Close();

                    RandomPassword.ElapsSec7z = RandomPassword.sw7zTemp.Elapsed.TotalSeconds;
                    RandomPassword.sw7zTemp = Stopwatch.StartNew();
                    RandomPassword.EndTime = DateTime.Now;
                }

            }

            catch (System.Exception Ex)
            {
                SysEx++;
                Console.WriteLine(Ex.Message);
            }
        }

        private static async Task<int> ExtractFile(string ZipDir, string CodedDir, string MyPasss, string SesPassCountS)
        {
            int LocalPassCount = 0;
            string SesPassCountSLocal = SesPassCountS;
            string SesPassCountSLocalSub = Miki.CalcStrings.Sub(SesPassCountSLocal, "1")[0];
            int MyExit = 100;

            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.UseShellExecute = false;
                pro.RedirectStandardError = true;
                pro.RedirectStandardOutput = true;
                pro.CreateNoWindow = false; //false is faster
                pro.FileName = ZipDir;
                //MyPasss = "aaaa";
                //Stopwatch s = Stopwatch.StartNew();
                pro.Arguments = string.Format("{0} \"{1}\" -p\"{2}\" -y", ZipParam, CodedDir, MyPasss); //x lub t, t działa szybciej a wynik i tak zapisywany jest do pliku
                //x or t, t is much faster, and password is saved to file (it is not necessary to decrypt file)

                Process x = await Task.Run(() => Process.Start(pro)).ConfigureAwait(false);

                x.BeginOutputReadLine();
                x.BeginErrorReadLine();
                x.WaitForExit();

                //string StOutput = x.StandardError.ReadToEnd();
                //string StError = x.StandardError.ReadToEnd();
                //x.WaitForExit();

                MyExit = x.ExitCode;

                x.Dispose();
                //x.Close();
                pro = null;

                //FIFO like
                Stopwatch WaitWatch = Stopwatch.StartNew();
                int LoopCount = 0;
                while (SessionPassCountDoneS != SesPassCountSLocalSub)
                {
                    LoopCount++;
                    await Task.Delay(1);
                }
                WaitWatch.Stop();
                long WaitTime = WaitWatch.ElapsedMilliseconds;
                PassCount++;

                //var sw = Stopwatch.StartNew();
                if (MyExit == 0)
                {
                    SessionPassCountDoneS = Miki.CalcStrings.Add("1", SessionPassCountDoneS)[0];
                    LocalPassCount = PassCount;
                    Console.WriteLine(string.Format("Password is: {0}", MyPasss));
                    SavePassword(MyPasss);

                    Console.WriteLine("Cracking successful, password in the Output directory. ;-)");
                    Console.ReadKey();
                    System.Environment.Exit(MyExit);
                }

                if (MyExit == 2)
                {
                    LocalPassCount = PassCount;
                    switch (RandomPassword.DisplayMode)
                    {
                        case 1:
                            Calculate7zSpeed();
                            Console.WriteLine(string.Format("{0}  {1}  Speed Temp / Full / Loops: {2} / {3} / {4}  ExC: {5}  ExCx: {6}  SysEx: {7}  7z: {8} / {9} / {10} W: {11}", SesPassCountSLocal, MyPasss, RandomPassword.MySpeedTemp, RandomPassword.MySpeed, LoopCount, MyExit, OtherCode, SysEx, RandomPassword.ActThreadCount, RandomPassword.PublicMyTaskCount, ListProc7za(), WaitTime));
                            break;
                        case 2:
                            Calculate7zSpeed();
                            Console.WriteLine(string.Format("{0}  {1}  Speed Temp / Full / Loops: {2} / {3} / {4}  ExC: {5}  ExCx: {6}  SysEx: {7}  7z: {8} / {9} / {10} W: {11}", SesPassCountSLocal, MyPasss, RandomPassword.MySpeedTemp, RandomPassword.MySpeed, LoopCount, MyExit, OtherCode, SysEx, RandomPassword.ActThreadCount, RandomPassword.PublicMyTaskCount, ListProc7za(), WaitTime));
                            RandomPassword.DisplayMode = 3;
                            break;
                        case 3:
                            break;
                    }

                }
                else

                {
                    OtherCode++;
                    LocalPassCount = PassCount;
                    Console.WriteLine(string.Format("{0}  {1}  Speed Temp / Full / Loops: {2} / {3} / {4}  ExC: {5}  ExCx: {6}  SysEx: {7}  7z: {8} / {9} / {10} W: {11}", SesPassCountSLocal, MyPasss, RandomPassword.MySpeedTemp, RandomPassword.MySpeed, LoopCount, MyExit, OtherCode, SysEx, RandomPassword.ActThreadCount, RandomPassword.PublicMyTaskCount, ListProc7za(), WaitTime));
                }


                if ((LocalPassCount % 100) == 0) //co 100 czasową obliczam prędkość (calculate Temporary speed after each 100 passwords)
                {
                    RandomPassword.ElapsSec7z = RandomPassword.sw7zTemp.Elapsed.TotalSeconds;
                    RandomPassword.sw7zTemp = Stopwatch.StartNew();
                    if (PassCount > 2000000000)
                    { PassCount = 0; }
                }

            }

            catch (System.Exception Ex)
            {
                SysEx++;
                PassCount++;
                LocalPassCount = PassCount;
                Console.WriteLine(Ex.Message);

            }
            SessionPassCountDoneS = Miki.CalcStrings.Add("1", SessionPassCountDoneS)[0];
            TempPassCountPublic++;
            return MyExit;

        }

        //---------------------------------------------------------------------------------Random

        /*public static void GetMaxCombinations(int CharTabCount, int PassLength)
        {
            RandomPassword.MaxCombinations = Math.Pow(CharTabCount, PassLength);
        }*/

        /* public static string GetRandomAlphanumericString(int length, int chartab)
         {
             string alphanumericCharacters = "";
             switch (chartab)
             {
                 case 1:
                     alphanumericCharacters =
                 "abcdefghijklmnopqrstuvwxyz";
                     break;
                 case 2:
                     alphanumericCharacters =
                 "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                     break;
                 case 3:
                     alphanumericCharacters =
                 "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                 "abcdefghijklmnopqrstuvwxyz";
                     break;
                 case 4:
                     alphanumericCharacters =
                 "0123456789";
                     break;
                 case 5:
                     alphanumericCharacters =
                 "abcdefghijklmnopqrstuvwxyz" +
                 "0123456789";
                     break;
                 case 6:
                     alphanumericCharacters =
                 "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                 "0123456789";
                     break;
                 case 7:
                     alphanumericCharacters =
                 "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                 "abcdefghijklmnopqrstuvwxyz" +
                 "0123456789";
                     break;

             }

             return GetRandomString(length, alphanumericCharacters);
         }*/


        /*public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);

            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);


        }*/

        //--------------------------------------------------------------------------- 7z Proces count
        private static int ListProc7z()
        {
            int My7zCount = 0;
            Process[] processCollection = Process.GetProcesses();
            foreach (Process p in processCollection)
            {
                if (p.ProcessName == "7za")
                {
                    My7zCount++;
                }
            }
            return My7zCount;
        }


        //------------------------PerfCounter


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

        public static void Calculate7zSpeed()
        {
            RandomPassword.EndTime = DateTime.Now;
            TimeSpan span = RandomPassword.EndTime.Subtract(RandomPassword.StartTime);
            double MyInterval = span.TotalSeconds;
            if (MyInterval > 0)
            {
                double DTempSpeed = (PassCount * 60) / MyInterval;
                RandomPassword.MySpeed = Convert.ToInt64(DTempSpeed);
            }

            double MyTempInterval = RandomPassword.ElapsSec7z;
            if (MyTempInterval > 0)
            {
                double DMySPeedTemp = (100 * 60) / MyTempInterval;
                RandomPassword.MySpeedTemp = Convert.ToInt64(DMySPeedTemp);
            }
        }

        public static void CalculateSpeed()
        {
            //RandomPassword.EndTime = DateTime.Now;
            TimeSpan span = RandomPassword.EndTime.Subtract(RandomPassword.StartTime);
            double MyInterval = span.TotalSeconds;
            if (MyInterval > 0)
            {
                string STempSpeed = Miki.CalcStrings.Mul(SessionPassCountDoneS, "60")[0];
                STempSpeed = Miki.CalcStrings.Div(STempSpeed, Convert.ToString(Convert.ToInt64(MyInterval)))[0];
                //double DTempSpeed = (PassCount * 60) / MyInterval;
                RandomPassword.MySpeed = Convert.ToInt64(STempSpeed);
            }

            double MyTempInterval = RandomPassword.ElapsSec7z;
            if (MyTempInterval > 0)
            {
                double DMySPeedTemp = (LehmerGen.Passwords.DictSize * 60) / MyTempInterval;
                RandomPassword.MySpeedTemp = Convert.ToInt64(DMySPeedTemp);
            }
        }

        public static void SaveProgress(string DoneIter, string MaxIter, string JobKind)
        {

            string ProgressFile = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.txt");
            string ProgressFileBak = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.bak.txt");

            if (JobKind == "7z")
            {
                Calculate7zSpeed();
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "    " + SessionPassCountDoneS + "    SAVE    " + DoneIter + "    FROM:   " + MaxIter + "    Speed Act/Full: " + MySpeedTemp + " / " + MySpeed + " /m    Tasks7z: " + PublicMyTaskCount);
            }
            if (JobKind == "HC")
            {
                CalculateSpeed();
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "    " + SessionPassCountDoneS + "    SAVE    " + DoneIter + "    FROM:   " + MaxIter + "    Speed Act/Full: " + MySpeedTemp + " / " + MySpeed + " /m    FalsePos: " + FalsePosCount);
            }
            if (JobKind == "JTR")
            {
                CalculateSpeed();
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "    " + SessionPassCountDoneS + "    SAVE    " + DoneIter + "    FROM:   " + MaxIter + "    Speed Act/Full: " + MySpeedTemp + " / " + MySpeed + " /m    FalsePos: " + FalsePosCount);
            }


            //IterCount = IterAll;

            if (File.Exists(ProgressFile))
            {
                if (File.Exists(ProgressFileBak)) // delete prev backup
                    File.Delete(ProgressFileBak);

                File.Move(ProgressFile, ProgressFileBak);
            }

            //if (File.Exists(ProgressFile))
            //{ File.Delete(ProgressFile); }


            if (!File.Exists(ProgressFile))
            {
                // Create a file to write to.

                using (Stream fs = new FileStream(ProgressFile, FileMode.Create, FileAccess.Write, FileShare.None, 0x1000, FileOptions.WriteThrough))
                using (StreamWriter shellConfigWriter = new StreamWriter(fs))
                {

                    shellConfigWriter.WriteLine(DoneIter);
                    shellConfigWriter.Write(String.Join(String.Empty, MaxIter));

                    shellConfigWriter.Flush();
                    shellConfigWriter.Dispose();
                    shellConfigWriter.Close();
                    //shellConfigWriter.BaseStream.Flush();
                }


                /*using (StreamWriter Writer = File.CreateText(ProgressFile))
                {
                    Writer.WriteLine(DoneIter);
                    Writer.Write(String.Join(String.Empty, MaxIter));
                    Writer.Flush();
                    Writer.Dispose();
                    Writer.Close();
                }*/
            }




            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }

        public static List<string> ReadProgress()
        {
            string ProgressFile = System.IO.Path.Combine(ProgramDir, "Config", "SessionProgress.txt");
            string IterLastDone;

            List<string> ProgressStringList = new List<string>();

            using (System.IO.StreamReader Pfile = new System.IO.StreamReader(ProgressFile))
            {
                while ((IterLastDone = Pfile.ReadLine()) != null)
                { ProgressStringList.Add(IterLastDone); }
                Pfile.Dispose();
                Pfile.Close();
            }

            return ProgressStringList;

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
                        shellConfigWriter.Write(String.Join(String.Empty, CrackedPass));

                        shellConfigWriter.Flush();
                        shellConfigWriter.Dispose();
                        shellConfigWriter.Close();
                        //shellConfigWriter.BaseStream.Flush();
                    }

                    // Create a file to write to.
                    /*using (StreamWriter sw = File.CreateText(MyPasswordFile))
                    {
                        sw.WriteLine("Password is:");
                        sw.Write(String.Join(String.Empty, CrackedPass));
                        sw.Flush();
                        sw.Dispose();
                        sw.Close();
                    }*/
                }

            }

        }

        public static int ListProc7za()
        {
            int My7zCount = 0;

            Process[] processCollection = Process.GetProcesses();

            foreach (Process p in processCollection)
            {
                //await ;
                if (p.ProcessName == "7za")
                {
                    My7zCount++;
                }
            }
            //My7zCount = 0;

            return My7zCount;
        }


    }

}


namespace LehmerGen
{
    class Passwords
    {
        public static List<long> IncrementList; //static
        public static List<long> PoolValueList; //static
        public static List<long> DivList; //static - 62
        public static List<long> DigPassListCurr; //dynamic
        public static List<long> DigPassListNext; //dynamic
        public static List<char> CharactersList;
        public static int PassGenerated = 0;
        public static string IterDone = "";
        public static string IterToDo = "";
        public static int WhatIs;
        public static string PassTotalCount;
        public static string PassIterCountFix = "";
        public static string PassIterMaxFix = "";
        public static string PassIterPoolMaxFix = "";
        public static string PassIterCountNFix = "0";
        public static string Increment = "";
        public static string IncrementBase;
        public static string IncrementPower;
        public static int PassMinLength;
        public static int PassMaxLength;
        public static int PassCharListLength;
        public static string PassCharCount;
        public static uint HCDictSize;
        public static uint JTRDictSize;
        public static uint DictSize;
        public static List<char> MyCharList = new List<char>();
        public static int TestPassCheck;
        public static int TestPassLoops;

        public static void InitializeGenerator()
        {

            PassCharListLength = PassMaxLength - 1;

            PassIterCountFix = Miki.CalcStrings.Pow(PassCharCount, Convert.ToString(PassMaxLength))[0]; //include 000000000....00 - zzzzzzzzz....zz, eg. for 00-99 we have 100 possibilities (10^2), so the MaxValue is 99.
            PassIterMaxFix = Miki.CalcStrings.Add(Passwords.PassIterCountFix, "-1")[0]; //the MaxValue is 1 less
            PassTotalCount = PassIterCountFix;
            if (PassMaxLength != PassMinLength)
            {

                for (int i = PassMinLength; i <= PassMaxLength; i++)
                {
                    string ThisPassPartCount = Miki.CalcStrings.Pow(PassCharCount, Convert.ToString(i))[0];
                    PassIterCountNFix = Miki.CalcStrings.Add(PassIterCountNFix, ThisPassPartCount)[0];
                }
                PassTotalCount = PassIterCountNFix;

            }


            Passwords.Increment = Miki.CalcStrings.Pow(IncrementBase, IncrementPower)[0];


            List<long> XtempIncr = Miki.CalcConvert.StringToLongList(Increment, 18);
            List<List<long>> DivMaxValue = Miki.CalcConvert.StringsToLongLists(PassCharCount, PassIterCountFix, 18);

            Passwords.IncrementList = XtempIncr; //Increment list
            Passwords.PoolValueList = DivMaxValue[1]; //PassPool list
            Passwords.DivList = DivMaxValue[0]; // PassCharCount List

            Console.WriteLine("Pass Iteration Count:             " + Passwords.PassIterCountFix);
            Console.WriteLine("Pass Max Iteration Value:         " + Passwords.PassIterMaxFix);
            Console.WriteLine("Pass Total Count:                 " + Passwords.PassTotalCount);
            Console.WriteLine("Increment:                        " + Passwords.Increment);

            if (Miki.CalcStrings.Div(PassIterCountFix, Passwords.Increment)[1] != "0") //Check if Increment value is good for LCG
            { Console.WriteLine("TRUE - Good Increment: Iteration Count % Increment != 0"); }
            else
            { Console.WriteLine("FALSE - Bad Increment: Iteration Count % Increment == 0"); return; }

        }


        public static void PrepareGeneratorNext(bool GetFirst)
        {

            string PassNumber = "";
            switch (Passwords.WhatIs)
            {
                case 0:
                    PassNumber = DigFromPass(LehmerGen.Passwords.IterToDo);
                    break;
                case 1:
                    PassNumber = Miki.CalcStrings.Div(Miki.CalcStrings.Mul(LehmerGen.Passwords.IterToDo, Passwords.Increment)[0], Passwords.PassIterCountFix)[1];
                    break;
                case 2:
                    PassNumber = LehmerGen.Passwords.IterToDo;
                    break;
            }

            string Xtemp = PassNumber;

            List<long> XtempIncr = Miki.CalcConvert.StringToLongList(Xtemp, 18);

            DigPassListCurr = XtempIncr;

            if (GetFirst == false)
            {
                _ = PassFromDig();
                DigPassListCurr = (DigPassListNext);
                Console.WriteLine("XXXX");
            }

        }



        public static List<string> PassListFromDig(uint PassToBeGen, bool CounterDisplay)
        {

            List<string> PassTextListLocal = new List<string>();
            PassGenerated = 0;

            while (true)
            {
                string Pass = PassFromDig();

                PassTextListLocal.Add(Pass);
                PassGenerated++;

                if (CounterDisplay == true)
                {
                    if (PassGenerated % 100000 == 0)
                    {

                        int PassPercent = (int)(PassGenerated * 100 / PassToBeGen);
                        string PassGeneratedString = string.Format("Pass Generated: {0}    {1} %", PassGenerated, PassPercent);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                }
                if (PassGenerated == PassToBeGen)
                {
                    if (CounterDisplay == true)
                    {

                        int PassPercent = (int)(PassGenerated * 100 / PassToBeGen);
                        string PassGeneratedString = string.Format("Pass Generated: {0}    {1} %", PassGenerated, PassPercent);
                        Console.Write("\r{0}   ", PassGeneratedString);
                    }
                    break;
                }

            }

            return PassTextListLocal;

        }


        public static string PassFromDig()
        {
            //generate long string-number and convert to 32-char password
            //based on Lehmer algorithm
            //very good to test calculator
            //to avoid lots of conversion long lists <-> strings, all calculation are made on lists
            //becouse of calculation on lists, list must be global
            string Pass = "";
            long MyDiv = 1000000000000000000;
            if (LehmerGen.Passwords.MyCharList.Count == 0)
            {

                List<long> Reminder;
                //int k;

                Reminder = DigPassListCurr;

                //k = 0;

                for (int h = PassCharListLength; h >= 0; h--) //(int h = PassCharListLength - PassIterPartialNFix; h >= 0; h--)
                {
                    List<List<long>> templist = Miki.CalcLists.Div(Reminder, DivList, MyDiv, true);
                    int TempInt = (int)templist[1][0]; //reminder
                    LehmerGen.Passwords.MyCharList.Add(CharactersList[TempInt]);
                    Reminder = templist[0]; //return with quotient
                }

                MyCharList.Reverse();
                Pass = new string(MyCharList.ToArray());
                //return Pass;
            }



            if (PassIterCountNFix != "0")
            {


                if (MyCharList[0] == (char)'0' && MyCharList.Count > 1)
                {
                    LehmerGen.Passwords.MyCharList.RemoveAt(0);
                    Pass = new string(MyCharList.ToArray());

                }

                if (MyCharList[0] != (char)'0' || MyCharList.Count == 1)
                {
                    List<long> PartDigPassListNext = Miki.CalcLists.Add(DigPassListCurr, IncrementList, MyDiv);
                    DigPassListNext = Miki.CalcLists.Div(PartDigPassListNext, PoolValueList, MyDiv, true)[1];
                    DigPassListCurr = DigPassListNext; //new
                    Pass = new string(MyCharList.ToArray());
                    LehmerGen.Passwords.MyCharList.Clear();
                }

                return Pass;
            }
            else
            {
                Pass = new string(MyCharList.ToArray());
                List<long> PartDigPassListNext = Miki.CalcLists.Add(DigPassListCurr, IncrementList, MyDiv);
                DigPassListNext = Miki.CalcLists.Div(PartDigPassListNext, PoolValueList, MyDiv, true)[1];
                DigPassListCurr = DigPassListNext; //new
                LehmerGen.Passwords.MyCharList.Clear();
                return Pass;
            }

        }
        public static string DigFromPass(string MyPass)
        {
            //convert 32-char password to string-number

            List<char> CharList = new List<char>(MyPass);
            int k = 0;
            List<long> kList = new List<long>() { k };

            List<long> DigList = new List<long>() { 0 };



            for (int i = CharList.Count - 1; i >= 0; i--)
            {

                int CharInt = CharactersList.IndexOf(CharList[i]);

                List<long> CharIntList = new List<long>() { CharInt };

                List<long> MyPower = Miki.CalcLists.Pow(DivList, kList);
                List<long> TempDigList = Miki.CalcLists.Mul(CharIntList, MyPower, 1000000000);
                TempDigList = Miki.CalcConvert.ConvertLists(TempDigList, 1000000000, 1000000000000000000);
                DigList = Miki.CalcLists.Add(DigList, TempDigList, 1000000000000000000);
                k++;
                kList[0] = k;
            }

            string Dig = Miki.CalcConvert.LongListToString(DigList, "000000000000000000");
            return Dig;
        }


        public static void TestGenerator(int WithCheck, int LoopCount)
        {
            Console.WriteLine("");
            Console.WriteLine("Generator started, wait...");
            DateTime FullStart = DateTime.Now;
            DateTime LoopStart = DateTime.Now;
            DateTime FullEnd;
            DateTime LoopEnd;

            string ThisCounter = "0";
            int ThisCounterInt = 0;

            while (true)
            {


                ThisCounterInt += 1;

                if (WithCheck == 1)
                {
                    List<long> DigPassListAct = DigPassListCurr;
                    List<string> PassTest = PassListFromDig(1, false);
                    string DigPassDTest = DigFromPass(PassTest[0]);

                    if (DigPassDTest == Miki.CalcConvert.LongListToString(DigPassListAct, "000000000000000000"))
                    {
                        if (ThisCounterInt % LoopCount == 0)
                        {
                            FullEnd = DateTime.Now;
                            LoopEnd = DateTime.Now;
                            TimeSpan spanFull = FullEnd.Subtract(FullStart);
                            TimeSpan spanLoop = LoopEnd.Subtract(LoopStart);

                            string MyIntervalFull = Miki.CalcStrings.Div(Convert.ToString(Convert.ToInt64(spanFull.TotalMilliseconds)), "1000")[0];
                            double MyIntervalLoop = spanLoop.TotalMilliseconds / 1000; //Miki.CalcStrings.Div(Convert.ToString(Convert.ToInt64(spanLoop.TotalMilliseconds)), "1000")[0];

                            string speedFull = Miki.CalcStrings.Div(ThisCounter, MyIntervalFull)[0];
                            double speedLoop = Math.Round(LoopCount / MyIntervalLoop, 2);
                            ThisCounter = Miki.CalcStrings.Add(ThisCounter, Convert.ToString(ThisCounterInt))[0];

                            Console.WriteLine(string.Format("{0}    {1}    SpeedTootal: {2}    SpeedLoop: {3}", ThisCounter, PassTest[0], speedFull, speedLoop));
                            ThisCounterInt = 0;
                            LoopStart = DateTime.Now;
                        }
                    }
                    else { Console.WriteLine("ERROR"); }
                }
                else
                {
                    List<long> DigPassListAct = DigPassListCurr;
                    List<string> PassTest = PassListFromDig(1, false);

                    if (ThisCounterInt % LoopCount == 0)
                    {
                        FullEnd = DateTime.Now;
                        LoopEnd = DateTime.Now;
                        TimeSpan spanFull = FullEnd.Subtract(FullStart);
                        TimeSpan spanLoop = LoopEnd.Subtract(LoopStart);
                        string MyIntervalFull = Miki.CalcStrings.Div(Convert.ToString(Convert.ToInt64(spanFull.TotalMilliseconds)), "1000")[0];
                        double MyIntervalLoop = spanLoop.TotalMilliseconds / 1000;

                        string speedFull = Miki.CalcStrings.Div(ThisCounter, MyIntervalFull)[0];
                        double speedLoop = Math.Round(LoopCount / MyIntervalLoop, 2);
                        ThisCounter = Miki.CalcStrings.Add(ThisCounter, Convert.ToString(ThisCounterInt))[0];

                        Console.WriteLine(string.Format("{0}    {1}    SpeedTootal: {2}    SpeedLoop: {3}", ThisCounter, PassTest[0], speedFull, speedLoop));
                        ThisCounterInt = 0;
                        LoopStart = DateTime.Now;
                    }
                }
                if (Passwords.PassTotalCount == ThisCounter)
                {
                    return;
                }
            }
        }

        public static void MyPerformanceCheck()
        {
            DateTime FullStart = DateTime.Now;
            DateTime LoopStart = DateTime.Now;
            DateTime FullEnd;
            DateTime LoopEnd;

            int ThisCounter = 0;

            int LoopCount = 2000000;

            string TestString = "123456789012345678";

            int Opt = 0;
            //long TestString = 123456789012345678;
            //long[] TestList = new long[1];
            //List<long> TestList = new List<long>() { TestString };
            //List<long> TestList = new List<long>();
            //TestList.Clear();
            //long[] TestList = new long[] { TestString };
            while (true)
            {

                ThisCounter += 1;
                //string to long
                //long aaa = Int64.Parse(TestString); //27 800
                //long aaa = Convert.ToInt64(TestString); //27 500

                //long to string
                //string aaa = Convert.ToString(TestString); //18 600
                //string aaa = TestString.ToString(); //17 600

                //Zmiana
                //TestList[0] = TestString; //arr 333 000
                //TestList[0] = TestString; //list 215 000

                //Dodawanie
                //TestList = TestList.Append(TestString).ToArray(); TestList = new long[] { }; //arr 10 500
                //TestList.Add(TestString); TestList.Clear(); // list 220 000

                //Czytanie
                //long abv = TestList[0]; // arr 295 000
                //long abv = TestList[0]; //list 275 000

                //Tworzenie
                //List<long> TestList = new List<long>() { TestString }; //52 000
                //long[] TestListA = new long[] { TestString }; //150 000

                //Tworzenie + dodanie
                //long[] TestListA = new long[] { TestString }; List<long> TestListL = TestListA.ToList(); // 17 300
                //List<long> TestListL = new List<long>() { TestString }; long[] TestListA = TestListL.ToArray(); //26 200

                //Konwersja
                //List<long> TestListL = TestList.ToList(); //19 200
                //long [] TestListA = TestList.ToArray(); // 50 000

                //Czytanie Count
                //int i = TestList.Count(); //array 60 000
                //int i = TestList.Count(); //list 146 000

                if ((ThisCounter % LoopCount) == 0)
                {

                    FullEnd = DateTime.Now;
                    LoopEnd = DateTime.Now;

                    TimeSpan spanFull = FullEnd.Subtract(FullStart);
                    TimeSpan spanLoop = LoopEnd.Subtract(LoopStart);
                    int MyIntervalFull = (int)spanFull.TotalMilliseconds;
                    int MyIntervalLoop = (int)spanLoop.TotalMilliseconds;

                    int speedFull = ThisCounter / MyIntervalFull;
                    int speedLoop = LoopCount / MyIntervalLoop;

                    Console.WriteLine(string.Format("{0}     SpeedTootal: {1}    SpeedLoop: {2}   Opt: {3}", ThisCounter, speedFull, speedLoop, Opt));
                    LoopStart = DateTime.Now;

                }

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
                    isIntDig1 = isNumeric(Dig1); // Dig1.All(char.IsDigit);
                    LineRev += 1;
                    if (isIntDig1 == false)
                    { Console.WriteLine("Digits only!!!... try again..."); LineRev += 2; }
                }

                while (Dig1.Substring(0, 1) == "0") //first digit can't be 0
                { Dig1 = Dig1.Substring(1); }

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
                    isIntDig2 = isNumeric(Dig2);// Dig2.All(char.IsDigit);
                    if (isIntDig2 == false)
                    { Console.WriteLine("Digits only... try again..."); LineRev += 2; }
                }

                while (Dig2.Substring(0, 1) == "0") //first digit can't be 0
                { Dig2 = Dig2.Substring(1); }

                for (int i = 0; i <= LineRev; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.WriteLine("\r{0}   ", Dig2);
                LineRev = 0;



                List<string> MyResult = new List<string>();
                List<string> MyResultCheck = new List<string>();
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

        public static bool isNumeric(string s)
        {
            char[] MyString = s.ToCharArray();
            for (int i = 0; i < MyString.Length; i++)
            {
                if (int.TryParse(MyString[i].ToString(), out int n) == false)
                { return false; }
            }
            return true;
        }



        public static void RunCalcTest()
        {
            //MikiAdd, MikiSub, MikiMul - returns [0]-Int-string-Result, [1]-Abs(Int-string-Result)
            //MikiDiv - returns [0]-Int-string-Result, [1]-Rest from division
            //MikiPow - returns [0]-Int-string-Result

            List<string> MyOutputList = new List<string>();
            string[] TestCalc = new string[2];
            var sw = Stopwatch.StartNew();

            string Dig1 = "102543890397977681684285524423227768626815861760032026538";
            //string Dig1 = "99999999999999999999999999999999999999999999999999999999";
            //string Dig2 = "101111111397977681684285524423227768626815861760032026538";
            string Dig2 = "815861760032026538";

            Console.WriteLine(Dig1 + "  Length: " + Dig1.Length);
            Console.WriteLine(Dig2 + "  Length: " + Dig2.Length);
            List<string> MyOutputListM = new List<string>();
            List<string> MyOutputListA = new List<string>();

            int RepCount = 100;

            long swMin = 1000000000000;
            long swMax = 0;
            long swMsMin = 1000000000;
            Console.WriteLine("++++++++++++++++++++++++++++++");
            Console.WriteLine("-------ADD-------");
            for (int i = 0; i <= RepCount; i++)
            {
                sw = Stopwatch.StartNew();
                MyOutputList = CalcStrings.Add(Dig1, Dig2);
                sw.Stop();
                if (sw.ElapsedTicks < swMin) { swMin = sw.ElapsedTicks; swMsMin = sw.ElapsedMilliseconds; }
                if (sw.ElapsedTicks > swMax) { swMax = sw.ElapsedTicks; }
            }
            Console.WriteLine(string.Format("Ticks Min:  {0}    Ticks Max:  {1}    ms Min:  {2}", swMin, swMax, swMsMin));
            Console.WriteLine(MyOutputList[0]);

            Console.WriteLine("-----ADD CHECK-----");
            MyOutputListA = CalcStrings.Sub(MyOutputList[0], Dig2);
            Console.WriteLine(MyOutputListA[0]);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(MyOutputListA[0] == Dig1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("++++++++++++++++++++++++++++++");

            swMin = 1000000000000;
            swMax = 0;
            swMsMin = 1000000000;
            Console.WriteLine("-------SUB-------");
            for (int i = 0; i <= RepCount; i++)
            {
                sw = Stopwatch.StartNew();
                MyOutputList = CalcStrings.Sub(Dig1, Dig2);
                sw.Stop();
                if (sw.ElapsedTicks < swMin) { swMin = sw.ElapsedTicks; swMsMin = sw.ElapsedMilliseconds; }
                if (sw.ElapsedTicks > swMax) { swMax = sw.ElapsedTicks; }
            }
            Console.WriteLine(string.Format("Ticks Min:  {0}    Ticks Max:  {1}    ms Min:  {2}", swMin, swMax, swMsMin));
            Console.WriteLine(MyOutputList[0]);

            Console.WriteLine("-----SUB CHECK-----");
            MyOutputListA = CalcStrings.Add(MyOutputList[0], Dig2);
            Console.WriteLine(MyOutputListA[0]);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(MyOutputListA[0] == Dig1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("++++++++++++++++++++++++++++++");

            swMin = 1000000000000;
            swMax = 0;
            swMsMin = 1000000000;
            Console.WriteLine("-------MUL-------");
            for (int i = 0; i <= RepCount; i++)
            {
                sw = Stopwatch.StartNew();
                MyOutputList = CalcStrings.Mul(Dig1, Dig2);
                sw.Stop();
                if (sw.ElapsedTicks < swMin) { swMin = sw.ElapsedTicks; swMsMin = sw.ElapsedMilliseconds; }
                if (sw.ElapsedTicks > swMax) { swMax = sw.ElapsedTicks; }
            }
            Console.WriteLine(string.Format("Ticks Min:  {0}    Ticks Max:  {1}    ms Min:  {2}", swMin, swMax, swMsMin));
            Console.WriteLine(MyOutputList[0]);

            Console.WriteLine("-----MUL CHECK-----");
            MyOutputListA = CalcStrings.Div(MyOutputList[0], Dig2);
            Console.WriteLine(MyOutputListA[0]);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(MyOutputListA[0] == Dig1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("++++++++++++++++++++++++++++++");

            swMin = 1000000000000;
            swMax = 0;
            swMsMin = 1000000000;
            Console.WriteLine("-------DIV-------");
            for (int i = 0; i <= RepCount; i++)
            {
                sw = Stopwatch.StartNew();
                MyOutputList = CalcStrings.Div(Dig1, Dig2);
                sw.Stop();
                if (sw.ElapsedTicks < swMin) { swMin = sw.ElapsedTicks; swMsMin = sw.ElapsedMilliseconds; }
                if (sw.ElapsedTicks > swMax) { swMax = sw.ElapsedTicks; }
            }
            Console.WriteLine(string.Format("Ticks Min:  {0}    Ticks Max:  {1}    ms Min:  {2}", swMin, swMax, swMsMin));
            Console.WriteLine(MyOutputList[0]);
            Console.WriteLine(MyOutputList[1]);

            Console.WriteLine("-----DIV CHECK-----");
            MyOutputListM = CalcStrings.Mul(MyOutputList[0], Dig2);
            MyOutputListA = CalcStrings.Add(MyOutputListM[0], MyOutputList[1]);
            Console.WriteLine(MyOutputListA[0]);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(MyOutputListA[0] == Dig1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("++++++++++++++++++++++++++++++");

            swMin = 1000000000000;
            swMax = 0;
            swMsMin = 1000000000;
            Console.WriteLine("-------POW-------");
            for (int i = 0; i <= RepCount; i++)
            {
                sw = Stopwatch.StartNew();
                MyOutputList = CalcStrings.Pow(Dig1, "10");
                sw.Stop();
                if (sw.ElapsedTicks < swMin) { swMin = sw.ElapsedTicks; swMsMin = sw.ElapsedMilliseconds; }
                if (sw.ElapsedTicks > swMax) { swMax = sw.ElapsedTicks; }
            }
            Console.WriteLine(string.Format("Ticks Min:  {0}    Ticks Max:  {1}    ms Min:  {2}", swMin, swMax, swMsMin));
            Console.WriteLine(MyOutputList[0]);
            Console.WriteLine(MyOutputList[1]);
        }


    }

    public static class CalcIntExt
    {
        public static int IntLength(long i)
        {

            if (i == 0)
                return 1;

            return (int)Math.Floor(Math.Log10(i)) + 1;
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
                return @this.Substring(0, count);
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

    }

    public static class CalcLists
    {

        public static List<long> Add(List<long> Dig1List, List<long> Dig2List, long MyDiv)
        {
            //Old, very good

            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }

            List<long> MyOutput = new List<long>();

            List<long> Dig1ListTemp;
            List<long> Dig2ListTemp;

            int Dig1ListTempCount = Dig1ListCount;
            int Dig2ListTempCount = Dig2ListCount;

            long tempUp = 0;
            long temp;

            if (Dig1ListCount < Dig2ListCount)
            {
                Dig1ListTemp = new List<long>(Dig2List);
                Dig2ListTemp = new List<long>(Dig1List);
                Dig1ListTempCount = Dig2ListCount;
                Dig2ListTempCount = Dig1ListCount;
            }
            else
            {
                Dig1ListTemp = new List<long>(Dig1List);
                Dig2ListTemp = new List<long>(Dig2List);
            }

            int loops = Dig1ListTempCount - 1;
            for (int i = 0; i <= loops; i++) //adding Lists
            {
                long NextDig2Number = 0;
                if (i < Dig2ListTempCount)
                {
                    NextDig2Number = Dig2ListTemp[i];

                }


                temp = Dig1ListTemp[i] + NextDig2Number + tempUp;

                if (temp >= MyDiv)
                {
                    tempUp = 1;
                    temp -= MyDiv;
                    MyOutput.Add(temp);
                    if (i == loops)
                    {
                        MyOutput.Add(tempUp);
                        break; //????????
                    }

                }
                else
                {
                    tempUp = 0;
                    MyOutput.Add(temp);
                }

            }
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedTicks);
            return MyOutput;
        }



        public static List<long> Sub(List<long> Dig1List, List<long> Dig2List, long MyDiv)
        {
            int Dig1ListCount = Dig1List.Count;
            if (Dig1ListCount == 1 && Dig1List[0] == 0)
            { return Dig2List; }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            { return Dig1List; }

            List<long> Dig1ListTemp;
            List<long> Dig2ListTemp;
            List<long> MyOutput = new List<long>();
            int debt = 0;
            long temp;

            int Dig1ListTempCount = Dig1ListCount;
            int Dig2ListTempCount = Dig2ListCount;

            if (CalcCompare.ListBigger(Dig1List, Dig2List) == 2)
            {
                Dig1ListTemp = new List<long>(Dig2List);
                Dig2ListTemp = new List<long>(Dig1List);
                Dig1ListTempCount = Dig2ListCount;
                Dig2ListTempCount = Dig1List.Count;
            }
            else
            {
                Dig1ListTemp = new List<long>(Dig1List);
                Dig2ListTemp = new List<long>(Dig2List);
            }

            //int ListDiff = Dig1ListTempCount - Dig2ListTempCount;

            for (int i = 0; i < Dig1ListTempCount; i++) //Subtract arrays
            {
                long NextDig2Number = 0;
                if (i < Dig2ListTempCount)
                {
                    NextDig2Number = Dig2ListTemp[i];
                }


                temp = (Dig1ListTemp[i] - NextDig2Number) - debt;

                if (temp < 0)
                {

                    debt = 1;
                    MyOutput.Add(temp + MyDiv);
                }
                else
                //if (temp > 0)
                {
                    debt = 0;
                    MyOutput.Add(temp);
                }

            }

            for (int i = MyOutput.Count - 1; i >= 1; i--)
            {
                if (MyOutput[i] == 0)
                { MyOutput.RemoveAt(i); }
                if (MyOutput[MyOutput.Count - 1] != 0) //^
                { break; }
            }

            return MyOutput;
        }

        public static List<long> Mul(List<long> Dig1List, List<long> Dig2List, long MyDiv)
        {

            int Dig1ListCount = Dig1List.Count;
            if (Dig1List.Count == 1)
            {
                if (Dig1List[0] == 1)
                { return Dig2List; }
                if (Dig1List[0] == 0)
                { return Dig1List; }

            }
            int Dig2ListCount = Dig2List.Count;
            if (Dig2List.Count == 1)
            {
                if (Dig2List[0] == 1)
                { return Dig1List; }
                if (Dig2List[0] == 0)
                { return Dig2List; }
            }

            int Dig1ListTempCount = Dig1ListCount;
            int Dig2ListTempCount = Dig2ListCount;

            int MyPoss = 0;
            List<long> Dig1ListTemp;
            List<long> Dig2ListTemp;
            long tempUp; ;

            if (Dig1List.Count < Dig2List.Count)
            {
                Dig1ListTemp = new List<long>(Dig2List);
                Dig2ListTemp = new List<long>(Dig1List);
                Dig1ListTempCount = Dig2ListCount;
                Dig2ListTempCount = Dig1ListCount;
            }
            else
            {
                Dig1ListTemp = new List<long>(Dig1List);
                Dig2ListTemp = new List<long>(Dig2List);
            }


            int Loop1Count = Dig1ListTempCount; //Dig1List.Count == Dig2List.Count
            int Loop2Count = Dig2ListTempCount;
            int ResultCount = (Dig1ListTempCount + Dig2ListTempCount) - 1;
            List<long> ResultList = new List<long>(new long[ResultCount]); // Product (ResultList) can be max Dig1List.Count + Dig2List.Count digit long or 1 less

            for (int i = 0; i < Loop2Count; i++)
            {

                for (int k = 0; k < Loop1Count; k++)
                {

                    long temp = (Dig2ListTemp[i] * Dig1ListTemp[k]) + ResultList[MyPoss];
                    if (temp >= MyDiv)
                    {
                        tempUp = temp / MyDiv;
                        temp %= MyDiv;
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



        public static List<List<long>> Div(List<long> Dig1List, List<long> Dig2List, long MyDiv18, bool CheckDecimal)
        {

            List<List<long>> MyOutput = new List<List<long>>();
            int Dig1ListCount = Dig1List.Count;
            int Dig2ListCount = Dig2List.Count;
            if (Dig1ListCount < Dig2ListCount) // fast pass
            {
                MyOutput.Add(new List<long>() { 0 });
                MyOutput.Add(Dig1List);
                return MyOutput;
            }

            if (Dig1ListCount == 1 && Dig2ListCount == 1) // sometimes we can simply divide two longs - fast pass
            {
                if (Dig2List[0] != 0)
                {
                    MyOutput.Add(new List<long>() { Dig1List[0] / Dig2List[0] });
                    MyOutput.Add(new List<long>() { Dig1List[0] % Dig2List[0] });
                }
                else
                {
                    MyOutput.Add(new List<long>() { 0 }); //error Div by 0, but I don't care
                    MyOutput.Add(new List<long>() { 0 });
                    Console.WriteLine("ERROR - Div by 0 - Return: 0");
                }
                return MyOutput;
            }


            int Dig2ListFirstLength;
            int Dig2ListLength;

            int Dig1ListFirstLength;
            int Dig1ListLength;

            int BiggerList = 1;

            Dig2ListFirstLength = CalcIntExt.IntLength(Dig2List[Dig2ListCount - 1]); //static
            Dig2ListLength = (Dig2ListCount - 1) * 18 + Dig2ListFirstLength; //static

            Dig1ListFirstLength = CalcIntExt.IntLength(Dig1List[Dig1ListCount - 1]); //must be counted in each loop and it is necessary here
            Dig1ListLength = (Dig1ListCount - 1) * 18 + Dig1ListFirstLength; //must be counted in each loop and it is necessary here

            if (Dig1ListLength < Dig2ListLength)
            {
                MyOutput.Add(new List<long>() { 0 });
                MyOutput.Add(Dig1List);
                return MyOutput;
            }

            if (Dig1ListLength == Dig2ListLength)
            {
                BiggerList = CalcCompare.ListBigger(Dig1List, Dig2List);

                switch (BiggerList)
                {
                    case 0:
                        MyOutput.Add(new List<long>() { 1 });
                        MyOutput.Add(new List<long>() { 0 });
                        return MyOutput;
                    case 2:
                        MyOutput.Add(new List<long>() { 0 });
                        MyOutput.Add(Dig1List);
                        return MyOutput;
                }

            }

            if (CheckDecimal == true) //sometimes we can simply calculate decimals
            {

                if (Dig1ListLength < 29 && Dig2ListLength < 29)
                {
                    decimal DecimalDig1 = CalcCompare.GetDecimalFromList(Dig1List);
                    decimal DecimalDig2 = CalcCompare.GetDecimalFromList(Dig2List);

                    decimal TempRestDec = DecimalDig1 % DecimalDig2;
                    decimal DecDecOutput = ((DecimalDig1 - TempRestDec) / DecimalDig2);

                    List<long> LongResult = new List<long>();
                    if (DecDecOutput >= MyDiv18)
                    { LongResult = new List<long>() { (long)(DecDecOutput % MyDiv18), (long)(DecDecOutput / MyDiv18) }; }
                    else
                    { LongResult = new List<long>() { (long)(DecDecOutput) }; }

                    List<long> LongRest = new List<long>();
                    if (TempRestDec >= MyDiv18)
                    { LongRest = new List<long>() { (long)(TempRestDec % MyDiv18), (long)(TempRestDec / MyDiv18) }; }
                    else
                    { LongRest = new List<long>() { (long)(TempRestDec) }; }

                    MyOutput.Add(LongResult);
                    MyOutput.Add(LongRest);
                    return MyOutput;
                }

            }


            List<long> TempDig2List18 = new List<long>();
            List<long> TempDig2List9 = new List<long>();
            List<long> TempMultiplyList = new List<long>();
            long First18DigDig1;
            long First17DigDig2;
            long MyDiv9 = 1000000000;

            List<long> Dig2OrigList9 = CalcConvert.ConvertLists(Dig2List, MyDiv18, MyDiv9); //static, necessary for multiplication

            //prepare number from Dig2 to estimate multiplier, take 17 digits
            if (Dig2ListLength > 17)
            {
                First17DigDig2 = (CalcCompare.GetLongFromList(Dig2List, 16, 18) * 10) + 9; //last digit is uknown, so it can be 9
            }
            else
            {
                First17DigDig2 = CalcCompare.GetLongFromList(Dig2List, 17, 18); //static, necessary to find "safe multiplier"
            }


            List<long> MultiplyList = new List<long>() { 0 };

            while (BiggerList < 2) //division by repeated subtraction
            {

                List<long> SafeMultiplierList18 = new List<long>();


                if (Dig1ListLength < 29 && Dig2ListLength < 29) //sometimes we can simply calculate on decimals at the end
                {

                    if (Dig1ListLength < 19 && Dig2ListLength < 19) // or on longs
                    {
                        long LongDig1 = Dig1List[0];
                        long LongDig2 = Dig2List[0];

                        long TempRestLong = LongDig1 % LongDig2;
                        long LongOutput = ((LongDig1 - TempRestLong) / LongDig2); //SafeMultiplierList18

                        long LongTempDig2 = LongOutput * LongDig2; // TempDig2List18

                        SafeMultiplierList18 = new List<long>() { LongOutput };
                        TempDig2List18 = new List<long>() { LongTempDig2 };
                    }
                    else
                    {
                        decimal DecimalDig1 = CalcCompare.GetDecimalFromList(Dig1List);
                        decimal DecimalDig2 = CalcCompare.GetDecimalFromList(Dig2List);

                        decimal TempRestDec = DecimalDig1 % DecimalDig2;
                        decimal DecDecOutput = ((DecimalDig1 - TempRestDec) / DecimalDig2); //SafeMultiplierList18

                        if (DecDecOutput >= MyDiv18) //SafeMultiplierList18
                        { SafeMultiplierList18 = new List<long>() { (long)(DecDecOutput % MyDiv18), (long)(DecDecOutput / MyDiv18) }; }
                        else
                        { SafeMultiplierList18 = new List<long>() { (long)(DecDecOutput) }; }

                        decimal DecTempDig2 = DecDecOutput * DecimalDig2; // TempDig2List18
                        if (DecTempDig2 >= MyDiv18)
                        { TempDig2List18 = new List<long>() { (long)(DecTempDig2 % MyDiv18), (long)(DecTempDig2 / MyDiv18) }; }
                        else
                        { TempDig2List18 = new List<long>() { (long)(DecTempDig2) }; }

                    }
                }
                else //but if numbers are bigger then decimal
                {
                    //here will be numbers longer then 18 digits, only
                    First18DigDig1 = CalcCompare.GetLongFromList(Dig1List, 17, 18); //so we take 17 digits... must be counted in each loop

                    { First18DigDig1 *= 10; } //and multiply by 10 because next digit is uknown, so it can be 0

                    long SafeMultiplier = First18DigDig1 / First17DigDig2; //it is max 18 digits

                    if (Dig2ListLength == Dig1ListLength) // This is necessary.
                    {
                        SafeMultiplier /= 10;
                        if (SafeMultiplier == 0) { SafeMultiplier = 1; }
                    }

                    SafeMultiplierList18 = new List<long>() { SafeMultiplier };


                    List<long> SafeMultiplierList9 = new List<long>();
                    if (SafeMultiplier >= MyDiv9) //faster
                    { SafeMultiplierList9 = new List<long>() { SafeMultiplier % MyDiv9, SafeMultiplier / MyDiv9 }; }
                    else
                    { SafeMultiplierList9 = new List<long>() { SafeMultiplier }; }

                    TempDig2List9 = (CalcLists.Mul(Dig2OrigList9, SafeMultiplierList9, MyDiv9));

                    TempDig2List18 = (CalcConvert.ConvertLists(TempDig2List9, MyDiv9, MyDiv18));

                    //int TempDig2ListFirstLength = CalcIntExt.IntLength(TempDig2List18[^1]); //must be counted in each loop
                    int TempDig2List9Count = TempDig2List9.Count;
                    int TempDig2ListLength = (TempDig2List9Count - 1) * 9 + CalcIntExt.IntLength(TempDig2List9[TempDig2List9Count - 1]); //must be counted in each loop

                    int ZerosToAdd = Dig1ListLength - TempDig2ListLength;

                    if (ZerosToAdd > 0) //if 0 then we multiply by 1
                    {
                        long FirstTempDig2List = CalcCompare.GetLongFromList(TempDig2List18, 1, 18);
                        long FirstDig1List = CalcCompare.GetLongFromList(Dig1List, 1, 18);

                        if (FirstDig1List - FirstTempDig2List < 0)
                        { ZerosToAdd -= 1; }

                        //Console.WriteLine(Dig1ListLength + "      " + TempDig2ListLength);
                        int ZerosD = (ZerosToAdd) / 9;
                        int ZerosU = (ZerosToAdd) % 9;
                        //Console.WriteLine(ZerosD + "      " + ZerosU);
                        List<long> MultiplyBy10List = new List<long>();

                        for (int t = 0; t < ZerosD; t++)
                        { MultiplyBy10List.Add(0); }
                        MultiplyBy10List.Add((long)Math.Pow(10, ZerosU));

                        TempDig2List9 = (CalcLists.Mul(TempDig2List9, MultiplyBy10List, MyDiv9));
                        SafeMultiplierList9 = (CalcLists.Mul(SafeMultiplierList9, MultiplyBy10List, MyDiv9));
                    }

                    TempDig2List18 = (CalcConvert.ConvertLists(TempDig2List9, MyDiv9, MyDiv18));
                    SafeMultiplierList18 = (CalcConvert.ConvertLists(SafeMultiplierList9, MyDiv9, MyDiv18));

                }



                Dig1List = CalcLists.Sub(Dig1List, TempDig2List18, MyDiv18);
                MultiplyList = (CalcLists.Add(MultiplyList, SafeMultiplierList18, MyDiv18));

                //And we must check if we can break calculation. Checking is "time waster", it's the best place for it
                BiggerList = CalcCompare.ListBigger(Dig1List, Dig2List);

                if (BiggerList == 2)
                { break; }
                int ThisDig2ListCount = Dig1List.Count;
                Dig1ListFirstLength = CalcIntExt.IntLength(Dig1List[ThisDig2ListCount - 1]); //must be counted in each loop
                Dig1ListLength = (ThisDig2ListCount - 1) * 18 + Dig1ListFirstLength; //must be counted in each loop

            }


            MyOutput.Add(MultiplyList);
            MyOutput.Add(Dig1List);

            return MyOutput;
        }



        public static List<long> Pow(List<long> Dig1List, List<long> Dig2List)
        {

            List<long> MyPowerList = new List<long>() { 1 };
            List<long> MyPowerListAdd = new List<long>() { 1 };

            long MyDivM = 1000000000;
            long MyDivA = 1000000000000000000;

            //You can write condition to calculate power with proper result sign
            //or remember, if -Dig1Orig and Dig2Orig % 2 != 0 ResultSign = "-"
            int Dig2ListCount = Dig2List.Count;
            if (Dig2ListCount == 1 && Dig2List[0] == 0)
            {
                List<long> MyOutput1 = new List<long>() { 1 };
                return MyOutput1;
            }

            if (Dig2ListCount == 1 && Dig2List[0] == 1)
            {
                return Dig1List;
            }


            List<long> MyOutputList = Dig1List;

            while (CalcCompare.ListBigger(MyPowerList, Dig2List) == 2)
            {
                MyOutputList = CalcLists.Mul(MyOutputList, Dig1List, MyDivM);
                MyPowerList = CalcLists.Add(MyPowerList, MyPowerListAdd, MyDivA);
            }

            return MyOutputList;

        }

    }
    public class CalcStrings
    {
        public static List<string> Add(string Dig1Orig, string Dig2Orig)
        {

            List<string> MyOutput = new List<string>() { "", "" };

            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;

            if (Dig1Orig.Substring(0, 1) == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig.Substring(1); //[1..]
            }
            if (Dig2Orig.Substring(0, 1) == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig.Substring(1);
            }

            if (Dig1 == "0")
            {
                MyOutput[0] = Dig2Orig;
                MyOutput[1] = Dig2;
                return MyOutput;
            }

            if (Dig2 == "0")
            {
                MyOutput[0] = Dig1Orig;
                MyOutput[1] = Dig1;
                return MyOutput;
            }

            if (Dig1.Length < 29 && Dig2.Length < 29)
            {
                decimal IntOutput = Convert.ToDecimal(Dig1Orig) + Convert.ToDecimal(Dig2Orig);
                MyOutput[0] = Convert.ToString(IntOutput);
                MyOutput[1] = Convert.ToString(Math.Abs(IntOutput));
                return MyOutput;
            }

            if (Dig1Sign == "-" && Dig2Sign == "-")
            { ResultSign = "-"; }

            List<List<long>> DigsLists = CalcConvert.StringsToLongLists(Dig1, Dig2, 18);

            List<long> Dig1List = DigsLists[0];
            List<long> Dig2List = DigsLists[1];

            long MyDiv = 1000000000000000000;

            int DigBiggerTemp = CalcCompare.ListBigger(Dig1List, Dig2List);

            if (Dig1Sign == "-" && Dig2Sign != "-")
            {
                MyOutput[1] = CalcConvert.LongListToString(CalcLists.Sub(Dig1List, Dig2List, MyDiv), "000000000000000000");
                if (DigBiggerTemp == 1)
                { ResultSign = "-"; }
                MyOutput[0] = ResultSign + MyOutput[1];
                return MyOutput;
            }

            if (Dig1Sign != "-" && Dig2Sign == "-")
            {
                MyOutput[1] = CalcConvert.LongListToString(CalcLists.Sub(Dig1List, Dig2List, MyDiv), "000000000000000000");
                if (DigBiggerTemp == 2)
                { ResultSign = "-"; }
                MyOutput[0] = ResultSign + MyOutput[1];
                return MyOutput;
            }

            MyOutput[1] = CalcConvert.LongListToString(CalcLists.Add(Dig1List, Dig2List, MyDiv), "000000000000000000");

            if (MyOutput[1] == "")
            {
                MyOutput[1] = "0";
            }

            MyOutput[0] = ResultSign + MyOutput[1];

            return MyOutput;
        }

        public static List<string> Sub(string Dig1Orig, string Dig2Orig)
        {

            List<string> MyOutput = new List<string>() { "", "", "" };
            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;

            if (Dig1.Substring(0, 1) == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig.Substring(1);
            }
            if (Dig2.Substring(0, 1) == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig.Substring(1);
            }

            if (Dig1 == "0")
            {
                if (Dig2Sign == "-")
                {
                    MyOutput[0] = Dig2;
                    MyOutput[1] = Dig2;
                }
                else
                {
                    MyOutput[0] = "-" + Dig2;
                    MyOutput[1] = Dig2;
                }
                return MyOutput;
            }

            if (Dig2 == "0")
            {
                MyOutput[0] = Dig1Orig;
                MyOutput[1] = Dig1;
                return MyOutput;
            }

            if (Dig1.Length < 29 && Dig2.Length < 29)
            {
                decimal IntOutput = Convert.ToDecimal(Dig1Orig) - Convert.ToDecimal(Dig2Orig);
                MyOutput[0] = Convert.ToString(IntOutput);
                MyOutput[1] = Convert.ToString(Math.Abs(IntOutput));
                return MyOutput;
            }

            List<List<long>> DigsLists = CalcConvert.StringsToLongLists(Dig1, Dig2, 18);

            List<long> Dig1List = DigsLists[0];
            List<long> Dig2List = DigsLists[1];
            long MyDiv = 1000000000000000000;

            int DigBiggerTemp = CalcCompare.ListBigger(Dig1List, Dig2List);

            if (Dig1Sign == "-" && Dig2Sign != "-")
            {
                MyOutput[1] = CalcConvert.LongListToString(CalcLists.Add(Dig1List, Dig2List, MyDiv), "000000000000000000");
                ResultSign = "-";
                MyOutput[0] = ResultSign + MyOutput[1];
                return MyOutput;
            }

            if (Dig1Sign != "-" && Dig2Sign == "-")
            {
                MyOutput[1] = CalcConvert.LongListToString(CalcLists.Add(Dig1List, Dig2List, MyDiv), "000000000000000000");
                ResultSign = "";
                MyOutput[0] = ResultSign + MyOutput[1];
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

            MyOutput[1] = CalcConvert.LongListToString(CalcLists.Sub(Dig1List, Dig2List, MyDiv), "000000000000000000");

            if (MyOutput[1] == "")
            { MyOutput[1] = "0"; }

            MyOutput[0] = ResultSign + MyOutput[1];

            return MyOutput;
        }

        public static List<string> Mul(string Dig1Orig, string Dig2Orig)
        {

            List<string> MyOutput = new List<string>() { "", "", "" };
            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;

            if (Dig1.Substring(0, 1) == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig.Substring(1);
            }
            if (Dig2.Substring(0, 1) == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig.Substring(1);
            }

            if (Dig2 == "0" || Dig2 == "0")
            {
                MyOutput[0] = "0";
                MyOutput[1] = "0";
                return MyOutput;
            }

            if (Dig1.Length + Dig2.Length < 29)
            {
                decimal IntOutput = Convert.ToDecimal(Dig1Orig) * Convert.ToDecimal(Dig2Orig);
                MyOutput[0] = Convert.ToString(IntOutput);
                MyOutput[1] = Convert.ToString(Math.Abs(IntOutput));
                return MyOutput;
            }

            if (Dig1Sign != Dig2Sign)
            {
                ResultSign = "-";
            }

            List<List<long>> DigsLists = CalcConvert.StringsToLongLists(Dig1, Dig2, 9);

            List<long> Dig1List = DigsLists[0];
            List<long> Dig2List = DigsLists[1];

            long MyDiv = 1000000000;

            MyOutput[1] = CalcConvert.LongListToString(CalcLists.Mul(Dig1List, Dig2List, MyDiv), "000000000");

            if (MyOutput[1] == "")
            {
                MyOutput[1] = "0";
            }

            MyOutput[0] = ResultSign + MyOutput[1];

            return MyOutput;
        }

        public static List<string> Pow(string Dig1Orig, string Dig2Orig)
        {
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;
            string Dig1Sign = "";
            string Dig2Sign = "";

            List<string> MyOutput = new List<string>() { Dig1, "", "" };
            List<long> MyPowerList = new List<long>() { 1 };
            List<long> MyPowerListAdd = new List<long>() { 1 };

            if (Dig1Orig.Substring(0, 1) == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig.Substring(1);
            }
            if (Dig2Orig.Substring(0, 1) == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig.Substring(1);
            }

            //You can write condition to calculate power with proper result sign
            //or remember, if -Dig1Orig and Dig2Orig % 2 != 0 ResultSign = "-"

            if (Dig2 == "0")
            {
                MyOutput[0] = "1";
                return MyOutput;
            }

            if (Dig2 == "1")
            {
                MyOutput[0] = Dig1Orig;
                return MyOutput;
            }

            List<long> Dig1List = CalcConvert.StringToLongList(Dig1, 9);
            List<long> Dig2List = CalcConvert.StringToLongList(Dig2, 18);
            List<long> MyOutputList = CalcLists.Pow(Dig1List, Dig2List); // Dig1List;

            MyOutput[0] = CalcConvert.LongListToString(MyOutputList, "000000000");
            return MyOutput;
        }

        public static List<string> Div(string Dig1Orig, string Dig2Orig)
        {
            //most difficult part of job
            List<string> MyOutput = new List<string>() { "", "", "" };
            string ResultSign = "";
            string Dig1Sign = "";
            string Dig2Sign = "";
            string Dig1 = Dig1Orig;
            string Dig2 = Dig2Orig;
            int Dig1Length = Dig1.Length;
            int Dig2Length = Dig2.Length;

            if (Dig1.Substring(0, 1) == "-")
            {
                Dig1Sign = "-";
                Dig1 = Dig1Orig.Substring(1);
            }
            if (Dig2.Substring(0, 1) == "-")
            {
                Dig2Sign = "-";
                Dig2 = Dig2Orig.Substring(1);
            }

            if (Dig2 == "0")
            {
                MyOutput[0] = "ERROR Div by 0";
                MyOutput[1] = "ERROR Div by 0";
                return MyOutput;
            }

            if (Dig1Sign != Dig2Sign)
            {
                ResultSign = "-";
            }

            if (Dig1Length < Dig2Length)
            {
                MyOutput[0] = "0";
                MyOutput[1] = Dig2Sign + Dig1;
                return MyOutput;
            }

            if (Dig2 == "1")
            {
                MyOutput[0] = ResultSign + Dig1;
                MyOutput[1] = "0";
                return MyOutput;
            }

            if (Dig1 == "0")
            {
                MyOutput[0] = "0";
                MyOutput[1] = "0";
                return MyOutput;
            }

            if (Dig1Length < 29 && Dig2Length < 29)
            {
                decimal Dig1Dec = Convert.ToDecimal(Dig1Orig);
                decimal Dig2Dec = Convert.ToDecimal(Dig2Orig);
                decimal TempRestDec = Dig1Dec % Dig2Dec;
                decimal DecDecOutput = ((Dig1Dec - TempRestDec) / Dig2Dec);
                MyOutput[0] = Convert.ToString(DecDecOutput);
                MyOutput[1] = Convert.ToString(TempRestDec);
                return MyOutput;
            }


            List<List<long>> DigsList = CalcConvert.StringsToLongLists(Dig1, Dig2, 18);

            List<long> Dig1List = DigsList[0];
            List<long> Dig2List = DigsList[1];

            int BiggerList = CalcCompare.ListBigger(Dig1List, Dig2List);

            if (Dig1Length == Dig2Length)
            {

                if (BiggerList == 2)
                {
                    MyOutput[0] = "0";
                    MyOutput[1] = Dig1Sign + Dig1;
                    return MyOutput;
                }

                if (BiggerList == 0 && Dig1 != "0")
                {
                    MyOutput[0] = ResultSign + "1";
                    MyOutput[1] = "0";
                    return MyOutput;
                }

            }


            if (Dig1Length >= Dig2Length)
            {
                //long MyDiv = 1000000000;
                long MyDiv = 1000000000000000000;

                List<List<long>> MyResult = CalcLists.Div(Dig1List, Dig2List, MyDiv, false);

                string Multiply = CalcConvert.LongListToString(MyResult[0], "000000000000000000"); //Temporary Result(Quotient) //create strings from list after repeated subtractions
                MyOutput[0] = CalcConvert.LongListToString(MyResult[1], "000000000000000000"); //create strings from list after repeated subtractions (Temporary rest)

                //and change places of results
                MyOutput[1] = MyOutput[0]; //Rest
                MyOutput[0] = Multiply; //Quotient

                //last conditions to avoid empty lists
                if (MyOutput[0] == "") { MyOutput[0] = "0"; }
                if (MyOutput[1] == "") { MyOutput[1] = "0"; }

                //propably not nedded, but to avoid incomplete subtraction
                if (MyOutput[1] == Dig2)
                {
                    MyOutput = CalcStrings.Add(MyOutput[0], "1");
                    MyOutput[1] = "0";
                }


                //add proper sign for quotient and rest (REST NOT MODULO)
                MyOutput[0] = ResultSign + MyOutput[0];
                MyOutput[1] = Dig1Sign + MyOutput[1];

                return MyOutput;
            }

            return MyOutput;
        }

    }

    class CalcCompare
    {
        public static int StringBigger(string Dig1Orig, string Dig2Orig)
        {

            long DigsDiff = Dig1Orig.Length - Dig2Orig.Length;

            if (DigsDiff > 0)
            { return 1; }
            if (DigsDiff < 0)
            { return 2; }
            if (DigsDiff == 0)
            {
                if (Dig1Orig == Dig2Orig)
                { return 0; }

                List<List<long>> ListTest = CalcConvert.StringsToLongLists(Dig1Orig, Dig2Orig, 18);

                List<long> Dig1List = ListTest[0];
                List<long> Dig2List = ListTest[1];
                int Dig1ListCount = Dig1List.Count;
                for (int i = Dig1ListCount - 1; i >= 0; i--)
                {
                    if (Dig1List[i] > Dig2List[i])
                    {
                        return 1;
                    }
                    if (Dig1List[i] < Dig2List[i])
                    {
                        return 2;
                    }
                }

                return 0;
            }

            return 0;
        }

        public static int ListBigger(List<long> Dig1List, List<long> Dig2List)
        {
            int Dig1ListCount = Dig1List.Count;
            int Dig2ListCount = Dig2List.Count;
            long DigsDiff = Dig1ListCount - Dig2ListCount;

            if (DigsDiff > 0)
            { return 1; }
            if (DigsDiff < 0)
            { return 2; }
            if (DigsDiff == 0)
            {
                for (int i = Dig1ListCount - 1; i >= 0; i--)
                {
                    long Diff = Dig1List[i] - Dig2List[i];
                    if (Diff > 0)
                    {
                        return 1;
                    }
                    if (Diff < 0)
                    {
                        return 2;
                    }
                }
                return 0;
            }


            return 0;

        }

        public static long GetLongFromList(List<long> DigList, int DigCount, int MyDiv)
        {
            //long FirstLong = 0; // DigCount max 18
            long FullLong = 0;
            long AddLong = 0;
            int DigListCount = DigList.Count;
            int DigListFirstLength = CalcIntExt.IntLength(DigList[DigListCount - 1]); //obliczane w każdej pętli //^

            if (DigListFirstLength == DigCount)
            { return DigList[DigListCount - 1]; } //^


            if (DigListFirstLength > DigCount)
            { return (DigList[DigListCount - 1] / (long)Math.Pow(10, DigListFirstLength - DigCount)); } //^
            else
            {

                if (DigList.Count > 1)
                {
                    FullLong = (DigList[DigListCount - 1] * (long)Math.Pow(10, (MyDiv - DigListFirstLength))); //^
                    AddLong = (DigList[DigListCount - 2] / (long)Math.Pow(10, (DigListFirstLength))); //^2
                    FullLong += AddLong;
                    return (FullLong / (long)Math.Pow(10, MyDiv - DigCount));
                }
                else
                {
                    return DigList[DigList.Count - 1]; //^
                }

            }

        }

        public static decimal GetDecimalFromList(List<long> DigList)
        {
            decimal FulDec = 0;
            int DigListCount = DigList.Count;
            decimal MyDiv = 1000000000000000000;
            if (DigList.Count > 1)
            {

                FulDec = (DigList[DigListCount - 1] * MyDiv); //^1
                FulDec += DigList[DigListCount - 2]; //^2
                return FulDec;
            }
            else
            {
                return DigList[DigListCount - 1];
            }
        }
    }

    public class CalcConvert
    {

        public static long StringToLL(string DigString)
        {

            long DigLL = 0;
            int DigStringLength = DigString.Length;

            for (int i = 0; i < DigStringLength; i++) //16 900
            {
                DigLL = DigLL * 10 + (DigString[i] - '0');
            }

            return DigLL;
        }
        public static List<List<long>> StringsToLongLists(string Dig1Orig, string Dig2Orig, int FragLength)
        {
            List<List<long>> MyOutput = new List<List<long>>();
            List<long> Dig1List = new List<long>();
            List<long> Dig2List = new List<long>();
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




            if (CommonLength > FragLength) //cut number string into n-digit int array (from right to left)
            {
                int ModLength1 = CommonLength % FragLength;
                int PartCount1 = CommonLength / FragLength;

                int ModLength2 = Dig2Length % FragLength;
                int PartCount2 = Dig2Length / FragLength;

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
                    Dig1List.Add(Convert.ToInt64(Dig1.Substring(0, ModLength1))); //get n-digit string to list
                }
                if (ModLength2 > 0)
                {
                    Dig2List.Add(Convert.ToInt64(Dig2.Substring(0, ModLength2)));
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

            return MyOutput;
        }




        public static List<long> StringToLongList(string Dig1Orig, int FragLength)
        {
            List<long> Dig1List = new List<long>();

            int CommonLength = Dig1Orig.Length;

            if (CommonLength > FragLength) //cut number string into n-digit int array (from right to left)
            {
                int ModLength = CommonLength % FragLength;
                int PartCount = CommonLength / FragLength;

                for (int i = PartCount - 1; i >= 0; i--)
                {
                    int start = ModLength + (i * FragLength);
                    Dig1List.Add(Convert.ToInt64(Dig1Orig.Substring(start, FragLength))); //get n-digit string to list
                }

                if (ModLength > 0)
                {
                    Dig1List.Add(Convert.ToInt64(Dig1Orig.Substring(0, ModLength))); //get n-digit string to list
                }
            }
            else
            {
                Dig1List.Add(Convert.ToInt64(Dig1Orig)); //if number is smaller then n digit
            }

            return Dig1List;
        }

        public static string LongListToString(List<long> DigList, string StringPlaces)
        {

            string MyOutput;
            List<string> MyOutputList = new List<string>();

            int loops = DigList.Count - 1;

            for (int i = loops; i >= 0; i--)
            {


                if (i == loops) //(i == DigList.Count - 1)
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
            return MyOutput;
        }

        public static List<long> ConvertLists(List<long> DigList, long FromMyDiv, long ToMyDiv)
        {
            List<long> MyResult = new List<long>();
            int Loops = DigList.Count - 1;
            if (FromMyDiv > ToMyDiv)
            {
                for (int i = 0; i <= Loops; i++)
                {
                    MyResult.Add(DigList[i] % ToMyDiv);
                    long NextDig = DigList[i] / ToMyDiv;

                    if (NextDig > 0 && i <= Loops)
                    { MyResult.Add(NextDig); }

                }


                if (MyResult.Count == 1)
                { return MyResult; }

                return MyResult;
            }
            else if (FromMyDiv < ToMyDiv)
            {
                for (int i = 0; i <= Loops; i++)
                {
                    MyResult.Add(DigList[i]);

                    if (i < Loops && DigList[i + 1] > 0)
                    { MyResult[MyResult.Count - 1] += DigList[i + 1] * FromMyDiv; }
                    i += 1;
                }

                if (MyResult.Count == 1)
                { return MyResult; }

                return MyResult;
            }

            return DigList;
        }

        public static List<long> CleanListt(List<long> DigList)
        {


            if (DigList.Count == 1)
            {
                return DigList;
            }


            while (DigList[DigList.Count - 1] == 0)
            {
                DigList.RemoveAt(DigList.Count - 1);
                if (DigList.Count == 1)
                { break; }
            }

            return DigList;
        }

    }
}
