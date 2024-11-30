
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ToDoList.Configurations;
using ToDoList.Models;
using ToDoList.Repository;
using ToDoList.UnitOfWorks;

namespace ToDoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string txt = "ToDoList";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(op =>
            {
                op.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ToDoList API - V1",
                    Version = "v1",
                    Description = "An API for managing tasks. This API allows users to create, update, and delete tasks, set priorities, and track due dates. Users can also mark tasks as completed and manage their task lists efficiently.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ahmed Salah Mohammed",
                        Email = "ahmedsalahmohammed98@gmail.com"
                    }
                });
            }
  
                );
            builder.Services.AddDbContext<ToDoListContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("ToDoDB")));
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(TaskMapperConfig));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(txt);

            app.MapControllers();

            app.Run();
        }
    }
}
