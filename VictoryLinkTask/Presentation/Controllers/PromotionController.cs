using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VictoryLinkTask.Service.DTOs;
using VictoryLinkTask.Service.Interfaces;

namespace VictoryLinkTask.Presentation.Controllers
{
    
    public class PromotionController : ApiController
    {

        public IPromotionService _promotionService;
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        /// <summary>
        /// Save the MobileNumber,Action in Request table along with the Receiving date 
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns>1 if added successfully , 2 otherwize</returns>
        [HttpPost]
        [Route("api/ReceiveRequest")]
        public async Task<IHttpActionResult> ReceiveRequest(ReceivePromotionInputDto inputDto)
        {
            GeneralResponseDto result = await _promotionService.ReceiveRequest(inputDto);
            return Ok(result);
        }

        /// <summary>
        /// update the Request record associated with the given MobileNumber table along with the Handling Date  
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns>1 if added successfully , 2 otherwize</returns>
        [HttpPost]
        [Route("api/HandleRequest")]
        public async Task<IHttpActionResult> HandleRequest(HandlePromotionInputDto inputDto)
        {
            GeneralResponseDto result = await _promotionService.HandleRequest(inputDto);
            return Ok(result);
        }
    }
}
