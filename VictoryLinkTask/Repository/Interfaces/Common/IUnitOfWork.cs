using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VictoryLinkTask.Core;
using VictoryLinkTask.Repository.Interfaces.Custom;

namespace VictoryLinkTask.Repository.Interfaces.Common
{
    public interface IUnitOfWork: IDisposable
    {
        Task<int> Commit();
        IRepository<Promotion> Promotion { get; }
    }
}
