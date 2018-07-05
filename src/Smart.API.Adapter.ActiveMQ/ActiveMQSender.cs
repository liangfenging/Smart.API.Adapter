using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Smart.API.Adapter.Common;
using System;

namespace Smart.API.Adapter.ActiveMQ
{
    public class ActiveMQSender
    {
        private static IConnectionFactory _IConnectionFactory;

        /// <summary>
        /// 初始化ActiveMQ服务
        /// </summary>       
        static ActiveMQSender()
        {
            try
            {
                if (_IConnectionFactory == null)
                {
                    _IConnectionFactory = new ConnectionFactory("failover:(" + CommonSettings.ActiveMQUrl + ")?timeout=3000");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("初始化ActiveMQ服务端错误,", ex);
            }
        }

        /// <summary>
        /// 发送消息到队列
        /// </summary>
        /// <param name="tcpUrl">ActiveMQ服务地址</param>
        /// <param name="message">发送的消息内容</param>
        /// <param name="queueName">队列名</param>
        /// <param name="filter">过滤关键字</param>
        public static void SendQueueMessage(string message, string queueName = null, string filter = null)
        {
            //建立连接
            using (IConnection connection = CreateConnection())
            {
                //创建Session会话
                using (ISession session = connection.CreateSession())
                {
                    if (string.IsNullOrWhiteSpace(queueName))
                    {
                        queueName = CommonSettings.ActiveMQQueueOrTopic;
                    }
                    //创建MQ的Queue
                    IMessageProducer prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName));
                    //创建一个发送消息的对象
                    ITextMessage IMessage = prod.CreateTextMessage();
                    IMessage.Text = message; //给这个消息对象赋实际的消息
                    //设置消息对象的过滤关键字属性
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        IMessage.Properties.SetString("filter", filter);
                    }
                    //NonPersistent 非持久化消息
                    prod.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    prod.Priority = MsgPriority.Normal;
                    prod.TimeToLive = TimeSpan.MinValue;
                    prod.Send(IMessage);
                }
            }
        }

        /// <summary>
        /// 发送消息到主题
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="topicName">主题名</param>
        /// <param name="bPersistent">是否持久化消息</param>
        public static void SendTopicMessage(string message, string topicName = null,string filter = null,bool bPersistent = false)
        {
            //建立连接
            using (IConnection connection = CreateConnection())
            {
                //创建Session会话
                using (ISession session = connection.CreateSession())
                {
                    if (string.IsNullOrWhiteSpace(topicName))
                    {
                        topicName = CommonSettings.ActiveMQQueueOrTopic;
                    }
                    //创建MQ的Queue
                    IMessageProducer prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topicName));
                    //创建一个发送消息的对象
                    ITextMessage IMessage = prod.CreateTextMessage();
                    IMessage.Text = message; //给这个消息对象赋实际的消息
                    //设置消息对象的过滤关键字属性
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        IMessage.Properties.SetString("filter", filter);
                    }
                    prod.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    //NonPersistent 非持久化消息
                    if (bPersistent)
                    {
                        prod.DeliveryMode = MsgDeliveryMode.Persistent;
                    }
                    
                    prod.Priority = MsgPriority.Normal;
                    prod.TimeToLive = TimeSpan.MinValue;
                    prod.Send(IMessage);
                }
            }
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        private static IConnection CreateConnection()
        {
            IConnection connection;
            if (!string.IsNullOrEmpty(CommonSettings.ActiveMQName))
            {
                connection = _IConnectionFactory.CreateConnection(CommonSettings.ActiveMQName, CommonSettings.ActiveMQPassword);
            }
            else
            {
                connection = _IConnectionFactory.CreateConnection();
            }
            return connection;
        }
    }
}
