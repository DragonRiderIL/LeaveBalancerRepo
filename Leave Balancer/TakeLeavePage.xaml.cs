using System;
using Microsoft.Phone.Controls;

namespace Leave_Balancer
{
    public partial class TakeLeavePage : PhoneApplicationPage
    {
        public TakeLeavePage()
        {
            InitializeComponent();
        }

        private void TakeLeave_Click(object sender, EventArgs e)
        {
            PayPeriodUtilities.SaveLeave(new LeaveEntry
            {
                LeaveDate = (DateTime)this.LeaveDatePicker.Value,
                LeaveHours = double.Parse(this.Hourstaken.Text),
                Type = (bool)this.AnnualLeaveType.IsChecked ? LeaveType.Annual : LeaveType.Sick
            });

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void GoHome_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}