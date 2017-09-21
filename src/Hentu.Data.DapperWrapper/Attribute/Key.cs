using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data.DapperWrapper.Attribute
{
	/// <summary>
	/// 实体属性和数据库字段映射自定义特性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class Key:System.Attribute
	{
		/// <summary>
		/// 实体类对应的表的字段名称
		/// </summary>
		public string Name
		{
			set;get;
		}
	}
}
