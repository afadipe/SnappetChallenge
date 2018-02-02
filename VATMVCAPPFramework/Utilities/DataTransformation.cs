using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace VATMVCAPPFramework.Utilities
{
    public class DataTransformation
    {
        public static DataSet ExcelToDataSet(string SourceFilename)
        {
            DataSet ds = new DataSet();
            try
            {
                string connStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;\"", SourceFilename);

                OleDbConnection conn = new OleDbConnection(connStr);
                conn.Open();
                DataTable schemaDT = conn.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
                conn.Close();

                string tableName = schemaDT.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [{0}]", tableName), conn);

                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(ds);

            }
            catch (Exception)
            {                

            }

            return ds;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        
    }

    public class CommonUtility
    {
        public static string GetSHA512(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] data = UE.GetBytes(text);
            SHA512 hashString = new SHA512Managed();
            byte[] hashValue = hashString.ComputeHash(data);
            string hex = ByteToString(hashValue);
            return hex;
        }
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        public static string Tokenize(string amount)
        {
            return (amount.Split('.')[0].Replace(",", ""));
        }
    }
}