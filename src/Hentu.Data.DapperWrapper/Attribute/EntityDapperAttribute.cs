using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data.DapperWrapper.Attribute
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class EntityTableAttribute: System.Attribute
	{
		private string tableName;
		/// <summary>
		/// 实体实际对应的表名
		/// </summary>
		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		public string IdentityName {
			set;get;
		}

		public string PrimaryKeyName { set; get; }
	}

	///// <summary>
	///// 自增ID属性
	///// </summary>
	//[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	//public class EntityIdentityAttribute : System.Attribute
	//{
	//}

	///// <summary>
	///// 主键属性
	///// </summary>
	//[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	//public class EntityPrimaryKeyAttribute : System.Attribute
	//{

	//}
}
