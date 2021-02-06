using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VictoryLinkTask.Core;
using VictoryLinkTask.Repository.Implementation.Custom;
using VictoryLinkTask.Repository.Interfaces.Common;
using VictoryLinkTask.Repository.Interfaces.Custom;

namespace VictoryLinkTask.Repository.Implementation.Common
{
    public class UnitOfWork: IUnitOfWork
    {
        public Promotions_Entities AppDbContext { get; }

        public UnitOfWork(Promotions_Entities appContext)
        {
            AppDbContext = appContext;
        }
        // Initialization code
        public IRepository<Promotion> Promotion => new PromotionRepository(AppDbContext);

        // other methods
        public Task<int> Commit()
        {
            return AppDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
        }
    }
}
