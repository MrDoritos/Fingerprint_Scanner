using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FingerPrintScannerBackend
{
    class FingerWizard
    {
        private static List<string> options = new List<string>() {"Delete all matches", "Delete multiple", "Delete first", "Don't delete accounts", "Don't delete accounts, set id to 0" };

        public static void DeleteWizard()
        {
            bool deletefromarduino = false;
            deletefromarduino = Log.QueryBool("Delete the key from the arduino?");
            if (Log.QueryBool("Delete by user (y) or Id (n)"))
            {
                var user = Log.QueryMultiChoice("User to delete?", Program.fingerAccounts.Select(n => n.Name).ToList());
                var tempid = Program.fingerAccounts.Where(n => n.Name == user).First().templateId;
                if (deletefromarduino && INTERFACEDelete(tempid) == 1)
                {
                    if (Log.QueryBool("An error occured, delete the users with matching Id?"))
                    {

                        var to = deletes(tempid);
                        if (to.Count < 1)
                        {
                            Log.Info("The users have been spared");
                        }
                        else
                        {
                            Log.Info("Deleting...");
                            foreach (var usr in to)
                            {
                                Console.WriteLine(usr.Name);
                            }
                            Program.fingerAccounts.RemoveAll(n => to.Any(n.Equals));
                        }
                    }
                    else
                    {
                        Log.Info("The users have been spared");
                    }
                }
                else
                {
                    var to = deletes(tempid);
                    if (to.Count < 1)
                    {
                        Log.Info("The users have been spared");
                    }
                    else
                    {
                        Log.Info("Deleting...");
                        foreach (var usr in to)
                        {
                            Console.WriteLine(usr.Name);
                        }
                        Program.fingerAccounts.RemoveAll(n => to.Any(n.Equals));
                    }

                }
            }
            else
            {
                int delete = Log.QueryInt($"Template Number?", 1, 127);
                if (deletefromarduino && INTERFACEDelete(delete) == 1)
                {
                    if (Log.QueryBool("An error occured, delete the users with matching Id?"))
                    {
                        var to = deletes(delete);
                        if (to.Count < 1)
                        {
                            Log.Info("The users have been spared");
                        }
                        else
                        {
                            Log.Info("Deleting...");
                            foreach (var usr in to)
                            {
                                Console.WriteLine(usr.Name);
                            }
                            Program.fingerAccounts.RemoveAll(n => to.Any(n.Equals));
                        }
                    }
                    else
                    {
                        Log.Info("The users have been spared");
                    }
                }
                else
                {
                    var to = deletes(delete);
                    if (to.Count < 1)
                    {
                        Log.Info("The users have been spared");
                    }
                    else
                    {
                        Log.Info("Deleting...");
                        foreach (var usr in to)
                        {
                            Console.WriteLine(usr.Name);
                        }
                        Program.fingerAccounts.RemoveAll(n => to.Any(n.Equals));
                    }
                }
            }
        }

        private static List<FingerAccount> deletes(int id)
        {
            var ans = (Log.QueryMultiChoice("Type of delete", options));
            if (ans == options[0])
            {
                return Program.fingerAccounts.Where(n => n.templateId == id).ToList();
            }
            else if (ans == options[1])
            {
                List<FingerAccount> fff = new List<FingerAccount>();            
                while (true)
                {
                    var op = Log.QueryMultiChoice("Users to delete", new List<string>(Program.fingerAccounts.Where(n => n.templateId == id && !(fff.Any(n.Equals))).Select(n => n.Name)) { "Done"});
                    if (op == "Done")
                    {
                        return fff;
                    }
                    fff.Add(Program.fingerAccounts.Where(n => n.Name == op && n.templateId == id).First());
                }
            } else if (ans == options[2])
            {
                return new List<FingerAccount>() { Program.fingerAccounts.Where(n => n.templateId == id).First() };
            } else if (ans == options[3])
            {
                return new List<FingerAccount>();
            } else if (ans == options[4])
            {
                Program.fingerAccounts.Where(n => n.templateId == id).Select(n => n.templateId = 0);
                return new List<FingerAccount>();
            }
            return new List<FingerAccount>();
        }

        public static int INTERFACEDelete(int templatenum = 0)
        {
            var finray = Program.fingerRelay;
            if (templatenum == 0)
            {
                templatenum = Log.QueryInt($"Template Number, Current Templates {Program.fingerRelay.TemplateCount}", 1, 127);
            }
            string init = finray.SendCommandGetData(FingerRelay.Commands.DELETE_USER);
            finray.serialPort.DiscardInBuffer();
            finray.SendByte((byte)templatenum);
            Console.WriteLine(finray.RecieveData());
            switch (finray.RecieveData())
            {
                case "OK":                    
                    return 0;
                case "PACKREC": case "ERROR":
                    Log.Warning("Arduino errored");
                    return 1;
                case "FLASHERR":
                    Log.Warning("Flash error");
                    return 1;
                case "BADLOC":
                    Log.Warning("Bad location");
                    return 1;
            }
            finray.serialPort.DiscardInBuffer();
            return 1;
        }

        public static int CreateAScan(int templatenum = 0)
        {
            var fingerrelay = Program.fingerRelay;
            if (templatenum == 0)
            {
                templatenum = Log.QueryInt($"Template Number, Current Templates {Program.fingerRelay.TemplateCount}", 1, 127);
            }
            string init = fingerrelay.SendCommandGetData(FingerRelay.Commands.ENROLL_FINGER);
            fingerrelay.serialPort.DiscardInBuffer();
            fingerrelay.SendByte((byte)templatenum);
            Console.WriteLine(fingerrelay.RecieveData());
            Log.Info("Waiting for finger...");
            bool boolBreak = false;
            while (!boolBreak)
            {
                init = fingerrelay.RecieveData();
                switch (init)
                {
                    case "SCANNER_OK":
                        boolBreak = true;
                        break;
                    case "SCANNER_NOFINGER":
                        continue;
                    case "SCANNER_IMAGEFAIL":
                        continue;
                    case "SCANNER_ERROR":
                        continue;
                }
            }
            Log.Success("Found Finger");
            boolBreak = false;
            while (!boolBreak)
            {
                init = fingerrelay.RecieveData();
                switch (init)
                {
                    case "SCANNER_OK":
                        boolBreak = true;
                        break;
                    case "SCANNER_IMAGEMESS":
                        Log.Info("Imaged finger was too messy, try again");
                        return 0;
                    case "SCANNER_IMAGEFAIL":
                        Log.Info("Could not find fingerprint features, try again");
                        return 0;
                    case "SCANNER_ERROR":
                        Log.Info("Scanner errored.");
                        return 0;
                }
            }
            Log.Success("Image converted successfully");
            Log.Info("Now remove finger");
            Task.Delay(3000).GetAwaiter();
            int id = 0;
            try
            {
                id = Convert.ToInt16(fingerrelay.RecieveData().Split('_').Last());
                Console.WriteLine($"Template Id: {id}");
            }
            catch (Exception)
            {
                Log.Error("Failed. Try again");
            }

            Log.Info("Place same finger again");
            boolBreak = false;
            while (!boolBreak)
            {
                init = fingerrelay.RecieveData();
                switch (init)
                {
                    case "SCANNER_OK":
                        boolBreak = true;
                        break;
                    case "SCANNER_NOFINGER":
                        continue;
                    case "SCANNER_IMAGEFAIL":
                        continue;
                    case "SCANNER_ERROR":
                        continue;
                }
            }

            boolBreak = false;
            while (!boolBreak)
            {
                init = fingerrelay.RecieveData();
                switch (init)
                {
                    case "SCANNER_OK":
                        boolBreak = true;
                        break;
                    case "SCANNER_IMAGEMESS":
                        Log.Info("Imaged finger was too messy, try again");
                        return 0;
                    case "SCANNER_IMAGEFAIL":
                        Log.Info("Could not find fingerprint features, try again");
                        return 0;
                    case "SCANNER_ERROR":
                        Log.Info("Scanner errored.");
                        return 0;
                }
            }

            boolBreak = false;
            while (!boolBreak)
            {
                init = fingerrelay.RecieveData();
                switch (init)
                {
                    case "MATCH":
                        boolBreak = true;
                        Log.Success("Fingers matched!");
                        break;
                    case "ERROR":
                        Log.Info("Scanner errored.");
                        return 0;
                    case "NOMATCH":
                        Log.Info("Fingers did not match.");
                        return 0;
                }
            }

            boolBreak = false;
            while (!boolBreak)
            {
                init = fingerrelay.RecieveData();
                switch (init)
                {
                    case "OK":
                        Log.Success("Finger stored");
                        return id;
                    case "ERROR":
                        Log.Info("Scanner errored.");
                        return 0;
                    case "BADLOC":
                        Log.Info("Bad scan location, try again");
                        return 0;
                    case "FLASHERR":
                        Log.Info("Error writing to flash, try again");
                        return 0;
                }
            }
            return 0;
        }

        public static int Scan(bool message = true, bool timeout = true)
        {
            if (message)
            Log.Info("Place thumb on scanner");
            Stopwatch stopwatch = new Stopwatch();
            if (timeout)
            stopwatch.Start();
            var finrelay = Program.fingerRelay;
            string str = finrelay.SendCommandGetData(FingerRelay.Commands.GET_FINGERPRINT).Trim('\n', '\r');
            while (str == "END")
            {
                if (timeout)
                if (stopwatch.ElapsedMilliseconds > 10000)
                    return -1;
                str = finrelay.SendCommandGetData(FingerRelay.Commands.GET_FINGERPRINT).Trim('\n', '\r');
            }
            try
            {
                return Convert.ToInt16(str.Split('_').Last());
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
