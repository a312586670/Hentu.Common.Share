using System;
using Thrift.Server;

namespace Hentu.Demo.Service
{
    public class MainService
    {
        public MainService()
        {
            System.Threading.ThreadPool.SetMinThreads(30, 30);
            AppDomain.CurrentDomain.UnhandledException += (obj, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null)
                {
                    //LogHelper.LogError(ex.Message, ex);
                }
            };
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (obj, e) =>
            {
                e.SetObserved();
                e.Exception.Flatten().Handle(c =>
                {
                    //LogHelper.LogError(c.Message, c);
                    return true;
                });
            };
        }
        public void Start(string serviceName)
        {
            ThriftServerManager.Init();
            ThriftServerManager.Start();

            // LogHelper.LogInfo($"{serviceName}服务启动");
        }

        public void Stop(string serviceName)
        {
            ThriftServerManager.Stop();

            // LogHelper.LogInfo($"{serviceName}服务停止");
        }
        public void ShutDown(string serviceName)
        {
            ThriftServerManager.Stop();

            // LogHelper.LogInfo($"{serviceName}服务停止");
        }
    }
}
