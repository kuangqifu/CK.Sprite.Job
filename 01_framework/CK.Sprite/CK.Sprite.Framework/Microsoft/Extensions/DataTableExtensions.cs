using CK.Sprite.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Dynamic;

namespace System.Data
{
    /// <summary>
    /// DataTable 扩展
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// 将DataTable 转换成 List<dynamic>
        /// reverse 反转：控制返回结果中是只存在 FilterField 指定的字段,还是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        /// FilterField  字段过滤，FilterField 为空 忽略 reverse 参数；返回DataTable中的全部数
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="reverse">
        /// 反转：控制返回结果中是只存在 FilterField 指定的字段,还是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        ///</param>
        /// <param name="FilterField">字段过滤，FilterField 为空 忽略 reverse 参数；返回DataTable中的全部数据</param>
        /// <returns>List<dynamic></returns>
        public static List<dynamic> ToDynamicList(this DataTable table, bool reverse = true, params string[] FilterField)
        {
            var modelList = new List<dynamic>();
            foreach (DataRow row in table.Rows)
            {
                dynamic model = new ExpandoObject();
                var dict = (IDictionary<string, object>)model;
                foreach (DataColumn column in table.Columns)
                {
                    if (FilterField.Length != 0)
                    {
                        if (reverse == true)
                        {
                            if (!FilterField.Contains(column.ColumnName))
                            {
                                dict[column.ColumnName] = row[column];
                            }
                        }
                        else
                        {
                            if (FilterField.Contains(column.ColumnName))
                            {
                                dict[column.ColumnName] = row[column];
                            }
                        }
                    }
                    else
                    {
                        dict[column.ColumnName] = row[column];
                    }
                }
                modelList.Add(model);
            }
            return modelList;
        }
    }
}
