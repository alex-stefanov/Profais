using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Profais.Common.Exceptions;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Worker;
using Profais.Services.ViewModels.Shared;

using static Profais.Common.Constants.UserConstants;

namespace Profais.Services.Implementations;

public class WorkerService(
    UserManager<ProfUser> userManager,
    IRepository<ProfUser, string> userRepository,
    IRepository<ProfTask, int> taskRepository,
    IRepository<ProfUserTask, object> userTaskRepository)
    : IWorkerService
{
    public async Task<PagedResult<UserViewModel>> GetPagedAvaliableWorkersAsync(
        int pageNumber,
        int pageSize,
        int taskId)
    {
        List<string> idsNotToSelect = await GetExcludedUserIdsAsync();

        IQueryable<ProfUser> query = userRepository
            .GetAllAttached()
            .Include(u => u.UserTasks)
                .ThenInclude(ut => ut.Task)
            .Where(u => !u.UserTasks
                .Any(ut => !ut.Task.IsCompleted)
                     && !idsNotToSelect.Contains(u.Id));

        int totalCount = await query.CountAsync();

        List<UserViewModel> items = await query
            .OrderBy(x => x.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserFirstName = u.FirstName,
                UserLastName = u.LastName,
                Role = string.Empty,
            })
            .ToListAsync();

        foreach (UserViewModel user in items)
        {
            ProfUser profUser = await userManager
                .FindByIdAsync(user.Id)
                ?? throw new ItemNotFoundException($"User wasnt found with id `{user.Id}`");

            var roles = await userManager.GetRolesAsync(profUser);

            user.Role = roles.FirstOrDefault()!;
        }

        return new PagedResult<UserViewModel>
        {
            Items = items,
            AdditionalProperty = taskId,
            PaginationViewModel = new PaginationViewModel
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
            },
        };
    }

    public async Task<PagedResult<UserViewModel>> GetPagedWorkersFromTaskAsync(
        int pageNumber,
        int pageSize,
        int taskId)
    {
        IQueryable<ProfUser> query = userRepository
            .GetAllAttached()
            .Include(u => u.UserTasks)
                .ThenInclude(ut => ut.Task)
            .Where(u => u.UserTasks
                .Any(ut => ut.TaskId == taskId));

        int totalCount = await query.CountAsync();

        List<UserViewModel> items = await query
            .OrderBy(x => x.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserFirstName = u.FirstName,
                UserLastName = u.LastName,
                Role = string.Empty,
            })
            .ToListAsync();

        foreach (UserViewModel user in items)
        {
            ProfUser profUser = await userManager
                .FindByIdAsync(user.Id)
                ?? throw new ItemNotFoundException($"User wasnt found with id `{user.Id}`");

            var roles = await userManager.GetRolesAsync(profUser);

            user.Role = roles.FirstOrDefault()!;
        }

        return new PagedResult<UserViewModel>
        {
            Items = items,
            AdditionalProperty = taskId,
            PaginationViewModel = new PaginationViewModel
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
            },
        };
    }

    public async Task AssignWorkersToTaskAsync(
       int taskId,
       IEnumerable<string> workerIds)
    {
        ProfTask task = await taskRepository.GetByIdAsync(taskId)
            ?? throw new ItemNotFoundException($"Task with id `{taskId}` not found");

        List<ProfUserTask> existingAssignments = await GetExistingUserAssignmentsAsync(taskId, workerIds);

        List<string> workersToAssign = workerIds
            .Where(workerId => !existingAssignments
                .Any(ut => ut.WorkerId == workerId))
            .ToList();

        foreach (string workerId in workersToAssign)
        {
            var userTask = new ProfUserTask
            {
                TaskId = taskId,
                WorkerId = workerId
            };

            await userTaskRepository.AddAsync(userTask);
        }
    }

    public async Task RemoveWorkersFromTaskAsync(
        int taskId,
        IEnumerable<string> workerIds)
    {
        List<ProfUserTask> existingAssignments = await GetExistingUserAssignmentsAsync(taskId, workerIds);

        foreach (ProfUserTask assignment in existingAssignments)
        {
            await userTaskRepository.DeleteAsync(assignment);
        }
    }

    private async Task<List<string>> GetExcludedUserIdsAsync()
    {
        var adminUsers = await userManager.GetUsersInRoleAsync(AdminRoleName);
        var managerUsers = await userManager.GetUsersInRoleAsync(ManagerRoleName);
        var clientUsers = await userManager.GetUsersInRoleAsync(ClientRoleName);

        List<string> excludedUserIds =
        [
            .. adminUsers.Select(x => x.Id),
            .. managerUsers.Select(x => x.Id),
            .. clientUsers.Select(x => x.Id),
        ];

        return excludedUserIds;
    }

    private async Task<List<ProfUserTask>> GetExistingUserAssignmentsAsync(
        int taskId,
        IEnumerable<string> workerIds)
    {
        List<ProfUserTask> result = await userTaskRepository
            .GetAllAttached()
            .Where(ut => ut.TaskId == taskId && workerIds.Contains(ut.WorkerId))
            .ToListAsync();

        return result;
    }
}