using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AisBenefits.Models.ExcelReport
{
    public class TableItemSelector<TModel>
    {
        public string Title;
        public Func<TModel, object> selector;

        public TableItemSelector(string title, Func<TModel, object> selector)
        {
            Title = title;
            this.selector = selector;
        }
    }
}
