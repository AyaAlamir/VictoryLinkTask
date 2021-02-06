using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VictoryLinkTask.Service.DTOs
{
    public class ReceivePromotionInputDto
    {
        [Required]
        public int MobileNumber { get; set; }
        [Required]
        public string Action { get; set; }
    }
}