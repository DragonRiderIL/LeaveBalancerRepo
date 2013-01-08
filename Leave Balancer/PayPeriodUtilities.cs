using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

namespace Leave_Balancer
{
    public static class PayPeriodUtilities
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

        public static Basis GetBaseValues()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Basis baseValues= new Basis();

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
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!storage.FileExists("LeaveRecords.xml"))
            {
                CreateLeaveFile();
            }

            XElement _xml;
            IsolatedStorageFileStream location = new IsolatedStorageFileStream(@"LeaveSettings.xml", System.IO.FileMode.Open, storage);
            System.IO.StreamReader file = new System.IO.StreamReader(location);
            _xml = XElement.Parse(file.ReadToEnd());
        }

        public static void CreateLeaveFile()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XDocument _doc = new XDocument();
                XElement baseSetting = new XElement("LeaveEntries");
                _doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), baseSetting);
                IsolatedStorageFileStream location = new IsolatedStorageFileStream("LeaveRecords.xml", FileMode.Create, storage);
                StreamWriter file = new StreamWriter(location);
                _doc.Save(file);
                file.Dispose();
                location.Dispose();
            }
        }
    }
}
