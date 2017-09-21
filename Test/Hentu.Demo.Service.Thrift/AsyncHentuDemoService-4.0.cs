using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Util;

namespace Hentu.Demo.Service.Thrift
{
    /// <summary>
    /// 异步HentuDemoService
    /// </summary>
    public class AsyncHentuDemoService
    {
		﻿		public interface Iface_client
        {
            ﻿            /// <summary>
/// Demo类 用户 添加
/// @param user 用户
/// </summary>
/// <param name="user"></param>
			Task<System.Boolean> DemoUser_Add(Hentu.Demo.Service.Thrift.DemoUser user, object asyncState = null);
﻿            /// <summary>
/// Demo类 获取批量用户
/// </summary>
			Task<System.Collections.Generic.List<Hentu.Demo.Service.Thrift.DemoUser>> DemoUser_GetList(object asyncState = null);

        }
﻿        /// <summary>
        /// 异步客户端实现
        /// </summary>
        public class Client : Iface_client
        {
            #region Private Members
            /// <summary>
            /// thrift client
            /// </summary>
            private IThriftClient client_ = null;
            #endregion

            #region Constructors
            /// <summary>
            /// new
            /// </summary>
            /// <param name="client"></param>
            /// <exception cref="ArgumentNullException">client is null</exception>
            public Client(IThriftClient client)
            {
                if (client == null)
                    throw new ArgumentNullException("client");

                this.client_ = client;
            }
            #endregion

            #region Iface_client Members
            ﻿            public Task<System.Boolean> DemoUser_Add(Hentu.Demo.Service.Thrift.DemoUser user, object asyncState = null)
            {
                var taskSource_ = new TaskCompletionSource<System.Boolean>(asyncState);

                //构建请求发送buffer
                int seqID_ = this.client_.NextRequestSeqID();
                var sendBuffer_ = ThriftMarshaller.Serialize(new TMessage("DemoUser_Add", TMessageType.Call, seqID_),
                    new HentuDemoService.DemoUser_Add_args()
                    {
                        User = user
                    });

                //开始异步发送
                this.client_.Send("Hentu.Demo.Service.Thrift.HentuDemoService+Iface", "DemoUser_Add", seqID_, sendBuffer_, (ex_) =>
                {
                    //处理异常回调
                    taskSource_.SetException(ex_);
                },
                (payload_) =>
                {
                    if (payload_ == null || payload_.Length == 0)
                    {
                        taskSource_.SetException(new TApplicationException(
                            TApplicationException.ExceptionType.MissingResult, "DemoUser_Add failed: Did not receive any data."));
                        return;
                    }

                    TMessage recvMsg_;
                    TApplicationException exServer_ = null;
                    HentuDemoService.DemoUser_Add_result result_ = null;

                    var oproto_ = ThriftMarshaller.GetBinaryProtocol(payload_);
                    try
                    {
                        //read TMessage
                        recvMsg_ = oproto_.ReadMessageBegin();
                        //read server return exception
                        if (recvMsg_.Type == TMessageType.Exception)
                            exServer_ = TApplicationException.Read(oproto_);
                        else
                        {
                            //read result
                            result_ = new HentuDemoService.DemoUser_Add_result();
                            result_.Read(oproto_);
                        }
                    }
                    catch (System.Exception ex_)
                    {
                        oproto_.Transport.Close();
                        taskSource_.SetException(ex_);
                        return;
                    }
                    oproto_.Transport.Close();

                    if (exServer_ != null)
                        taskSource_.SetException(exServer_);
                    else
                    {
                        if (result_.__isset.success)
                        {
                            taskSource_.SetResult(result_.Success);
                            return;
                        }
						
                        taskSource_.SetException(new TApplicationException(
                            TApplicationException.ExceptionType.MissingResult, "DemoUser_Add failed: unknown result"));
                    }
                });

                return taskSource_.Task;
            }
﻿            public Task<System.Collections.Generic.List<Hentu.Demo.Service.Thrift.DemoUser>> DemoUser_GetList(object asyncState = null)
            {
                var taskSource_ = new TaskCompletionSource<System.Collections.Generic.List<Hentu.Demo.Service.Thrift.DemoUser>>(asyncState);

                //构建请求发送buffer
                int seqID_ = this.client_.NextRequestSeqID();
                var sendBuffer_ = ThriftMarshaller.Serialize(new TMessage("DemoUser_GetList", TMessageType.Call, seqID_),
                    new HentuDemoService.DemoUser_GetList_args()
                    {
                        
                    });

                //开始异步发送
                this.client_.Send("Hentu.Demo.Service.Thrift.HentuDemoService+Iface", "DemoUser_GetList", seqID_, sendBuffer_, (ex_) =>
                {
                    //处理异常回调
                    taskSource_.SetException(ex_);
                },
                (payload_) =>
                {
                    if (payload_ == null || payload_.Length == 0)
                    {
                        taskSource_.SetException(new TApplicationException(
                            TApplicationException.ExceptionType.MissingResult, "DemoUser_GetList failed: Did not receive any data."));
                        return;
                    }

                    TMessage recvMsg_;
                    TApplicationException exServer_ = null;
                    HentuDemoService.DemoUser_GetList_result result_ = null;

                    var oproto_ = ThriftMarshaller.GetBinaryProtocol(payload_);
                    try
                    {
                        //read TMessage
                        recvMsg_ = oproto_.ReadMessageBegin();
                        //read server return exception
                        if (recvMsg_.Type == TMessageType.Exception)
                            exServer_ = TApplicationException.Read(oproto_);
                        else
                        {
                            //read result
                            result_ = new HentuDemoService.DemoUser_GetList_result();
                            result_.Read(oproto_);
                        }
                    }
                    catch (System.Exception ex_)
                    {
                        oproto_.Transport.Close();
                        taskSource_.SetException(ex_);
                        return;
                    }
                    oproto_.Transport.Close();

                    if (exServer_ != null)
                        taskSource_.SetException(exServer_);
                    else
                    {
                        if (result_.__isset.success)
                        {
                            taskSource_.SetResult(result_.Success);
                            return;
                        }
						
                        taskSource_.SetException(new TApplicationException(
                            TApplicationException.ExceptionType.MissingResult, "DemoUser_GetList failed: unknown result"));
                    }
                });

                return taskSource_.Task;
            }

