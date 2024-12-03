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
        IEnumerable<ProfUser> adminUsers = await userManager
            .GetUsersInRoleAsync(AdminRoleName);

        IEnumerable<ProfUser> managerUsers = await userManager
            .GetUsersInRoleAsync(ManagerRoleName);

        List<string> idsNotToSelect = [];

        idsNotToSelect
            .AddRange(adminUsers
                .Select(x => x.Id));

        idsNotToSelect
            .AddRange(managerUsers
                .Select(x => x.Id));

        IQueryable<ProfUser> query = userRepository
            .GetAllAttached()
            .Include(u => u.UserTasks)
                .ThenInclude(ut => ut.Task)
            .Where(u => !u.UserTasks
                .Any(ut => !ut.Task.IsCompleted)
                && !idsNotToSelect.Contains(u.Id));

        int totalCount = await query
            .CountAsync();

        List<UserViewModel> items = await query
            .OrderBy(x => x.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserFirstName = u.FirstName,
                UserLastName = u.LastName,
                Role = userManager.GetRolesAsync(u).Result
                    .FirstOrDefault()!
            })
            .ToListAsync();

        return new PagedResult<UserViewModel>
        {
            Items = items,
            AdditionalProperty = taskId,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
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

        int totalCount = await query
            .CountAsync();

        List<UserViewModel> items = await query
            .OrderBy(x => x.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserFirstName = u.FirstName,
                UserLastName = u.LastName,
                Role = userManager.GetRolesAsync(u).Result
                    .FirstOrDefault()!
            })
            .ToListAsync();

        return new PagedResult<UserViewModel>
        {
            Items = items,
            AdditionalProperty = taskId,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task AssignWorkersToTaskAsync(
       int taskId,
       IEnumerable<string> workerIds)
    {
        ProfTask task = await taskRepository.
            GetByIdAsync(taskId)
            ?? throw new ItemNotFoundException($"Task with id `{taskId}` not found");

        List<ProfUserTask> existingAssignments = await userTaskRepository
            .GetAllAttached()
            .Where(ut => ut.TaskId == taskId && workerIds
                .Contains(ut.WorkerId))
            .ToListAsync();

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

            await userTaskRepository
                .AddAsync(userTask);
        }
    }

    public async Task RemoveWorkersFromTaskAsync(
        int taskId,
        IEnumerable<string> workerIds)
    {
        ProfTask task = await taskRepository.
             GetByIdAsync(taskId)
             ?? throw new ItemNotFoundException($"Task with id `{taskId}` not found");

        List<ProfUserTask> existingAssignments = await userTaskRepository
            .GetAllAttached()
            .Where(ut => ut.TaskId == taskId && workerIds
                .Contains(ut.WorkerId))
            .ToListAsync();

        foreach (ProfUserTask assignment in existingAssignments)
        {
            await userTaskRepository
                .DeleteAsync(assignment);
        }
    }
}