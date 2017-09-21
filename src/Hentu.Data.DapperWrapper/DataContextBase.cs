using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hentu.Data.DapperWrapper
{
    public class DataContextBase : IDisposable
    {
        private DbBase _dbbase = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useMaster">查询时是否从主库中获取数据</param>
        public DataContextBase(bool useMaster, string dbPrefix = "")
        {
            _dbbase = useMaster ? DbBaseFactory.StartNewMaster(dbPrefix) : DbBaseFactory.StartNewSlaver(dbPrefix);
        }

        public DataContextBase(DbBase db)
        {
            _dbbase = db;
        }

        public DbBase Db
        {
            get { return this._dbbase; }
        }

        protected TResult Excute<TResult>(Func<TResult> func, bool close = false)
        {
            try
            {
                return func();
            }
            finally
            {
                if (close)
                {
                    this._dbbase.DbConnecttion.Close();
                }
            }
        }

        protected void Excute(Action action, bool close = false)
        {
            try
            {
                action();
            }
            finally
            {
                if (close)
                {
                    this._dbbase.DbConnecttion.Close();
                }
            }
        }
        
        public void Dispose()
        {
            if (_dbbase == null)
                return;
            _dbbase.Dispose();
        }

    }
}
