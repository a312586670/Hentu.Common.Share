using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Client;
namespace Hentu.Demo.Service.ThriftTest
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var proxy = ThriftClientManager.GetClient<Hentu.Demo.Service.Thrift.AsyncHentuDemoService.Iface_client>("HentuDemoService");
				var result = proxy.DemoUser_Add(null).Result;
				System.Console.Write(result);
				
			}
			catch (Exception ex) {
				System.Console.Write(ex);
			}
			System.Console.ReadKey();
		}
	}
}
