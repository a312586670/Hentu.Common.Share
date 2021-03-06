/**
 * Autogenerated by Thrift Compiler (0.9.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Hentu.Demo.Service.Thrift
{

  /// <summary>
  /// Demo�� �û�
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class DemoUser : TBase
  {
    private int _UserId;
    private long _UserIds;
    private string _UserName;
    private bool _IsValid;
    private List<string> _UserEmails;
    private Dictionary<string, string> _UserAges;
    private EnumUserSex _UserSex;
    private long _RegTime;

    public int UserId
    {
      get
      {
        return _UserId;
      }
      set
      {
        __isset.UserId = true;
        this._UserId = value;
      }
    }

    public long UserIds
    {
      get
      {
        return _UserIds;
      }
      set
      {
        __isset.UserIds = true;
        this._UserIds = value;
      }
    }

    public string UserName
    {
      get
      {
        return _UserName;
      }
      set
      {
        __isset.UserName = true;
        this._UserName = value;
      }
    }

    public bool IsValid
    {
      get
      {
        return _IsValid;
      }
      set
      {
        __isset.IsValid = true;
        this._IsValid = value;
      }
    }

    public List<string> UserEmails
    {
      get
      {
        return _UserEmails;
      }
      set
      {
        __isset.UserEmails = true;
        this._UserEmails = value;
      }
    }

    public Dictionary<string, string> UserAges
    {
      get
      {
        return _UserAges;
      }
      set
      {
        __isset.UserAges = true;
        this._UserAges = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="EnumUserSex"/>
    /// </summary>
    public EnumUserSex UserSex
    {
      get
      {
        return _UserSex;
      }
      set
      {
        __isset.UserSex = true;
        this._UserSex = value;
      }
    }

    public long RegTime
    {
      get
      {
        return _RegTime;
      }
      set
      {
        __isset.RegTime = true;
        this._RegTime = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool UserId;
      public bool UserIds;
      public bool UserName;
      public bool IsValid;
      public bool UserEmails;
      public bool UserAges;
      public bool UserSex;
      public bool RegTime;
    }

    public DemoUser() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I32) {
              UserId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              UserIds = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              UserName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Bool) {
              IsValid = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.List) {
              {
                UserEmails = new List<string>();
                TList _list0 = iprot.ReadListBegin();
                for( int _i1 = 0; _i1 < _list0.Count; ++_i1)
                {
                  string _elem2 = null;
                  _elem2 = iprot.ReadString();
                  UserEmails.Add(_elem2);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Map) {
              {
                UserAges = new Dictionary<string, string>();
                TMap _map3 = iprot.ReadMapBegin();
                for( int _i4 = 0; _i4 < _map3.Count; ++_i4)
                {
                  string _key5;
                  string _val6;
                  _key5 = iprot.ReadString();
                  _val6 = iprot.ReadString();
                  UserAges[_key5] = _val6;
                }
                iprot.ReadMapEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I32) {
              UserSex = (EnumUserSex)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I64) {
              RegTime = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("DemoUser");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.UserId) {
        field.Name = "UserId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(UserId);
        oprot.WriteFieldEnd();
      }
      if (__isset.UserIds) {
        field.Name = "UserIds";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(UserIds);
        oprot.WriteFieldEnd();
      }
      if (UserName != null && __isset.UserName) {
        field.Name = "UserName";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(UserName);
        oprot.WriteFieldEnd();
      }
      if (__isset.IsValid) {
        field.Name = "IsValid";
        field.Type = TType.Bool;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(IsValid);
        oprot.WriteFieldEnd();
      }
      if (UserEmails != null && __isset.UserEmails) {
        field.Name = "UserEmails";
        field.Type = TType.List;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.String, UserEmails.Count));
          foreach (string _iter7 in UserEmails)
          {
            oprot.WriteString(_iter7);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (UserAges != null && __isset.UserAges) {
        field.Name = "UserAges";
        field.Type = TType.Map;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.String, TType.String, UserAges.Count));
          foreach (string _iter8 in UserAges.Keys)
          {
            oprot.WriteString(_iter8);
            oprot.WriteString(UserAges[_iter8]);
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.UserSex) {
        field.Name = "UserSex";
        field.Type = TType.I32;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)UserSex);
        oprot.WriteFieldEnd();
      }
      if (__isset.RegTime) {
        field.Name = "RegTime";
        field.Type = TType.I64;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(RegTime);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("DemoUser(");
      sb.Append("UserId: ");
      sb.Append(UserId);
      sb.Append(",UserIds: ");
      sb.Append(UserIds);
      sb.Append(",UserName: ");
      sb.Append(UserName);
      sb.Append(",IsValid: ");
      sb.Append(IsValid);
      sb.Append(",UserEmails: ");
      sb.Append(UserEmails);
      sb.Append(",UserAges: ");
      sb.Append(UserAges);
      sb.Append(",UserSex: ");
      sb.Append(UserSex);
      sb.Append(",RegTime: ");
      sb.Append(RegTime);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
