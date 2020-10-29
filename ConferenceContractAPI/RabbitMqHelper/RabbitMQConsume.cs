using ConferenceContractAPI.RabbitServices;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConferenceContractAPI.API.RabbitMqHelper
{
    public class RabbitMQConsume : IHostedService, IDisposable
    {
        public PCQueueService PCQueueService = new PCQueueService();

        public PCPaidAmountQueueService PCPaidAmountQueueService = new PCPaidAmountQueueService();

        public static IConnection connection;

        public static IModel channel;

        public RabbitMQConsume(IConnection rabbitConnection)
        {
            connection = rabbitConnection;
            channel = connection.CreateModel();
        }

        private static void Run(byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            PCQueueService.Consume("pcf_contractNumber_queue", channel);
            PCPaidAmountQueueService.Consume("ex_totalAmountByPerContractNumber_queue", channel);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            channel?.Close();
            channel?.Dispose();

            connection?.Close();
            connection?.Dispose();
        }
    }
}
