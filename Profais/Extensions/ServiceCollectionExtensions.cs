using Profais.Data.Models;
using Profais.Data.Repositories;
using Profais.Services.Implementations;
using Profais.Services.Interfaces;

namespace Profais.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IRepository<Material, int>, ProfRepository<Material, int>>();
        services.AddScoped<IRepository<Message, object>, ProfRepository<Message, object>>();
        services.AddScoped<IRepository<Penalty, int>, ProfRepository<Penalty, int>>();
        services.AddScoped<IRepository<ProfProject, int>, ProfRepository<ProfProject, int>>();
        services.AddScoped<IRepository<ProfProjectRequest, int>, ProfRepository<ProfProjectRequest, int>>();
        services.AddScoped<IRepository<ProfSpecialistRequest, int>, ProfRepository<ProfSpecialistRequest, int>>();
        services.AddScoped<IRepository<ProfTask, int>, ProfRepository<ProfTask, int>>();
        services.AddScoped<IRepository<ProfUser, string>, ProfRepository<ProfUser, string>>();
        services.AddScoped<IRepository<ProfUserPenalty, object>, ProfRepository<ProfUserPenalty, object>>();
        services.AddScoped<IRepository<ProfUserTask, object>, ProfRepository<ProfUserTask, object>>();
        services.AddScoped<IRepository<ProfWorkerRequest, int>, ProfRepository<ProfWorkerRequest, int>>();
        services.AddScoped<IRepository<TaskMaterial, object>, ProfRepository<TaskMaterial, object>>();
        services.AddScoped<IRepository<Vehicle, int>, ProfRepository<Vehicle, int>>();

        return services;
    }

    public static IServiceCollection RegisterUserDefinedServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICommonService, CommonService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<IProjectRequestService, ProjectRequestService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ISpecialistRequestService, SpecialistRequestService>();
        services.AddScoped<IWorkerRequestService, WorkerRequestService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkerService, WorkerService>();

        return services;
    }
}
