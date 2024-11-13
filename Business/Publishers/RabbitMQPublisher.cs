using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using MongoDB.Bson.IO;

namespace Business.Publishers
{
    public class RabbitMQPublisher<T> : IRabbitMQPublisher<T>
    {
        private readonly RabbitMQSetting _rabbitMqSetting;

        public RabbitMQPublisher(IOptions<RabbitMQSetting> rabbitMqSetting)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
        }

        public async Task PublishMessageAsync(T message, string queueName)
        {

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = "Order Recieved"u8.ToArray();


            await Task.Run(() => channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body));
        }

    }
}
