using ConferenceContractAPI.Common;
using ConferenceContractAPI.ConferenceContractService;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceContractAPI.RabbitServices
{
    public class ConferenceContractQueueService
    {

        ConferenceContractImpService _servicesImpl = new ConferenceContractImpService();
        public void Consume(string queueName, IModel channel)
        {
            try
            {
                channel.QueueDeclare(queueName, true, false, false);

                //事件基本消费者
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                //接收到消息事件
                consumer.Received += (ch, ea) =>
                {
                    //取到数据并操作数据
                    try
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        //拿到个人合同号修改PersonContract表内IsCommitAbstract字段值
                        var count = UpdatePayedPriceByContractId(message);
                        if (count > -1)
                        {
                            //确认该消息已被消费
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        else
                        {
                            channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, false);
                        LogHelper.Error(this, ex);
                    }
                };
                //启动消费者 设置为手动应答消息
                channel.BasicConsume(queueName, false, consumer);
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }
        }

        private int UpdatePayedPriceByContractId(string message)
        {
            var count = 0;
            try
            {
                var model = JsonConvert.DeserializeObject<ConferenceContractQueueVM>(message);

                if (model!=null)
                {
                    count = _servicesImpl.UpdatePaiedAndStatusByContractNumber(model);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(this, ex);
            }
            return count;
        }
    }

    public class ConferenceContractQueueVM
    {
        public string ContractNumber { get; set; }

        public string PaymentStatusCode { get; set; }

        public string TotalPaid { get; set; }

        public string TotalPaidUnit { get; set; }
    }
}
