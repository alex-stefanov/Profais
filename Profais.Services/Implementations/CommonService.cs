using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;

namespace Profais.Services.Implementations;

public class CommonService(
    IRepository<ProfUser, string> userRepository)
    : ICommonService
{
    public async Task<int> GetTotalPagesAsync(int pageSize)
    {
        int totalUsers = await userRepository
            .GetAllAttached()
            .Include(u => u.UserTasks)
                .ThenInclude(ut => ut.Task)
            .Where(u => !u.UserTasks
                .Any(ut => !ut.Task.IsCompleted))
            .CountAsync();

        int totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

        return totalPages;
    }
}