            #endregion
        }

﻿        /// <summary>
        /// service interface for server processor
        /// </summary>
		public interface Iface_server
        {
            ﻿            void DemoUser_Add(Hentu.Demo.Service.Thrift.DemoUser user, Action<System.Boolean> callback);
﻿            void DemoUser_GetList(Action<System.Collections.Generic.List<Hentu.Demo.Service.Thrift.DemoUser>> callback);

        }
﻿        /// <summary>
        /// 异步Processor
        /// </summary>
        public class Processor : IAsyncProcessor
        {
            #region Delegates
            /// <summary>
            /// process handle delegate
            /// </summary>
            /// <param name="message"></param>
            /// <param name="iproto"></param>
            /// <param name="callback"></param>
            private delegate void ProcessHandle(TMessage message, TProtocol iproto, Action<byte[]> callback);
            #endregion

            #region Private Members
            /// <summary>
            /// service实现对象
            /// </summary>
            private Iface_server _face = null;
            /// <summary>
            /// process handle dic
            /// </summary>
            private Dictionary<string, ProcessHandle> processMap_ =
                 new Dictionary<string, ProcessHandle>();
            #endregion

            #region Constructors
            /// <summary>
            /// new
            /// </summary>
            /// <param name="face"></param>
            /// <exception cref="ArgumentNullException">face is null</exception>
            public Processor(Iface_server face)
            {
                if (face == null)
                    throw new ArgumentNullException("face");

                this._face = face;
				processMap_["DemoUser_Add"]=DemoUser_Add_Process;
processMap_["DemoUser_GetList"]=DemoUser_GetList_Process;

            }
            #endregion

            #region IAsyncProcessor Members
            /// <summary>
            /// process
            /// </summary>
            /// <param name="payload"></param>
            /// <param name="callback"></param>
            public void Process(byte[] payload, Action<byte[]> callback)
            {
                var iproto = ThriftMarshaller.GetBinaryProtocol(payload);

                TMessage message;
                try
                {
                    message = iproto.ReadMessageBegin();
                }
                catch (System.Exception)
                {
                    iproto.Transport.Close();
                    return;
                }

                ProcessHandle handle = null;
                if (this.processMap_.TryGetValue(message.Name, out handle))
                {
                    handle(message, iproto, callback);
                }
                else
                {
                    iproto.Transport.Close();
                    callback(ThriftMarshaller.Serialize(new TMessage(message.Name, TMessageType.Exception, message.SeqID),
                        new TApplicationException(TApplicationException.ExceptionType.UnknownMethod,
                            string.Concat("Invalid method name: '", message.Name, "'"))));
                }
            }
            #endregion

            #region Private Methods
			﻿            private void DemoUser_Add_Process(TMessage message, TProtocol iproto, Action<byte[]> callback)
            {
                var args = new HentuDemoService.DemoUser_Add_args();
                try
                {
                    args.Read(iproto);
                }
                catch (System.Exception ex)
                {
                    iproto.Transport.Close();
                    callback(ThriftMarshaller.Serialize(new TMessage(message.Name, TMessageType.Exception, message.SeqID),
                        new TApplicationException(TApplicationException.ExceptionType.Unknown, ex.Message)));
                    return;
                }
                iproto.Transport.Close();

                int seqID = message.SeqID;
                try
                {
                    this._face.DemoUser_Add(args.User, (result) =>
                    {
                        callback(ThriftMarshaller.Serialize(new TMessage("DemoUser_Add", TMessageType.Reply, seqID),
                            new HentuDemoService.DemoUser_Add_result
                            {
                                Success = result
                            }));
                    });
                }
                catch (System.Exception ex)
                {
                    callback(ThriftMarshaller.Serialize(new TMessage(message.Name, TMessageType.Exception, message.SeqID),
                        new TApplicationException(TApplicationException.ExceptionType.Unknown, ex.ToString())));
                }
            }﻿            private void DemoUser_GetList_Process(TMessage message, TProtocol iproto, Action<byte[]> callback)
            {
                var args = new HentuDemoService.DemoUser_GetList_args();
                try
                {
                    args.Read(iproto);
                }
                catch (System.Exception ex)
                {
                    iproto.Transport.Close();
                    callback(ThriftMarshaller.Serialize(new TMessage(message.Name, TMessageType.Exception, message.SeqID),
                        new TApplicationException(TApplicationException.ExceptionType.Unknown, ex.Message)));
                    return;
                }
                iproto.Transport.Close();

                int seqID = message.SeqID;
                try
                {
                    this._face.DemoUser_GetList((result) =>
                    {
                        callback(ThriftMarshaller.Serialize(new TMessage("DemoUser_GetList", TMessageType.Reply, seqID),
                            new HentuDemoService.DemoUser_GetList_result
                            {
                                Success = result
                            }));
                    });
                }
                catch (System.Exception ex)
                {
                    callback(ThriftMarshaller.Serialize(new TMessage(message.Name, TMessageType.Exception, message.SeqID),
                        new TApplicationException(TApplicationException.ExceptionType.Unknown, ex.ToString())));
                }
            }
            #endregion
        }


    }
}