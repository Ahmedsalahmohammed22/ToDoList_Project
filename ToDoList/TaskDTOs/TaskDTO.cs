using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.TasKDTOs
{
    public class TaskDTO
    {

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; }
        public string Priority { get; set; }
    }
}
