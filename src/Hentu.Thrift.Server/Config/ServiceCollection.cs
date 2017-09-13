﻿using System;
using System.Configuration;

namespace Hentu.Thrift.Server.Config
{
    /// <summary>
    /// thrift service collection
    /// </summary>
    [ConfigurationCollection(typeof(ServiceConfig), AddItemName = "service")]
    public class ServiceCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// create new element
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfig();
        }
        /// <summary>
        /// 获取指定元素的Key。
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ServiceConfig).Name;
        }
        /// <summary>
        /// 获取指定位置的对象。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public ServiceConfig this[int i]
        {
            get { return BaseGet(i) as ServiceConfig; }
        }
        /// <summary>
        /// 获取指定key的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ServiceConfig Get(string key)
        {
            return BaseGet(key) as ServiceConfig;
        }
    }
}