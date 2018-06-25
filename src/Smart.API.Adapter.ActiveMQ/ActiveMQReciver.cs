using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Smart.API.Adapter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.API.Adapter.ActiveMQ
{
    public class ActiveMQReciver
    {
        /// <summary>
        /// 订阅消息委托
        /// </summary>
        /// <param name="message"></param>
        public delegate void ActiveMQReciveMsg(string message);

        /// <summary>
        /// 订阅消息事件
        /// </summary>
        public event ActiveMQReciveMsg OnActiveMQReciveMsg;


        private static IConnectionFactory _IConnectionFactory;
        public ActiveMQReciver()
        {
            try
            {
                if (_IConnectionFactory == null)
                {
                    _IConnectionFactory = new ConnectionFactory(CommonSettings.ActiveMQUrl);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("初始化ActiveMQ服务端错误,", ex);
            }
        }

        public void StartReciveMsg(string queueName = null, string filter = null)
        {
            //开始连接.用户名密码              
            IConnection connection = CreateConnection();
            //如果你要持久“订阅”，则需要设置ClientId，这样程序运行当中被停止，恢复运行时，能拿到没接收到的消息！   
            connection.ClientId = "SmartAPIAdapter";
            // 启动连接  
            connection.Start();
            // 创建一个session会话  
            ISession session = connection.CreateSession();
            if (string.IsNullOrWhiteSpace(queueName))
            {
                queueName = CommonSettings.ActiveMQQueue;
            }
            //Create the Consumer  
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

        /// <summary>
        /// 接收消息的订阅事件
        /// </summary>
        /// <param name="message"></param>
        private void consumer_Listener(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            if (OnActiveMQReciveMsg != null)
            {
                OnActiveMQReciveMsg(msg.Text);
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
