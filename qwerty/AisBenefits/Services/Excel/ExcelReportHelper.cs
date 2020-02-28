using AisBenefits.Models.ExcelReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AisBenefits.Services.Excel
{
    public static class ExcelReportHelper
    {
        public static Expression<Func<TSource, bool>> FilterEqual<TSource, TValue>(
          Expression<Func<TSource, TValue>> filterValue, TValue value)
        {
            return Expression.Lambda<Func<TSource, bool>>(Expression.Equal(filterValue.Body, Expression.Constant(value)), filterValue.Parameters);
        }
        public static Expression<Func<TSource, bool>> FilterEqual<TSource>(
            Expression<Func<TSource, DateTime>> filterValue, DateTime value)
        {
            return Expression.Lambda<Func<TSource, bool>>(Expression.Equal(filterValue.Body, Expression.Constant(value)), filterValue.Parameters);
        }
        public static Expression<Func<TSource, bool>> FilterLess<TSource, TValue>(
            Expression<Func<TSource, TValue>> filterValue, TValue value)
        {
            return Expression.Lambda<Func<TSource, bool>>(Expression.LessThan(filterValue.Body, Expression.Constant(value)), filterValue.Parameters);
        }

        public static Expression<Func<TSource, bool>> FilterGreater<TSource, TValue>(
            Expression<Func<TSource, TValue>> filterValue, TValue value)
        {
            return Expression.Lambda<Func<TSource, bool>>(Expression.GreaterThan(filterValue.Body, Expression.Constant(value)), filterValue.Parameters);
        }

        public static Expression<Func<TSource, bool>> FilterNotEqual<TSource, TValue>(
            Expression<Func<TSource, TValue>> filterValue, TValue value)
        {
            return Expression.Lambda<Func<TSource, bool>>(Expression.NotEqual(filterValue.Body, Expression.Constant(value)), filterValue.Parameters);
        }

        public static Expression<Func<TSource, bool>> FilterContains<TSource>(
            Expression<Func<TSource, string>> filterValue, string value)
        {
            return Expression.Lambda<Func<TSource, bool>>(Expression.Call(filterValue, "Contains", new[] { typeof(string) }, Expression.Constant(value)));
        }


        public static IQueryable<TSource> Conditional<TSource>(this IQueryable<TSource> sources, bool t, Expression<Func<TSource, bool>> where)
        {
            return t
                ? sources.Where(where)
                : sources;
        }

        public static IQueryable<TSource> FilterBy<TSource, TValue>(this IQueryable<TSource> sources, Expression<Func<TSource, TValue>> filterValue,
            FilterItem<TValue> item)
        where TValue: struct
        {
            if (!item.IsFiltered) return sources;

            switch (item.FilterType)
            {
                case ReportFilterType.Equal:
                    {
                        return sources.Where(FilterEqual(filterValue, item.Value.Value));
                    };
                case ReportFilterType.Less:
                    {
                        return sources.Where(FilterLess(filterValue, item.Value.Value));
                    };
                case ReportFilterType.More:
                    {
                        return sources.Where(FilterGreater(filterValue, item.Value.Value));
                    };
                case ReportFilterType.NotEqual:
                    {
                        return sources.Where(FilterNotEqual(filterValue, item.Value.Value));
                    }
                    ;
                default:
                    throw new InvalidDataException();
            }

        }

        public static IQueryable<TSource> FilterBy<TSource>(this IQueryable<TSource> sources, Expression<Func<TSource, string>> filterValue,
            FilterItemString item)
        {
            if (!item.IsFiltered) return sources;

            switch (item.FilterType)
            {
                case ReportFilterType.Equal:
                    {
                        return sources.Where(FilterContains(filterValue, item.Value));
                    };
                case ReportFilterType.Less:
                    {
                        return sources.Where(FilterLess(filterValue, item.Value));
                    };
                case ReportFilterType.More:
                    {
                        return sources.Where(FilterGreater(filterValue, item.Value));
                    };
                case ReportFilterType.NotEqual:
                    {
                        return sources.Where(FilterNotEqual(filterValue, item.Value));
                    }
                    ;
                default:
                    throw new InvalidDataException();
            }

        }




    }
}
