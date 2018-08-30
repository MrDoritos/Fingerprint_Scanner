using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace FingerprintScanner
{
    class ArduinoCom
    {
        public SerialPort ArduinoPort { get; private set; }
        public FingerRelay FingerRelay { get; private set; }
        

        public ArduinoCom(SerialPort port) { ArduinoPort = port;  FingerRelay = new FingerRelay(port); }

        public ArduinoCom() { ArduinoPort = null; FingerRelay = null; }

        public bool TryOpen(string port, int baud, out ArduinoCom arduinoCom)
        {
            arduinoCom = new ArduinoCom();
            try
            {
                SerialPort serialPort = new SerialPort(port, baud);
                serialPort.Open();
                //Form1.AppendTextBox($"Arduino communications service started on {serialPort.PortName}\n");
                arduinoCom = new ArduinoCom(serialPort);
                return true;
            }
            catch (Exception e)
            {
                Form1.AppendTextBox($"{e.Message}\n");
            }
            return false;
        }
    }
    class FingerRelay
    {
        public SerialPort serialPort { get; private set; }
        public bool Lock = false;
        public bool inFinger = false;
        List<byte> currentbuffer = new List<byte>();

        public enum Commands
        {
            QUERY_TEMPLATE_COUNT = 0x01,
            ENROLL_FINGER = 0x02,
            GET_FINGERPRINT = 0x03,
            CLEAR_DATABASE = 0x04,
            DELETE_USER = 0x05,
            QUERY_THREAD = 0x06,
        }

        public int TemplateCount
        {
            get { return GetTemplateCount(); }
        }

        public FingerRelay(SerialPort serialPort)
        {
            this.serialPort = serialPort;
        }

        public void RandomThread()
        {
            while (true && serialPort.IsOpen)
            {

                if (!Lock && !inFinger)
                {
                    string inff = SendCommandGetData(Commands.QUERY_THREAD);
                    if (inff != "NO")
                    {
                        CommandReciever(inff);
                        while (inFinger)
                        {
                            Thread.Sleep(500);
                            CommandReciever(RecieveData());
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private void CommandReciever(string inp)
        {
            try
            {
                string read = inp;
                if (read.StartsWith("SCANNER_START_READ")) {
                    inFinger = true; Form1.AppendTextBox("Reading fingerprint...\n"); }
                else
                    if (inFinger)
                {
                    if (read.StartsWith("END")) {
                        inFinger = false; Form1.AppendTextBox("No longer reading fingerprint...\n"); }
                    else if (read.StartsWith("ID_"))
                    {
                        int f = Convert.ToInt16(read.Split('_').Last());
                        Form1.AppendTextBox($"Finger found, Id {f}\n");
                        if (Form1.fingerAccountManager.fingerAccounts.ContainsKey(f))
                        {
                            var acc = Form1.fingerAccountManager.fingerAccounts.First(n => n.Key == f);
                            acc.Value.ToggleLogINOUT();
                            //Form1.Refresh.Invoke();
                        }
                        else
                        {
                            Form1.AppendTextBox($"Invalid fingerprint template {f}\n");
                        }
                    }
                }
            } catch(Exception e)
            {
                Form1.AppendTextBox($"Error: {e.Message}\n");
            }
        }
        

        private int GetTemplateCount()
        {
            for (int retries = 0; retries < 4; retries++)
            {
                try
                {
                    return Convert.ToInt16(SendCommandGetData(Commands.QUERY_TEMPLATE_COUNT).Split('_').Last());
                }
                catch (Exception)
                {

                }
            }
            return -1;
        }

        public string RecieveData()
        {
            return (serialPort.ReadLine().Trim('\n', '\r'));
        }

        public string SendCommandGetData(Commands command)
        {
            while (true)
            {
                Task.Delay(100).Wait();
                if (!Lock)
                {
                    Lock = true;
                    try
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(new byte[] { (byte)command }, 0, 1);
                        Lock = false;
                        return serialPort.ReadLine().Trim('\n', '\r');
                    }
                    catch (Exception)
                    {
                        Lock = false;
                        return "ERROR";
                    }
                }
            }
        }

        public void SendByte(byte data)
        {
            serialPort.Write(new byte[] { data }, 0, 1);
        }
    }
}

