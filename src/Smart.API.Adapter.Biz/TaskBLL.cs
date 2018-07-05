﻿using Smart.API.Adapter.DataAccess.Task;
using Smart.API.Adapter.Models;
using System;

namespace Smart.API.Adapter.Biz
{
    public class TaskBLL
    {
        public TaskQueueEntity GetInterfaceTask()
        {
            return TaskQueueDAL.ProxyInstance.GetTaskWithLock(2);
        }

        public int ReTaskExecution(int ReSecond)
        {
            return TaskQueueDAL.ProxyInstance.ReTaskExecution(ReSecond);
        }

        public int InsertTask(string Content, int taskType)
        {
            return TaskQueueDAL.ProxyInstance.InsertTask(Content, taskType);
        }

        public bool SetTaskCompleted(int taskId, bool successful)
        {
            try
            {
                if (TaskQueueDAL.ProxyInstance.SetTaskCompleted(taskId,successful))
                {
                    if (successful) 
                    {
                        TaskQueueDAL.ProxyInstance.SetTaskArchived(taskId);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
