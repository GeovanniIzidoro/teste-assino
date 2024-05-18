using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManagement.Data;
using TaskManagement.Services; // Certifique-se de incluir o namespace correto

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaskContext>(opt =>
    opt.UseInMemoryDatabase("TaskList"));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagement", Version = "v1" });
});
builder.Services.AddHostedService<SlaNotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagement v1"));
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
