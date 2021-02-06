using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VictoryLinkTask.Service.DTOs;

namespace VictoryLinkTask.Service.Interfaces
{
    public interface IPromotionService
    {
        Task<GeneralResponseDto> ReceiveRequest(ReceivePromotionInputDto InputDto);
        Task<GeneralResponseDto> HandleRequest(HandlePromotionInputDto InputDto);
    }
}
