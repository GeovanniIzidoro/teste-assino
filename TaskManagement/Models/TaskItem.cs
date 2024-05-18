namespace TaskManagement.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SLA { get; set; } // SLA in hours
        public string FilePath { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
