using System;

namespace Smart.API.Adapter.Models
{
    public class ActiveMQInfo
    {
        /// <summary>
        /// Topic Name
        /// Or 
        /// Queue Name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string MessageId
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public bool IsQueue { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsTopic { get; set; }

        public enumActiveMQPriority Priority
        {
            get;
            set;
        }

        public enumActiveMQDestinationType MQType
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }
    }


    public enum enumActiveMQPriority
    {
        Lowest = 0,
        VeryLow = 1,
        Low = 2,
        AboveLow = 3,
        BelowNormal = 4,
        Normal = 5,
        AboveNormal = 6,
        High = 7,
        VeryHigh = 8,
        Highest = 9,
    }

    public enum enumActiveMQDestinationType
    {
        Queue = 0,
        Topic = 1,
        TemporaryQueue = 2,
        TemporaryTopic = 3,
    }
}
