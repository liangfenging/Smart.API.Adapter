using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Smart.API.Adapter.Common;
using Smart.API.Adapter.Biz;
using Smart.API.Adapter.Models;
using Smart.API.Adapter.TaskService.InterfaceService;

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
