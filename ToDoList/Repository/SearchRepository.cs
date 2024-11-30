using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.TasKDTOs;
using TaskStatus = ToDoList.Models.TaskStatus;

namespace ToDoList.Repository
{
    public class SearchRepository
    {
        ToDoListContext _context;
        public SearchRepository(ToDoListContext context)
        {
            _context = context;
        }
        public async Task<List<ToDoTask>> GetTasksByCompleteStatus(bool completed)
        {

            return await _context.Tasks.Where(t => t.Status == (completed ? TaskStatus.Completed : TaskStatus.InCompleted)).ToListAsync();
        }
        public async Task<List<ToDoTask>> GetTasksByDueDateAsync(DateTime due_date)
        {

            return await _context.Tasks.Where(t => t.DueDate == due_date).ToListAsync();
        }
        public async Task<List<ToDoTask>> GetTasksByPriorityAsync(TaskPriority taskPriority)
        {

            return await _context.Tasks.Where(t => t.Priority == taskPriority).ToListAsync();
        }


    }
}
