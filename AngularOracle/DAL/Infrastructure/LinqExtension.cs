using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace AngularOracle.DAL.Infrastructure
{
    public static class LinqExtension
    {
        /// <summary>
        /// Применить сортировку
        /// </summary>
        public static IQueryable<T> ApplyOrder<T>(this IQueryable<T> data, string property, string direction)
        {
            IQueryable<T> result = data;

            direction = !string.IsNullOrEmpty(direction) ? direction.ToUpper() : "ASC";

            if (string.IsNullOrEmpty(property))
                property = typeof(T).GetProperties().FirstOrDefault().Name;

            ParameterExpression x = Expression.Parameter(typeof(T), "x");

            Type propertyType = typeof(T).GetProperties().FirstOrDefault(x => x.Name.ToLower().Equals(property.ToLower())).PropertyType;

            UnaryExpression unaryExpression = Expression.Convert(Expression.Property(x, property), propertyType);

            if (propertyType.FullName == typeof(string).FullName)
            {
                Expression<Func<T, string>> expression = Expression.Lambda<Func<T, string>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(int).FullName)
            {
                Expression<Func<T, int>> expression = Expression.Lambda<Func<T, int>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(Int64?).FullName)
            {
                Expression<Func<T, Int64?>> expression = Expression.Lambda<Func<T, Int64?>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(Int64).FullName)
            {
                Expression<Func<T, Int64>> expression = Expression.Lambda<Func<T, Int64>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(double).FullName)
            {
                Expression<Func<T, double>> expression = Expression.Lambda<Func<T, double>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(decimal).FullName)
            {
                Expression<Func<T, decimal>> expression = Expression.Lambda<Func<T, decimal>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(bool).FullName)
            {
                Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(DateTime).FullName)
            {
                Expression<Func<T, DateTime>> expression = Expression.Lambda<Func<T, DateTime>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else if (propertyType.FullName == typeof(DateTime?).FullName)
            {
                Expression<Func<T, DateTime?>> expression = Expression.Lambda<Func<T, DateTime?>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
            else
            {
                Expression<Func<T, object>> expression = Expression.Lambda<Func<T, object>>(unaryExpression, x);

                return (direction == "ASC") ? result.OrderBy(expression) : result.OrderByDescending(expression);
            }
        }

        //public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> data, PagingInfo pagingInfo)
        //{
        //    if (pagingInfo.FilterParams != null && pagingInfo.FilterParams.Count() > 0)
        //    {
        //        ParameterExpression x = Expression.Parameter(typeof(T), "x");
        //        List<Expression> filterExpressions = pagingInfo.FilterParams.Select(field =>
        //        {
        //            MemberExpression propertyExpression = Expression.Property(x, field.name);

        //            return GetPropertyFilterExpression(propertyExpression, field.value);
        //        })
        //        .Where(expr => expr != null).ToList();

        //        if (filterExpressions.Any())
        //        {
        //            Expression wholeExpression = filterExpressions.First();
        //            foreach (var expr in filterExpressions.Skip(1))
        //                wholeExpression = Expression.MakeBinary(ExpressionType.AndAlso, wholeExpression, expr);

        //            Expression<Func<T, bool>> filterFuncExpression = Expression.Lambda<Func<T, bool>>(wholeExpression, x);
        //            data = data.Where(filterFuncExpression);
        //        }
        //    }

        //    return data;
        //}

        private static Expression GetPropertyFilterExpression(MemberExpression propertyExpression, string value)
        {
            Type valueType = propertyExpression.Type;
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                valueType = valueType.GetGenericArguments()[0];
            }

            if (valueType == typeof(string))
            {
                return Expression.Call(
                    Expression.Call(propertyExpression, typeof(string).GetMethod("ToLower", new Type[0])),
                    typeof(string).GetMethod("Contains"),
                    Expression.Constant(value.ToLower()));
            }

            if (valueType.BaseType == typeof(Enum))
            {
                object enumValue;
                if (string.IsNullOrEmpty(value) || value == "-")
                {
                    enumValue = null;
                }
                else
                {
                    try
                    {
                        enumValue = Enum.Parse(valueType, value);
                    }
                    catch
                    {
                        enumValue = null;
                    }
                }

                if (enumValue != null)
                {
                    return Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(enumValue), propertyExpression.Type));
                }
            }

            if (valueType == typeof(int))
            {
                int intValue;
                if (int.TryParse(value, out intValue))
                {
                    return Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(intValue), propertyExpression.Type));
                }
            }

            if (valueType == typeof(Int64))
            {
                Int64 int64Value;
                if (Int64.TryParse(value, out int64Value))
                {
                    return Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(int64Value), propertyExpression.Type));
                }
            }

            if (valueType == typeof(bool))
            {
                bool boolValue;
                if (bool.TryParse(value, out boolValue))
                {
                    return Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(boolValue), propertyExpression.Type));
                }
            }

            if (valueType == typeof(decimal))
            {
                decimal decimalValue;
                if (decimal.TryParse(
                    value.Replace(CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator),
                    out decimalValue))
                {
                    return
                    Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(decimalValue), propertyExpression.Type));
                }
            }

            if (valueType == typeof(DateTime))
            {
                DateTime dateValue;
                DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);

                if (dateValue != DateTime.MinValue)
                {
                    return Expression.And(Expression.GreaterThan(propertyExpression, Expression.Convert(Expression.Constant(dateValue), propertyExpression.Type)), Expression.LessThan(propertyExpression, Expression.Convert(Expression.Constant(dateValue.AddDays(1)), propertyExpression.Type)));
                }
            }

            return null;
        }
    }
}
