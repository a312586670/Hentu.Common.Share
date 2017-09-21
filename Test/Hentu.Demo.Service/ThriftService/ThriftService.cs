using Hentu.Demo.Service.Thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hentu.Demo.Service
{
    public class ThriftService : AsyncHentuDemoService.Iface_server
    {
        public void DemoUser_Add(DemoUser user, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public void DemoUser_GetList(Action<List<DemoUser>> callback)
        {
            throw new NotImplementedException();
        }
    }
}
