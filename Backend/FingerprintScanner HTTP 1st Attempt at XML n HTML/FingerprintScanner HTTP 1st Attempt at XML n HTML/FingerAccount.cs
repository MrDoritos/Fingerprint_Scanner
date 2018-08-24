using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace FingerPrintScannerBackend
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
        public IDictionary<int, Bitmap> scans { get; private set; }
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
            scans = new Dictionary<int, Bitmap>();
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

            [Obsolete("Obsolete after being able to interface scanner", true)]
        public void AddScan(Bitmap scan)
        {
            if (scans.Count < 1)
            {
                scans.Add(1, scan);
            }
            else
            {
                scans.Add(scans.Keys.Max() + 1, scan);
            }
        }

        

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

    class FingerOperations
    {
        [Obsolete("Not recommended, requires a lot more overhead", false)]
        public static FingerAccount GetFingerAccount(Bitmap source, List<FingerAccount> fingerAccounts)
        {
            List<bool> sourceHash = GetHash(source);

            IDictionary<FingerAccount, IDictionary<Bitmap, List<bool>>> hashes = new Dictionary<FingerAccount, IDictionary<Bitmap, List<bool>>>();

            //Calculate the hashes of all scans (I should probably make it so it calculates once)
            foreach (var finger in fingerAccounts)
            {
                hashes.Add(finger, new Dictionary<Bitmap, List<bool>>());
                foreach (var fingerimage in finger.scans)
                {
                    hashes[finger].Add(fingerimage.Value, GetHash(fingerimage.Value));
                }
            }

            IDictionary<FingerAccount, int> points = new Dictionary<FingerAccount, int>();

            foreach (var fingeraccount in hashes)
            {
                points.Add(fingeraccount.Key, 0);
                foreach (var hash in fingeraccount.Value)
                {
                    points[fingeraccount.Key] += sourceHash.Zip(hash.Value, (i, j) => i == j).Count(eq => eq);
                }
            }

            return points.Where(n => n.Value == points.Values.Max()).First().Key;
        }

        private static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            //create new image with 16x16 pixel
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false                
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                }
            }
            return lResult;
        }
    }
}
