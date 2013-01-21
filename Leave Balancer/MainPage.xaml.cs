

namespace Leave_Balancer
{
    using System;
    using System.Windows;
    using Microsoft.Phone.Controls;

    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.IO.IsolatedStorage.IsolatedStorageFile file = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

            if (!file.FileExists("LeaveSettings.xml"))
            {
                //NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
            }
            else
            {

                this.CurrentPayPeriod.Text = Leave_Balancer.PayPeriodUtilities.GetCurrentPayPeriod().ToLongDateString();
                this.CalculateCurrent();
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void CalculateCurrent()
        {
            Basis values = PayPeriodUtilities.GetBaseValues();

            int multiplier = PayPeriodUtilities.ElaspedPayPeriods(values.StartingPayPeriod, PayPeriodUtilities.GetCurrentPayPeriod());

            UsedLeave used = PayPeriodUtilities.GetUsedLeaveBalances();

            this.CurrentAnnualBalance.Text = (values.StartingAnnual + (values.AnnualAccrue * multiplier) - used.Annual).ToString();
            this.CurrentSickBalance.Text = (values.StartingSick + (values.SickAccrue * multiplier) - used.Sick).ToString();
        }

        private void TakeLeave_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/TakeLeavePage.xaml", UriKind.Relative));
        }

        private void LeaveReport_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LeaveReportPage.xaml", UriKind.Relative));
        }
    }
}