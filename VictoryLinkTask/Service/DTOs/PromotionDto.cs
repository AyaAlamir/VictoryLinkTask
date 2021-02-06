using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VictoryLinkTask.Service.DTOs
{
    public class PromotionDto
    {
        public int MobileNumber { get; set; }
        public string Action { get; set; }
        public Nullable<bool> IsHandled { get; set; }
        public System.DateTime RequestDate { get; set; }
        public Nullable<System.DateTime> HandlingDate { get; set; }
    }
}