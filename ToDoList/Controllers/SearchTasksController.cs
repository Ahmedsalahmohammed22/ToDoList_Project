
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Models;
using ToDoList.Repository;
using ToDoList.TasKDTOs;
using ToDoList.UnitOfWorks;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchTasksController : ControllerBase
    {
        UnitOfWork _unit;
        IMapper _autoMapper;
        public SearchTasksController(UnitOfWork unit , IMapper autoMapper)
        {
            _unit = unit;
            _autoMapper = autoMapper;

        }

        [HttpGet("tasks")]
        [SwaggerOperation(Summary = "Retrieve tasks by their completion status",
                  Description = "Fetches tasks based on whether they are completed or not. The `completed` parameter indicates whether to retrieve completed or incomplete tasks.")]
        [SwaggerResponse(200, "Successfully retrieved tasks", typeof(List<TaskDTO>))]
        [SwaggerResponse(404, "No tasks found for the given completion status")]
        public async Task<IActionResult> GetTasksByCompletionStatus(bool completed)
        {
            List<ToDoTask> tasks = await _unit.SearchRepository.GetTasksByCompleteStatus(completed);
            if(!tasks.Any()) return NotFound();
            var tasksDTO = _autoMapper.Map<List<TaskDTO>>(tasks);
            return Ok(tasksDTO);
        }
        [HttpGet]
        public async Task<IActionResult> GetTasksByDueDate(DateTime due_date)
        {
            List<ToDoTask> tasks = await _unit.SearchRepository.GetTasksByDueDateAsync(due_date);
            if (!tasks.Any()) return NotFound();
            var tasksDTO = _autoMapper.Map<List<TaskDTO>>(tasks);
            return Ok(tasksDTO);
        }
        [HttpGet("tasksByPriority")]
        [SwaggerOperation(Summary = "Retrieve tasks by priority",
                  Description = "Fetches tasks based on their priority level. The `taskPriority` parameter indicates the level of priority to filter tasks.")]
        [SwaggerResponse(200, "Successfully retrieved tasks by priority", typeof(List<TaskDTO>))]
        [SwaggerResponse(404, "No tasks found with the specified priority")]
        public async Task<IActionResult> GetTasksByPriority(TaskPriority taskPriority)
        {
            List<ToDoTask> tasks = await _unit.SearchRepository.GetTasksByPriorityAsync(taskPriority);
            if (!tasks.Any()) return NotFound();
            var tasksDTO = _autoMapper.Map<List<TaskDTO>>(tasks);
            return Ok(tasksDTO);
        }
    }
}
