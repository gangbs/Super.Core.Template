using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Super.Core.Infrastruct.Query2Exp
{
    public static class QueryExpression
    {
        /// <summary>
        /// 将查询条件model转换成表达式树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateExpression<T, K>(K model)
        {
            var allProperty = typeof(K).GetProperties();
            var lstValidProperty = new List<Tuple<int, PropertyInfo>>();

            //1.先去除掉忽略项
            foreach (var p in allProperty)
            {
                //如果有Ignore直接忽略
                if (p.GetCustomAttribute<IgnoreAttribute>() != null) continue;

                //如果model中对应的值为null，也直接忽略
                if (p.GetValue(model) == null) continue;

                var compare = p.GetCustomAttribute<QueryCompareAttribute>();

                int order = compare != null ? compare.Order : 0;

                lstValidProperty.Add(new Tuple<int, PropertyInfo>(order, p));
            }

            if (lstValidProperty.Count == 0)
            {
                return x => true;
            }

            lstValidProperty = lstValidProperty.OrderBy(x => x.Item1).ToList();

            ParameterExpression parameter = Expression.Parameter(typeof(T));
            Expression query = null;
            for (int i = 0; i < lstValidProperty.Count; i++)
            {
                var p = lstValidProperty[i].Item2;
                var val = p.GetValue(model);

                string fieldName = p.Name;
                CompareType compareType = CompareType.Eq;
                var compare = p.GetCustomAttribute<QueryCompareAttribute>();
                if (compare != null)
                {
                    fieldName = string.IsNullOrWhiteSpace(compare.FieldName) ? p.Name : compare.FieldName;
                    compareType = compare.compare;
                }


                ConstantExpression constant = Expression.Constant(val, typeof(T).GetProperty(fieldName).PropertyType);
                //ConstantExpression constant = Expression.Constant(val, val.GetType());
                MemberExpression member = Expression.PropertyOrField(parameter, fieldName);
                Expression exp;
                switch (compareType)
                {
                    case CompareType.Eq:
                        exp = Expression.Equal(member, constant);
                        break;
                    case CompareType.NotEq:
                        exp = Expression.NotEqual(member, constant);
                        break;
                    case CompareType.Gt:
                        exp = Expression.GreaterThan(member, constant);
                        break;
                    case CompareType.GtEq:
                        exp = Expression.GreaterThanOrEqual(member, constant);
                        break;
                    case CompareType.Lt:
                        exp = Expression.LessThan(member, constant);
                        break;
                    case CompareType.LtEq:
                        exp = Expression.LessThanOrEqual(member, constant);
                        break;
                    case CompareType.Like:
                        exp = Expression.Call(member, typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }), constant);
                        break;
                    case CompareType.LeftLike:
                        exp = Expression.Call(member, typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }), constant);
                        break;
                    case CompareType.RightLike:
                        exp = Expression.Call(member, typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }), constant);
                        break;
                    default:
                        exp = Expression.Equal(member, constant);
                        break;
                }


                if (i == 0)
                {
                    query = exp;
                }
                else
                {
                    query = Expression.And(query, exp);
                }
            }
            return Expression.Lambda<Func<T, bool>>(query, parameter);
        }


        public static Expression<Func<T, bool>> AddAndExpression<T>(this Expression<Func<T, bool>> exp, CompareType compareType, string parameterName, object val)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            ConstantExpression constant = Expression.Constant(val);
            MemberExpression member = Expression.PropertyOrField(exp.Parameters[0], parameterName);

            Expression expAdd;
            switch (compareType)
            {
                case CompareType.Eq:
                    expAdd = Expression.Equal(member, constant);
                    break;
                case CompareType.NotEq:
                    expAdd = Expression.NotEqual(member, constant);
                    break;
                case CompareType.Gt:
                    expAdd = Expression.GreaterThan(member, constant);
                    break;
                case CompareType.GtEq:
                    expAdd = Expression.GreaterThanOrEqual(member, constant);
                    break;
                case CompareType.Lt:
                    expAdd = Expression.LessThan(member, constant);
                    break;
                case CompareType.LtEq:
                    expAdd = Expression.LessThanOrEqual(member, constant);
                    break;
                case CompareType.Like:
                    expAdd = Expression.Call(member, typeof(string).GetMethod(nameof(string.Contains)), constant);
                    break;
                case CompareType.LeftLike:
                    expAdd = Expression.Call(member, typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }), constant);
                    break;
                case CompareType.RightLike:
                    expAdd = Expression.Call(member, typeof(string).GetMethod(nameof(string.EndsWith)), constant);
                    break;
                default:
                    expAdd = Expression.Equal(member, constant);
                    break;
            }

            var query = Expression.And(exp.Body, expAdd);

            return Expression.Lambda<Func<T, bool>>(query, exp.Parameters);
        }

    }
}
