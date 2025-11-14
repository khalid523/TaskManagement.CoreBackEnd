using TaskManagement.Core.Entities;
using WorkTask = TaskManagement.Core.Entities.WorkTask;

namespace TaskManagement.Core.Interfaces;

public interface IWorkTaskRepository
{
    Task<WorkTask?> GetByIdAsync(int id);
    Task<IEnumerable<WorkTask>> GetAllAsync();
    Task<IEnumerable<WorkTask>> GetByUserIdAsync(int userId);
    Task<WorkTask> AddAsync(WorkTask task);
    Task<WorkTask> UpdateAsync(WorkTask task);
    Task<bool> DeleteAsync(int id);
}