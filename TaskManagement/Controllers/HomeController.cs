using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly TaskDbContext _context;

        public HomeController(TaskDbContext context)
        {
            _context = context;
        }

        // 1. TRANG CHỦ (Home Dashboard)
        public async Task<IActionResult> Index(int? projectId)
        {
            var projects = await _context.Projects.ToListAsync();

            // Nếu không chọn ProjectId, mặc định lấy Project đầu tiên
            int selectedId = projectId ?? (projects.FirstOrDefault()?.ProjectId ?? 1);

            var tasks = await _context.TaskItems
                .Where(t => t.ProjectId == selectedId)
                .ToListAsync();

            ViewBag.ListProjects = projects;
            ViewBag.ListTasks = tasks;
            ViewBag.SelectedProjectId = selectedId;

            // Quote ngẫu nhiên
            ViewBag.DailyQuote = "Hành trình ngàn dặm bắt đầu từ một bước chân nhỏ.";
            ViewBag.DailyAuthor = "Lão Tử";

            return View();
        }

        // 2. TÍNH NĂNG NHANH CHO TASK
        [HttpPost]
        public async Task<IActionResult> AddTask(string title, string priority, int projectId)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var newTask = new TaskItem
                {
                    Title = title,
                    Priority = priority,
                    Status = "Pending",
                    ProjectId = projectId,
                    CreatedAt = DateTime.Now
                };
                _context.TaskItems.Add(newTask);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { projectId });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                task.Status = task.Status == "Completed" ? "Pending" : "Completed";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(int id, string title, string priority)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                task.Title = title;
                task.Priority = priority;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null)
            {
                int pId = task.ProjectId;
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = pId });
            }
            return RedirectToAction(nameof(Index));
        }

        // 3. CÁC TRANG TRÊN SIDEBAR (Projects, Analytics, Settings)

        // Trang Quản lý Dự án (Icon Thư mục)
        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects.Include(p => p.Tasks).ToListAsync();
            return View(projects);
        }

        // Trang Thống kê (Icon Biểu đồ)
        public async Task<IActionResult> Analytics()
        {
            var totalTasks = await _context.TaskItems.CountAsync();
            var completedTasks = await _context.TaskItems.CountAsync(t => t.Status == "Completed");
            var pendingTasks = totalTasks - completedTasks;

            ViewBag.TotalTasks = totalTasks;
            ViewBag.CompletedTasks = completedTasks;
            ViewBag.PendingTasks = pendingTasks;
            ViewBag.CompletionRate = totalTasks > 0 ? (completedTasks * 100) / totalTasks : 0;

            return View();
        }

        // Trang Cài đặt (Icon Bánh răng)
        public IActionResult Settings()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}