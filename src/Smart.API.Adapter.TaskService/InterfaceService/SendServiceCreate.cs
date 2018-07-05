using System.Configuration;

namespace Smart.API.Adapter.TaskService.InterfaceService
{
    public class SendServiceCreate
    {
        public static ISendService Instance
        {
            get
            {
                string cfgSendServiceName = ConfigurationManager.AppSettings["SendServiceName"];
                if (string.IsNullOrWhiteSpace(cfgSendServiceName))
                {
                    cfgSendServiceName = "";
                }
                switch (cfgSendServiceName)
                {
                    default:
                        {
                            return new SendService();
                        }
                }
            }
        }
    }
}
