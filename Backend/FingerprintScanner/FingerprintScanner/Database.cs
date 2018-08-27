using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FingerprintScanner
{
    class Database
    {
        Accounts Accounts;
        string filename;
        public bool Autosave = false;
        public TimeSpan autosaveTime = new TimeSpan(0,15,0);
        private DateTime timeLastSaved = DateTime.Now;


        public Database(string filename, Accounts accounts)
        {
            this.Accounts = accounts;
            this.filename = filename;
        }

        public async Task AutoSave()
        {
            while (Autosave)
            {
                if (DateTime.Now.Subtract(timeLastSaved).TotalMinutes > autosaveTime.TotalMinutes)
                {
                    timeLastSaved = DateTime.Now;
                    Save(filename, Accounts);
                    Form1.AppendTextBox("Autosaved Database");
                }
                await Task.Delay(60000);
            }
        }

        public static void Save(string filename, Accounts accounts)
        {
            JObject joson = new JObject();
            JArray jArray = new JArray();
            foreach (var fu_ck in accounts.fingerAccounts.Values)
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
                File.WriteAllText(filename, JsonConvert.SerializeObject(joson));
            }
        }

        public static Accounts Read(string filename)
        {
            if (!File.Exists(filename))
            { return new Accounts(); }


            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(File.ReadAllText(filename));
            }
            catch (JsonReaderException)
            {
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception e)
            {
            }
            if (jObject == null) { return new Accounts(); }
            try
            {
                var ffff = new Dictionary<int, FingerAccount>();
                foreach (JObject val in (JArray)jObject["users"])
                {
                    var usr = new FingerAccount((string)val["name"], (int)val["templateId"]);
                    usr._totalTime = new TimeSpan((long)val["totalTime"]);
                    usr.creationDate = (DateTime)val["creationDate"];
                    ffff.Add(usr.templateId, usr);
                }
                //Form1.logBox.AppendText("Read database");
                return new Accounts() { fingerAccounts = ffff };
            }
            catch (Exception)
            {
            }
            return new Accounts();
        }
    }
}

