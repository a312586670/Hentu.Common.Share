using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data
{
    public enum OrderEnum
    {
        Asc,
        Desc
    }

    public class Operator
    {
        public static Operator Eq = new Operator("=");
        public static Operator Neq = new Operator("<>");
        public static Operator Gt = new Operator(">");
        public static Operator GtOrEq = new Operator(">=");
        public static Operator Lt = new Operator("<");
        public static Operator LtOrEq = new Operator("<=");
        public static Operator IsNull = new Operator(" IS NULL ");
        public static Operator IsNotNull = new Operator(" IS NOT NULL ");
        public static Operator In = new Operator(" IN ");
        public static Operator NotIn = new Operator(" NOT IN ");
        public static Operator Between = new Operator(" BETWEEN  ");
        public static Operator NotBetween = new Operator(" NOT BETWEEN  ");

        public static Operator Like = new Operator(" LIKE ");


        public Operator(string literal)
        {
            _literal = literal;
        }

        protected string _literal;
        public override string ToString()
        {
            return _literal;
        }
    }

    public class LogicOperator : Operator
    {

        public static LogicOperator Or = new LogicOperator(" OR ");
        public static LogicOperator And = new LogicOperator(" AND ");

        public LogicOperator(string literal) : base(literal) { }

    }

    public interface IExpression
    {
        string Expression { get; }
    }

    public interface ITableExpression : IExpression
    {
        string Name { get; set; }
    }


    public interface ISqlExpression : IExpression
    {

    }

    public interface IInsertExpression : ISqlExpression
    {
        ITableExpression Table { get; set; }

        IPropertyExpression[] Properties { get; set; }

        ISetableValueExpression[] Values { get; set; }
    }

    public interface IDeleteExpression : ISqlExpression
    {
        ITableExpression Table { get; set; }

        IWhereExpression Where { get; set; }

    }

    public interface IUpdateExpression : ISqlExpression
    {
        ITableExpression Table { get; set; }

        ISetExpression Set { get; set; }

        IWhereExpression Where { get; set; }
    }

    public interface ISelectExpression : ISqlExpression
    {
        ITableExpression[] Tables { get; set; }

        ISelectItemExpression[] Selects { get; set; }

        IWhereExpression Where { get; set; }

        IOrderByExpression OrderBy { get; set; }

        ILimitExpression Limit { get; set; }
    }

    public interface ICustomerSqlExpression : ISqlExpression { }

    public interface ISetExpression : IExpression
    {
        ISetItemExpression[] Sets { get; set; }
    }

    public interface ISetItemExpression : IExpression
    {
        IPropertyExpression Property { get; set; }
        ISetableValueExpression Value { get; set; }
    }

    public interface ISetableValueExpression : IValueExpression { }

    public interface ICustomerSetableValueExpression : ISetableValueExpression { }

    public interface ISelectItemExpression : IExpression { }

    public interface ICustomerSlectItemExpression : ISelectItemExpression { }

    public interface IAsExpression : ISelectItemExpression
    {
        ISelectItemExpression SelectItem { get; set; }
        IAsPropertyExpression AsProperty { get; set; }
    }

    public interface IAsPropertyExpression : IExpression { }

    public interface IOrderExpression : IExpression
    {
        IPropertyExpression Property { get; set; }
        OrderEnum Order { get; set; }
    }

    public interface IOrderByExpression : IExpression
    {
        IOrderExpression[] Orders { get; set; }
    }

    public interface ILimitExpression : IExpression
    {
        int Offset { get; set; }
        int Count { get; set; }
    }
    
    public interface IWhereExpression : IExpression
    {
        IFilterExpression Filter { get; set; }
    }
    
    public interface IFilterExpression : IExpression { }

    public interface IBinarayExpression : IFilterExpression
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        IExpression A { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        Operator Op { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        IExpression B { get; set; }
    }

    public interface ILogicExpression : IBinarayExpression
    {
        LogicOperator LogicOp { get; set; }
    }

    public interface ICustomerFilterExpression : IFilterExpression { }

    public interface IPropertyExpression : IExpression, ISelectItemExpression
    {
        string Name { get; set; }

        ITableExpression Table { get; set; }
    }

    public interface IValueExpression : IExpression
    {
        object Value { get; set; }
    }

    public interface ILiteralValueExpression : ISetableValueExpression, ISelectItemExpression
    {

    }

    public interface IParamExpression : ISetableValueExpression { }

    public interface ICollectionExpression : IValueExpression
    {
        ILiteralValueExpression[] Values { get; set; }
    }

    public interface ICustomerValueExpression : IValueExpression { }

    public interface ICustomerExpression : IExpression, ICustomerSqlExpression , ICustomerSlectItemExpression, ICustomerValueExpression, ICustomerSetableValueExpression, ICustomerFilterExpression{ }


    public class TableExpression : ITableExpression
    {
        public TableExpression(string name)
        {
            Name = name;
        }

        public string Expression
        {
            get;
            protected set;
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                Expression = string.Format("`{0}`", _name);
            }
        }
    }

    public class PropertyExpression : IPropertyExpression
    {
        public PropertyExpression(string name, ITableExpression table = null)
        {
            _name = name;
            _table = table;
            GenExpression();
        }

        public string Expression
        {
            get;
            protected set;
        }

        private string _name = null;
        private ITableExpression _table = null;

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                GenExpression();
            }
        }

        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format("{1}`{0}`", Name, Table != null ? Table.Expression + "." : string.Empty);
        }
    }

    public class LiteralValueExpression : ILiteralValueExpression
    {
        public LiteralValueExpression(object value)
        {
            Value = value;
        }

        private string expression = null;
        public string Expression
        {
            get
            {
                return expression;
            }
        }

        private object _value = null;

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                var valueType = _value?.GetType();
                if (valueType == typeof(string))
                {
                    expression = "'" + _value.ToString() + "'";
                }
                else if (valueType == typeof(DateTime))
                {
                    expression = string.Format("'{0:yyyy-MM-dd HH:mm:ss}'", _value);
                }
                else if(valueType.IsEnum)
                {
                    expression = Convert.ToInt32(value).ToString();
                }
                else
                {
                    expression = Value?.ToString();
                }
            }
        }
    }

    public class ParamExpression : IParamExpression
    {
        public ParamExpression(string param)
        {
            Value = param;
        }

        private string expression = null;
        public string Expression
        {
            get
            {
                return expression;
            }
        }

        private string _value = null;
        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value as string;
                expression = "@" + _value;
            }
        }
    }

    public class CollectionExpression : ICollectionExpression
    {
        public CollectionExpression(params ILiteralValueExpression[] values)
        {
            Values = values;
        }

        private ILiteralValueExpression[] _values = null;

        public string Expression
        {
            get;
            protected set;
        }

        public object Value
        {
            get
            {
                return _values;
            }

            set
            {
                SetCollection(value as ILiteralValueExpression[]);
            }
        }

        public ILiteralValueExpression[] Values
        {
            get
            {
                return _values;
            }

            set
            {
                SetCollection(value);
            }
        }

        protected void SetCollection(ILiteralValueExpression[] values)
        {
            _values = values;
            Expression = string.Format("({0})", _values.Join(",", exp => exp.Expression));
        }
    }

    public class BetweenValueExpression : IValueExpression
    {
        public BetweenValueExpression(IValueExpression min, IValueExpression max)
        {
            Value = new IValueExpression[] { min, max };
        }

        private string expression = null;
        public string Expression
        {
            get
            {
                return expression;
            }
        }

        private IValueExpression[] _value;
        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value as IValueExpression[];
                expression = string.Format("{0} AND {1}", _value[0].Expression, _value[1].Expression);
            }
        }
    }

    internal class NoValueExpression : IValueExpression
    {
        public string Expression
        {
            get
            {
                return string.Empty;
            }
        }

        public object Value
        {
            get
            {
                return null;
            }

            set
            {
            }
        }
    }


    public class WhereExpression : IWhereExpression
    {
        public WhereExpression(IFilterExpression filter)
        {
            _filter = filter;
            GenExpression();
        }

        private IFilterExpression _filter = null;

        public string Expression
        {
            get;
            protected set;
        }

        public IFilterExpression Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                _filter = value;
                GenExpression();
            }
        }

        protected void GenExpression()
        {
            Expression = _filter == null ? "" : string.Format(" WHERE {0} ", _filter.Expression);
        }
    }

    public class BinarayExpression : IBinarayExpression
    {

        public BinarayExpression(IExpression a, Operator op, IExpression b)
        {
            _a = a;
            _op = op;
            _b = b;
            GenExpression();
        }

        private IExpression _a = null;
        /// <summary>
        /// 属性名称
        /// </summary>
        public IExpression A { get { return _a; } set { _a = value; GenExpression(); } }

        private Operator _op = null;
        /// <summary>
        /// 操作符
        /// </summary>
        public Operator Op { get { return _op; } set { _op = value; GenExpression(); } }

        private IExpression _b = null;
        /// <summary>
        /// 值
        /// </summary>
        public IExpression B { get { return _b; } set { _b = value; GenExpression(); } }

        public virtual string Expression
        {
            get;
            protected set;
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format("{0} {1} {2}", A?.Expression, Op.ToString(), B?.Expression);
        }
    }

    public class LogicExpression : BinarayExpression, ILogicExpression
    {
        public LogicExpression(IExpression a, LogicOperator op, IExpression b) : base(a, op, b)
        {
        }

        public LogicOperator LogicOp
        {
            get
            {
                return base.Op as LogicOperator;
            }

            set
            {
                base.Op = value;
            }
        }
    }

    public class SetItemExpression : ISetItemExpression
    {
        public SetItemExpression(IPropertyExpression property, ISetableValueExpression value)
        {
            _property = property;
            _value = value;
            GenExpression();
        }

        public SetItemExpression(IPropertyExpression property)
            :this(property, new ParamExpression(property.Name))
        {
        }

        private IPropertyExpression _property = null;
        private ISetableValueExpression _value = null;

        public string Expression
        {
            get;
            protected set;
        }

        public IPropertyExpression Property
        {
            get
            {
                return _property;
            }

            set
            {
                _property = value;
                GenExpression();
            }
        }

        public ISetableValueExpression Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                GenExpression();
            }
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format("{0}={1}", Property?.Expression, Value?.Expression);
        }
    }

    public class SetExpression : ISetExpression
    {
        public SetExpression(ISetItemExpression[] sets)
        {
            _sets = sets;
            GenExpression();
        }

        public string Expression
        {
            get;
            protected set;
        }

        private ISetItemExpression[] _sets = null;
        public ISetItemExpression[] Sets
        {
            get
            {
                return _sets;
            }

            set
            {
                _sets = value;
                GenExpression();
            }
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format(" SET {0} ", _sets?.Join(",", set => set.Expression));
        }
    }

    public class AsExpression : IAsExpression
    {
        public AsExpression(ISelectItemExpression selectItem, IAsPropertyExpression asProperty)
        {
            _selectItem = selectItem;
            _asProperty = asProperty;
            GenExpression();
        }

        private ISelectItemExpression _selectItem = null;
        private IAsPropertyExpression _asProperty = null;

        public string Expression
        {
            get;
            protected set;
        }

        public ISelectItemExpression SelectItem
        {
            get
            {
                return _selectItem;
            }

            set
            {
                _selectItem = value;
                GenExpression();
            }
        }

        public IAsPropertyExpression AsProperty
        {
            get
            {
                return _asProperty;
            }

            set
            {
                _asProperty = value;
                GenExpression();
            }
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format(" {0} as {1} ", SelectItem?.Expression, AsProperty?.Expression);
        }
    }

    public class AsPropertyExpression : IAsPropertyExpression
    {
        public AsPropertyExpression(string value)
        {
            Expression = string.Format("`{0}`", value);
        }

        public string Expression
        {
            get;
            protected set;
        }
    }

    public class SelectAllItemExpression : ISelectItemExpression
    {
        public SelectAllItemExpression(ITableExpression table)
        {
            _table = table;
            GenExpression();
        }

        private ITableExpression _table = null;

        public ITableExpression Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;

            }
        }

        public string Expression
        {
            get;
            protected set;
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format(" {0}* ", Table == null ? string.Empty : Table.Expression + ".");
        }
    }

    public class OrderExpression : IOrderExpression
    {
        public OrderExpression(string property, OrderEnum order = OrderEnum.Asc)
        {
            this.property = new PropertyExpression(property);
            this.order = order;
            GenExpression();
        }
        public OrderExpression(IPropertyExpression property, OrderEnum order = OrderEnum.Asc)
        {
            this.property = property;
            this.order = order;
            GenExpression();
        }

        private string expression = null;
        public string Expression
        {
            get
            {
                return expression;
            }
        }

        private OrderEnum order = OrderEnum.Asc;
        private IPropertyExpression property = null;
        public OrderEnum Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
                GenExpression();
            }
        }

        public IPropertyExpression Property
        {
            get
            {
                return property;
            }
            set
            {
                property = value;
                GenExpression();
            }
        }

        protected virtual void GenExpression()
        {
            expression = string.Format("{0} {1}", Property?.Expression, Order == OrderEnum.Asc ? "ASC" : "DESC");
        }
    }

    public class OrderByExpression : IOrderByExpression
    {
        public OrderByExpression(params IOrderExpression[] orders)
        {
            Orders = orders;
        }

        private string expression = null;
        public string Expression
        {
            get
            {
                return expression;
            }
        }

        private IOrderExpression[] orders;
        public IOrderExpression[] Orders
        {
            get
            {
                return orders;
            }

            set
            {
                orders = value;
                if (orders?.Length > 0)
                {
                    expression = string.Format("ORDER BY {0}", orders.Join(",", order => order.Expression));
                }
                else
                {
                    expression = string.Empty;
                }
            }
        }
    }

    public class InsertExpression : IInsertExpression
    {
        public InsertExpression(ITableExpression table, IPropertyExpression[] properties, ISetableValueExpression[] values)
        {
            _table = table;
            _properties = properties;
            _values = values;
            GenExpression();
        }

        public string Expression
        {
            get;
            protected set;
        }

        private ITableExpression _table = null;
        private IPropertyExpression[] _properties = null;
        private ISetableValueExpression[] _values = null;


        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }

        public IPropertyExpression[] Properties
        {
            get
            {
                return _properties;
            }

            set
            {
                _properties = value;
                GenExpression();
            }
        }

        public ISetableValueExpression[] Values
        {
            get
            {
                return _values;
            }

            set
            {
                _values = value;
                GenExpression();
            }
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format("INSERT INTO {0}({1}) VALUES({2})", Table?.Expression, Properties?.Join(",", p => p.Expression), Values?.Join(",", v => v.Expression));
        }
    }

    public class DeleteExpression : IDeleteExpression
    {
        public DeleteExpression(ITableExpression table, IWhereExpression where)
        {
            _table = table;
            _where = where;
            GenExpression();
        }

        private ITableExpression _table = null;
        private IWhereExpression _where = null;

        public string Expression
        {
            get;
            protected set;
        }

        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }

        public IWhereExpression Where
        {
            get
            {
                return _where;
            }

            set
            {
                _where = value;
                GenExpression();
            }
        }

        protected virtual void GenExpression()
        {
            Expression = string.Format("DELETE FROM {0} {1}", Table?.Expression, Where?.Expression);
        }
    }

    public class UpdateExpression : IUpdateExpression
    {
        public UpdateExpression(ITableExpression table, ISetExpression set, IWhereExpression where)
        {
            _table = table;
            _set = set;
            _where = where;
            GenExpression();
        }

        private ITableExpression _table = null;
        private ISetExpression _set = null;
        private IWhereExpression _where = null;

        public string Expression
        {
            get;
            protected set;
        }

        public ITableExpression Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                GenExpression();
            }
        }
        public ISetExpression Set
        {
            get
            {
                return _set;
            }

            set
            {
                _set = value;
                GenExpression();
            }
        }

        public IWhereExpression Where
        {
            get
            {
                return _where;
            }

            set
            {
                _where = value;
                GenExpression();
            }
        }

        private void GenExpression()
        {
            Expression = string.Format("UPDATE {0} {1} {2}", Table?.Expression, Set?.Expression, Where?.Expression);
        }
    }

    public class SelectExpression : ISelectExpression
    {
        public SelectExpression(ITableExpression[] tables, ISelectItemExpression[] selects, IWhereExpression where, IOrderByExpression orderby = null, ILimitExpression limit=null)
        {
            _tables = tables;
            _selects = selects;
            _where = where;
            _orderby = orderby;
            _limit = limit;
            GenExpression();
        }

        private ITableExpression[] _tables = null;
        private ISelectItemExpression[] _selects = null;
        private IWhereExpression _where = null;
        private IOrderByExpression _orderby = null;
        private ILimitExpression _limit = null;

        public string Expression
        {
            get;
            protected set;
        }


        public ITableExpression[] Tables
        {
            get
            {
                return _tables;
            }

            set
            {
                _tables = value;
                GenExpression();
            }
        }

        public ISelectItemExpression[] Selects
        {
            get
            {
                return _selects;
            }

            set
            {
                _selects = value;
                GenExpression();
            }
        }

        public IWhereExpression Where
        {
            get
            {
                return _where;
            }

            set
            {
                _where = value;
                GenExpression();
            }
        }

        public IOrderByExpression OrderBy
        {
            get
            {
                return _orderby;
            }

            set
            {
                _orderby = value;
                GenExpression();
            }
        }

        public ILimitExpression Limit
        {
            get
            {
                return _limit;
            }

            set
            {
                _limit = value;
                GenExpression();
            }
        }

        private void GenExpression()
        {
            Expression = string.Format("SELECT {1} FROM {0} {2} {3} {4}", Tables?.Join(",", t => t.Expression), Selects?.Join(",", s => s.Expression), Where?.Expression, OrderBy?.Expression, Limit?.Expression);
        }
    }

    public class LimitExpression : ILimitExpression
    {
        public LimitExpression(int offset, int count)
        {
            _offset = offset;
            _count = count;
            GenExpression();
        }

        private int _offset = 0;
        private int _count = 1;

        public string Expression
        {
            get;
            protected set;
        }
        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value > 0 ? value : 1;
                GenExpression();
            }
        }

        public int Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value > 0 ? value : 0;
                GenExpression();
            }
        }
        
        protected virtual void GenExpression()
        {
            Expression = string.Format("limit {0},{1}", Offset, Count);
        }
    }

    public class CustomerExpression : ICustomerExpression
    {
        public CustomerExpression(string expression)
        {
            (this as ICustomerValueExpression).Value = expression;
        }

        public string Expression
        {
            get;
            private set;
        }

        private object _value;
        object IValueExpression.Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Expression = _value?.ToString();
            }
        }
    }
}