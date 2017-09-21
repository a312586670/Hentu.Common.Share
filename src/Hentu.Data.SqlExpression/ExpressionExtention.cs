using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hentu.Data
{
    public static class ExpressionExtention
    {
        #region Property

        #region ShortCut

        public static IParamExpression ToP(this IPropertyExpression property, string param = null)
        {
            return ToParam(property, param);
        }

        public static ISetItemExpression SetC(this IPropertyExpression property, string customer)
        {
            return SetVarCustomer(property, customer);
        }

        public static ISetItemExpression SetP(this IPropertyExpression property, string param = null)
        {
            return SetVarParam(property, param);
        }

        #endregion

        public static IParamExpression ToParam(this IPropertyExpression property, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = property.Name;
            return new ParamExpression(param);
        }

        public static ISetItemExpression SetVarCustomer(this IPropertyExpression property, string customer)
        {
            return Set(property, new CustomerExpression(customer));
        }

        public static ISetItemExpression SetVarParam(this IPropertyExpression property, string param = null)
        {
            return Set(property, property.ToParam(param));
        }

        public static ISetItemExpression Set(this IPropertyExpression property, object value = null)
        {
            return Set(property, new LiteralValueExpression(value));
        }

        public static ISetItemExpression Set(this IPropertyExpression property, ISetableValueExpression value)
        {
            return new SetItemExpression(property, value);
        }

        public static ISelectItemExpression As(this IPropertyExpression property, string asName)
        {
            return new AsExpression(property, new AsPropertyExpression(asName));
        }

        #endregion

        #region Select

        #region ShortCut

        #endregion

        public static ISelectExpression Select(this ITableExpression table, params string[] properties)
        {
            var items = properties.Select(p => new PropertyExpression(p) as ISelectItemExpression);
            return Select(table, items.ToArray());
        }

        public static ISelectExpression Select(this ITableExpression table, params ISelectItemExpression[] items)
        {
            return new SelectExpression(new ITableExpression[] { table }, items, null, null);
        }

        public static ISelectExpression Select(this IEnumerable<ITableExpression> tables, params ISelectItemExpression[] items)
        {
            return new SelectExpression(tables.ToArray(), items, null, null);
        }


        public static ISelectExpression Where(this ISelectExpression select, IFilterExpression filter)
        {
            select.Where = new WhereExpression(filter);
            return select;
        }

        #region OrderBy

        public static ISelectExpression OrderBy(this ISelectExpression select, params IOrderExpression[] orders)
        {
            var orderby = new OrderByExpression(orders);
            return select.OrderBy(orderby);
        }

        public static ISelectExpression OrderBy(this ISelectExpression select, IOrderByExpression orderby)
        {
            select.OrderBy = orderby;
            return select;
        }

        public static IOrderExpression Desc(this IPropertyExpression p)
        {
            return new OrderExpression(p, OrderEnum.Desc);
        }

        public static IOrderExpression Asc(this IPropertyExpression p)
        {
            return new OrderExpression(p, OrderEnum.Asc);
        }

        #endregion

        #region Limit

        public static ISelectExpression Limit(this ISelectExpression select, int offset, int count)
        {
            select.Limit = new LimitExpression(offset, count);
            return select;
        }

        public static ISelectExpression Limit(this ISelectExpression select, ILimitExpression limit)
        {
            select.Limit = limit;
            return select;
        }

        public static ISelectExpression Page(this ISelectExpression select, int pageindex, int pagesize)
        {
            select.Limit = new LimitExpression(pagesize * (pageindex - 1), pagesize);
            return select;
        }

        #endregion

        #endregion

        #region Insert

        #region ShortCut

        public static IInsertExpression InsertP(this ITableExpression table, params string[] properties)
        {
            return InsertVarParam(table, properties);
        }

        public static IInsertExpression ValuesP(this IInsertExpression insert)
        {
            return ValuesVarParam(insert);
        }

        #endregion

        public static IInsertExpression Insert(this ITableExpression table, params string[] properties)
        {
            var columns = properties.Select(p => new PropertyExpression(p) as IPropertyExpression);

            return new InsertExpression(table, columns.ToArray(), null);
        }

        public static IInsertExpression InsertVarParam(this ITableExpression table, params string[] properties)
        {
            var columns = properties.Select(p => new PropertyExpression(p) as IPropertyExpression);
            var _params = properties.Select(p => new ParamExpression(p) as IParamExpression);
            return new InsertExpression(table, columns.ToArray(), _params.ToArray());
        }

        public static IInsertExpression Values(this IInsertExpression insert, params ISetableValueExpression[] values)
        {
            insert.Values = values;
            return insert;
        }

        public static IInsertExpression ValuesVarParam(this IInsertExpression insert)
        {
            insert.Values = insert.Properties.Select(p => new ParamExpression(p.Name)).ToArray();
            return insert;
        }

        #endregion

        #region Delete

        #region ShortCut

        #endregion

        public static IDeleteExpression Delete(this ITableExpression table)
        {
            return new DeleteExpression(table, null);
        }

        public static IDeleteExpression Where(this IDeleteExpression delete, IFilterExpression filter)
        {
            delete.Where = new WhereExpression(filter);
            return delete;
        }

        #endregion

        #region Update

        #region ShortCut

        public static IUpdateExpression UpdateP(this ITableExpression table, params string[] properties)
        {
            return UpdateVarParam(table, properties);
        }

        public static IUpdateExpression ValuesP(this IUpdateExpression update)
        {
            return ValuesVarParam(update);
        }

        public static IUpdateExpression ValuesC(this IUpdateExpression update, params string[] values)
        {
            return ValuesVarCustomer(update, values);
        }

        public static IUpdateExpression SetItemP(this IUpdateExpression update, IPropertyExpression property, string param = null)
        {
            return SetItemVarParam(update, property, param);
        }

        public static IUpdateExpression SetItemC(this IUpdateExpression update, IPropertyExpression property, string customer)
        {
            return SetItemVarCustomer(update, property, customer);
        }

        public static IUpdateExpression SetItemP(this IUpdateExpression update, string property, string param = null)
        {
            return SetItemVarParam(update, property, param);
        }

        public static IUpdateExpression SetItemC(this IUpdateExpression update, string property, string customer)
        {
            return SetItemVarCustomer(update, property, customer);
        }

        public static ISetExpression ValuesP(this ISetExpression set)
        {
            return ValuesVarParam(set);
        }

        public static ISetExpression ValuesC(this ISetExpression set, params string[] values)
        {
            return ValuesVarCustomer(set, values);
        }

        public static ISetExpression SetItemP(this ISetExpression set, IPropertyExpression property, string param = null)
        {
            return SetItemVarParam(set, property, param);
        }

        public static ISetExpression SetItemC(this ISetExpression set, IPropertyExpression property, string customer)
        {
            return SetItemVarCustomer(set, property, customer);
        }

        public static ISetItemExpression ValueObj(this ISetItemExpression setItem, object value)
        {
            return ValueVarObject(setItem, value);
        }

        public static ISetItemExpression ValueP(this ISetItemExpression setItem, string param = null)
        {
            return ValueVarParam(setItem, param);
        }

        public static ISetItemExpression ValueC(this ISetItemExpression setItem, string customer)
        {
            return ValueVarCustomer(setItem, customer);
        }

        #endregion

        public static IUpdateExpression Update(this ITableExpression table, params string[] properties)
        {
            var columns = properties.Select(p => new PropertyExpression(p) as IPropertyExpression);
            var set = new SetExpression(columns.Select(c => new SetItemExpression(c, null) as ISetItemExpression).ToArray());
            return new UpdateExpression(table, set, null);
        }

        public static IUpdateExpression UpdateVarParam(this ITableExpression table, params string[] properties)
        {
            var set = new SetExpression(properties.Select(p => new SetItemExpression(new PropertyExpression(p), new ParamExpression(p)) as ISetItemExpression).ToArray());
            return new UpdateExpression(table, set, null);
        }

        public static IUpdateExpression Values(this IUpdateExpression update, params ISetableValueExpression[] values)
        {
            update.Set = update.Set.Values(values);
            return update;
        }

        public static IUpdateExpression Values(this IUpdateExpression update, params object[] values)
        {
            update.Set = update.Set.Values(values);
            return update;
        }

        public static IUpdateExpression ValuesVarParam(this IUpdateExpression update)
        {
            update.Set = update.Set.ValuesVarParam();
            return update;
        }

        public static IUpdateExpression ValuesVarCustomer(this IUpdateExpression update, params string[] values)
        {
            update.Set = update.Set.ValuesVarCustomer(values);
            return update;
        }

        public static IUpdateExpression SetItem(this IUpdateExpression update, ISetItemExpression setItem)
        {
            update.Set = update.Set.SetItem(setItem);
            return update;
        }

        public static IUpdateExpression SetItem(this IUpdateExpression update, IPropertyExpression property, ISetableValueExpression value)
        {
            update.Set = update.Set.SetItem(property, value);
            return update;
        }

        public static IUpdateExpression SetItem(this IUpdateExpression update, IPropertyExpression property, object value)
        {
            update.Set = update.Set.SetItem(property, value);
            return update;
        }

        public static IUpdateExpression SetItemVarParam(this IUpdateExpression update, IPropertyExpression property, string param = null)
        {
            update.Set = update.Set.SetItemVarParam(property, param);
            return update;
        }

        public static IUpdateExpression SetItemVarCustomer(this IUpdateExpression update, IPropertyExpression property, string customer)
        {
            update.Set = update.Set.SetItemVarCustomer(property, customer);
            return update;
        }

        public static IUpdateExpression SetItem(this IUpdateExpression update, string property, ISetableValueExpression value)
        {
            return SetItem(update, new PropertyExpression(property), value);
        }

        public static IUpdateExpression SetItem(this IUpdateExpression update, string property, object value)
        {
            return SetItem(update, new PropertyExpression(property), value);
        }

        public static IUpdateExpression SetItemVarParam(this IUpdateExpression update, string property, string param = null)
        {
            return SetItemVarParam(update, new PropertyExpression(property), param);
        }

        public static IUpdateExpression SetItemVarCustomer(this IUpdateExpression update, string property, string customer)
        {
            return SetItemVarCustomer(update, new PropertyExpression(property), customer);

        }

        public static IUpdateExpression Where(this IUpdateExpression update, IFilterExpression filter)
        {
            update.Where = new WhereExpression(filter);
            return update;
        }

        #region ISetExpression

        public static ISetExpression Values(this ISetExpression set, params ISetableValueExpression[] values)
        {
            for (int i = 0, j = 0; i < set.Sets.Length && j < values.Length;)
            {
                var setitem = set.Sets[i++];
                if (setitem.Value == null) setitem.Value = values[j++];
            }
            set.Sets = set.Sets;
            return set;
        }

        public static ISetExpression Values(this ISetExpression set, params object[] values)
        {
            return Values(set, values.Select(v => new LiteralValueExpression(v)).ToArray());
        }

        public static ISetExpression ValuesVarParam(this ISetExpression set)
        {
            return Values(set, set.Sets.Select(s => new ParamExpression(s.Property.Name)).ToArray());
        }

        public static ISetExpression ValuesVarCustomer(this ISetExpression set, params string[] values)
        {
            return Values(set, values.Select(v=>new CustomerExpression(v)).ToArray());
        }

        public static ISetExpression SetItem(this ISetExpression set, ISetItemExpression setItem)
        {
            var list = (set.Sets?.ToList() ?? new List<ISetItemExpression>());
            list.RemoveAll(s => s.Property.Name == setItem.Property.Name);
            list.Add(setItem);
            set.Sets = list.ToArray();
            return set;
        }

        public static ISetExpression SetItem(this ISetExpression set, IPropertyExpression property, ISetableValueExpression value)
        {
            return SetItem(set, property.Set(value));
        }

        public static ISetExpression SetItem(this ISetExpression set, IPropertyExpression property, object value)
        {
            return SetItem(set, property.Set(value));
        }

        public static ISetExpression SetItemVarParam(this ISetExpression set, IPropertyExpression property, string param = null)
        {
            return SetItem(set, property.SetVarParam(param));
        }

        public static ISetExpression SetItemVarCustomer(this ISetExpression set, IPropertyExpression property, string customer)
        {
            return SetItem(set, property.SetVarCustomer(customer));
        }

        #endregion

        #region ISetItemExpression

        public static ISetItemExpression ValueVarObject(this ISetItemExpression setItem, object value)
        {
            setItem.Value = new LiteralValueExpression(value);
            return setItem;
        }

        public static ISetItemExpression ValueVarParam(this ISetItemExpression setItem, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = setItem.Property.Name;
            setItem.Value = new ParamExpression(param);
            return setItem;
        }

        public static ISetItemExpression ValueVarCustomer(this ISetItemExpression setItem, string customer)
        {
            setItem.Value = new CustomerExpression(customer);
            return setItem;
        }

        #endregion

        #endregion

        #region FilterExpression

        public static ILogicExpression Or(this IFilterExpression a, IFilterExpression b)
        {
            return new LogicExpression(a, LogicOperator.Or, b);
        }

        public static ILogicExpression And(this IFilterExpression a, IFilterExpression b)
        {
            return new LogicExpression(a, LogicOperator.And, b);
        }


        public static IFilterExpression Eq(this IPropertyExpression p, IValueExpression exp)
        {
            return new BinarayExpression(p, Operator.Eq, exp);
        }

        public static IFilterExpression Neq(this IPropertyExpression p, IValueExpression exp)
        {
            return new BinarayExpression(p, Operator.Neq, exp);
        }

        public static IFilterExpression Gt(this IPropertyExpression p, IValueExpression exp)
        {
            return new BinarayExpression(p, Operator.Gt, exp);
        }

        public static IFilterExpression GtOrEq(this IPropertyExpression p, IValueExpression exp)
        {
            return new BinarayExpression(p, Operator.GtOrEq, exp);
        }

        public static IFilterExpression Lt(this IPropertyExpression p, IValueExpression exp)
        {
            return new BinarayExpression(p, Operator.Lt, exp);
        }

        public static IFilterExpression LtOrEq(this IPropertyExpression p, IValueExpression exp)
        {
            return new BinarayExpression(p, Operator.LtOrEq, exp);
        }

        public static IFilterExpression Like(this IPropertyExpression p, IValueExpression exp)
        {
            return new BinarayExpression(p, Operator.Like, exp);
        }

        public static IFilterExpression Eq(this IPropertyExpression p, object value)
        {
            return p.Eq(new LiteralValueExpression(value));
        }

        public static IFilterExpression Neq(this IPropertyExpression p, object value)
        {
            return p.Neq(new LiteralValueExpression(value));
        }

        public static IFilterExpression Gt(this IPropertyExpression p, object value)
        {
            return p.Gt(new LiteralValueExpression(value));
        }

        public static IFilterExpression GtOrEq(this IPropertyExpression p, object value)
        {
            return p.GtOrEq(new LiteralValueExpression(value));
        }

        public static IFilterExpression Lt(this IPropertyExpression p, object value)
        {
            return p.Lt(new LiteralValueExpression(value));
        }

        public static IFilterExpression LtOrEq(this IPropertyExpression p, object value)
        {
            return p.LtOrEq(new LiteralValueExpression(value));
        }

        public static IFilterExpression Like(this IPropertyExpression p, object value)
        {
            return p.Like(new LiteralValueExpression(value));
        }
        public static IFilterExpression EqVarParam(this IPropertyExpression p, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = p.Name;
            return p.Eq(new ParamExpression(param));
        }

        public static IFilterExpression NeqVarParam(this IPropertyExpression p, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = p.Name;
            return p.Neq(new ParamExpression(param));
        }

        public static IFilterExpression GtVarParam(this IPropertyExpression p, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = p.Name;
            return p.Gt(new ParamExpression(param));
        }

        public static IFilterExpression GtOrEqVarParam(this IPropertyExpression p, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = p.Name;
            return p.GtOrEq(new ParamExpression(param));
        }

        public static IFilterExpression LtVarParam(this IPropertyExpression p, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = p.Name;
            return p.Lt(new ParamExpression(param));
        }

        public static IFilterExpression LtOrEqVarParam(this IPropertyExpression p, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = p.Name;
            return p.LtOrEq(new ParamExpression(param));
        }

        public static IFilterExpression LikeVarParam(this IPropertyExpression p, string param = null)
        {
            if (string.IsNullOrWhiteSpace(param)) param = p.Name;
            return p.Like(new ParamExpression(param));
        }

        public static IFilterExpression IsNull(this IPropertyExpression p)
        {
            return new BinarayExpression(p, Operator.IsNull, new NoValueExpression());
        }

        public static IFilterExpression IsNotNull(this IPropertyExpression p)
        {
            return new BinarayExpression(p, Operator.IsNotNull, new NoValueExpression());
        }

        public static IFilterExpression In(this IPropertyExpression p, params object[] values)
        {
            return new BinarayExpression(p, Operator.In, new CollectionExpression(values.Select(v => new LiteralValueExpression(v)).ToArray()));
        }

        public static IFilterExpression In(this IPropertyExpression p, ICustomerValueExpression value)
        {
            return new BinarayExpression(p, Operator.In, value);
        }

        public static IFilterExpression NotIn(this IPropertyExpression p, params object[] values)
        {
            return new BinarayExpression(p, Operator.NotIn, new LiteralValueExpression(values.Select(v => new LiteralValueExpression(v)).ToArray()));
        }

        public static IFilterExpression NotIn(this IPropertyExpression p, ICustomerValueExpression value)
        {
            return new BinarayExpression(p, Operator.NotIn, value);
        }

        public static IFilterExpression Between(this IPropertyExpression p, object a, object b)
        {
            return new BinarayExpression(p, Operator.Between, new BetweenValueExpression(new LiteralValueExpression(a), new LiteralValueExpression(b)));
        }

        public static IFilterExpression Between(this IPropertyExpression p, ICustomerValueExpression value)
        {
            return new BinarayExpression(p, Operator.Between, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Expression:{p.Expression} BETWEEN @{p.Name}1 AND @{p.Name}2</returns>
        public static IFilterExpression BetweenVarParam(this IPropertyExpression p)
        {
            return new BinarayExpression(p, Operator.Between, new BetweenValueExpression(new ParamExpression(string.Format("{0}1", p.Name)), new ParamExpression(string.Format("{0}2", p.Name))));
        }

        public static IFilterExpression NotBetween(this IPropertyExpression p, object a, object b)
        {
            return new BinarayExpression(p, Operator.NotBetween, new BetweenValueExpression(new LiteralValueExpression(a), new LiteralValueExpression(b)));
        }

        public static IFilterExpression NotBetween(this IPropertyExpression p, ICustomerValueExpression value)
        {
            return new BinarayExpression(p, Operator.NotBetween, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Expression:{p.Expression} BETWEEN @{p.Name}1 AND @{p.Name}2</returns>
        public static IFilterExpression NotBetweenVarParam(this IPropertyExpression p)
        {
            return new BinarayExpression(p, Operator.NotBetween, new BetweenValueExpression(new ParamExpression(string.Format("{0}1", p.Name)), new ParamExpression(string.Format("{0}2", p.Name))));
        }

        #endregion
    }
}
