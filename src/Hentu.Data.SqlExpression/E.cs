using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data
{
    /// <summary>
    /// SqlExpression Entry Class
    /// Sql表达式入口类
    /// </summary>
    public static class E
    {
        #region ShortCut
        
        public static ITableExpression T(string table)
        {
            return Table(table);
        }

        public static IEnumerable<ITableExpression> Ts(params string[] tables)
        {
            return Tables(tables);
        }

        public static IPropertyExpression Prop(string property, string table = null)
        {
            return Property(property, table);
        }

        public static IPropertyExpression Pr(string property, string table = null)
        {
            return Prop(property, table);
        }

        public static IValueExpression Val(object value)
        {
            return Value(value);
        }

        public static IValueExpression V(object value)
        {
            return Val(value);
        }

        public static IParamExpression P(string param)
        {
            return Param(param);
        }

        public static IEnumerable<IParamExpression> Ps(params string[] _params)
        {
            return Params(_params);
        }

        public static ICustomerExpression C(string expression)
        {
            return Customer(expression);
        }

        #endregion

        public static ITableExpression Table(string table)
        {
            return new TableExpression(table);
        }

        public static IEnumerable<ITableExpression> Tables(params string[] tables)
        {
            return tables.Select(t => new TableExpression(t));
        }

        public static IPropertyExpression Property(string property, string table = null)
        {
            return new PropertyExpression(property, string.IsNullOrWhiteSpace(table) ? null : Table(table));
        }

        public static IValueExpression Value(object value)
        {
            return new LiteralValueExpression(value);
        }

        public static IParamExpression Param(string param)
        {
            return new ParamExpression(param);
        }

        public static IEnumerable<IParamExpression> Params(params string[] _params)
        {
            return _params.Select(p => Param(p));
        }

        public static CollectionExpression Collection(params ILiteralValueExpression[] values)
        {
            return new CollectionExpression(values);
        }

        public static CollectionExpression Collection(params object[] values)
        {
            return new CollectionExpression(values.Select(v => new LiteralValueExpression(v)).ToArray());
        }

        public static BetweenValueExpression Between(ILiteralValueExpression min, ILiteralValueExpression max)
        {
            return new BetweenValueExpression(min, max);
        }

        public static BetweenValueExpression Between(object min, object max)
        {
            return new BetweenValueExpression(new LiteralValueExpression(min), new LiteralValueExpression(max));
        }

        public static IFilterExpression And(params IFilterExpression[] filters)
        {
            if (filters?.Length == 0) return null;
            var filter = filters[0];
            for(var i = 1; i < filters.Length; i++)
            {
                filter = filter.And(filters[i]);
            }
            return filter;
        }

        public static IFilterExpression Or(params IFilterExpression[] filters)
        {
            if (filters?.Length == 0) return null;
            var filter = filters[0];
            for (var i = 1; i < filters.Length; i++)
            {
                filter = filter.Or(filters[i]);
            }
            return filter;
        }

        public static IOrderByExpression OrderBy(params IOrderExpression[] orders)
        {
            return new OrderByExpression(orders);
        }

        public static IOrderExpression Desc(string property)
        {
            return new OrderExpression(property, OrderEnum.Desc);
        }

        public static IOrderExpression Asc(string property)
        {
            return new OrderExpression(property);
        }

        public static ILimitExpression Limit(int count, int offset = 0)
        {
            return new LimitExpression(offset, count);
        }

        public static ILimitExpression Page(int pageindex, int pagesize)
        {
            return new LimitExpression(pagesize * (pageindex - 1), pagesize);
        }

        public static ICustomerExpression Customer(string expression)
        {
            return new CustomerExpression(expression);
        }
    }
}
