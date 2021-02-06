using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VictoryLinkTask.Core;
using VictoryLinkTask.Repository.Interfaces.Common;
using VictoryLinkTask.Service.DTOs;
using VictoryLinkTask.Service.Interfaces;

namespace VictoryLinkTask.Service.Implementation
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromotionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResponseDto> HandleRequest(List<HandlePromotionInputDto> InputDto)
        {
            GeneralResponseDto updated = new GeneralResponseDto { status = 2, Message = "fail" };
            foreach (var item in InputDto)
            {
                bool exists = await _unitOfWork.Promotion.GetAnyAsync(p => p.MobileNumber != item.MobileNumber);
                if (exists)
                {
                    Promotion promotion = await _unitOfWork.Promotion.FirstOrDefaultAsync(p => p.MobileNumber == item.MobileNumber);
                    if (promotion != null)
                    {
                        promotion.IsHandled = true;
                        promotion.HandlingDate = DateTime.Now;
                    }
                }
            }
            if (await _unitOfWork.Commit() > default(int))
            {
                updated.status = 1;
                updated.Message = "Success";
            }

            return updated;
        }

        public async Task<GeneralResponseDto> ReceiveRequest(ReceivePromotionInputDto InputDto)
        {
            GeneralResponseDto added = new GeneralResponseDto { status = 2, Message = "Failed" };
            bool isExist = await _unitOfWork.Promotion.GetAnyAsync(p => p.MobileNumber == InputDto.MobileNumber);
            if (!isExist)
            {
                //create random id
                Random rnd = new Random();
                int id = rnd.Next(1, rnd.Next(2,500))*10;

                Promotion promotion = new Promotion
                {
                    RequestId = id,
                    MobileNumber = InputDto.MobileNumber,
                    Action = InputDto.Action,
                    IsHandled = false,
                    RequestDate = DateTime.Now
                };
                _unitOfWork.Promotion.CreateAsyn(promotion);

                if (await _unitOfWork.Commit() > default(int))
                {
                    added.status = 1;
                    added.Message = "Success";
                }
            }
            return added;
        }
    }
}