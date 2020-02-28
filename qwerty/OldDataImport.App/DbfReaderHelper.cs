using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetDBF;
using DotNetDBF.Enumerable;
using OldDataImport.App.Entities;

namespace OldDataImport.Infrastructure
{
    public static class DbfReaderHelper
    {
        public static IEnumerable<T> ReadAll<T>(string path)
            where T : class
        {
            using (var file = File.OpenRead(path))
            using (var dbf = new DBFReader(file){ CharEncoding = CodePagesEncodingProvider.Instance.GetEncoding("Windows-1251") })
            {
                return dbf.AllRecords<T>();
            }
        }

        public static void WriteAll<T>(string path, IEnumerable<T> records)
            where T : class
        {
            var type = typeof(T);

            var dbfFields = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => (
                    MapProperty(p),
                    p)
                ).ToArray();


            using (var file = File.OpenWrite(path))
            using (var dbf = new DBFWriter(file) { CharEncoding = CodePagesEncodingProvider.Instance.GetEncoding("Windows-1251") })
            {
                dbf.Fields = dbfFields.Select(f => f.Item1).ToArray();

                foreach(var record in records)
                    dbf.WriteRecord(dbfFields.Select(f => f.Item2.GetValue(record)).ToArray());
            }
        }

        static DBFField MapProperty(PropertyInfo p)
        {
            var fieldType = Map(p.PropertyType);

            switch (fieldType)
            {
                case NativeDbType.Date:
                    return new DBFField(p.Name, fieldType);
                case NativeDbType.Numeric:
                    return new DBFField(p.Name, fieldType, p.GetCustomAttribute<MaxLengthAttribute>().Length, p.GetCustomAttribute<DecimalLengthAttribute>()?.Length ?? 0);
                default:
                    return new DBFField(p.Name, fieldType, p.GetCustomAttribute<MaxLengthAttribute>().Length);
            }
        }

        static NativeDbType Map(Type type)
        {
            if (type == typeof(decimal) || type == typeof(int))
                return NativeDbType.Numeric;
            if (type == typeof(string))
                return NativeDbType.Char;
            if (type == typeof(DateTime))
                return NativeDbType.Date;
            if (type == typeof(bool))
                return NativeDbType.Logical;

            throw new NotImplementedException();
        }
    }
}
