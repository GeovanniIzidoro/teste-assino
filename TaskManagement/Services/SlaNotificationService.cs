using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Data;
using TaskManagement.Models;

public class SlaNotificationService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer;

    public SlaNotificationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TaskContext>();
            var now = DateTime.UtcNow;
            var tasks = context.TaskItems.Where(t => !t.IsCompleted && now > t.CreatedAt.AddHours(t.SLA)).ToList();

            foreach (var task in tasks)
            {
                // Enviar notificação
                Console.WriteLine($"O SLA da tarefa {task.Title} Venceu.");
                // Aqui você pode implementar a lógica para enviar um e-mail, mensagem, etc.
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
