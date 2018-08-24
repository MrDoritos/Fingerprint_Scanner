using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FingerPrintScannerBackend
{
    class Log
    {
        private static ConsoleColor errorColor = ConsoleColor.Red;
        private static ConsoleColor infoColor = ConsoleColor.Blue;
        private static ConsoleColor warningColor = ConsoleColor.Yellow;
        private static ConsoleColor successColor = ConsoleColor.Green;
        private static ConsoleColor queryColor = ConsoleColor.Cyan;

        /// <summary>
        /// Print info
        /// </summary>
        /// <param name="info">The info</param>
        public static void Info(string info)
        {
            Console.ForegroundColor = infoColor;
            Console.Write("[INFO]");
            Console.ResetColor();
            Console.WriteLine(" " + info);
        }
        /// <summary>
        /// Print a warning
        /// </summary>
        /// <param name="warning">The warning</param>
        public static void Warning(string warning)
        {
            Console.ForegroundColor = warningColor;
            Console.Write("[WARNING]");
            Console.ResetColor();
            Console.WriteLine(" " + warning);
        }
        /// <summary>
        /// Print a success
        /// </summary>
        /// <param name="success">The success</param>
        public static void Success(string success)
        {
            Console.ForegroundColor = successColor;
            Console.Write("[OK]");
            Console.ResetColor();
            Console.WriteLine(" " + success);
        }
        /// <summary>
        /// Ask the user a question with a string answer
        /// </summary>
        /// <param name="query">The question</param>
        /// <returns>The answer</returns>
        public static string Query(string query)
        {
            Console.ForegroundColor = queryColor;
            Console.Write("[QUERY]");
            Console.ResetColor();
            Console.WriteLine(" " + query);
            return Console.ReadLine();
        }
        /// <summary>
        /// Ask the user a question with an optional answer
        /// </summary>
        /// <param name="query">Question</param>
        /// <param name="default_">The default answer</param>
        /// <returns>The answer</returns>
        public static string Query(string query, string default_)
        {
            Console.ForegroundColor = queryColor;
            Console.Write("[QUERY]");
            Console.ResetColor();
            Console.WriteLine(" " + query + " (optional, defaults to \"" + default_ + "\")");
            string input = Console.ReadLine();
            if (input.Length > 0)
            {
                return input;
            }
            else
            {
                return default_;
            }
        }
        /// <summary>
        /// Querys a list of tags from the user
        /// </summary>
        /// <param name="query">The question</param>
        /// <returns>The tags</returns>
        public static List<string> QueryTags(string query)
        {
            Console.ForegroundColor = queryColor;
            Console.Write("[QUERY]");
            Console.ResetColor();
            Console.WriteLine(" " + query);
            Console.WriteLine("Press escape to stop adding tags / Enter to add a tag / Delete to remove previous tag");
            List<string> tags = new List<string>();
            string currentbuffer = "";
            while (true)
            {
                List(tags, 100);
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        return tags;
                    case ConsoleKey.Enter:
                        tags.Add(currentbuffer);
                        currentbuffer = "";
                        var lastcurs = Console.CursorTop;
                        Console.SetCursorPosition(0, Console.WindowHeight);
                        Console.Write("                                 ");
                        Console.SetCursorPosition(0, lastcurs);
                        continue;
                    case ConsoleKey.Delete:
                        tags.Remove(tags.Last());
                        continue;
                    case ConsoleKey.Backspace:
                        if (currentbuffer.Length > 0)
                        {
                            currentbuffer = currentbuffer.Substring(0, currentbuffer.Length - 1);
                            var lastdcur = Console.CursorTop;
                            Console.SetCursorPosition(0, Console.WindowHeight);
                            Console.Write(currentbuffer + " ");
                            Console.SetCursorPosition(0, lastdcur);
                        }
                        continue;
                    default:
                        try
                        {
                            char ch = key.KeyChar;
                            currentbuffer += ch;
                            var lastcur = Console.CursorTop;
                            Console.SetCursorPosition(0, Console.WindowHeight);
                            Console.Write(currentbuffer);
                            Console.SetCursorPosition(0, lastcur);
                        }
                        catch (Exception) { }
                        continue;
                }
            }
        }
        
        public static int QueryInt(string query, int min, int max)
        {
            Console.ForegroundColor = queryColor;
            Console.Write("[QUERY]");
            Console.ResetColor();
            Console.WriteLine($" {query} [int ({min}-{max})]");
            while (true)
            {
                try
                {
                    int num = Convert.ToInt16(Console.ReadLine());
                    if (num >= min && num <= max)
                    {
                        return num;
                    }
                    else
                    {
                        Info("Out of bounds of the query");
                    }
                }
                catch (Exception)
                {
                    Info("Invalid Integer");
                }
            }
        }

        public static int QueryInt(string query)
        {
            Console.ForegroundColor = queryColor;
            Console.Write("[QUERY]");
            Console.ResetColor();
            Console.WriteLine(" " + query + " [int]");
            while (true)
            {
                try
                {
                    return Convert.ToInt16(Console.ReadLine());
                }
                catch (Exception)
                {
                    Info("Invalid Integer");
                }
            }
        }


        /// <summary>
        /// Ask the user a boolean question
        /// </summary>
        /// <param name="query">The question</param>
        /// <returns>The answer</returns>
        public static bool QueryBool(string query)
        {
            Console.ForegroundColor = queryColor;
            Console.Write("[QUERY]");
            Console.ResetColor();
            Console.WriteLine(" " + query + " [y/n]");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (key.Key == ConsoleKey.N)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Ask the user a multiple choice question
        /// </summary>
        /// <param name="query">The question</param>
        /// <param name="choices">The choices</param>
        /// <returns></returns>
        public static string QueryMultiChoice(string query, List<string> choices)
        {
            //Console.Clear();
            Console.ForegroundColor = queryColor;
            Console.Write("[QUERY]");
            Console.ResetColor();
            Console.WriteLine(" " + query);
            int selected = 0;
            List(choices, 0);
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selected > 0)
                        {
                            selected--;
                        }
                        else
                        {
                            selected = choices.Count - 1;
                        }
                        List(choices, selected);
                        continue;
                    case ConsoleKey.DownArrow:
                        if (selected < choices.Count - 1)
                        {
                            selected++;
                        }
                        else
                        {
                            selected = 0;
                        }
                        List(choices, selected);
                        continue;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.ForegroundColor = queryColor;
                        Console.Write("[QUERY]");
                        Console.ResetColor();
                        Console.WriteLine(" " + query);
                        List(choices, selected);
                        Console.SetCursorPosition(0, Console.CursorTop + choices.Count);
                        //for (int i = 0; i < choices.Count; i++)
                        //    Console.WriteLine();
                        Console.WriteLine("You selected: " + choices[selected]);
                        return choices[selected];
                }
            }
        }
        /// <summary>
        /// Print an error to the console
        /// </summary>
        /// <param name="error">The optional message</param>
        /// <param name="e">The optional exception</param>
        /// <param name="showStackTrace">Show the StackTrace</param>
        public static void Error(string error = null, Exception e = null, bool showStackTrace = false)
        {
            if (error != null || e != null)
            {
                Console.ForegroundColor = errorColor;
                Console.Write("[ERROR]");
                Console.ResetColor();
                if (error == null && e != null)
                {
                    Console.WriteLine(e.Message);
                    if (showStackTrace)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
                else if (error != null && e == null)
                {
                    Console.WriteLine(error);
                }
                else if (error != null && e != null)
                {
                    Console.WriteLine(error);
                    Console.WriteLine(e.Message);
                    if (showStackTrace)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
        }
        /// <summary>
        /// Shamelessly list a string list
        /// </summary>
        /// <param name="base_">The string list</param>
        /// <param name="selected">Any fucking number</param>
        private static void List(List<string> base_, int selected)
        {           
            Console.ResetColor();
            var lastcursor = Console.CursorTop;
            var currentcursor = lastcursor;
            //Console.Clear();
            for (int i = 0; i < base_.Count; i++)
            {
                //Console.WriteLine();
                if (selected == i)
                {
                    Console.ForegroundColor = queryColor;
                    Console.WriteLine(base_[i]);
                    //Console.SetCursorPosition(0, currentcursor);
                    currentcursor++;
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(base_[i]);
                    //Console.SetCursorPosition(0, currentcursor);
                    currentcursor++;
                }
            }
            Console.ResetColor();
            Console.SetCursorPosition(0, lastcursor);
        }
    }
}
