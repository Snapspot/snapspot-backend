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
    public class ActiveUserRepository : IActiveUserRepository
    {
        private readonly AppDbContext _context;

        public ActiveUserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CheckLogin(Guid userId)
        {
            // Lấy ngày hiện tại, chỉ lấy phần ngày (không có giờ)
            var today = DateTime.Today;

            // Kiểm tra xem đã có bản ghi nào cho userId vào ngày hôm nay chưa
            var existingRecord = await _context.UserActives
                .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.LoginDate.Date == today);

            // Nếu không tìm thấy bản ghi, tạo mới
            if (existingRecord == null)
            {
                var newUserActive = new UserActive
                {
                    UserId = userId,
                    LoginDate = today, // Lưu ngày hiện tại
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.UserActives.AddAsync(newUserActive);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountActiveUserByDate(DateTime date)
        {
            // Chắc chắn rằng chỉ lấy phần ngày của date
            var specificDate = date.Date;

            // Đếm số lượng bản ghi tương ứng với ngày đã cho
            var count = await _context.UserActives
                .CountAsync(ua => ua.LoginDate.Date == specificDate);

            return count;
        }
    }
}
