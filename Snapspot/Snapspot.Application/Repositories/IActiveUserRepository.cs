using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Repositories
{
    public interface IActiveUserRepository
    {
        Task CheckLogin(Guid userId);
        Task<int> CountActiveUserByDate(DateTime date);
    }
}
