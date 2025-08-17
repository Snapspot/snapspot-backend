using Snapspot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IAgencyViewRepository
    {
        Task Create(AgencyView newItem);
        Task<bool> IsExist(Guid agencyId, Guid userId);
    }
}
