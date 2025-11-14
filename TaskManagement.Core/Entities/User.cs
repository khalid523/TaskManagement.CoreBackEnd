
namespace TaskManagement.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "Admin" or "User"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
    }
}
