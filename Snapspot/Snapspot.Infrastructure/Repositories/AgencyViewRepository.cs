using Microsoft.EntityFrameworkCore;
using Snapspot.Application.Repositories;
using Snapspot.Domain.Entities;
using Snapspot.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Infrastructure.Repositories
{
    public class AgencyViewRepository : IAgencyViewRepository
    {
        private readonly AppDbContext _context;

        public AgencyViewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(AgencyView newItem)
        {
            await _context.AgencyViews.AddAsync(newItem);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(Guid agencyId, Guid userId)
        {
            var today = DateTime.Today;

            var exists = await _context.AgencyViews
                .AnyAsync(av => av.AgencyId == agencyId && av.UserId == userId && av.ViewDate.Date == today);

            return exists;
        }
    }
}
