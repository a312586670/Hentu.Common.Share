using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data.DapperWrapper
{
	public abstract class DataContextBasic<DataType> : DataContextBasic
		 where DataType : class
	{
		public DataContextBasic(bool isMaster, string dbPrefix = "")
			: base(isMaster, dbPrefix)
		{

		}

		public DataContextBasic(bool isMaster, object splitTableId, int splitLevel = 3, string dbPrefix = "") : base(isMaster, splitTableId, splitLevel, dbPrefix)
		{
		}

		public virtual bool Insert(DataType item)
		{
			if (item == null) return false;

			var memberProperties = GetAllProperties(GetAutoKeyProperty());

			var sql = E.T(GetTableName())
				.Insert(memberProperties)
				.Values(E.Ps(memberProperties).ToArray())
				.Expression;

			using (var db = Db)
			{
				int ret = 0;
				if (!string.IsNullOrWhiteSpace(GetAutoKeyProperty()))
				{
					sql += ";SELECT LAST_INSERT_ID() id";
					var r = db.DbConnecttion.ExecuteScalar(sql, item);
					if (r != null)
					{
						FillAutoKey(item, Convert.ToInt64(r));
						ret = Convert.ToInt32(r);
					}
				}
				else
				{
					ret = db.DbConnecttion.Execute(sql, item);
				}

				if (ret <= 0)
					return false;
				else
					return true;
			}
		}

		public virtual bool Insert(IInsertExpression insert, IDictionary<string, object> paramDic)
		{
			if (paramDic == null) paramDic = new Dictionary<string, object>();

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			return Insert(insert, dyParams);
		}

		public virtual bool Insert(IInsertExpression insert, object paramObj = null)
		{
			if (paramObj == null) paramObj = new object();

			var sql = insert.Expression;

			using (var db = Db)
			{
				var ret = db.DbConnecttion.Execute(sql, paramObj);
				return ret > 0;
			}
		}

		public virtual bool Delete(DataType item)
		{
			if (item == null) return false;

			var keys = GetKeyProperties();

			var delete = E.T(GetTableName())
				.Delete()
				.Where(E.And(keys.Select(k => E.Prop(k).EqVarParam()).ToArray()));
			return Delete(delete, item);
		}

		public virtual bool Delete(IFilterExpression filter, object paramObj = null)
		{
			var delete = E.T(GetTableName())
				.Delete()
				.Where(filter);

			return Delete(delete, paramObj);
		}

		public virtual bool Delete(IFilterExpression filter, IDictionary<string, object> paramDic)
		{
			var delete = E.T(GetTableName())
				.Delete()
				.Where(filter);

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			return Delete(delete, dyParams);
		}

		public virtual bool Delete(IDeleteExpression delete, IDictionary<string, object> paramDic)
		{
			if (paramDic == null) paramDic = new Dictionary<string, object>();

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			return Delete(delete, dyParams);
		}

		public virtual bool Delete(IDeleteExpression delete, object paramObj = null)
		{
			if (paramObj == null) paramObj = new object();

			var sql = delete.Expression;

			using (var db = Db)
			{
				var ret = db.DbConnecttion.Execute(sql, paramObj);
				return ret > 0;
			}
		}

		public virtual bool Update(DataType item, params string[] properties)
		{
			if (item == null) return false;

			var keys = GetKeyProperties();
			if (!(properties?.Length > 0))
			{
				properties = GetAllProperties(GetAutoKeyProperty());
			}

			var update = E.T(GetTableName())
				.Update(properties)
				.Values(E.Ps(properties).ToArray())
				.Where(E.And(keys.Select(k => E.Prop(k).EqVarParam()).ToArray()));

			return Update(update, item);
		}

		public virtual bool Update(IFilterExpression filter, object paramObj, params string[] properties)
		{
			if (!(properties?.Length > 0))
			{
				properties = GetAllProperties(GetAutoKeyProperty());
			}
			var update = E.T(GetTableName())
				.Update(properties)
				.Values(E.Ps(properties).ToArray())
				.Where(filter);

			return Update(update, paramObj);
		}

		public virtual bool Update(IFilterExpression filter, IDictionary<string, object> paramDic, params string[] properties)
		{
			if (!(properties?.Length > 0))
			{
				properties = GetAllProperties(GetAutoKeyProperty());
			}
			var update = E.T(GetTableName())
				.Update(properties)
				.Values(E.Ps(properties).ToArray())
				.Where(filter);

			return Update(update, paramDic);
		}

		public virtual bool Update(IUpdateExpression update, IDictionary<string, object> paramDic)
		{
			if (paramDic == null) paramDic = new Dictionary<string, object>();

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			return Update(update, dyParams);
		}

		public virtual bool Update(IUpdateExpression update, object paramObj = null)
		{
			if (paramObj == null) paramObj = new object();

			var sql = update.Expression;

			using (var db = Db)
			{
				var ret = db.DbConnecttion.Execute(sql, paramObj);
				return ret > 0;
			}
		}

		public virtual bool Exist(params object[] keyVals)
		{
			var keys = GetKeyProperties();

			var filter = E.And(keys.Select(k => E.Prop(k).EqVarParam()).ToArray());

			var dyParams = new DynamicParameters();
			for (var i = 0; i < keys.Length; i++)
			{
				dyParams.Add("@" + keys[i], keyVals[i]);
			}
			return Exist(filter, dyParams);
		}

		public virtual bool Exist(IFilterExpression filter, object paramObj = null)
		{
			if (paramObj == null) paramObj = new object();

			var sql = E.T(GetTableName())
				.Select(E.C("COUNT(*)"))
				.Where(filter)
				.Expression;

			using (var db = Db)
			{
				var ret = db.DbConnecttion.ExecuteScalar(sql, paramObj);
				return (long)ret > 0;
			}
		}

		public virtual bool Exist(IFilterExpression filter, IDictionary<string, object> paramDic)
		{
			if (paramDic == null) paramDic = new Dictionary<string, object>();

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			return Exist(filter, dyParams);
		}

		public virtual DataType Get(params object[] keyVals)
		{
			var keys = GetKeyProperties();
			var allProperties = GetAllProperties();

			var select = E.T(GetTableName())
				.Select(allProperties)
				.Where(E.And(keys.Select(k => E.Prop(k).EqVarParam()).ToArray()));

			var dyParams = new DynamicParameters();
			for (var i = 0; i < keys.Length; i++)
			{
				dyParams.Add("@" + keys[i], keyVals[i]);
			}
			return Get(select, dyParams);
		}

		public virtual DataType Get(IFilterExpression filter, object paramObj = null)
		{
			var allProperties = GetAllProperties();

			var select = E.T(GetTableName())
				.Select(allProperties)
				.Where(filter);

			return Get(select, paramObj);
		}

		public virtual DataType Get(IFilterExpression filter, IDictionary<string, object> paramDic)
		{
			var allProperties = GetAllProperties();

			var select = E.T(GetTableName())
				.Select(allProperties)
				.Where(filter);

			return Get(select, paramDic);
		}

		public virtual DataType Get(ISelectExpression select, IDictionary<string, object> paramDic)
		{
			if (paramDic == null) paramDic = new Dictionary<string, object>();

			var sql = select.Expression;

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			using (var db = Db)
			{
				return db.DbConnecttion.Query<DataType>(sql, dyParams).FirstOrDefault();
			}
		}

		public virtual DataType Get(ISelectExpression select, object paramObj = null)
		{
			if (paramObj == null) paramObj = new object();
			var sql = select.Expression;

			using (var db = Db)
			{
				return db.DbConnecttion.Query<DataType>(sql, paramObj).FirstOrDefault();
			}
		}

		public virtual int GetTotalCount(IFilterExpression filter, IDictionary<string, object> paramDic)
		{
			if (paramDic == null) paramDic = new Dictionary<string, object>();

			var sql = E.T(GetTableName())
				.Select(E.C("COUNT(*)"))
				.Where(filter)
				.Expression;

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			using (var db = Db)
			{
				var ret = (int)(long)db.DbConnecttion.ExecuteScalar(sql, paramDic);
				return ret;
			}
		}

		public virtual int GetTotalCount(IFilterExpression filter, object paramObj = null)
		{
			if (paramObj == null) paramObj = new object();

			var sql = E.T(GetTableName())
				.Select(E.C("COUNT(*)"))
				.Where(filter)
				.Expression;

			using (var db = Db)
			{
				var ret = (int)(long)db.DbConnecttion.ExecuteScalar(sql, paramObj);
				return ret;
			}
		}

		public virtual IEnumerable<DataType> GetList(ISelectExpression select, IDictionary<string, object> paramDic)
		{
			if (paramDic == null) paramDic = new Dictionary<string, object>();

			var sql = select.Expression;

			var dyParams = new DynamicParameters();
			foreach (var c in paramDic)
			{
				dyParams.Add("@" + c.Key, c.Value);
			}
			using (var db = Db)
			{
				return db.DbConnecttion.Query<DataType>(sql, dyParams);
			}
		}

		public virtual IEnumerable<DataType> GetList(ISelectExpression select, object paramObj = null)
		{
			if (paramObj == null) paramObj = new object();

			var sql = select.Expression;

			using (var db = Db)
			{
				return db.DbConnecttion.Query<DataType>(sql, paramObj);
			}
		}

		public virtual IEnumerable<DataType> GetList(IFilterExpression filter, IOrderByExpression orderby, ILimitExpression limit, IDictionary<string, object> paramDic)
		{
			return GetList(E.T(GetTableName())
				.Select(GetAllProperties())
				.Where(filter)
				.OrderBy(orderby)
				.Limit(limit), paramDic);
		}

		public virtual IEnumerable<DataType> GetList(IFilterExpression filter, IOrderByExpression orderby, ILimitExpression limit, object paramObj = null)
		{
			return GetList(E.T(GetTableName())
				.Select(GetAllProperties())
				.Where(filter)
				.OrderBy(orderby)
				.Limit(limit), paramObj);
		}

		protected virtual void FillAutoKey(DataType item, long id)
		{

		}
	}

	public abstract class DataContextBasic : DataContextBase
	{
		#region static

		private static System.Collections.Concurrent.ConcurrentDictionary<string, string> TableNameCache = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();

		/// <summary>
		/// 计算分表哈希码
		/// </summary>
		/// <param name="id"></param>
		/// <param name="length">1：16张表；2：256张表；3,：4096张表</param>
		/// <param name="tableName">表名</param>
		/// <returns></returns>
		public static string HashTable<T>(T id, int length = 3, string tableName = null)
		{
			if (length <= 0)
				throw new ArgumentOutOfRangeException("length", "length必须是大于零的值。");

			var hash = id.ToString().MD5();

			var subfix = hash.Substring(0, length).ToUpper();
			if (string.IsNullOrWhiteSpace(tableName))
			{
				return subfix;
			}
			else
			{
				return string.Format("{0}_{1}", tableName, subfix);
			}
		}

		#endregion


		protected string _tableName = null;
		protected object _splitTableId = null;
		protected int _splitLevel = 3;

		public DataContextBasic(bool isMaster, string dbPrefix = "")
			: base(isMaster, dbPrefix)
		{
		}

		public DataContextBasic(bool isMaster, object splitTableId, int splitLevel = 3, string dbPrefix = "") : this(isMaster, dbPrefix)
		{
			_splitTableId = splitTableId;
			_splitLevel = splitLevel;
		}


		/// <summary>
		/// 分表哈希串
		/// </summary>
		public string TableHashCode { get; protected set; }

		public virtual string GetTableName()

		{
			if (_tableName == null)
			{
				var tablePrefix = GetTablePrefix();
				if (string.IsNullOrEmpty(tablePrefix))
					tablePrefix = GetType().Name.Replace("DataContext", "");
				_tableName = tablePrefix;
				if (_splitTableId != null)
				{
					TableHashCode = HashTable(_splitTableId, _splitLevel);
					_tableName = HashTable(_splitTableId, _splitLevel, _tableName);
				}
				TryCreateTable(_tableName);
			}
			return _tableName;
		}

		/// <summary>
		/// 获取主键
		/// </summary>
		/// <returns></returns>
		protected abstract string[] GetKeyProperties();

		/// <summary>
		/// 获取自增键
		/// </summary>
		/// <returns></returns>
		protected abstract string GetAutoKeyProperty();

		/// <summary>
		/// 获得表的前缀
		/// </summary>
		/// <returns></returns>
		protected abstract string GetTablePrefix();

		/// <summary>
		/// 获取所有字段
		/// </summary>
		/// <returns></returns>
		protected abstract string[] GetAllProperties(params string[] excepts);

		/// <summary>
		/// 创建表
		/// </summary>
		/// <param name="tableName"></param>
		protected virtual void TryCreateTable(string tableName)
		{
			if (!string.IsNullOrEmpty(tableName) && !TableNameCache.ContainsKey(tableName))
			{
				var sql = GenerateCreateSql(tableName);
				if (!string.IsNullOrWhiteSpace(sql))
				{
					using (var db = Db)
					{
						db.DbConnecttion.Execute(sql);
					}
				}

				TableNameCache.TryAdd(tableName, sql);
			}
		}

		/// <summary>
		/// 建表sql
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		protected virtual string GenerateCreateSql(string tableName)
		{
			return string.Empty;
		}

		public virtual bool Delete(params object[] keyVals)
		{
			var keys = GetKeyProperties();
			var dyParams = new DynamicParameters();
			for (var i = 0; i < keys.Length; i++)
			{
				dyParams.Add("@" + keys[i], keyVals[i]);
			}
;
			var sql = E.T(GetTableName()).Delete()
				.Where(E.And(keys.Select(k => E.Prop(k).EqVarParam()).ToArray()))
				.Expression;

			using (var db = Db)
			{
				var ret = db.DbConnecttion.Execute(sql, dyParams);
				return ret > 0;
			}
		}

		public virtual bool Update(IDictionary<string, object> propertyVals, params object[] keyVals)
		{
			var keys = GetKeyProperties();

			var dyParams = new DynamicParameters();
			for (var i = 0; i < keys.Length; i++)
			{
				dyParams.Add("@_" + keys[i], keyVals[i]);
			}
			foreach (var p in propertyVals)
			{
				dyParams.Add("@" + p.Key, p.Value);
			}

			var sql = E.T(GetTableName()).Update(propertyVals.Keys.ToArray())
				.Values(E.Ps(propertyVals.Keys.ToArray()).ToArray())
				.Where(E.And(keys.Select(k => E.Prop(k).EqVarParam("_" + k)).ToArray()))
				.Expression;

			using (var db = Db)
			{
				var ret = db.DbConnecttion.Execute(sql, dyParams);
				return ret > 0;
			}
		}
	}
}
