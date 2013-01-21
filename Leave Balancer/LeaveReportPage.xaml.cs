using System;
    using System.Collections.Generic;
    using System.Linq;
using System.Windows;
    using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace Leave_Balancer
{
    public partial class LeaveReportPage : PhoneApplicationPage
    {
        public LeaveReportPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(LeaveReportPage_Loaded);
            this.PayperiodSelector.SelectionChanged += new SelectionChangedEventHandler(PayperiodSelector_SelectionChanged);
        }

        private void PayperiodSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedPayPeriod = DateTime.Parse(this.PayperiodSelector.SelectedItem.ToString());
            DateTime payPeriodEndDate = PayPeriodUtilities.GetPayPeriodEndDate(selectedPayPeriod);

            AnnualReportList.ItemsSource = PayPeriodUtilities.GetCurrentLeaveEntries()
                .Where(o =>
                    o.Type == LeaveType.Annual &&
                    (o.LeaveDate >= selectedPayPeriod &&
                    o.LeaveDate <= payPeriodEndDate)
                    )
                .ToList();
            SickReportList.ItemsSource = PayPeriodUtilities.GetCurrentLeaveEntries()
                .Where(o =>
                    o.Type == LeaveType.Sick &&
                    (o.LeaveDate >= selectedPayPeriod &&
                    o.LeaveDate <= payPeriodEndDate)
                    )
                .ToList();
        }

        private void LeaveReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime currentPayperiod = PayPeriodUtilities.GetCurrentPayPeriod();
            List<string> periodList = new List<string>();

            for (int i = 9; i >= 1; i--)
            {
                string item = currentPayperiod.AddDays(-(i * 14)).ToLongDateString();
                periodList.Add(item);
            }

            periodList.Add(currentPayperiod.ToLongDateString());

            for (int i = 1; i <= 9; i++)
            {
                string item = currentPayperiod.AddDays(i * 14).ToLongDateString();
                periodList.Add(item);
            }
            PayperiodSelector.ItemsSource = periodList;
            PayperiodSelector.SelectedItem = currentPayperiod.ToLongDateString();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}