using System;

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
