using System;
using System.Collections.Generic;

#nullable disable

namespace Online_Quiz.Models
{
    public partial class Result
    {
        public int? UserId { get; set; }
        public DateTime Date { get; set; }
        public double Score { get; set; }
        public bool IsPass { get; set; }

        public virtual User User { get; set; }
    }
}
