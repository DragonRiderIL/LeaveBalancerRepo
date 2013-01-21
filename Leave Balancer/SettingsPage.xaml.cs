using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Phone.Controls;

namespace Leave_Balancer
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!storage.FileExists("LeaveSettings.xml"))
            {
                this.PayPeriodBase.Text = PayPeriodUtilities.GetCurrentPayPeriod().AddDays(-1).ToShortDateString();
                this.AnnualBalance.Focus();
            }
            else
            {
                using (storage)
                {
                    XElement _xml;
                    IsolatedStorageFileStream location = new IsolatedStorageFileStream(@"LeaveSettings.xml", System.IO.FileMode.Open, storage);
                    System.IO.StreamReader file = new System.IO.StreamReader(location);
                    _xml = XElement.Parse(file.ReadToEnd());
                    if (_xml.Name.LocalName != null)
                    {
                        this.AnnualBalance.Text = _xml.Attribute("AnnualBalance").Value;
                        this.SickBalance.Text = _xml.Attribute("SickBalance").Value;
                        this.AnnualAccrue.Text = _xml.Attribute("AnnualAccrue").Value;
                        this.SickAccrue.Text = _xml.Attribute("SickAccrue").Value;
                        this.PayPeriodBase.Text = _xml.Attribute("PayPeriod").Value;
                    }
                    file.Dispose();
                    location.Dispose();
                }
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            System.IO.IsolatedStorage.IsolatedStorageFile storage = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

            using (storage)
            {
                XDocument _doc = new XDocument();
                XElement baseSetting = new XElement("BaseSetting");
                XAttribute AnnualBalance = new XAttribute("AnnualBalance", this.AnnualBalance.Text);
                XAttribute SickBalance = new XAttribute("SickBalance", this.SickBalance.Text);
                XAttribute AnnualAccrue = new XAttribute("AnnualAccrue", this.AnnualAccrue.Text);
                XAttribute SickAccrue = new XAttribute("SickAccrue", this.SickAccrue.Text);
                XAttribute PayPeriod = new XAttribute("PayPeriod", this.PayPeriodBase.Text);
                baseSetting.Add(AnnualBalance, SickBalance, AnnualAccrue, SickAccrue, PayPeriod);
                _doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), baseSetting);
                IsolatedStorageFileStream location = new IsolatedStorageFileStream("LeaveSettings.xml", FileMode.Create, storage);
                StreamWriter file = new StreamWriter(location);
                _doc.Save(file);
                file.Dispose();
                location.Dispose();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}