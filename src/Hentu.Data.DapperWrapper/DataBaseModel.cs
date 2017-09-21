using Hentu.Data.DapperWrapper.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data.DapperWrapper
{
	public class DataBaseModel<T>
	{
		private string TablePrifix { set; get; }

		private string _IdentityKey { set; get; }

		private string _Keys { set; get; }
		public DataBaseModel()
		{
			var customes = typeof(T).GetCustomAttributes(false);
			string _tableName = typeof(T).Name;
			string _identityKey = "Id";
			string _Keys = "Id";
			foreach (var custom in customes)
			{
				var tableAttribute = custom as EntityTableAttribute;
				_tableName = tableAttribute.TableName ?? _tableName;
				_identityKey = tableAttribute.IdentityName ?? _identityKey;
				_Keys = tableAttribute.PrimaryKeyName ?? _Keys;
			}
			this.TablePrifix = _tableName;
			this._IdentityKey = _identityKey;
			this._Keys = _Keys;
		}

		public string TableName()
		{
			return this.TablePrifix;
		}
		/// <summary>
		/// 主键: Id
		/// </summary>
		public List<string> Key()
		{
			if (_Keys.Contains(","))
				return _Keys.Split(',').ToList();
			else
				return new List<string> { _Keys };
		}

		/// <summary>
		/// 自增键: Id
		/// </summary>
		public string IdentityKey()
		{
			return _IdentityKey;
		}

		/// <summary>
		/// 获取除主键之外的其他成员熟悉
		/// </summary>
		public List<string> Member()
		{
			List<string> list = new List<string>();
			var properties = typeof(T).GetProperties();
			foreach (var properity in properties)
			{
				var customes = properity.GetCustomAttributes(false);
				var properityName = properity.Name;
				if (customes.Count() > 0)
				{
					var k = customes.GetValue(0) as Key;
					properityName = k.Name ?? _Keys;
				}
				if (!Key().Contains(properity.Name))
					list.Add(properity.Name);
			}
			return list;
		}

		/// <summary>
		/// 获取所有属性名称
		/// </summary>
		/// <param name="excepts">排除的属性</param>
		public List<string> All(params string[] excepts)
		{
			var all = Member();
			all.Add(Key().FirstOrDefault());
			return all.Except(excepts).ToList();
		}

		public string GetSelectField()
		{
			return string.Join(",", All().ToArray());
		}
	}
}
