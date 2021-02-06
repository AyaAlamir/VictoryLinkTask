using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandleRequestsWindowsService.DTOs
{
    public class HandlePromotionInputDTO
    {
        [Required]
        public int MobileNumber { get; set; }
    }
}
