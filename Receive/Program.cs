using System.Net.Mail;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "order", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    int dots = message.Split('.').Length - 1;
    await Task.Delay(dots * 1000);
    var email = "istiaqsamiul@gmail.com";
    var password = "";
    var host = "";
    var port = 123;

    var smtpClient = new SmtpClient(host, port);
    smtpClient.EnableSsl = true;
    smtpClient.UseDefaultCredentials = false;
    var msg = new MailMessage(email, "", "No subject", message);
    await smtpClient.SendMailAsync(msg);
    Console.WriteLine(" [x] Done");
    //return Task.CompletedTask;
};

await channel.BasicConsumeAsync("order", autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();