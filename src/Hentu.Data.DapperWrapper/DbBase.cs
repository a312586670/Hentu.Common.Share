using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hentu.Data.DapperWrapper
{
    public class DbBase : IDisposable
    {
        private string _providerName = string.Empty;
        private IDbConnection dbConnecttion;
        private DbProviderFactory dbFactory;
        public IDbConnection DbConnecttion
        {
            get
            {
                return dbConnecttion;
            }
        }
        public string ProviderName
        {
            get
            {
                return _providerName;
            }
            private set
            {
                _providerName = value;
            }
        }

        public DbBase(string connectionStringName)
        {
            var connStr = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName))
                _providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            else
                throw new Exception("ConnectionStrings中没有配置提供程序ProviderName！");

            dbFactory = DbProviderFactories.GetFactory(_providerName);
            dbConnecttion = dbFactory.CreateConnection();
            dbConnecttion.ConnectionString = connStr;
            //dbConnecttion.Open();
        }

        public void Dispose()
        {
            if (dbConnecttion == null)
            {
                return;
            }
            dbConnecttion.Close();
            dbConnecttion.Dispose();
        }
    }
}