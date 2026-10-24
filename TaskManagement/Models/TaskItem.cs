using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    [Table("TaskItems")] // <--- Thêm dòng này để chỉ định đúng tên bảng trong SQL Server
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string Priority { get; set; } = "Medium";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}