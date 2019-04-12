using SpearHead.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.Common.Helpers
{
    public static class ExtensionMethods
    {
        public static List<T> ToListof<T>(this DataTable dt) where T : IModelIdentifier
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            int rowNumber = 1;
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {

                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                instanceOfT.Row = rowNumber;
                rowNumber++;
                return instanceOfT;
            }).ToList();

            return targetList;
        }

    }
}
