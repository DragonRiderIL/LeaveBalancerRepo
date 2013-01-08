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

namespace Leave_Balancer
{
    public class LeaveEntry
    {
        public DateTime LeaveDate { get; set; }

        public double LeaveHours { get; set; }

        public LeaveType Type { get; set; }
    }
}
