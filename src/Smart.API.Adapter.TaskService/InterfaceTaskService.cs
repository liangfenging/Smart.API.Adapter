using System.ServiceProcess;

namespace Smart.API.Adapter.TaskService
{
    partial class InterfaceTaskService : ServiceBase
    {
        public InterfaceTaskService()
        {
            InitializeComponent();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {

        }
    }
}
