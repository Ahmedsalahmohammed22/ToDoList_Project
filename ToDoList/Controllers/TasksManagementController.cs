using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Models;
using ToDoList.Repository;
using ToDoList.TasKDTOs;
using ToDoList.UnitOfWorks;
using TaskStatus = ToDoList.Models.TaskStatus;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksManagementController : ControllerBase
    {
        UnitOfWork _unit;
        IMapper _mapper;
        public TasksManagementController(UnitOfWork unit , IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        [HttpGet("tasks")]
        [SwaggerOperation(Summary = "Retrieve all tasks", Description = "Retrieves a list of all tasks in the database.")]
        [SwaggerResponse(200, "Successfully retrieved tasks", typeof(List<TaskDTO>))]
        [SwaggerResponse(404, "No tasks found")]
        public async Task<IActionResult> GetAllTasks()
        {
            List<ToDoTask> tasks = await _unit.TaskRepository.GetAll();
            if(!tasks.Any()) return NotFound();
            var tasksDTO = _mapper.Map<List<TaskDTO>>(tasks);
            return Ok(tasksDTO);
        }
        [HttpGet("tasks/{id}")]
        [SwaggerOperation(Summary = "Retrieve task by ID", Description = "Fetches a task using the task ID.")]
        [SwaggerResponse(200, "Successfully retrieved task", typeof(TaskDTO))]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> GetTask(int id)
        {
            ToDoTask task = await _unit.TaskRepository.Get(id);
            if (task == null) return NotFound();
            var taskDTO = _mapper.Map<ToDoTask>(task);
            return Ok(taskDTO);
        }
        [HttpPost("CreateTask")]
        [SwaggerOperation(Summary = "Create a new task", Description = "Creates a task using the task data.")]
        [SwaggerResponse(201, "Task successfully created", typeof(TaskDTO))]
        [SwaggerResponse(400, "Invalid task data")]
        public async Task<IActionResult> CreateTask(TaskDTO taskDTO)
        {
            if (taskDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            ToDoTask task = _mapper.Map<ToDoTask>(taskDTO);
            await _unit.TaskRepository.Add(task);
            await _unit.Save();
            return CreatedAtAction("GetTask", new { id = task.Id }, taskDTO);
        }
        [HttpPut("EditTask/{id}")]
        [SwaggerOperation(Summary = "Update an existing task", Description = "Updates an existing task with the new details.")]
        [SwaggerResponse(200, "Task successfully updated", typeof(TaskDTO))]
        [SwaggerResponse(400, "Invalid task ID or task data")]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> EditTask(int id , TaskDTO taskDTO)
        {
            if(id <= 0) return BadRequest("Invalid task ID. ");
            ToDoTask _task = await _unit.TaskRepository.Get(id);
            if(_task == null) return NotFound($"Task with ID {id} not found.");
            _mapper.Map(taskDTO, _task);
            _unit.TaskRepository.Update(_task);
            await _unit.Save();
            var updateTaskDTO = _mapper.Map<TaskDTO>(_task);
            return Ok(updateTaskDTO);
        }
        [HttpDelete("DeleteTask/{id}")]
        [SwaggerOperation(Summary = "Delete a task", Description = "Deletes a task using the task ID.")]
        [SwaggerResponse(200, "Task successfully deleted")]
        [SwaggerResponse(400, "Invalid task ID")]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (id <= 0) return BadRequest("Invalid task ID.");
            ToDoTask task = await _unit.TaskRepository.Get(id);
            if (task == null) return NotFound($"Task with ID {id} not found.");
            await _unit.TaskRepository.Delete(id);
            await _unit.Save();
            List<ToDoTask> tasks = await _unit.TaskRepository.GetAll();
            var tasksDTO = _mapper.Map<List<TaskDTO>>(tasks);
            return Ok(tasksDTO);
        }

        [HttpPut("{id}/Complete")]
        [SwaggerOperation(Summary = "Mark task as completed", Description = "Marks a task as completed using the specified task ID.")]
        [SwaggerResponse(200, "Successfully marked the task as completed", typeof(TaskDTO))]
        [SwaggerResponse(400, "Invalid task ID")]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> MarkTaskAsCompleted(int id)
        {
            if (id <= 0) return BadRequest("Invalid task ID.");
            ToDoTask task = await _unit.TaskRepository.Get(id);
            if (task == null) return NotFound($"Task with ID {id} not found.");
            task.Status = TaskStatus.Completed;
            _unit.TaskRepository.Update(task);
            await _unit.Save();
            var updateTaskDTO = _mapper.Map<TaskDTO>(task);
            return Ok(updateTaskDTO);
        }
        [SwaggerOperation(Summary = "Mark task as incomplete", Description = "Marks a task as incomplete using the specified task ID.")]
        [SwaggerResponse(200, "Successfully marked the task as incomplete", typeof(TaskDTO))]
        [SwaggerResponse(400, "Invalid task ID")]
        [SwaggerResponse(404, "Task not found")]
        [HttpPut("{id}/InComplete")]
        public async Task<IActionResult> MarkTaskAsInCompleted(int id)
        {
            if (id <= 0) return BadRequest("Invalid task ID.");
            ToDoTask task = await _unit.TaskRepository.Get(id);
            if (task == null) return NotFound($"Task with ID {id} not found.");
            task.Status = TaskStatus.InCompleted;
            _unit.TaskRepository.Update(task);
            await _unit.Save();
            var updateTaskDTO = _mapper.Map<TaskDTO>(task);
            return Ok(updateTaskDTO);
        }
        [HttpPut("{id}/Priority")]
        [SwaggerOperation(Summary = "Update task priority", Description = "Updates the priority of the task with the specified ID.")]
        [SwaggerResponse(200, "Successfully updated the task priority", typeof(TaskDTO))]
        [SwaggerResponse(400, "Invalid task ID or priority")]
        [SwaggerResponse(404, "Task not found")]
        public async Task<IActionResult> UpdateTaskPriority(int id , [FromQuery]TaskPriority priority)
        {
            if (id <= 0) return BadRequest("Invalid task ID.");
            ToDoTask task = await _unit.TaskRepository.Get(id);
            if (task == null) return NotFound($"Task with ID {id} not found.");
            task.Priority = priority;
            _unit.TaskRepository.Update(task);
            await _unit.Save();
            var updateTaskDTO = _mapper.Map<TaskDTO>(task);
            return Ok(updateTaskDTO);
        }



    }
}
