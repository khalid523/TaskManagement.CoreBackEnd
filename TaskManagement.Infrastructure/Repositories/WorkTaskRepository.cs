using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class WorkTaskRepository : IWorkTaskRepository
{
    private readonly ApplicationDbContext _context;

    public WorkTaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkTask?> GetByIdAsync(int id)
    {
        return await _context.WorkTasks.Include(t => t.AssignedToUser).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<WorkTask>> GetAllAsync()
    {
        return await _context.WorkTasks.Include(t => t.AssignedToUser).ToListAsync();
    }

    public async Task<IEnumerable<WorkTask>> GetByUserIdAsync(int userId)
    {
        return await _context.WorkTasks
            .Include(t => t.AssignedToUser)
            .Where(t => t.AssignedToUserId == userId)
            .ToListAsync();
    }

    public async Task<WorkTask> AddAsync(WorkTask task)
    {
        _context.WorkTasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<WorkTask> UpdateAsync(WorkTask task)
    {
        task.UpdatedAt = DateTime.UtcNow;
        _context.WorkTasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.WorkTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return false;

        _context.WorkTasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}