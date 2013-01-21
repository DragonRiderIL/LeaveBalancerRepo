using System;

namespace Leave_Balancer
{
    public class LeaveEntry
    {
        public DateTime LeaveDate { get; set; }

        public double LeaveHours { get; set; }

        public LeaveType Type { get; set; }

        public string DateDisplay 
        {
            get
            {
                return this.LeaveDate.ToLongDateString();
            }
        }

        public string HoursDisplay 
        {
            get
            {
                return this.LeaveHours.ToString();
            }
        }

        public string TypeDisplay 
        {
            get
            {
                return this.Type.ToString();
            }
        }
    }
}
