using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandleRequestsWindowsService.DTOs
{
    public class Promotion
    {
        public int RequestId { get; set; }
        public int MobileNumber { get; set; }
        public string Action { get; set; }
        public Nullable<bool> IsHandled { get; set; }
        public System.DateTime RequestDate { get; set; }
        public Nullable<System.DateTime> HandlingDate { get; set; }
    }
}
