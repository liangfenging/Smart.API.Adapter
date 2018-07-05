using Smart.API.Adapter.Models;

namespace Smart.API.Adapter.TaskService.InterfaceService
{
    public interface ISendService
    {


        /// <summary>
        /// 即时推送
        /// </summary>
        /// <param name="task"></param>
        void Send(TaskQueueEntity task);
    }
}
