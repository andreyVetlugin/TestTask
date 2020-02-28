using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Helpers
{
    public static class ClassExtentions
    {
        public static TValue SelectFieldOrDefault<TSource, TValue>(this TSource source, Func<TSource, TValue> selector)
            where TSource : class
        {
            if (source != null)
                return selector(source);
            return default(TValue);
        }

        public static TValue SelectObject<TSource, TValue>(this TSource source, Func<TSource, TValue> selector)
        {
            return selector(source);
        }
    }
}
