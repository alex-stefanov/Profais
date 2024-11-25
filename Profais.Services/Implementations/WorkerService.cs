using Microsoft.EntityFrameworkCore;
using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Interfaces;
using Profais.Services.ViewModels.Worker;

namespace Profais.Services.Implementations;

public class WorkerService(
    IRepository<ProfUser, string> userRepository,
    IRepository<ProfTask, int> taskRepository,
    IRepository<ProfUserTask, object> userTaskRepository)
    : IWorkerService
{
    public async Task<IEnumerable<UserViewModel>> GetAvailableWorkersAsync(
        int page,
        int pageSize)
    {
        List<UserViewModel> availableUsers = await userRepository
            .GetAllAttached()
            .Include(u => u.UserTasks)
                .ThenInclude(ut => ut.Task)
            .Where(u => !u.UserTasks
                .Any(ut => !ut.Task.IsCompleted))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                UserFirstName = u.FirstName,
                UserLastName = u.LastName
            })
            .ToListAsync();

        return availableUsers;
    }

    public async Task AssignWorkersToTaskAsync(
        int taskId,
        List<string> workerIds)
    {
        ProfTask? task = await taskRepository.
            GetByIdAsync(taskId);

        if (task is null)
        {
            throw new ArgumentException(nameof(taskId), "Task not found");
        }

        List<ProfUserTask> existingAssignments = await userTaskRepository
            .GetAllAttached()
            .Where(ut => ut.TaskId == taskId && workerIds.Contains(ut.WorkerId))
            .ToListAsync();

        List<string> workersToAssign = workerIds
            .Where(workerId => !existingAssignments.Any(ut => ut.WorkerId == workerId))
            .ToList();

        foreach (var workerId in workersToAssign)
        {
            var userTask = new ProfUserTask
            {
                TaskId = taskId,
                WorkerId = workerId
            };
            await userTaskRepository.AddAsync(userTask);
        }
    }
}
