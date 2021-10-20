using System;
using System.Collections.Generic;

#nullable disable

namespace SimChargeWinApp.Models
{
    public partial class TblSimCharge
    {
        public long Id { get; set; }
        public DateTime? DateTime { get; set; }
        public string Value { get; set; }
    }
}
