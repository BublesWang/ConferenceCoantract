using ConferenceContractAPI.Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceContractAPI.RabbitMqHelper
{
    public class RabbitMQPublisher
    {
        //private string EXCHANGE_NAME = "_Exchange";
        //private string ROUTING_KEY = "_Route";
        private string Uri = "amqp://guest:guest@rabbitmq:5672";

        public bool Publish(string prefix, string queue_name, string message)
        {
            bool result = false;
            string EXCHANGE_NAME = prefix + "_Exchange";
            string ROUTING_KEY = prefix + "_Route";

            var factory = new ConnectionFactory
            {
                //factory.Uri = "amqp://user:pass@hostName:port/vhost";
                Uri = new Uri(Uri),
                AutomaticRecoveryEnabled = true
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    try
                    {
                        //创建一个type = "direct" 、持久化的、非自动删除的交换器
                        channel.ExchangeDeclare(EXCHANGE_NAME, "direct", true, false, null);
                        //创建一个持久的、非排他的、非自动删除的队列
                        channel.QueueDeclare(queue_name, true, false, false, null);
                        //将交换器与队列通过路由键绑定            
                        channel.QueueBind(queue_name, EXCHANGE_NAME, ROUTING_KEY, null);

                        channel.ConfirmSelect();
                        //04.创建消息并发送
                        var body = Encoding.UTF8.GetBytes(message);
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        channel.BasicPublish(EXCHANGE_NAME, ROUTING_KEY, properties, body);
                        //等待发布成功并返回发布状态
                        var isOk = channel.WaitForConfirms();
                        if (!isOk)
                        {
                            LogHelper.Error(this, "The message is not reached to the server!");
                        }
                        result = isOk;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(this, ex);
                    }

                    return result;
                }
            }
        }
    }
}
