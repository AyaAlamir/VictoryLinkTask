using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VictoryLinkTask.Service.DTOs
{
    public class HandlePromotionInputDto
    {
        [Required]
        public int MobileNumber { get; set; }
    }
}