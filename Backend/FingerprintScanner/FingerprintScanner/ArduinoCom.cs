using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


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
        public bool doingTask = false;

        public enum Commands
        {
            QUERY_TEMPLATE_COUNT = 0x01,
            ENROLL_FINGER = 0x02,
            GET_FINGERPRINT = 0x03,
            CLEAR_DATABASE = 0x04,
            DELETE_USER = 0x05,
        }

        public int TemplateCount
        {
            get { return GetTemplateCount(); }
        }

        public FingerRelay(SerialPort serialPort)
        {
            this.serialPort = serialPort;
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
                if (!doingTask)
                {
                    doingTask = true;
                    try
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(new byte[] { (byte)command }, 0, 1);
                        doingTask = false;
                        return serialPort.ReadLine();
                    }
                    catch (Exception)
                    {
                        doingTask = false;
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

