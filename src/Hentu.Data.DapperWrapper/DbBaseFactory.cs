using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hentu.Data.DapperWrapper
{
    /// <summary>
    /// dataContext factory
    /// </summary>
    static public class DbBaseFactory
    {
        #region Private Members
        
        static ConcurrentDictionary<string, Indicator> connDict = new ConcurrentDictionary<string, Indicator>();

        #endregion

        #region Constructors

        /// <summary>
        /// static new
        /// </summary>
        static DbBaseFactory()
        {
            foreach (ConnectionStringSettings child in ConfigurationManager.ConnectionStrings)
            {

                var index = child.Name.IndexOf('.');
                var dbstr = index < 0 ? string.Empty : child.Name.Substring(0,index);
                var tuple = connDict.GetOrAdd(dbstr, k => new Indicator { Master = null, Pos = 0, Secondary = new List<string>() });
                var flstr = index < 0 || index == child.Name.Length - 1 ? child.Name : child.Name.Substring(index+1);
                if (flstr == "master")
                {
                    tuple.Master = child.Name;
                }
                else if (flstr.StartsWith("secondary"))
                {
                    tuple.Secondary.Add(child.Name);
                }
            }

            if (connDict.Values.FirstOrDefault(var => var.Master == null) != null)
            {
                throw new ArgumentException("未能找到主库设置.");
            }

        }
        #endregion

        #region Static Methods
        /// <summary>
        /// start new dataContext for master
        /// </summary>
        /// <returns></returns>
        static public DbBase StartNewMaster(string prefix = "")
        {
            Indicator tuple = null;
            if (!connDict.TryGetValue(prefix, out tuple))
            {
                throw new KeyNotFoundException(string.Format("不存在前缀为'{0}'的连接字符串。", prefix));
            }

            return new DbBase(tuple.Master);
        }

        /// <summary>
        /// start new dataContext for secondary
        /// </summary>
        /// <returns></returns>
        static public DbBase StartNewSlaver(string prefix = "")
        {
            Indicator tuple = null;
            if (!connDict.TryGetValue(prefix, out tuple))
            {
                throw new KeyNotFoundException(string.Format("不存在前缀为'{0}'的连接字符串。", prefix));
            }

            if (tuple.Secondary.Count == 0)
                return new DbBase(tuple.Master);

            return new DbBase(tuple.Secondary[(int)(Interlocked.Increment(ref tuple.Pos) % tuple.Secondary.Count)]);
        }
        #endregion
        
        #region Helper class

        public class Indicator
        {
            public string Master { get; set; }
            public List<string> Secondary { get; set; }
            public long Pos;
        }

        #endregion
    }
}
