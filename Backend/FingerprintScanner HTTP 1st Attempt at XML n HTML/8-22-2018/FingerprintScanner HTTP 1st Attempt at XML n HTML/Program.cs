using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.Net.Http;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FingerPrintScannerBackend
{
    class Program
    {
        static public SerialPort arduino = null;
        static public FingerRelay fingerRelay = null;
        static public TcpListener TcpListener;

        static public List<FingerAccount> fingerAccounts = new List<FingerAccount>();
        
        static public string[] mainMenuItems = new string[] { "View Keys", "Add Key", "Log In", "Log Out", "Use existing finger", "Get all fingerRelay data", "Delete All Keys", "Save Database", "Delete users"};

        static public string HostName = null;//static public string HostName = "iansweb.org";
        static public ushort Port = 8080;

        static void Main(string[] args)
        {
            while (arduino == null)
            {
                string port = Log.QueryMultiChoice("Port name?", SerialPort.GetPortNames().ToList());
                ushort baud = Convert.ToUInt16(Log.QueryMultiChoice("Baud rate?", new List<string>() { "9600", "57600" }));
                arduino = new SerialPort(port, baud);
                try
                {
                    arduino.Open();
                    Task.Run(() => ListenPort(arduino));
                }
                catch (Exception e)
                {
                    Log.Error("Could not open " + port, e);
                    arduino = null;
                }
            }

            while (HostName == null)
            {
                HostName = Log.Query("Hostname?", "iansweb.org");
                if (!(HostName.Length > 1))
                {
                    Console.WriteLine("Invalid Address");
                    HostName = null;
                }
            }

            Console.WriteLine($"COM Port: {arduino.PortName}");
            Console.WriteLine($"Baud Rate: {arduino.BaudRate}");
            Console.WriteLine($"HostName: {HostName}");
            Console.WriteLine($"Port: {Port}");
            Task.Run(StartWebServer);
            
            fingerRelay = new FingerRelay(arduino);
            fingerAccounts = parseJson();
            Task.Run(AutoSaveDatabase);
            Log.Info("Autosaving databases every " + new TimeSpan(0, 0, 600000).Minutes + " minutes");
            while (true)
            {
                string choice = Log.QueryMultiChoice("Main Menu", mainMenuItems.ToList());
                if (choice == mainMenuItems[0])
                {
                    //View keys
                    foreach (var fingeraccount in fingerAccounts)
                    {
                        Console.WriteLine($"{fingeraccount.Name}, {fingeraccount.loggedIn}, {fingeraccount.templateId}");
                    }
                    Console.ReadKey(true);
                }
                else if (choice == mainMenuItems[1])
                {
                    //Add key
                    int id = 0;
                    
                    while (id == 0)
                    {
                        if (Log.QueryBool("Create new scan?"))
                        {
                            id = FingerWizard.CreateAScan();
                        }
                        else
                        {
                            try
                            {
                                id = Convert.ToInt16(Log.Query("Template Id", (fingerRelay.TemplateCount + 1).ToString()));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Invalid Integer");
                            }
                        }
                    }
                    var newuser = new FingerAccount(Log.Query("User's Name", ("user" + (fingerRelay.TemplateCount + 1).ToString())), id);                   
                    fingerAccounts.Add(newuser);
                } else if (choice == mainMenuItems[2])
                {
                    //Log In
                    var users = fingerAccounts.Where(n => !(n.loggedIn));
                    if (users.Count() > 0)
                    {
                        string user = Log.QueryMultiChoice("User to log in?", users.Select(n => n.Name).ToList());
                        var matches = fingerAccounts.Where(n => n.Name == user && !(n.loggedIn));
                        if (matches.Count() > 0)
                        {
                            Console.WriteLine($"Logged in the first account that has the name of {matches.First().Name}");
                            matches.First().LogIn();
                        }
                        else
                        {
                            Console.WriteLine("No matches found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No users are logged out");
                    }
                } else if (choice == mainMenuItems[3])
                {
                    //Log Out
                    var users = fingerAccounts.Where(n => n.loggedIn);
                    if (users.Count() > 0)
                    {
                        string user = Log.QueryMultiChoice("User to log out?", users.Select(n => n.Name).ToList());
                        var matches = fingerAccounts.Where(n => n.Name == user && n.loggedIn);
                        if (matches.Count() > 0)
                        {
                            Console.WriteLine($"Logged out the first account that has the name of {matches.First().Name}");
                            matches.First().LogOut();
                        }
                        else
                        {
                            Console.WriteLine("No matches found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No users are logged in");
                    }
                } else if (choice == mainMenuItems[4])
                {
                    if (fingerAccounts.Count > 0)
                    {
                        //DirectoryInfo directoryInfo = new DirectoryInfo(".");
                        //System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(new System.Drawing.Bitmap(Log.QueryMultiChoice("Source bitmap?", directoryInfo.EnumerateFiles().Where(n => n.Name.EndsWith(".bmp")).Select(n => n.Name).ToList())));
                        //var finger = FingerOperations.GetFingerAccount(bitmap, fingerAccounts);
                        //Console.WriteLine($"{finger.Name}, {finger.loggedIn}");
                        var scan = FingerWizard.Scan();
                        if (scan < 0)
                        {
                            Log.Info("Scan failed or timed out");
                        }
                        else
                        {
                            if (fingerAccounts.Any(n => n.templateId == scan))
                            {
                                var acc = fingerAccounts.Where(n => n.templateId == scan).First();
                                Log.Success("Scan found " + acc.Name + ", template id " + scan);
                                acc.ToggleLogINOUT();
                                if (acc.loggedIn)
                                {
                                    Log.Info("Logged in");
                                }
                                else
                                {
                                    Log.Info("Logged out");
                                }
                            }
                            else
                            {
                                Log.Info("No accounts found for this scan");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No fingers for comparison!");
                    }
                } else if (choice == mainMenuItems[5])
                {
                    if (fingerRelay != null)
                    {
                        Console.WriteLine($"{fingerRelay.TemplateCount} Templates");
                    }
                    else
                    {
                        Console.WriteLine("FingerRelay non-existant");
                    }
                } else if (choice == mainMenuItems[6])
                {
                    var dele = Log.QueryMultiChoice("Clear what?", new List<string>() { "Users", "Templates", "Both", "Go back" });
                    switch (dele)
                    {
                        case "Users":
                            fingerAccounts = new List<FingerAccount>();
                            break;
                        case "Templates":
                            fingerRelay.SendCommandGetData(FingerRelay.Commands.CLEAR_DATABASE);
                            break;
                        case "Both":
                            fingerAccounts = new List<FingerAccount>();
                            fingerRelay.SendCommandGetData(FingerRelay.Commands.CLEAR_DATABASE);
                            break;
                        case "Go back":
                            break;
                    }
                } else if (choice == mainMenuItems[7])
                {
                    SaveJson().GetAwaiter();
                } else if (choice == mainMenuItems[8])
                {
                    FingerWizard.DeleteWizard();
                }
            }
        }

        public static async Task ListenPort(SerialPort arduino)
        {
            try
            {
            }
            catch (Exception e)
            {
                Log.Error("Port error " + e);
            }
        }

        static public void UpdateXML()
        {

        }

        public static async Task StartWebServer()
        {
            try
            {
                TcpListener = new TcpListener(new IPEndPoint(Dns.GetHostAddresses(HostName)[0], Port));
                TcpListener.Start();
                await Task.Run(() => Listener(TcpListener));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Listener could not start: \n{e.Message}");
            }
        }

        public static async Task Listener(TcpListener socket)
        {
            Log.Success("Internal HTTP server listening for clients");
            while (true)
            {                
                try
                {
                    var client = socket.AcceptTcpClient();
                    //Console.WriteLine($"New client {client.Client.RemoteEndPoint.ToString()}");
                    await Task.Run(() => SocketHandler(client));
                } catch(Exception e)
                {
                    Log.Error("Client caused exception", e);
                }
            }
        }

        public static async Task SocketHandler(TcpClient client)
        {
            NetworkStream stream;
            //client.Disconnect(false);
            try
            {
                stream = client.GetStream();
                await SendHTML(stream);
                await stream.FlushAsync();
                client.Close();
            }     catch(Exception e)
            {
                Console.WriteLine($"A socket was closed: {client.Client.RemoteEndPoint.ToString()}");
            }     
        }

        public static async Task SendHTML(NetworkStream client)
        {
            using (StreamWriter sw = new StreamWriter(client))
            {

                int i;
                byte[] bytes = new byte[65565];
                string data;
                string puredata;
                while ((i = client.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    puredata = data;
                    data = data.ToUpper();
                    if (data.Split(' ').Any("GET".Equals))
                    {
                        if (data.Split(' ').ToList().IndexOf("GET") < data.Split(' ').Length + 1)
                        {
                            string request = data.Split(' ')[data.Split(' ').ToList().IndexOf("GET") + 1];
                            switch (request)
                            {
                                case "/":
                                    byte[] payLoad = Encoding.UTF8.GetBytes(File.ReadAllText("index.html"));
                                    byte[] header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 200 OK\r\ncontent-type: text/html; charset=utf-8\r\ncontent-length: " + payLoad.Length + "\r\n\r\n"));
                                    await sw.WriteAsync(Encoding.UTF8.GetChars(header));
                                    await sw.WriteAsync(Encoding.UTF8.GetChars(payLoad));
                                    await sw.FlushAsync();
                                    break;
                                case "/DATABASE.XML":
                                    XmlDocument xml = new XmlDocument();
                                    XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
                                    XmlElement root = xml.DocumentElement;
                                    xml.InsertBefore(xmlDeclaration, root);
                                    XmlElement items = xml.CreateElement(string.Empty, "fingers", string.Empty);
                                    xml.AppendChild(items);
                                    foreach (var finger in fingerAccounts)
                                    {
                                        XmlElement fingernode = xml.CreateElement(string.Empty, "finger", string.Empty);
                                        fingernode.AppendChild(xml.CreateElement(string.Empty, "Name", string.Empty)).InnerText = finger.Name;
                                        fingernode.AppendChild(xml.CreateElement(string.Empty, "LogState", string.Empty)).InnerText = finger.loggedIn.ToString();
                                        if (finger.loggedIn)
                                        {
                                            fingernode.AppendChild(xml.CreateElement(string.Empty, "CurrentTime", string.Empty)).InnerText = Extensions.ToReadableString(finger.CurrentTime);
                                        }
                                        else
                                        {
                                            fingernode.AppendChild(xml.CreateElement(string.Empty, "CurrentTime", string.Empty)).InnerText = "n/a";
                                        }
                                        fingernode.AppendChild(xml.CreateElement(string.Empty, "TotalTime", string.Empty)).InnerText = Extensions.ToReadableString(finger.TotalTime);

                                        items.AppendChild(fingernode);
                                    }
                                    MemoryStream memstr = new MemoryStream();
                                    xml.Save(memstr);
                                    byte[] __payLoad = Encoding.UTF8.GetBytes(Encoding.UTF8.GetChars(memstr.ToArray()));
                                    byte[] __header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 200 OK\r\ncontent-type: text/xml; charset=utf-8\r\ncontent-length: " + __payLoad.Length + "\r\n\r\n"));
                                    await sw.WriteAsync(Encoding.UTF8.GetChars(__header));
                                    await sw.WriteAsync(Encoding.UTF8.GetChars(__payLoad));
                                    await sw.FlushAsync();
                                    break;                                   
                                default:
                                    if (File.Exists(request.TrimStart('/')))
                                    {
                                        byte[] _payLoad = Encoding.UTF8.GetBytes(File.ReadAllText(request.TrimStart('/')));
                                        byte[] _header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 200 OK\r\ncontent-type: text/html; charset=utf-8\r\ncontent-length: " + _payLoad.Length + "\r\n\r\n"));
                                        await sw.WriteAsync(Encoding.UTF8.GetChars(_header));
                                        await sw.WriteAsync(Encoding.UTF8.GetChars(_payLoad));
                                        await sw.FlushAsync();
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (data.Split(' ').Any("POST".Equals))
                    {
                        if (data.Split(' ').ToList().IndexOf("POST") < data.Split(' ').Length + 1)
                        {
                            string postrequest = data.Split(' ')[data.Split(' ').ToList().IndexOf("POST") + 1];
                            switch (postrequest)
                            {
                                case "/NEWUSER":
                                    var usr = AccountFromHTTP(puredata);
                                    byte[] _payLoad = new byte[0];
                                    byte[] _header = new byte[0];

                                    if (usr.Name == null || usr.Name.Length < 1)
                                    {
                                        //Log.Warning("POST request rejected, invalid data");
                                        //await sw.WriteAsync("HTTP/2.0 400 Bad Request");
                                        _payLoad = Encoding.UTF8.GetBytes("<html><head /><body><p>Invalid form data</p></body></html>");
                                        _header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 400 Bad Request\r\ncontent-type: text/html; charset=utf-8\r\ncontent-length: " + _payLoad.Length + "\r\n\r\n"));

                                    }
                                    else
                                    {
                                        Log.Success("New user added with the web client");
                                        //await sw.WriteAsync("HTTP/2.0 200 OK");
                                        _payLoad = Encoding.UTF8.GetBytes(File.ReadAllText("newusersuccess.html"));
                                        _header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 200 OK\r\ncontent-type: text/html; charset=utf-8\r\ncontent-length: " + _payLoad.Length + "\r\n\r\n"));
                                    }
                                    await sw.WriteAsync(Encoding.UTF8.GetChars(_header));
                                    await sw.WriteAsync(Encoding.UTF8.GetChars(_payLoad));
                                    await sw.FlushAsync();
                                    fingerAccounts.Add(usr);                                    
                                    break;
                            }
                        }
                    }
                }
                sw.Close();
            }
        }

        public static FingerAccount AccountFromHTTP(string request)
        {
            var payload = request.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None ).Last();
            var data = payload.Split('&').Select(n => n.Trim('\n', '\r'));
            int id = 0;
            string name = "";
            bool scan = false;
            foreach (var ssssssssss in data)
            {
                switch (ssssssssss.Split('=')[0].ToLower())
                {
                    case "a":
                        var val = ssssssssss.Split('=').Last();
                        if (val.ToLower() == "on")
                        {
                            scan = true;
                        }
                        else
                        {
                            scan = false;
                        }
                        break;
                    case "b":
                        id = int.Parse(ssssssssss.Split('=').Last());
                        break;
                    case "c":
                        name = ssssssssss.Split('=').Last();
                        break;
                }
            }
            if (scan && name.Length > 0)
            {                
                return new FingerAccount(name, FingerWizard.CreateAScan(id));
            }
            else
            {
                return new FingerAccount(name, id);
            }
        }


        public static async Task HandleGET(Socket client)
        {

        }

        public static async Task HandlePOST(Socket client)
        {

        }

        public static async Task AutoSaveDatabase()
        {
            while (true)
            {
                await Task.Delay(600000);
                await SaveJson("database.json");
            }
        }

        public static async Task SaveJson(string filename = null)
        {
            JObject joson = new JObject();
            JArray jArray = new JArray();
            foreach (var fu_ck in fingerAccounts)
            {
                JObject jObject = new JObject();
                jObject.Add("name", fu_ck.Name);
                jObject.Add("templateId", fu_ck.templateId);
                jObject.Add("totalTime", fu_ck.TotalTime.Ticks);
                jObject.Add("creationDate", fu_ck.creationDate);
                jArray.Add(jObject);
            }
            joson.Add("users", jArray);
            if (filename != null)
            {
                File.WriteAllText(filename, JsonConvert.SerializeObject(joson));
            }
            else
            {
                File.WriteAllText(Log.Query("Database file name?", "database.json"), JsonConvert.SerializeObject(joson));
            }
            Log.Success("Database saved!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        public static List<FingerAccount> parseJson()
        {
                DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
                var fil = Log.QueryMultiChoice("Json database file?", new List<string>(directoryInfo.EnumerateFiles().Select(n => n.Name)) { "None" });
                if (fil == "None") { return fingerAccounts; }
            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(File.ReadAllText(fil));
            }
            catch (JsonReaderException)
            {
                Log.Warning("Invalid Json file");
            }
            catch (FileNotFoundException)
            {
                Log.Warning("File no longer exists");
            }
            catch (Exception e)
            {
                Log.Error("Parsing", e);
            }
            if (jObject == null) { return fingerAccounts; }
            try
            {
                var ffff = new List<FingerAccount>();
                foreach (JObject val in (JArray)jObject["users"])
                {
                    var usr = new FingerAccount((string)val["name"], (int)val["templateId"]);
                    usr._totalTime = new TimeSpan((long)val["totalTime"]);
                    usr.creationDate = (DateTime)val["creationDate"];
                    ffff.Add(usr);
                }
                var sele = Log.QueryMultiChoice("Database Merge Type", new List<string>() { "Preserve duplicates", "Renew database", "Append existing" });
                if (sele == "Preserve duplicates")
                {
                    ffff.AddRange(fingerAccounts);
                    return ffff;
                }
                else if (sele == "Renew database")
                {
                    return ffff;
                }
                else if (sele == "Append existing")
                {
                    ffff.RemoveAll(n => fingerAccounts.Any(m => m.templateId.Equals(n.templateId)));
                    fingerAccounts.AddRange(ffff);
                    return fingerAccounts;
                }

            }
            catch (Exception)
            {
                Log.Error("Could not parse Json");
            }
            return new List<FingerAccount>();
        }
    }

    static class Extensions
    {
        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? String.Empty : "s") : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

            return formatted;
        }
    }
    public static class StreamHelpers
    {
        public static byte[] ReadFully(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
