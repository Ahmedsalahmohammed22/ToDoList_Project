using AutoMapper;
using ToDoList.Models;
using ToDoList.TasKDTOs;

namespace ToDoList.Configurations
{
    public class TaskMapperConfig:Profile
    {
        public TaskMapperConfig()
        {
            CreateMap<ToDoTask , TaskDTO>()
                .AfterMap((src , dest) =>
                {
                    dest.Status = src.Status.ToString();
                    dest.Priority = src.Priority.ToString();
                }).ReverseMap();
        }
    }
}
