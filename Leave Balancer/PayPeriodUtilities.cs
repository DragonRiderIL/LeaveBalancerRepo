using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Leave_Balancer
{
    public class PayPeriodUtilities
    {
        const string seedDate = "01/01/2012";

        public static DateTime GetCurrentPayPeriod()
        {
            DateTime lastSunday = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);

            if ((lastSunday.Subtract(DateTime.Parse(seedDate)).Days % 14) == 0)
            {
                return lastSunday;
            }
            else
            {
                return lastSunday.AddDays(-7);
            }
        }

        public static DateTime GetPayPeriodEndDate(DateTime current)
        {
            return current.AddDays(13);
        }

        public static DateTime GetNextPayPeriod(DateTime current)
        {
            return current.AddDays(14);
        }

        public static Basis GetBaseValues()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Basis baseValues = new Basis();

                XElement _xml;
                IsolatedStorageFileStream location = new IsolatedStorageFileStream(@"LeaveSettings.xml", System.IO.FileMode.Open, storage);
                System.IO.StreamReader file = new System.IO.StreamReader(location);
                _xml = XElement.Parse(file.ReadToEnd());
                if (_xml.Name.LocalName != null)
                {
                    baseValues.StartingAnnual = double.Parse(_xml.Attribute("AnnualBalance").Value);
                    baseValues.StartingSick = double.Parse(_xml.Attribute("SickBalance").Value);
                    baseValues.AnnualAccrue = double.Parse(_xml.Attribute("AnnualAccrue").Value);
                    baseValues.SickAccrue = double.Parse(_xml.Attribute("SickAccrue").Value);
                    baseValues.StartingPayPeriod = DateTime.Parse(_xml.Attribute("PayPeriod").Value);
                }
                file.Dispose();
                location.Dispose();

                return baseValues;
            }
        }

        public static int ElaspedPayPeriods(DateTime basePP, DateTime currentPP)
        {
            DateTime starting = basePP.AddDays(-13);

            return (int)((currentPP.Subtract(starting).Days) / 14);
        }

        public static void SaveLeave(LeaveEntry entry)
        {
            Collection<LeaveEntry> currentEntries = new Collection<LeaveEntry>();

            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (storage.FileExists("LeaveRecords.xml"))
            {
                currentEntries = GetCurrentLeaveEntries();
            }

            currentEntries.Add(entry);

            IsolatedStorageFileStream location = new IsolatedStorageFileStream("LeaveRecords.xml", FileMode.OpenOrCreate, storage);
            XmlSerializer serializer = new XmlSerializer(typeof(Collection<LeaveEntry>));

            StreamWriter file = new StreamWriter(location);
            serializer.Serialize(file, currentEntries);

            file.Dispose();
            location.Dispose();
        }

        public static Collection<LeaveEntry> GetCurrentLeaveEntries()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (storage.FileExists("LeaveRecords.xml"))
            {
                IsolatedStorageFileStream location = new IsolatedStorageFileStream("LeaveRecords.xml", System.IO.FileMode.Open, storage);
                System.IO.StreamReader file = new System.IO.StreamReader(location);
                XmlSerializer serializer = new XmlSerializer(typeof(Collection<LeaveEntry>));

                Collection<LeaveEntry> entries = (Collection<LeaveEntry>)serializer.Deserialize(file);

                file.Dispose();
                location.Dispose();

                return entries;
            }
            else
            {
                return new Collection<LeaveEntry>();
            }
        }

        public static UsedLeave GetUsedLeaveBalances()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!storage.FileExists("LeaveRecords.xml"))
            {
                return new UsedLeave();
            }

            //IsolatedStorageFileStream location = new IsolatedStorageFileStream("LeaveRecords.xml", System.IO.FileMode.Open, storage);
            //System.IO.StreamReader file = new System.IO.StreamReader(location);
            //XmlSerializer serializer = new XmlSerializer(typeof(Collection<LeaveEntry>));

            Collection<LeaveEntry> entries = GetCurrentLeaveEntries();

            UsedLeave balances = new UsedLeave();

            foreach (LeaveEntry item in entries)
            {
                if (item.Type == LeaveType.Annual)
                {
                    balances.Annual += item.LeaveHours;
                }
                else
                {
                    balances.Sick += item.LeaveHours;
                }
            }

            return balances;
        }
    }
}
