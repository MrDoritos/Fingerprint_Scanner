using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintScanner
{
    class FingerAccount : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public string Name;
        public bool loggedIn { get; private set; }
        public string folderName;
        //public IDictionary<int, Bitmap> scans { get; private set; }
        public int templateId { get; set; }

        private DateTime startTime;
        private TimeSpan _currentTime;
        public TimeSpan _totalTime;

        public TimeSpan CurrentTime
        {
            get
            {
                if (loggedIn)
                {
                    return new TimeSpan(DateTime.Now.Subtract(startTime).Ticks);
                }
                else
                {
                    return new TimeSpan(0);
                }
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                if (loggedIn)
                {
                    return new TimeSpan(_totalTime.Ticks + CurrentTime.Ticks);
                }
                else
                {
                    return _totalTime;
                }
            }
        }

        public DateTime creationDate;

        /// <summary>
        /// Initialize a finger
        /// </summary>
        /// <param name="Name">Name</param>
        public FingerAccount(string Name, int templateId)
        {
            this.templateId = templateId;
            folderName = $"{Name}_{new Random().Next(0, int.MaxValue)}";
            //scans = new Dictionary<int, Bitmap>();
            this.Name = Name;
            loggedIn = false;
            creationDate = DateTime.Now;
            _currentTime = new TimeSpan(0);
            _totalTime = new TimeSpan(0);
            startTime = DateTime.Now;
        }

        //private void GenerateFolder()
        //{
        //    Directory.CreateDirectory(folderName);
        //}

        //[Obsolete("Obsolete after being able to interface scanner", true)]
        //public void AddScan(Bitmap scan)
        //{
        //    if (scans.Count < 1)
        //    {
        //        scans.Add(1, scan);
        //    }
        //    else
        //    {
        //        scans.Add(scans.Keys.Max() + 1, scan);
        //    }
        //}



        /// <summary>
        /// Log out
        /// </summary>
        public void LogOut()
        {
            if (loggedIn)
            {
                _totalTime = _totalTime.Add(CurrentTime);
                _currentTime = new TimeSpan(0);
                loggedIn = false;
            }
        }

        /// <summary>
        /// Log in
        /// </summary>
        public void LogIn()
        {
            if (!loggedIn)
            {
                startTime = DateTime.Now;
                loggedIn = true;
            }
        }

        /// <summary>
        /// Toggle logging in/out
        /// </summary>
        public void ToggleLogINOUT()
        {
            if (loggedIn)
            {
                //_currentTime = DateTime.Now.Subtract(startTime);
                _totalTime = _totalTime.Add(CurrentTime);
                _currentTime = new TimeSpan(0);
                loggedIn = false;
            }
            else
            {
                startTime = DateTime.Now;
                loggedIn = true;
            }
        }
    }
}