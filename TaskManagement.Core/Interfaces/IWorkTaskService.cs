using WorkTask = TaskManagement.Core.Entities.WorkTask;

namespace TaskManagement.Core.Interfaces;

public interface IWorkTaskService
{
    Task<WorkTask?> GetTaskByIdAsync(int id);
    Task<IEnumerable<WorkTask>> GetAllTasksAsync();
    Task<IEnumerable<WorkTask>> GetUserTasksAsync(int userId);
    Task<WorkTask> CreateTaskAsync(string title, string description, int assignedToUserId);
    Task<WorkTask> UpdateTaskAsync(int id, string? title, string? description, string? status);
    Task<bool> DeleteTaskAsync(int id);
}