using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Smart.API.Adapter.Common;
using Smart.API.Adapter.Models;
using System;
using System.Collections.Generic;

namespace Smart.API.Adapter.ActiveMQ
{
    public class ActiveMQReciver
    {
        /// <summary>
        /// 订阅消息委托
        /// </summary>
        /// <param name="message"></param>
        public delegate void ActiveMQReciveMsg(ActiveMQInfo message);

        /// <summary>
        /// 订阅消息事件
        /// </summary>
        public event ActiveMQReciveMsg OnActiveMQReciveMsg;


        private static IConnectionFactory _IConnectionFactory;

        private IConnection connection;

        public ActiveMQReciver()
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

        public void StartReciveQueueMsg(Dictionary<string, string> DicQueueFilter)
        {
            if (DicQueueFilter == null || DicQueueFilter.Count <= 0)
            {
                return;
            }

            //开始连接.用户名密码              
            IConnection connection = CreateConnection();
            //如果你要持久“订阅”，则需要设置ClientId，这样程序运行当中被停止，恢复运行时，能拿到没接收到的消息！   
            connection.ClientId = "SmartAdapterReciveQueue";
            // 启动连接  
            connection.Start();
            // 创建一个session会话  
            ISession session = connection.CreateSession();
            foreach (string queueName in DicQueueFilter.Keys)
            {
                //Create the Consumer  
                string filter = DicQueueFilter[queueName];
                IMessageConsumer consumer;
                if (string.IsNullOrWhiteSpace(filter))
                {
                    consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName));
                }
                else
                {
                    consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName), "filter = '" + filter + "'");
                }
                //接收数据            
                consumer.Listener += new MessageListener(consumer_Listener);
            }
        }

        /// <summary>
        /// 持久化订阅主题消息
        /// </summary>
        /// <param name="DicTopicFilter"></param>
        public void StartReciveTopicMsg(Dictionary<string, string> DicTopicFilter)
        {
            if (DicTopicFilter == null || DicTopicFilter.Count <= 0)
            {
                return;
            }

            //开始连接.用户名密码              
            connection = CreateConnection();
            //如果你要持久“订阅”，则需要设置ClientId，这样程序运行当中被停止，恢复运行时，能拿到没接收到的消息！   
            connection.ClientId = "SmartAdapterReciveTopic";
            // 启动连接  
            connection.Start();
            // 创建一个session会话  
            ISession session = connection.CreateSession();
            foreach (string topicName in DicTopicFilter.Keys)
            {
                //Create the Consumer  
                string filter = DicTopicFilter[topicName];
                IMessageConsumer consumer;
                if (string.IsNullOrWhiteSpace(filter))
                {
                    consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topicName), "Topic_" + topicName, null, true);
                }
                else
                {
                    consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topicName), "Topic_" + topicName, "filter = '" + filter + "'", true);
                }

                //接收数据            
                consumer.Listener += new MessageListener(consumer_Listener);
            }
        }

        /// <summary>
        /// 接收消息的订阅事件
        /// </summary>
        /// <param name="message"></param>
        private void consumer_Listener(IMessage message)
        {
            if (OnActiveMQReciveMsg != null)
            {
                ActiveMQInfo info = new ActiveMQInfo();
                if (message.NMSDestination.IsTemporary)
                {
                    info.Name = ((Apache.NMS.ActiveMQ.Commands.ActiveMQTempDestination)message.NMSDestination).PhysicalName;
                }
                else
                {
                    info.Name = ((Apache.NMS.ActiveMQ.Commands.ActiveMQDestination)message.NMSDestination).PhysicalName;
                }
                info.IsQueue = message.NMSDestination.IsQueue;
                info.IsTopic = message.NMSDestination.IsTopic;
                info.IsTemporary = message.NMSDestination.IsTemporary;
                info.MessageId = message.NMSMessageId;
                info.MQType = (enumActiveMQDestinationType)((int)message.NMSDestination.DestinationType);
                info.Priority = (enumActiveMQPriority)((int)message.NMSPriority);
                info.Timestamp = message.NMSTimestamp;
                info.Type = message.NMSType;
                info.Message = ((ITextMessage)message).Text;
                OnActiveMQReciveMsg(info);
            }
        }


        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        private IConnection CreateConnection()
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

        /// <summary>
        /// 断开连接
        /// </summary>

        public void StopConnection()
        {
            if (connection != null)
            {
                connection.Stop();
                connection.Close();
                connection.Dispose();
            }

            OnActiveMQReciveMsg = null;
        }
    }
}
