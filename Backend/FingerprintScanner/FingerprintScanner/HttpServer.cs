using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.IO;
using System.Xml;

namespace FingerprintScanner
{
    class HttpServer
    {
        static public TcpListener TcpListener = null;
        //static public string HostName = null;
        static public string HostName = "localhost";
        static public ushort Port = 8080;
        static public Accounts accounts;

        public static async Task<bool> StartWebServer()
        {
            try
            {
                TcpListener = new TcpListener(new IPEndPoint(Dns.GetHostAddresses(HostName)[0], Port));
                TcpListener.Start();
                Task.Run(() => Listener(TcpListener));
                Form1.AppendTextBox($"HTTP Service started on {TcpListener.LocalEndpoint.ToString()}\n");
                return false;
            }
            catch (Exception e)
            {
                Form1.AppendTextBox(e.Message + "\n");
                return true ;
            }
        }

        public static async Task StopWebServer()
        {
            if (TcpListener != null)
            TcpListener.Stop();
            TcpListener = null;
            Form1.AppendTextBox($"HTTP Service stopped\n");
        }

        public static async Task Listener(TcpListener socket)
        {
            //Log.Success("Internal HTTP server listening for clients");
            while (TcpListener != null)
            {
                try
                {
                    var client = socket.AcceptTcpClient();
                    //Console.WriteLine($"New client {client.Client.RemoteEndPoint.ToString()}");
                    await Task.Run(() => SocketHandler(client));
                }
                catch (Exception e)
                {
                    //Log.Error("Client caused exception", e);
                    
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
            }
            catch (Exception e)
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
                                    foreach (var finger in  accounts.fingerAccounts.Values)
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
                                    var files = Directory.GetFiles(Environment.CurrentDirectory);
                                    if (files.Any(n => n.Contains(request.TrimStart('/').ToLower())))
                                    //if (File.Exists(request.TrimStart('/')))
                                    {
                                        byte[] _payLoad = Encoding.UTF8.GetBytes(File.ReadAllText(files.Where(n => n.Contains(request.TrimStart('/').ToLower())).First()));
                                        byte[] _header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 200 OK\r\ncontent-type: text/html; charset=utf-8\r\ncontent-length: " + _payLoad.Length + "\r\n\r\n"));
                                        await sw.WriteAsync(Encoding.UTF8.GetChars(_header));
                                        await sw.WriteAsync(Encoding.UTF8.GetChars(_payLoad));
                                        await sw.FlushAsync();
                                        break;
                                    }
                                    else
                                    {
                                        byte[] _payLoad = Encoding.UTF8.GetBytes("");
                                        byte[] _header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 404 Not Found\r\n\r\n"));
                                        await sw.WriteAsync(Encoding.UTF8.GetChars(_header));
                                        await sw.WriteAsync(Encoding.UTF8.GetChars(_payLoad));
                                        await sw.FlushAsync();
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
                            //switch (postrequest)
                            //{
                            //    case "/NEWUSER":
                            //        //var usr = AccountFromHTTP(puredata);
                            //        byte[] _payLoad = new byte[0];
                            //        byte[] _header = new byte[0];

                            //        if (usr.Name == null || usr.Name.Length < 1)
                            //        {
                            //            //Log.Warning("POST request rejected, invalid data");
                            //            //await sw.WriteAsync("HTTP/2.0 400 Bad Request");
                            //            _payLoad = Encoding.UTF8.GetBytes("<html><head /><body><p>Invalid form data</p></body></html>");
                            //            _header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 400 Bad Request\r\ncontent-type: text/html; charset=utf-8\r\ncontent-length: " + _payLoad.Length + "\r\n\r\n"));

                            //        }
                            //        else
                            //        {
                            //            Log.Success("New user added with the web client");
                            //            //await sw.WriteAsync("HTTP/2.0 200 OK");
                            //            _payLoad = Encoding.UTF8.GetBytes(File.ReadAllText("newusersuccess.html"));
                            //            _header = Encoding.UTF8.GetBytes(WebUtility.HtmlEncode("HTTP/2.0 200 OK\r\ncontent-type: text/html; charset=utf-8\r\ncontent-length: " + _payLoad.Length + "\r\n\r\n"));
                            //        }
                            //        await sw.WriteAsync(Encoding.UTF8.GetChars(_header));
                            //        await sw.WriteAsync(Encoding.UTF8.GetChars(_payLoad));
                            //        await sw.FlushAsync();
                            //        //fingerAccounts.Add(usr);
                            //        break;
                            //}
                        }
                    }
                }
                sw.Close();
            }
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
}
