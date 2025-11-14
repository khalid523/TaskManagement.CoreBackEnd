using TaskManagement.Core.Entities;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.API.Services;

public class WorkTaskService : IWorkTaskService
{
    private readonly IWorkTaskRepository _taskRepository;
    private readonly ILogger<WorkTaskService> _logger;

    public WorkTaskService(IWorkTaskRepository taskRepository, ILogger<WorkTaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<WorkTask?> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<WorkTask>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<IEnumerable<WorkTask>> GetUserTasksAsync(int userId)
    {
        return await _taskRepository.GetByUserIdAsync(userId);
    }

    public async Task<WorkTask> CreateTaskAsync(string title, string description, int assignedToUserId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required");

        var task = new WorkTask
        {
            Title = title,
            Description = description,
            AssignedToUserId = assignedToUserId,
            Status = "Pending"
        };

        return await _taskRepository.AddAsync(task);
    }

    public async Task<WorkTask> UpdateTaskAsync(int id, string? title, string? description, string? status)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task is null)
            throw new KeyNotFoundException($"Task with id {id} not found");

        if (!string.IsNullOrWhiteSpace(title))
            task.Title = title;
        if (!string.IsNullOrWhiteSpace(description))
            task.Description = description;
        if (!string.IsNullOrWhiteSpace(status))
            task.Status = status;

        return await _taskRepository.UpdateAsync(task);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _taskRepository.DeleteAsync(id);
    }
}