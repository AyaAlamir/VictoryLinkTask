using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoryLinkTask.Core;
using VictoryLinkTask.Repository.Implementation.Common;
using VictoryLinkTask.Repository.Interfaces.Custom;

namespace VictoryLinkTask.Repository.Implementation.Custom
{
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(Promotions_Entities appDbContext) : base(appDbContext)
        {

        }
    }
}