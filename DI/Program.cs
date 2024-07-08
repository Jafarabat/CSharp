using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddTransient<NotoficationService>();
services.AddTransient<IMessageService, EmailService>();
services.AddTransient<ILogger, ConsoleLogger>();
services.AddTransient<IDatabase, PostgresDatabase>();

var serviceProvider=services.BuildServiceProvider();

var notoficationService = serviceProvider.GetRequiredService<NotoficationService>();

notoficationService.Notify();

Console.ReadLine();
class NotoficationService(IMessageService messageService)
{
    private readonly IMessageService _messageService = messageService;

    public void Notify()
    {
        _messageService.SendMessage("Сообщение");
    }

    public void NotifyAll()
    {
        _messageService.SendMessage("Сообщение");
    }
}

interface IMessageService
{
    void SendMessage(string message);
}

class EmailService(ILogger logger, IDatabase database) : IMessageService
{

    private readonly ILogger _logger = logger;
    private readonly IDatabase _database = database;

    public void SendMessage(string message)
    {
        Console.WriteLine("Email - " + message);

        _logger.Log("Сообщение успешно отправлено");

        _database.Save("Сообщение успешно отправлено");
    }
}

class TelegramService : IMessageService
{
    public void SendMessage(string message)
    {
        Console.WriteLine("Telegram - " + message);
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}


public class PostgresDatabase(ILogger logger) : IDatabase
{

    private readonly ILogger _logger = logger;

    public void Save(string message)
    {
        Console.WriteLine("Сообщение сохранено");
        _logger.Log("Операция прошла успешно");
    }
}