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
    public class Basis
    {
        public double StartingAnnual { get; set; }

        public double StartingSick { get; set; }

        public double AnnualAccrue { get; set; }

        public double SickAccrue { get; set; }

        public DateTime StartingPayPeriod { get; set; }
    }
}
