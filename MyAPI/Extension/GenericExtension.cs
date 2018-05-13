using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace MyAPI.Extension
{
    public static class GenericExtension
    {
        public static DataTable ToDataTable<T>(this List<T> iList, string[] includeColumns)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);

                if (includeColumns.Any(s => string.Equals(s, propertyDescriptor.Name, StringComparison.OrdinalIgnoreCase)))
                    dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[includeColumns.Length];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[includeColumns[i]].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
